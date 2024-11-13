using System;
using System.Collections;
using UnityEngine;

public class AuraReleaseAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5;
    public float currentCooldown { get; set; }

    // Option
    public float operatingSpeed { get; set; } = 1;

    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    [Header("Gathering Aura Effect")]
    public GameObject gatheringAuraEffect;
    public Transform gatheringAuraEffectTransform;
    public AudioClip gatheringAuraSwordSwingSound;
    [Header("Aura Release Effect")]
    public GameObject auraReleaseEffect;
    public Transform auraReleaseEffectTransform;
    public AudioClip auraReleaseSwordSwingSound;

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

        StartCoroutine(CUseAbility(player));

    }

    private IEnumerator CUseAbility(Player player) {
        player.IsOperating = true; // 동작 시작
        player.DamageReduction = damageReductionTree ? 0.8f : 0; // 피해 감소율 적용

        // 애니메이션 실행
        player.animator.SetBool("isUsingAbility_AuraRelease", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);



        float time = 0.5f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        Transform playerTransform = player.transform;

        // 파티클 생성
        Vector3 gatheringAuraEffectPosition = gatheringAuraEffectTransform.position;
        Quaternion gatheringAuraEffectRotation = gatheringAuraEffectTransform.rotation;
        GameObject gatheringAuraEffectParticle = Instantiate(gatheringAuraEffect, gatheringAuraEffectPosition, gatheringAuraEffectRotation);
        GatheringAuraEffect gatheringAuraEffectScript = gatheringAuraEffectParticle.GetComponent<GatheringAuraEffect>();
        float gatheringAuraEffectSize = 0.4f;
        gatheringAuraEffectScript.size = gatheringAuraEffectSize + (radiusTree ? gatheringAuraEffectSize * 0.3f : 0);
        StartCoroutine(CUseAbility_UpdateParticlePosition(gatheringAuraEffectParticle.transform, playerTransform)); // 파티클 위치 갱신
        Destroy(gatheringAuraEffectParticle, 2);

        AudioSource audioSource = player.Sword.GetComponent<AudioSource>();

        // 효과음 재생
        audioSource.clip = gatheringAuraSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();



        time = 0.9f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 파티클 생성
        Vector3 auraReleaseEffectPosition = auraReleaseEffectTransform.position;
        Quaternion auraReleaseEffectRotation = auraReleaseEffectTransform.rotation;
        GameObject auraReleaseEffectParticle = Instantiate(auraReleaseEffect, auraReleaseEffectPosition, auraReleaseEffectRotation);
        AuraReleaseEffect auraReleaseEffectScript = auraReleaseEffectParticle.GetComponent<AuraReleaseEffect>();
        float auraReleaseEffectSize = 0.6f;
        auraReleaseEffectScript.size = auraReleaseEffectSize + (radiusTree ? auraReleaseEffectSize * 0.3f : 0);
        GameObject auraReleaseEffectCaster = player.gameObject;
        auraReleaseEffectScript.caster = auraReleaseEffectCaster;
        float auraReleaseEffectDamage = 50;
        auraReleaseEffectScript.damage = auraReleaseEffectDamage;
        StartCoroutine(CUseAbility_UpdateParticlePosition(auraReleaseEffectParticle.transform, playerTransform)); // 파티클 위치 갱신
        Destroy(auraReleaseEffectParticle, 2);

        // 효과음 재생
        audioSource.clip = auraReleaseSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();



        time = 0.5f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
        auraReleaseEffectParticle.GetComponent<SphereCollider>().enabled = false;



        time = 0.26f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 애니메이션 종료
        player.animator.SetBool("isUsingAbility_AuraRelease", false);

        player.DamageReduction = 0; // 피해 감소율 적용 해제
        player.IsOperating = false; // 동작 종료
    }

    private IEnumerator CUseAbility_UpdateParticlePosition(Transform particleTransform, Transform playerTransform) {
        while (particleTransform != null) {
            Vector3 particlePosition = particleTransform.position;
            Vector3 playerPosition = playerTransform.position;
            particleTransform.position = new Vector3(playerPosition.x, particlePosition.y, playerPosition.z);

            yield return null;
        }
    }
}