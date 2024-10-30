using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ILivingEntity {

    // Event
    public Action PlayerDeath;



    // Status
    private float maxHealth = 100.0f;
    public float MaxHealth {

        get {
            return maxHealth;
        }
        
        set {
            if (value < 1) value = 1;

            // 체력 조정
            maxHealth = value;
            if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;

            // UI 갱신
            HpFill.fillAmount = CurrentHealth / maxHealth;
            HpText.text = (int) CurrentHealth + " / " + (int) maxHealth;
        }

    }
    private float currentHealth;
    public float CurrentHealth {
        
        get {
            return currentHealth;
        }

        set {
            if (value < 0) value = 0;

            currentHealth = value > maxHealth ? maxHealth : value;

            // UI 갱신
            HpFill.fillAmount = currentHealth / MaxHealth;
            HpText.text = (int) currentHealth + " / " + (int) MaxHealth;

            if (currentHealth == 0) Die();
        }

    }
    private float moveSpeed = 5.0f;
    public float MoveSpeed {

        get {
            return moveSpeed;
        }

        set {
            moveSpeed = value;
        }

    }
    private bool isDead;
    public bool IsDead {

        get {
            return isDead;
        }

        private set {
            isDead = value;
        }

    }
    private bool isOperating;
    public bool IsOperating {

        get {
            return isOperating;
        }

        set {
            isOperating = value;
        }

    }
    private bool isCasting;
    public bool IsCasting {

        get {
            return isCasting;
        }

        set {
            isCasting = value;
        }

    }
    private int level;
    public int Level {

        get {
            return level;
        }

        private set {
            if (value < 0) value = 0;

            level = value;

            // TODO: UI 갱신
            LevelText.text = (level + 1).ToString();
        }
    }
    private int[] maxExp = {
        100, 200, 300, 400, 500,
        600, 700, 800, 900, 1000,
        1100, 1200, 1300, 1400, 1500,
        1600, 1700, 1800, 1900, 2000,
        2100, 2200, 2300, 2400, 2500,
        2600, 2700, 2800, 2900, 3000
    };
    public int[] MaxExp {

        get {
            return maxExp;
        }

        set {
            maxExp = value;

            // UI 갱신
            ExpFill.fillAmount = (float) currentExp / maxExp[level];
            ExpText.text = currentExp + " / " + maxExp[level];

            // 레벨 업
            if (currentExp >= MaxExp[level]) LevelUp();
        }

    }
    private int currentExp;
    public int CurrentExp {

        get {
            return currentExp;
        }

        set {
            if (value < 0) value = 0;

            currentExp = value;

            // UI 갱신
            ExpFill.fillAmount = (float) currentExp / maxExp[level];
            ExpText.text = currentExp + " / " + maxExp[level];

            // 레벨 업
            if (currentExp >= MaxExp[level]) LevelUp();
        }
    }
    


    // Option - Camera
    private float cameraFollowSpeed = 5.0f;
    public float CameraFollowSpeed {

        get {
            return cameraFollowSpeed;
        }

        set {
            cameraFollowSpeed = value;
        }

    }

    // Option - Movement
    private float rotateSpeed = 50.0f;
    public float RotateSpeed {

        get {
            return rotateSpeed;
        }

        set {
            rotateSpeed = value;
        }

    }
    


    // Object - Camera
    [SerializeField] private GameObject camera;
    public GameObject Camera {

        get {
            return camera;
        }

        private set {
            camera = value;
        }

    }

    // Object
    [SerializeField] private GameObject sword;
    public GameObject Sword {

        get {
            return sword;
        }

        private set {
            sword = value;
        }

    }
    [SerializeField] private GameObject abilityBundle;
    public GameObject AbilityBundle {
        
        get {
            return abilityBundle;
        }

        private set {
            abilityBundle = value;
        }

    }
    public GameObject bloodParticle;

    // Object - UI
    [SerializeField] private Text levelText;
    public Text LevelText {

        get {
            return levelText;
        }

        private set {
            levelText = value;
        }

    }
    [SerializeField] private Image hpFill;
    public Image HpFill {
        
        get {
            return hpFill;
        }

        private set {
            hpFill = value;
        }

    }
    [SerializeField] private Text hpText;
    public Text HpText {

        get {
            return hpText;
        }

        private set {
            hpText = value;
        }

    }
    [SerializeField] private Image expFill;
    public Image ExpFill {

        get {
            return expFill;
        }

        private set {
            expFill = value;
        }

    }
    [SerializeField] private Text expText;
    public Text ExpText {

        get {
            return expText;
        }

        set {
            expText = value;
        }

    }
    [SerializeField] private GameObject abilityCoolBundle;
    public GameObject AbilityCoolBundle {

        get {
            return abilityCoolBundle;
        }

        private set {
            abilityCoolBundle = value;
        }

    }
    
    // Object - Sound
    public AudioClip[] hitSounds;
    public AudioClip deathSound;
    


    // Camera
    public Vector3 cameraOffsetPosition { get; private set; }

    // Player Script
    public PlayerInput input { get; private set; }
    public PlayerMovement movement { get; private set; }

    // Player Component
    public CharacterController controller { get; private set; }
    public Animator animator { get; private set; }
    public AudioSource source { get; private set; }

    // Ability
    public IAbility[] abilities { get; private set; }
    public IAbility[] equippedAbilities { get; private set; } = new IAbility[4];

    // UI
    public Image[] abilityCoolFills { get; set; } = new Image[4];





    private void Start() {
        CurrentHealth = MaxHealth;
        IsDead = MaxHealth > 0 ? false : true;
        IsOperating = false;
        IsCasting = false;
        Level = 0;
        CurrentExp = 0;
        


        cameraOffsetPosition = Camera.transform.position;

        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        Transform abilityBundleTransform = AbilityBundle.transform;
        int abilityBundleChildCount = abilityBundleTransform.childCount;
        abilities = new IAbility[abilityBundleChildCount];
        for (int i = 0; i < abilityBundleChildCount; i++) abilities[i] = abilityBundleTransform.GetChild(i).GetComponent<IAbility>();
        equippedAbilities[0] = abilities[0];
        equippedAbilities[1] = abilities[1];
        equippedAbilities[2] = abilities[2];

        Transform abilityCoolBundleTransform = AbilityCoolBundle.transform;
        int abilityCoolBundleChildCount = abilityCoolBundleTransform.childCount;
        for (int i = 0; i < abilityCoolBundleChildCount; i++) abilityCoolFills[i] = abilityCoolBundleTransform.GetChild(i).GetChild(2).GetComponent<Image>();
    }

    private void Update() {
        // 시간이 흐르지 않을 경우
        if (Time.timeScale == 0) return;



        // 카메라 추적
        movement.FollowCamera(this, Camera.transform.position);



        // 플레이어가 사망했을 경우
        if (IsDead) return;
        
        // 플레이어가 능력을 시전 중일 경우
        if (IsCasting) return;

        // 이동
        movement.Move(this, input.horizontal, input.vertical);



        // 플레이어가 능력을 사용 중일 경우
        if (IsOperating) return;

        // 능력 사용
        if (input.useAbility[0]) UseAbility(0);
        if (input.useAbility[1]) UseAbility(1);
        if (input.useAbility[2]) UseAbility(2);
    }




    
    // 능력 사용
    public void UseAbility(int number) {
        equippedAbilities[number].UseAbility(this);

        StartCoroutine(CUseAbility_RunCooldownUI(number));
    }
    
    private IEnumerator CUseAbility_RunCooldownUI(int number) {
        IAbility ability = equippedAbilities[number];
        Image image = abilityCoolFills[number];

        while (true) {
            image.fillAmount = 1 - ability.currentCooldown / ability.maxCooldown;

            if (ability.currentCooldown == ability.maxCooldown) break;

            yield return null;
        }
    }



    // 레벨 업
    public void LevelUp() {
        // 경험치 및 레벨 수치 적용
        CurrentExp -= MaxExp[level];
        Level += 1;

        // TODO: 레벨 업 효과

    }

    
    
    // 회복
    public void Heal(float amount) {
        if (amount < 0) amount = 0;

        // 현재 체력 수치 적용
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }

    // 피해
    public void Damage(float amount) {
        if (amount < 0) amount = 0;



        // 현재 체력 수치 적용
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        // 파티클 재생
        Vector3 particlePosition = transform.position;
        particlePosition.y += 1.0f;
        GameObject particle = Instantiate(bloodParticle, particlePosition, transform.rotation);
        Destroy(particle, 1.0f);

        // 효과음 재생
        source.clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        // 체력이 0 이하일 경우
        if (CurrentHealth <= 0) Die(); // 사망
    }
    
    // 사망
    public void Die() {
        IsDead = true;

        // 효과음 재생
        source.clip = deathSound;
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();

        // 애니메이션 재생
        animator.SetBool("isDead", true);

        // 플레이어 사망 이벤트 호출
        PlayerDeath?.Invoke();
    }

}
