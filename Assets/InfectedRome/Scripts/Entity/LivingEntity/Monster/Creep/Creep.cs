using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private GameObject target;
    public GameObject Target {

        get {
            return target;
        }

        set {
            target = value;
        }

    }
    public GameObject bloodParticle;
    public GameObject jumpUpEffect;
    public GameObject jumpDownEffect;
    public GameObject roarEffect;
    public Transform roarTransform;
    public GameObject sniffEffect;
    public Transform sniffTransform;
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
        MaxHealth = 100.0f;
        CurrentHealth = MaxHealth;
        MoveSpeed = 2.5f;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = true;
    }

    // 옵션 초기화
    protected virtual void OptionInit() {
        DetectDistance = 100.0f;
        AttackDistance = 20.0f;
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
                    else if (10 >= distance && distance > 4) {
                        Chase(targetTransform); // 추격
                    }
                    else if (12 >= distance && distance > 10) {
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
        IsAttacking = true;

        // 애니메이션 재생
        animator.SetBool("isPunch", true);

        // 추격 대상 설정
        navMeshAgent.speed = 0.1f;
        Transform targetTransform = Target.transform;
        navMeshAgent.SetDestination(targetTransform.position);

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
        targetLivingEntity.Damage(15);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        animator.SetBool("isPunch", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        IsAttacking = false;
    }
    
    // JumpUp
    private IEnumerator JumpUp() {
        isAttacking = true;
        animator.SetBool("isJumpUp", true);



        float time = 0.47f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        Vector3 position = transform.position;
        position.y += 0.1f;
        Quaternion rotation = jumpUpEffect.transform.rotation;
        GameObject effect = Instantiate(jumpUpEffect, position, rotation);
        Destroy(effect, 1);

        // 충돌체 제거
        GetComponent<Collider>().enabled = false;



        time = 0.6f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

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
        animator.SetBool("isJumpDown", true);

        float time = 0.67f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 충돌체 생성
        GetComponent<Collider>().enabled = true;

        Vector3 position = transform.position;
        position.y += 0.1f;
        Quaternion rotation = jumpDownEffect.transform.rotation;
        GameObject firstEffect = Instantiate(jumpDownEffect, position, rotation);
        JumpDownEffect effect = firstEffect.GetComponent<JumpDownEffect>();
        effect.caster = gameObject;
        effect.damage = 30;
        Destroy(firstEffect, 1);

        time = 0.4f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        animator.SetBool("isJumpDown", false);

        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;
        
        isAttacking = false;
    }

    // Roar
    private IEnumerator Roar() {
        isAttacking = true;
        animator.SetBool("isRoar", true);



        float time = 0.605f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        GameObject firstEffect = Instantiate(roarEffect, roarTransform.position, roarTransform.rotation);
        RoarEffect effect = firstEffect.GetComponent<RoarEffect>();
        effect.caster = gameObject;
        effect.damage = 30;
        Destroy(firstEffect, 2);



        time = 1.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        animator.SetBool("isRoar", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        isAttacking = false;
    }

    // Bite
    private void Bite() {
        
    }

    // Eat
    private IEnumerator Eat() {
        IsAttacking = true;

        // 애니메이션 재생
        animator.SetBool("isEat", true);

        // 추격 대상 설정
        navMeshAgent.speed = 0.1f;
        Transform targetTransform = Target.transform;
        navMeshAgent.SetDestination(targetTransform.position);



        float time = 0.41f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = eatSounds[0];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();

        // 대상 공격
        ILivingEntity targetLivingEntity = Target.GetComponent<ILivingEntity>();
        targetLivingEntity.Damage(15);



        time = 1.4f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = eatSounds[1];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        time = 1.4f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        // 효과음 재생
        source.clip = eatSounds[2];
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        source.Play();



        time = 2;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        animator.SetBool("isEat", false);



        time = 1;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        IsAttacking = false;
    }

    // Sniff
    private IEnumerator Sniff() {
        isAttacking = true;
        animator.SetBool("isSniff", true);



        float time = 1.105f;
        yield return new WaitForSeconds(time);

        int childCount = sniffTransform.childCount;
        for (int i = 0; i < childCount; i++) {
            if (IsDead) yield break;

            Transform child = sniffTransform.GetChild(i);
            Vector3 position = child.transform.position;
            Quaternion rotation = sniffEffect.transform.rotation;

            GameObject firstEffect = Instantiate(sniffEffect, position, rotation);
            SniffEffect effect = firstEffect.GetComponent<SniffEffect>();
            effect.caster = gameObject;
            effect.damage = 20;
            Destroy(firstEffect, 1);

            time = 0.75f / childCount;
            yield return new WaitForSeconds(time);
        }



        time = 0.25f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        animator.SetBool("isSniff", false);



        time = 0.5f;
        yield return new WaitForSeconds(time);

        if (IsDead) yield break;

        isAttacking = false;
    }





    // 회복
    public void Heal(float amount) {
        if (amount < 0) return;

        // 현재 체력 수치 적용
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }


    // 피해
    public void Damage(float amount) {
        if (amount < 0) return;



        // 현재 체력 수치 적용
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        // 파티클 재생
        Vector3 particlePosition = transform.position;
        particlePosition.y += 3.0f;
        GameObject particle = Instantiate(bloodParticle, particlePosition, transform.rotation);
        Destroy(particle, 1.0f);



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
