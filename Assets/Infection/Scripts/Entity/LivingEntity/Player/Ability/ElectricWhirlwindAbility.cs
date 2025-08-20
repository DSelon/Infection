using System;
using System.Collections;
using UnityEngine;

public class ElectricWhirlwindAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 7;
    public float currentCooldown { get; set; }

    // Option
    public float operatingSpeed { get; set; } = 1;

    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    [Header("Electric Slash Effect")]
    public GameObject electricSlashEffect;
    public Transform electricSlashEffectTransform;
    public AudioClip electricSlashSwordSwingSound;

    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool radiusTree = false;
    [NonSerialized] public bool damageReductionTree = false;
    [NonSerialized] public bool bloodSuckingTree = false;



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

        StartCoroutine(CUseAbility(player));

    }
    
    private IEnumerator CUseAbility(Player player) {
        player.IsOperating = true; // 동작 시작
        player.DamageReduction = damageReductionTree ? 0.6f : 0; // 피해 감소율 적용

        Transform playerTransform = player.transform;

        // 파티클 생성
        Vector3 electricSlashEffectPosition = electricSlashEffectTransform.position;
        Quaternion electricSlashEffectRotation = electricSlashEffectTransform.rotation;
        GameObject electricSlashEffectParticle = Instantiate(electricSlashEffect, electricSlashEffectPosition, electricSlashEffectRotation);
        ElectricSlashEffect electricSlashEffectScript = electricSlashEffectParticle.GetComponent<ElectricSlashEffect>();
        float electricSlashEffectSize = 1;
        electricSlashEffectScript.size = electricSlashEffectSize + (radiusTree ? electricSlashEffectSize * 0.3f : 0);
        GameObject electricSlashEffectCaster = player.gameObject;
        electricSlashEffectScript.caster = electricSlashEffectCaster;
        float electricSlashEffectDamage = 70;
        electricSlashEffectScript.damage = electricSlashEffectDamage;
        electricSlashEffectScript.electricWhirlwindAbility = this;
        electricSlashEffectParticle.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(CUseAbility_UpdateParticlePosition(electricSlashEffectParticle.transform, playerTransform));
        Destroy(electricSlashEffectParticle, 3.5f);



        float time = 0.5f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 애니메이션 실행
        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        // 효과음 재생
        StartCoroutine(CUseAbility_MakeSound(player));



        time = 0.5f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) {
            Destroy(electricSlashEffectParticle);
            yield break;
        }

        // 파티클 충돌체 생성
        electricSlashEffectParticle.GetComponent<SphereCollider>().enabled = true;



        time = 1.45f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 애니메이션 종료
        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", false);

        player.DamageReduction = 0; // 피해 감소율 적용 해제
        player.IsOperating = false; // 동작 종료



        time = 0.55f;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
        electricSlashEffectParticle.GetComponent<SphereCollider>().enabled = false;
    }

    private IEnumerator CUseAbility_UpdateParticlePosition(Transform particleTransform, Transform playerTransform) {
        while (particleTransform != null) {
            Vector3 particlePosition = particleTransform.position;
            Vector3 playerPosition = playerTransform.position;
            particleTransform.position = new Vector3(playerPosition.x, particlePosition.y, playerPosition.z);

            yield return null;
        }
    }

    private IEnumerator CUseAbility_MakeSound(Player player) {
        AudioSource audioSource = player.Sword.GetComponent<AudioSource>();

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        float time;
        for (int i = 0; i < 5; i++) {
            time = (2.45f / operatingSpeed) / 7;
            yield return new WaitForSeconds(time);

            // 플레이어가 죽었을 경우
            if (player.IsDead) yield break;

            // 효과음 재생
            audioSource.clip = electricSlashSwordSwingSound;
            audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
            audioSource.Play();
        }
    }

}
