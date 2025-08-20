using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Creep : MonoBehaviour, ILivingEntity, IMonster {

    // Event
    public Action CreepDeath;



    // Status
    private float maxHealth;
    public float MaxHealth {

        get {
            return maxHealth;
        }

        set {
            if (value < 1) value = 1;

            maxHealth = value;
            if (CurrentHealth < maxHealth) CurrentHealth = maxHealth;

            // UI 갱신
            hpFill.fillAmount = CurrentHealth / maxHealth;
            hpText.text = (int) CurrentHealth + " / " + (int) maxHealth;
        }

    }
    private float currentHealth;
    public float CurrentHealth {
        
        get {
            return currentHealth;
        }

        set {
            if (value < 0) value = 0;

            currentHealth = value > MaxHealth ? MaxHealth : value;

            // UI 갱신
            hpFill.fillAmount = CurrentHealth / maxHealth;
            hpText.text = (int) CurrentHealth + " / " + (int) maxHealth;

            if (currentHealth <= MaxHealth / 2) {
                animator.SetBool("isCrouch", true);
                MoveSpeed = 6;
            }

            if (currentHealth == 0) Die();
        }

    }
    private float moveSpeed;
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

        protected set {
            isDead = value;
        }

    }
    private bool isAttacking;
    public bool IsAttacking {

        get {
            return isAttacking;
        }

        protected set {
            isAttacking = value;
        }

    }



    // Option
    private float detectDistance;
    public float DetectDistance {
        
        get {
            return detectDistance;
        }

        set {
            detectDistance = value;
        }

    }
    private float attackDistance;
    public float AttackDistance {
        
        get {
            return attackDistance;
        }

        set {
            attackDistance = value;
        }
    }



    // Object
    private GameObject target;
    public GameObject Target {

        get {
            return target;
        }

        set {
            target = value;
        }

    }
    public GameObject camera;
    public GameObject bloodParticle;
    [Header("Jump Up Effect")]
    public GameObject jumpUpEffect;
    public Transform jumpUpEffectTransform;
    public AudioClip jumpUpSound;
    [Header("Jump Down Effect")]
    public GameObject jumpDownEffect;
    public Transform jumpDownEffectTransform;
    public AudioClip jumpDownSound;
    [Header("Flamethrower Effect")]
    public GameObject flamethrowerEffect;
    public Transform flamethrowerEffectTransform;
    [Header("Lightning Strike Effect")]
    public GameObject lightningStrikeEffect;
    public Transform lightningStrikeEffectTransform;
    [Header("")]
    public Image hpFill;
    public Text hpText;
    public AudioClip deathSound;
    public AudioClip[] punchSounds = new AudioClip[2];
    public AudioClip[] eatSounds = new AudioClip[3];



    public Animator animator { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }
    public AudioSource source { get; private set; }





    private void Start() {
        StatusInit();
        OptionInit();

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();

        Target = GameObject.Find("Player");



        StartCoroutine(JumpDown());
    }


    private void Update() {

        // 크립이 사망했을 경우
        if (IsDead) return;

        // 공격 중일 경우
        if (IsAttacking) return;

        // 감지
        Detect(Target);

    }





    // 스테이터스 초기화
    protected virtual void StatusInit() {
        MaxHealth = 1200 /*- 200*/;
        CurrentHealth = MaxHealth;
        MoveSpeed = 2.5f;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = true;
    }

    // 옵션 초기화
    protected virtual void OptionInit() {
        DetectDistance = 500;
        AttackDistance = 20;
    }





    // 감지
    public void Detect(GameObject target) {
        Transform targetTransform = target.transform;
        float distance = Vector3.Distance(targetTransform.position, transform.position);

        ILivingEntity targetLivingEntity = target.GetComponent<ILivingEntity>();

        animator.SetFloat("moveSpeed", 0);

        // 플레이어가 살아있을 경우
        if (!targetLivingEntity.IsDead) {

            // 플레이어가 감지 거리 내에 있을 경우
            if (detectDistance >= distance && distance > attackDistance) {
                Chase(targetTransform); // 추격
            }

            // 플레이어가 공격 거리 내에 있을 경우
            else if (distance <= attackDistance) {
                if (!animator.GetBool("isCrouch")) {
                    if (4 >= distance) {
                        Attack(target, 0);
                    }
                    else if (8 >= distance && distance > 4) {
                        Chase(targetTransform); // 추격
                    }
                    else if (12 >= distance && distance > 8) {
                        int randomNumber = UnityEngine.Random.Range(0, 1000);
                        if (3 > randomNumber && randomNumber >= 0) {
                            Attack(target, 2);
                        }
                        else {
                            Chase(targetTransform); // 추격
                        }
                    }
                    else if (distance > 12) {
                        Attack(target, 1);
                    }
                }
                else {
                    if (4 >= distance) {
                        Attack(target, 3);
                    }
                    else if (8 >= distance && distance > 4) {
                        int randomNumber = UnityEngine.Random.Range(0, 1000);
                        if (2 > randomNumber && randomNumber >= 0) {
                            Attack(target, 4);
                        }
                        else {
                            Chase(targetTransform); // 추격
                        }
                    }
                    else if (12 >= distance && distance > 8) {
                        Chase(targetTransform); // 추격
                    }
                    else if (distance > 12) {
                        Chase(targetTransform); // 추격
                    }
                }
            }

            // 플레이어가 근처에 없을 경우
            else {
                Idle(); // 운휴
            }

        }

        // 플레이어가 죽었을 경우
        else {
            Chase(targetTransform); // 추격
        }
    }


    // 추격
    public void Chase(Transform targetTransform) {
        animator.SetFloat("moveSpeed", MoveSpeed > 2 ? 1.0f : 0.5f);
            
        navMeshAgent.speed = MoveSpeed;
        navMeshAgent.SetDestination(targetTransform.position);
    }
    
    
    // 공격
    public void Attack(GameObject target) {
        StartCoroutine(CAttack(target, 0));
    }

    public void Attack(GameObject target, int number) {
        StartCoroutine(CAttack(target, number));
    }

    private IEnumerator CAttack(GameObject target, int number) {
        // 추격 대상 설정
        navMeshAgent.speed = 0.1f;
        Transform targetTransform = target.transform;
        navMeshAgent.SetDestination(targetTransform.position);

        switch (number) {
            case 0:
                StartCoroutine(Punch());
                break;
            case 1:
                StartCoroutine(JumpUp());
                break;
            case 2:
                StartCoroutine(Roar());
                break;
            case 3:
                StartCoroutine(Eat());
                break;
            case 4:
                StartCoroutine(Sniff());
                break;
        }

        yield return null;
    }


    // 운휴
    public void Idle() {
        animator.SetFloat("moveSpeed", 0);

        navMeshAgent.speed = MoveSpeed;
        navMeshAgent.SetDestination(transform.position);
    }




    // Punch
    private IEnumerator Punch() {
        IsAttacking = true; // 공격 시작

        // 애니메이션 재생
        animator.SetBool("isPunch", true);

        // 회전
        transform.LookAt(Target.transform);

        // 효과음 재생
        source.clip = punchSounds[0];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        float time = 0.53f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = punchSounds[1];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();

        // 대상 공격
        ILivingEntity targetLivingEntity = Target.GetComponent<ILivingEntity>();
        targetLivingEntity.Damage(40);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 애니메이션 종료
        animator.SetBool("isPunch", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        IsAttacking = false; // 공격 종료
    }
    
    // JumpUp
    private IEnumerator JumpUp() {
        isAttacking = true; // 공격 시작

        // 애니메이션 재생
        animator.SetBool("isJumpUp", true);



        float time = 0.47f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 파티클 생성
        Vector3 jumpUpEffectPosition = jumpUpEffectTransform.position;
        Quaternion jumpUpEffectRotation = jumpUpEffectTransform.rotation;
        GameObject jumpUpEffectParticle = Instantiate(jumpUpEffect, jumpUpEffectPosition, jumpUpEffectRotation);
        JumpUpEffect jumpUpEffectScript = jumpUpEffectParticle.GetComponent<JumpUpEffect>();
        jumpUpEffectScript.size = 2;
        Destroy(jumpUpEffectParticle, 1);

        // 충돌체 제거
        GetComponent<Collider>().enabled = false;

        // 효과음 재생
        AudioSource jumpUpSource = jumpUpEffectParticle.GetComponent<AudioSource>();
        jumpUpSource.clip = jumpUpSound;
        jumpUpSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        jumpUpSource.Play();



        time = 0.6f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 애니메이션 종료
        animator.SetBool("isJumpUp", false);



        time = 0.25f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 위치 조정
        Vector3 thisPosition = transform.position;
        Vector3 targetPosition = Target.transform.position;
        transform.position = new Vector3(targetPosition.x, thisPosition.y, targetPosition.z);



        time = 0.25f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        StartCoroutine(JumpDown());
    }

    // JumpDown
    private IEnumerator JumpDown() {
        // 애니메이션 재생
        animator.SetBool("isJumpDown", true);



        float time = 0.67f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 충돌체 생성
        GetComponent<Collider>().enabled = true;

        // 파티클 생성
        Vector3 jumpDownEffectPosition = jumpDownEffectTransform.position;
        Quaternion jumpDownEffectRotation = jumpDownEffectTransform.rotation;
        GameObject jumpDownEffectParticle = Instantiate(jumpDownEffect, jumpDownEffectPosition, jumpDownEffectRotation);
        JumpDownEffect jumpDownEffectScript = jumpDownEffectParticle.GetComponent<JumpDownEffect>();
        jumpDownEffectScript.size = 2;
        jumpDownEffectScript.caster = gameObject;
        jumpDownEffectScript.damage = 30;
        Destroy(jumpDownEffectParticle, 1);

        // 효과음 재생
        AudioSource jumpDownSource = jumpDownEffectParticle.GetComponent<AudioSource>();
        jumpDownSource.clip = jumpDownSound;
        jumpDownSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        jumpDownSource.Play();

        // 카메라 흔들림 효과 부여
        Transform cameraTransform = camera.transform;
        Vector3 cameraPosition = cameraTransform.position;
        cameraTransform.position = new Vector3(cameraPosition.x, cameraPosition.y + 1.5f, cameraPosition.z);
        CoroutineUtility.CallWaitForSeconds(0.25f, () => {
            cameraPosition = cameraTransform.position;
            cameraTransform.position = new Vector3(cameraPosition.x, cameraPosition.y + 0.75f, cameraPosition.z);
        });



        time = 0.4f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 애니메이션 종료
        animator.SetBool("isJumpDown", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;
        
        isAttacking = false; // 공격 종료
    }

    // Roar
    private IEnumerator Roar() {
        isAttacking = true; // 공격 시작

        // 애니메이션 재생
        animator.SetBool("isRoar", true);



        float time = 0.305f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 회전
        transform.LookAt(Target.transform);



        time = 0.3f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 파티클 생성
        Vector3 flamethrowerEffectPosition = flamethrowerEffectTransform.position;
        Quaternion flamethrowerEffectRotation = flamethrowerEffectTransform.rotation;
        GameObject flamethrowerEffectParticle = Instantiate(flamethrowerEffect, flamethrowerEffectPosition, flamethrowerEffectRotation);
        FlamethrowerEffect flamethrowerEffectScript = flamethrowerEffectParticle.GetComponent<FlamethrowerEffect>();
        flamethrowerEffectScript.size = 6;
        flamethrowerEffectScript.caster = gameObject;
        flamethrowerEffectScript.damage = 30;
        Destroy(flamethrowerEffectParticle, 2);



        time = 1.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 애니메이션 종료
        animator.SetBool("isRoar", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        isAttacking = false; // 공격 종료
    }

    // Bite
    private void Bite() {
        
    }

    // Eat
    private IEnumerator Eat() {
        IsAttacking = true; // 공격 시작

        // 애니메이션 재생
        animator.SetBool("isEat", true);

        // 회전
        transform.LookAt(Target.transform);



        float time = 0.205f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = eatSounds[0];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();

        // 대상 공격
        ILivingEntity targetLivingEntity = Target.GetComponent<ILivingEntity>();
        targetLivingEntity.Damage(50);



        time = 0.7f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = eatSounds[1];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        time = 0.7f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = eatSounds[2];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        time = 1;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 애니메이션 종료
        animator.SetBool("isEat", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        IsAttacking = false; // 공격 종료
    }

    // Sniff
    private IEnumerator Sniff() {
        isAttacking = true; // 공격 시작

        // 애니메이션 시작
        animator.SetBool("isSniff", true);



        float time = 1.105f;
        yield return new WaitForSeconds(time);

        int childCount = lightningStrikeEffectTransform.childCount;
        for (int i = 0; i < childCount; i++) {
            if (IsDead) yield break;

            // 파티클 생성
            Transform child = lightningStrikeEffectTransform.GetChild(i);
            Vector3 lightningStrikeEffectPosition = child.position;
            Quaternion lightningStrikeEffectRotation = child.rotation;
            GameObject lightningStrikeEffectParticle = Instantiate(lightningStrikeEffect, lightningStrikeEffectPosition, lightningStrikeEffectRotation);
            LightningStrikeEffect lightningStrikeEffectScript = lightningStrikeEffectParticle.GetComponent<LightningStrikeEffect>();
            lightningStrikeEffectScript.size = 2;
            lightningStrikeEffectScript.caster = gameObject;
            lightningStrikeEffectScript.damage = 30;
            Destroy(lightningStrikeEffectParticle, 1);

            time = 0.75f / childCount;
            yield return new WaitForSeconds(time);
        }



        time = 0.25f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 애니메이션 종료
        animator.SetBool("isSniff", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        isAttacking = false; // 공격 종료
    }





    // 회복
    public void Heal(float amount) {
        if (amount < 0) return;

        // 현재 체력 수치 적용
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }


    // 피해
    public void Damage(float amount, bool isGenerateBloodEffect = true) {
        if (amount < 0) return;



        // 현재 체력 수치 적용
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        if (isGenerateBloodEffect) {
            // 파티클 재생
            Vector3 particlePosition = transform.position;
            particlePosition.y += 3.0f;
            GameObject particle = Instantiate(bloodParticle, particlePosition, transform.rotation);
            Destroy(particle, 1.0f);
        }



        // 체력이 0 이하일 경우
        if (CurrentHealth <= 0) Die(); // 사망
    }
    
    
    // 사망
    public void Die() {
        IsDead = true;

        // 충돌체 제거
        GetComponent<Collider>().enabled = false;

        // 애니메이션 재생
        animator.SetBool("isDead", true);

        navMeshAgent.speed = 0.0f;

        // 효과음 재생
        source.clip = deathSound;
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();

        // Creep 사망 이벤트 호출
        CreepDeath?.Invoke();

        // 객체 제거
        Destroy(gameObject, 10.0f);
    }
}
