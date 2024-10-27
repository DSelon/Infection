using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, ILivingEntity {
	
    [Header("Status")]
    [SerializeField] private float maxHealth = 50.0f;
    public float MaxHealth {

        get {
            return maxHealth;
        }

        set {
            if (value < 1) value = 1;

            maxHealth = value;
            if (CurrentHealth < maxHealth) CurrentHealth = maxHealth;

            // TODO: UI 갱신

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
    [SerializeField] private float moveSpeed = 3.5f;
    public float MoveSpeed {

        get {
            return moveSpeed;
        }

        set {
            moveSpeed = value;
        }

    }
    [SerializeField] private float power = 10.0f;
    public float Power {
        
        get {
            return power;
        }

        set {
            power = value;
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
    private bool isAttacking;
    public bool IsAttacking {

        get {
            return isAttacking;
        }

        private set {
            isAttacking = value;
        }

    }
    


    [Header("Option")]
    [SerializeField] private float attackSpeed = 1.0f;
    public float AttackSpeed {

        get {
            return attackSpeed;
        }

        set {
            attackSpeed = value;
        }

    }
    [SerializeField] private float attackTime = 0.3f;
    public float AttackTime {

        get {
            return attackTime;
        }

        set {
            attackTime = value;
        }

    }
    [SerializeField] private float detectDistance = 30.0f;
    public float DetectDistance {
        
        get {
            return detectDistance;
        }

        set {
            detectDistance = value;
        }

    }
    [SerializeField] private float attackDistance = 1.0f;
    public float AttackDistance {
        
        get {
            return attackDistance;
        }

        set {
            attackDistance = value;
        }
    }



    [Header("Object")]
    public GameObject target;
    public GameObject exp;
    public GameObject bloodParticle;

    public AudioClip[] growlSounds;
    public AudioClip deathSound;



    public Animator animator { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }
    public AudioSource source { get; private set; }





    private void Start() {
        CurrentHealth = MaxHealth;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();


        
        StartCoroutine(CMakeSound_Growl(source, growlSounds));
    }


    private void Update() {
        // 좀비가 사망했을 경우
        if (IsDead) return;

        // 좀비가 공격 중일 경우
        if (IsAttacking) return;



        // 감지
        Detect(target);
    }

    

    
    
    private IEnumerator CMakeSound_Growl(AudioSource speaker, AudioClip[] growlSound) {
        while (!IsDead) {
            speaker.clip = growlSound[Random.Range(0, growlSound.Length)];
            speaker.volume = PlayerPrefs.GetFloat("Option_SEVolume") / 8;
            speaker.Play();

            yield return new WaitForSeconds(Random.Range(3.0f, 10.0f));
        }
    }
    
    
    // 감지
    private void Detect(GameObject target) {
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
                StartCoroutine(CAttack(target)); // 공격
            }

            // 플레이어가 근처에 없을 경우
            else {
                Idle(); // 운휴
            }

        }

        // 플레이어가 죽었을 경우
        else {
            Idle(); // 운휴
        }
    }


    // 추격
    private void Chase(Transform targetTransform) {
        animator.SetFloat("moveSpeed", MoveSpeed > 2 ? 1.0f : 0.5f);
            
        navMeshAgent.speed = MoveSpeed;
        navMeshAgent.SetDestination(targetTransform.position);
    }
    
    
    // 공격
    private IEnumerator CAttack(GameObject target) {
        IsAttacking = true;

        animator.SetBool("isAttacking", true);
        animator.SetFloat("attackingSpeed", attackSpeed);
        animator.SetFloat("moveSpeed", 0);

        navMeshAgent.speed = 0.1f;
        Transform targetTransform = target.transform;
        navMeshAgent.SetDestination(targetTransform.position);



        float time = attackTime / attackSpeed / 2;
        yield return new WaitForSeconds(time);

        ILivingEntity targetLivingEntity = target.GetComponent<ILivingEntity>();
        targetLivingEntity.Damage(10);

        time = attackTime / attackSpeed / 2;
        yield return new WaitForSeconds(time);



        animator.SetBool("isAttacking", false);

        IsAttacking = false;
    }


    // 운휴
    private void Idle() {
        animator.SetFloat("moveSpeed", 0);

        navMeshAgent.speed = MoveSpeed;
        navMeshAgent.SetDestination(transform.position);
    }
    




    // 회복
    public void Heal(float amount) {
        if (amount < 0) return;



        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }


    // 피해
    public void Damage(float amount) {
        if (amount < 0) return;



        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

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

        GetComponent<Collider>().enabled = false;
        animator.SetBool("isDead", true);

        navMeshAgent.speed = 0.0f;

        source.clip = deathSound;
        source.volume = PlayerPrefs.GetFloat("Option_SEVolume") / 8;
        source.Play();

        Vector3 position = transform.position;
        position.y += 1.0f;
        Instantiate(exp, position, transform.rotation);

        Destroy(gameObject, 2.0f);
    }
}
