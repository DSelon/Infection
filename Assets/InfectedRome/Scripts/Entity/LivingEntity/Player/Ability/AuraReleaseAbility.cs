using System;
using System.Collections;
using UnityEngine;

public class AuraReleaseAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5;
    public float currentCooldown { get; set; }
    public float damage { get; set; } = 50;



    // Option
    public float operatingSpeed { get; set; } = 1;
    public float operatingTime { get; set; } = 2.15f;
    public float castingTime { get; set; } = 1.4f;
    public float chargeTime { get; set; } = 0.5f;
    public float[] particleLivingTimes { get; set; } = { 1, 1 };



    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject[] skillEffects = new GameObject[2];
    public AudioClip[] swordSwingSounds = new AudioClip[2];



    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool radiusTree = false;
    [NonSerialized] public bool damageReductionTree = false;





    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {

        // 시간이 흐르지 않을 경우
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
        player.DamageReduction = damageReductionTree ? 0.4f : 0;

        player.animator.SetBool("isUsingAbility_AuraRelease", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_AuraRelease", false);

        player.DamageReduction = 0;
        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        float time = chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);



        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 파티클 재생
        Vector3 firstPosition = playerTransform.transform.position;
        firstPosition.y += 1.5f;
        GameObject firstParticle = Instantiate(skillEffects[0], firstPosition, playerTransform.transform.rotation);
        AuraRelease01Effect auraRelease01Effect = firstParticle.GetComponent<AuraRelease01Effect>();
        auraRelease01Effect.size += radiusTree ? auraRelease01Effect.size * 0.5f : 0;
        StartCoroutine(CUseAbility_UpdateParticlePosition(firstParticle.transform, playerTransform));
        Destroy(firstParticle, particleLivingTimes[0]);



        time = castingTime / operatingSpeed - chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 파티클 재생
        Vector3 secondPosition = playerTransform.transform.position;
        secondPosition.y += 1.5f;
        GameObject secondParticle = Instantiate(skillEffects[1], secondPosition, playerTransform.transform.rotation);
        AuraRelease02Effect auraRelease02Effect = secondParticle.GetComponent<AuraRelease02Effect>();
        auraRelease02Effect.size += radiusTree ? auraRelease02Effect.size * 0.5f : 0;
        auraRelease02Effect.caster = player.gameObject;
        auraRelease02Effect.damage = damage;
        /*
        Rigidbody rigidbody = secondParticle.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.AddForce(playerTransform.transform.forward * moveSpeed);
        */
        StartCoroutine(CUseAbility_UpdateParticlePosition(secondParticle.transform, playerTransform));
        Destroy(secondParticle, particleLivingTimes[1]);

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



        float time = chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 효과음 재생
        audioSource.clip = swordSwingSounds[0];
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();



        time = castingTime / operatingSpeed - chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 효과음 재생
        audioSource.clip = swordSwingSounds[1];
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();
        
    }
}