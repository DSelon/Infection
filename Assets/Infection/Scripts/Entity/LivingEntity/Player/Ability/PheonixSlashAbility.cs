using System;
using System.Collections;
using UnityEngine;

public class PheonixSlashAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5;
    public float currentCooldown { get; set; }

    // Option
    public float operatingSpeed { get; set; } = 1;

    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    [Header("Flare Slash Effect")]
    public GameObject flareSlashEffect;
    public Transform flareSlashEffectTransform;
    public AudioClip flareSlashSwordSwingSound;
    [Header("Pheonix Effect")]
    public GameObject pheonixEffect;
    public Transform pheonixEffectTransform;

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
        player.animator.SetBool("isUsingAbility_PheonixSlash", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);



        float time = 0.8f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        Transform playerTransform = player.transform;

        // 파티클 생성
        Vector3 flareSlashEffectPosition = flareSlashEffectTransform.position;
        Quaternion flareSlashEffectRotation = flareSlashEffectTransform.rotation;
        GameObject flareSlashEffectParticle = Instantiate(flareSlashEffect, flareSlashEffectPosition, flareSlashEffectRotation);
        FlareSlashEffect flareSlashEffectScript = flareSlashEffectParticle.GetComponent<FlareSlashEffect>();
        float flareSlashEffectSize = 1;
        flareSlashEffectScript.size = flareSlashEffectSize + (radiusTree ? flareSlashEffectSize * 0.15f : 0);
        StartCoroutine(CUseAbility_UpdateParticlePosition(flareSlashEffectParticle.transform, playerTransform));
        Destroy(flareSlashEffectParticle, 2);

        // 파티클 생성
        Vector3 pheonixEffectPosition = pheonixEffectTransform.position;
        Quaternion pheonixEffectRotation = pheonixEffectTransform.rotation;
        GameObject pheonixEffectParticle = Instantiate(pheonixEffect, pheonixEffectPosition, pheonixEffectRotation);
        PheonixEffect pheonixEffectScript = pheonixEffectParticle.GetComponent<PheonixEffect>();
        float pheonixEffectSize = 1;
        pheonixEffectScript.size = pheonixEffectSize + (radiusTree ? pheonixEffectSize * 0.15f : 0);
        GameObject pheonixEffectCaster = player.gameObject;
        pheonixEffectScript.caster = pheonixEffectCaster;
        float pheonixEffectDamage = 50;
        pheonixEffectScript.damage = pheonixEffectDamage;
        Destroy(pheonixEffectParticle, 2);

        AudioSource audioSource = player.Sword.GetComponent<AudioSource>();

        // 효과음 재생
        audioSource.clip = flareSlashSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();



        time = 0.5f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
        pheonixEffectParticle.GetComponent<BoxCollider>().enabled = false;



        time = 0.71f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 애니메이션 종료
        player.animator.SetBool("isUsingAbility_PheonixSlash", false);

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
