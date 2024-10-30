using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Venom : MonoBehaviour, ILivingEntity, IMonster {
	
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

            currentHealth = value > maxHealth ? maxHealth : value;

            // TODO: UI 갱신


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
    private float attackPower;
    public float AttackPower {
        
        get {
            return attackPower;
        }

        set {
            attackPower = value;
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
    private float attackSpeed = 1.0f;
    public float AttackSpeed {

        get {
            return attackSpeed;
        }

        set {
            attackSpeed = value;
        }

    }
    private float attackTime = 0.3f;
    public float AttackTime {

        get {
            return attackTime;
        }

        set {
            attackTime = value;
        }

    }
    private float detectDistance = 100.0f;
    public float DetectDistance {
        
        get {
            return detectDistance;
        }

        set {
            detectDistance = value;
        }

    }
    private float attackDistance = 1.5f;
    public float AttackDistance {
        
        get {
            return attackDistance;
        }

        set {
            attackDistance = value;
        }
    }
    public float particleLivingTime { get; set; } = 1.0f;



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
    public GameObject exp;
    public GameObject bloodParticle;
    public GameObject venomExplosionParticle;

    public AudioClip[] growlSounds;
    public AudioClip deathSound;



    public Animator animator { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }
    public AudioSource source { get; private set; }





    private void Start() {
        StatusInit();

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();

        StartCoroutine(CMakeSound_Growl(source, growlSounds)); // 좀비 으르렁 사운드 재생
    }


    private void Update() {

        // 좀비가 사망했을 경우
        if (IsDead) return;

        // 좀비가 공격 중일 경우
        if (IsAttacking) return;

        // 감지
        Detect(Target);

    }

    

    
    
    // 스테이터스 초기화
    protected virtual void StatusInit() {
        MaxHealth = 30.0f;
        CurrentHealth = MaxHealth;
        MoveSpeed = 2.0f;
        AttackPower = 20.0f;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;
    }
    
    
    // 으르렁 사운드 재생
    private IEnumerator CMakeSound_Growl(AudioSource speaker, AudioClip[] growlSound) {
        while (!IsDead) {
            speaker.clip = growlSound[Random.Range(0, growlSound.Length)];
            speaker.volume = PlayerPrefs.GetFloat("Option_SEVolume") / 8;
            speaker.Play();

            yield return new WaitForSeconds(Random.Range(3.0f, 10.0f));
        }
    }
    
    
    // 감지
    public void Detect(GameObject target) {
        Transform targetTransform = target.transform;
        float distance = Vector3.Distance(targetTransform.position, transform.position);

        ILivingEntity targetLivingEntity = target.GetComponent<ILivingEntity>();

        // 플레이어가 살아있을 경우
        if (!targetLivingEntity.IsDead) {

            // 플레이어가 감지 거리 내에 있을 경우
            if (detectDistance >= distance && distance > attackDistance) {
                Chase(targetTransform); // 추격
            }

            // 플레이어가 공격 거리 내에 있을 경우
            else if (distance <= attackDistance) {
                Attack(target); // 공격
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
        StartCoroutine(CAttack(target));
    }

    private IEnumerator CAttack(GameObject target) {
        IsAttacking = true;

        // 애니메이션 재생
        animator.SetFloat("moveSpeed", 0);

        // 추격 대상 설정
        navMeshAgent.speed = 0.1f;
        Transform targetTransform = target.transform;
        navMeshAgent.SetDestination(targetTransform.position);



        float time = attackTime / attackSpeed / 2;
        yield return new WaitForSeconds(time);

        // 대상 공격
        Vector3 position = transform.position;
        position.y += 0.5f;
        GameObject particle = Instantiate(venomExplosionParticle, position, transform.rotation);
        VenomExplosion venomExplosion = particle.GetComponent<VenomExplosion>();
        venomExplosion.caster = gameObject;
        venomExplosion.damage = AttackPower;
        Destroy(particle, particleLivingTime);
        Destroy(gameObject);
    }


    // 운휴
    public void Idle() {
        animator.SetFloat("moveSpeed", 0);

        navMeshAgent.speed = MoveSpeed;
        navMeshAgent.SetDestination(transform.position);
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
        particlePosition.y += 1.0f;
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
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume") / 8;
        source.Play();

        // 경험치 떨구기
        Vector3 position = transform.position;
        position.y += 1.0f;
        Instantiate(exp, position, transform.rotation);

        // 객체 제거
        Destroy(gameObject, 2.0f);
    }

}
