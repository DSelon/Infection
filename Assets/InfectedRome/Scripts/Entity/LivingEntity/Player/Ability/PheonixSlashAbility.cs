using System;
using System.Collections;
using UnityEngine;

public class PheonixSlashAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5;
    public float currentCooldown { get; set; }
    public float damage { get; set; } = 50;



    // Option
    public float operatingSpeed { get; set; } = 1;
    public float operatingTime { get; set; } = 2;
    public float castingTime { get; set; } = 0.8f;
    public float[] particleLivingTimes { get; set; } = { 1, 2.5f };



    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject[] skillEffects = new GameObject[2];
    public AudioClip swordSwingSound;



    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool radiusTree = false;
    [NonSerialized] public bool damageReductionTree = false;





    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {
        if (Time.timeScale == 0) return;
        
        currentCooldown = currentCooldown < maxCooldown ? currentCooldown + Time.deltaTime * (cooldownTree ? 1.4f : 1) : maxCooldown;
    }



    public void UseAbility(Player player) {

        // 재사용 대기 시간일 경우
        if (currentCooldown < maxCooldown) return;

        // 다른 능력이 동작 중일 경우
        if (player.IsOperating) return;

        currentCooldown = 0;

        StartCoroutine(CUseAbility_RunAnimation(player));
        StartCoroutine(CUseAbility_GenerateParticle(player));
        StartCoroutine(CUseAbility_MakeSound(player));

    }
    
    public IEnumerator CUseAbility_RunAnimation(Player player) {
        player.IsOperating = true;
        player.DamageReduction = damageReductionTree ? 0.8f : 0;

        player.animator.SetBool("isUsingAbility_PheonixSlash", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_PheonixSlash", false);

        player.DamageReduction = 0;
        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;



        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        Vector3 position = playerTransform.transform.position;
        position.y += 1.5f;

        // 파티클 재생
        GameObject firstParticle = Instantiate(skillEffects[0], position, playerTransform.transform.rotation);
        PheonixSlash01Effect pheonixSlash01Effect = firstParticle.GetComponent<PheonixSlash01Effect>();
        pheonixSlash01Effect.size += radiusTree ? pheonixSlash01Effect.size * 0.15f : 0;
        StartCoroutine(CUseAbility_UpdateParticlePosition(firstParticle.transform, playerTransform));
        Destroy(firstParticle, particleLivingTimes[0]);

        // 파티클 재생
        GameObject secondParticle = Instantiate(skillEffects[1], position, playerTransform.transform.rotation);
        PheonixSlash02Effect pheonixSlash02Effect = secondParticle.GetComponent<PheonixSlash02Effect>();
        pheonixSlash02Effect.size += radiusTree ? pheonixSlash02Effect.size * 0.15f : 0;
        pheonixSlash02Effect.caster = player.gameObject;
        pheonixSlash02Effect.damage = damage;
        Destroy(secondParticle, particleLivingTimes[1]);



        time = 1.0f;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
        secondParticle.GetComponent<BoxCollider>().enabled = false;
    }

    public IEnumerator CUseAbility_UpdateParticlePosition(Transform particleTransform, Transform playerTransform) {
        while (particleTransform != null) {
            Vector3 particlePosition = particleTransform.position;
            Vector3 playerPosition = playerTransform.position;
            particleTransform.position = new Vector3(playerPosition.x, particlePosition.y, playerPosition.z);

            yield return null;
        }
    }

    public IEnumerator CUseAbility_MakeSound(Player player) {
        AudioSource audioSource = player.Sword.GetComponent<AudioSource>();



        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 효과음 재생
        audioSource.clip = swordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();
    }
}
