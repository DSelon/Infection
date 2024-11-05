using System;
using System.Collections;
using UnityEngine;

public class ElectricWhirlwindAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 7;
    public float currentCooldown { get; set; }
    public float damage { get; set; } = 70;



    // Option
    public float operatingSpeed { get; set; } = 1;
    public float operatingTime { get; set; } = 2.45f;
    public float castingTime { get; set; } = 0.5f;
    public float particleLivingTime { get; set; } = 3.5f;



    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject skillEffects;
    public AudioClip swordSwingSound;



    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool radiusTree = false;
    [NonSerialized] public bool damageReductionTree = false;
    [NonSerialized] public bool bloodSuckingTree = false;





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
        player.DamageReduction = damageReductionTree ? 0.6f : 0;



        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;
        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);



        time = (operatingTime / operatingSpeed) - (castingTime / operatingSpeed);
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", false);

        player.DamageReduction = 0;
        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        // 파티클 재생
        Vector3 position = playerTransform.transform.position;
        position.y += 1.5f;
        GameObject particle = Instantiate(skillEffects, position, playerTransform.transform.rotation);
        ElectricWhirlwind01Effect electricWhirlwind01Effect = particle.GetComponent<ElectricWhirlwind01Effect>();
        electricWhirlwind01Effect.size += radiusTree ? electricWhirlwind01Effect.size * 0.3f : 0;
        electricWhirlwind01Effect.caster = player.gameObject;
        electricWhirlwind01Effect.damage = damage;
        electricWhirlwind01Effect.electricWhirlwindAbility = this;
        particle.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(CUseAbility_UpdateParticlePosition(particle.transform, playerTransform));
        Destroy(particle, particleLivingTime);



        float time = 1f;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) {
            Destroy(particle);
            yield break;
        }

        // 파티클 충돌체 생성
        particle.GetComponent<SphereCollider>().enabled = true;



        time = 2f;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
        particle.GetComponent<SphereCollider>().enabled = false;
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

        for (int i = 0; i < 5; i++) {
            time = (operatingTime / operatingSpeed) / 7;
            yield return new WaitForSeconds(time);

            // 플레이어가 죽었을 경우
            if (player.IsDead) yield break;

            // 효과음 재생
            audioSource.clip = swordSwingSound;
            audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
            audioSource.Play();
        }
    }

}
