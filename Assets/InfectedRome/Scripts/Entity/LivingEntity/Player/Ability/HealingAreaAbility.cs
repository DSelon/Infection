using System;
using UnityEngine;

public class HealingAreaAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 10;
    public float currentCooldown { get; set; }

    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject healingAreaEffect;
    public Transform healingAreaEffectTransform;
    public GameObject healingEffect;
    public Transform healingEffectTransform;

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

        // 파티클 재생
        Vector3 healingAreaEffectPosition = healingAreaEffectTransform.position;
        Quaternion healingAreaEffectRotation = healingAreaEffectTransform.rotation;
        GameObject healingAreaEffectParticle = Instantiate(healingAreaEffect, healingAreaEffectPosition, healingAreaEffectRotation);
        HealingAreaEffect healingAreaEffectScript = healingAreaEffectParticle.GetComponent<HealingAreaEffect>();
        float healingAreaEffectSize = 1;
        healingAreaEffectScript.size = healingAreaEffectSize + (radiusTree ? healingAreaEffectSize * 0.1f : 0);
        GameObject healingAreaEffectCaster = player.gameObject;
        healingAreaEffectScript.caster = healingAreaEffectCaster;
        float healingAreaEffectHeal = 10;
        healingAreaEffectScript.heal = healingAreaEffectHeal + (increasedHealTree ? healingAreaEffectHeal * 0.1f : 0);
        GameObject healingAreaEffectHealingEffect = healingEffect;
        healingAreaEffectScript.healingEffect = healingAreaEffectHealingEffect;
        Transform healingAreaEffectHealingEffectTransform = healingEffectTransform;
        healingAreaEffectScript.healingEffectTransform = healingAreaEffectHealingEffectTransform;
        float duration = 5;
        duration = duration + (durationTree ? duration * 0.15f : 0);
        float healingAreaEffectDuration = duration;
        Destroy(healingAreaEffectParticle, healingAreaEffectDuration);

    }

}
