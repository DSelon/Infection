using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ILivingEntity {

    [Header("Status")]
    [SerializeField] private float maxHealth = 100.0f;
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
            PlayerInfo_hp_fill.fillAmount = CurrentHealth / maxHealth;
            PlayerInfo_hp_text.text = (int) CurrentHealth + " / " + (int) maxHealth;
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
            PlayerInfo_hp_fill.fillAmount = currentHealth / MaxHealth;
            PlayerInfo_hp_text.text = (int) currentHealth + " / " + (int) MaxHealth;

            if (currentHealth == 0) Die();
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
            PlayerInfo_level_text.text = (level + 1).ToString();
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
            PlayerInfo_exp_fill.fillAmount = (float) currentExp / maxExp[level];
            PlayerInfo_exp_text.text = currentExp + " / " + maxExp[level];

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
            PlayerInfo_exp_fill.fillAmount = (float) currentExp / maxExp[level];
            PlayerInfo_exp_text.text = currentExp + " / " + maxExp[level];

            // 레벨 업
            if (currentExp >= MaxExp[level]) LevelUp();
        }
    }
    [SerializeField] private float moveSpeed = 5.0f;
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
    


    [Header("Option")]
    [SerializeField] private float cameraFollowSpeed = 5.0f;
    public float CameraFollowSpeed {

        get {
            return cameraFollowSpeed;
        }

        set {
            cameraFollowSpeed = value;
        }

    }

    // Player Movement
    [SerializeField] private float rotateSpeed = 50.0f;
    public float RotateSpeed {

        get {
            return rotateSpeed;
        }

        set {
            rotateSpeed = value;
        }

    }
    


    [Header("Object")]
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

    // UI
    [SerializeField] private Text playerInfo_level_text;
    public Text PlayerInfo_level_text {

        get {
            return playerInfo_level_text;
        }

        private set {
            playerInfo_level_text = value;
        }

    }
    [SerializeField] private Image playerInfo_hp_fill;
    public Image PlayerInfo_hp_fill {
        
        get {
            return playerInfo_hp_fill;
        }

        private set {
            playerInfo_hp_fill = value;
        }

    }
    [SerializeField] private Text playerInfo_hp_text;
    public Text PlayerInfo_hp_text {

        get {
            return playerInfo_hp_text;
        }

        private set {
            playerInfo_hp_text = value;
        }

    }
    [SerializeField] private Image playerInfo_exp_fill;
    public Image PlayerInfo_exp_fill {

        get {
            return playerInfo_exp_fill;
        }

        private set {
            playerInfo_exp_fill = value;
        }

    }
    [SerializeField] private Text playerInfo_exp_text;
    public Text PlayerInfo_exp_text {

        get {
            return playerInfo_exp_text;
        }

        set {
            playerInfo_exp_text = value;
        }

    }
    [SerializeField] private GameObject abilityCoolGaugeBundle;
    public GameObject AbilityCoolGaugeBundle {

        get {
            return abilityCoolGaugeBundle;
        }

        private set {
            abilityCoolGaugeBundle = value;
        }

    }
    
    // Sound
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
    public Image[] abilityCoolGaugeBundle_gauge_fills { get; set; } = new Image[4];





    private void Start() {
        CurrentHealth = MaxHealth;
        Level = 0;
        CurrentExp = 0;
        IsDead = MaxHealth > 0 ? false : true;
        


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

        Transform coolGaugeBundleTransform = AbilityCoolGaugeBundle.transform;
        int coolGaugeBundleChildCount = coolGaugeBundleTransform.childCount;
        for (int i = 0; i < coolGaugeBundleChildCount; i++) abilityCoolGaugeBundle_gauge_fills[i] = coolGaugeBundleTransform.GetChild(i).GetChild(2).GetComponent<Image>();
    }

    private void Update() {
        // 시간이 흐르지 않을 경우
        if (Time.timeScale == 0) return;



        // 카메라 추적
        movement.FollowCamera(this, Camera.transform.position);

        // 플레이어가 사망했을 경우
        if (IsDead) return;



        // 이동
        movement.Move(this, input.horizontal, input.vertical);

        // 능력 사용
        if (input.useAbility[0]) UseAbility(0);
    }




    
    // 능력 사용
    public void UseAbility(int number) {
        equippedAbilities[number].UseAbility(this);

        StartCoroutine(CUseAbility_RunCooldownUI(number));
    }
    
    private IEnumerator CUseAbility_RunCooldownUI(int number) {
        IAbility ability = equippedAbilities[number];
        Image image = abilityCoolGaugeBundle_gauge_fills[number];
        while (true) {
            image.fillAmount = 1 - ability.currentCooldown / ability.cooldown;

            if (ability.currentCooldown == ability.cooldown) break;

            yield return null;
        }
    }



    // 레벨 업
    public void LevelUp() {
        CurrentExp -= MaxExp[level];
        Level += 1;

        // TODO: 레벨 업 효과
    }

    
    
    // 회복
    public void Heal(float amount) {
        if (amount < 0) amount = 0;



        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }


    // 피해
    public void Damage(float amount) {
        if (amount < 0) amount = 0;



        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        Vector3 particlePosition = transform.position;
        particlePosition.y += 1.0f;
        GameObject particle = Instantiate(bloodParticle, particlePosition, transform.rotation);
        Destroy(particle, 1.0f);

        source.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        // 체력이 0 이하일 경우
        if (CurrentHealth <= 0) Die(); // 사망
    }
    

    // 사망
    public void Die() {
        IsDead = true;

        source.clip = deathSound;
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();

        animator.SetBool("isDead", true);
    }

}
