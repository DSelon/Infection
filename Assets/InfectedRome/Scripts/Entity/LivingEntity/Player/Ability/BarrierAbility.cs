using System;
using System.Collections;
using UnityEngine;

public class BarrierAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 8;
    public float currentCooldown { get; set; }
    public float duration { get; set; } = 3;



    // Option
    public float particleLivingTime { get; set; } = 3;



    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject skillEffect;



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

        UseAbility_GenerateParticle(player);

    }

    public void UseAbility_GenerateParticle(Player player) {
        float invincibilityTime = player.InvincibilityTime;
        float tDuration = duration + (durationTree ? duration * 0.1f : 0);
        player.InvincibilityTime = invincibilityTime > tDuration ? invincibilityTime : tDuration;

        player.Heal(recoveryHealthTree ? player.MaxHealth * 0.05f : 0);

        // 파티클 재생
        Transform playerTransform = player.transform;
        Vector3 position = playerTransform.transform.position;
        position.y += 1;
        Quaternion rotation = skillEffect.transform.rotation;
        GameObject particle = Instantiate(skillEffect, position, rotation);
        StartCoroutine(CUseAbility_UpdateParticlePosition(particle.transform, playerTransform));
        Destroy(particle, particleLivingTime + (durationTree ? particleLivingTime * 0.1f : 0));
    }

    public IEnumerator CUseAbility_UpdateParticlePosition(Transform particleTransform, Transform playerTransform) {
        while (particleTransform != null) {
            Vector3 particlePosition = particleTransform.position;
            Vector3 playerPosition = playerTransform.position;
            particleTransform.position = new Vector3(playerPosition.x, particlePosition.y, playerPosition.z);

            yield return null;
        }
    }

}
