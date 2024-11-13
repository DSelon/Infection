using System;
using System.Collections;
using UnityEngine;

public class BarrierAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 8;
    public float currentCooldown { get; set; }

    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    [Header("Barrier Effect")]
    public GameObject barrierEffect;
    public Transform barrierEffectTransform;

    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool durationTree = false;
    [NonSerialized] public bool recoveryHealthTree = false;



    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {

        // 시간이 흐르지 않을 경우
        if (Time.timeScale == 0) return;
        
        currentCooldown = currentCooldown < maxCooldown ? currentCooldown + Time.deltaTime * (cooldownTree ? 1.25f : 1) : maxCooldown;

    }



    public void UseAbility(Player player) {

        // 재사용 대기 시간일 경우
        if (currentCooldown < maxCooldown) return;

        currentCooldown = 0;
        
        // 무적 시간 적용
        float invincibilityTime = player.InvincibilityTime;
        float duration = 3;
        duration = duration + (durationTree ? duration * 0.1f : 0);
        player.InvincibilityTime = invincibilityTime > duration ? invincibilityTime : duration;

        player.Heal(recoveryHealthTree ? player.MaxHealth * 0.05f : 0);

        // 파티클 생성
        Transform playerTransform = player.transform;
        Vector3 barrierEffectPosition = barrierEffectTransform.position;
        Quaternion barrierEffectRotation = barrierEffectTransform.rotation;
        GameObject barrierEffectParticle = Instantiate(barrierEffect, barrierEffectPosition, barrierEffectRotation);
        BarrierEffect barrierEffectScript = barrierEffectParticle.GetComponent<BarrierEffect>();
        float barrierEffectSize = 0.4f;
        barrierEffectScript.size = barrierEffectSize;
        StartCoroutine(CUseAbility_UpdateParticlePosition(barrierEffectParticle.transform, playerTransform));
        float barrierEffectDuration = duration;
        Destroy(barrierEffectParticle, barrierEffectDuration);

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
