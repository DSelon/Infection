using System;
using UnityEngine;

public class HealingAreaAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 10;
    public float currentCooldown { get; set; }
    public float heal { get; set; } = 10;
    public float duration { get; set; } = 5;



    // Option
    public float particleLivingTime { get; set; } = 5;



    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject[] skillEffects = new GameObject[2];



    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool durationTree = false;
    [NonSerialized] public bool radiusTree = false;
    [NonSerialized] public bool increasedHealTree = false;





    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {

        // 시간이 흐르지 않을 경우
        if (Time.timeScale == 0) return;
        
        currentCooldown = currentCooldown < maxCooldown ? currentCooldown + Time.deltaTime * (cooldownTree ? 1.2f : 1) : maxCooldown;

    }



    public void UseAbility(Player player) {

        // 재사용 대기 시간일 경우
        if (currentCooldown < maxCooldown) return;

        currentCooldown = 0;

        UseAbility_GenerateParticle(player);

    }

    public void UseAbility_GenerateParticle(Player player) {

        // 파티클 재생
        Transform playerTransform = player.transform;
        Vector3 position = playerTransform.position;
        position.y += 0.1f;
        Quaternion rotation = skillEffects[0].transform.rotation;
        GameObject particle = Instantiate(skillEffects[0], position, rotation);
        HealingArea01Effect healingArea01Effect = particle.GetComponent<HealingArea01Effect>();
        healingArea01Effect.size += radiusTree ? healingArea01Effect.size * 0.2f : 0;
        healingArea01Effect.caster = player.gameObject;
        healingArea01Effect.heal = heal + (increasedHealTree ? heal * 0.1f : 0);
        healingArea01Effect.effect = skillEffects[1];
        Destroy(particle, particleLivingTime + (durationTree ? particleLivingTime * 0.15f : 0));

    }

}
