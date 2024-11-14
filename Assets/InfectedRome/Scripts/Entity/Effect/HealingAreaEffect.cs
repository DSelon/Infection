using UnityEngine;

public class HealingAreaEffect : MonoBehaviour {

    public float size { get; set; }

    public GameObject caster { get; set; }
    public float heal { get; set; }
    public GameObject healingEffect { get; set; }
    public Transform healingEffectTransform { get; set; }

    private float maxDelayTime = 0.05f;
    private float currentDelayTime;



    private void Start() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Option_SEVolume");

        foreach (Transform transform in GetComponentsInChildren<Transform>()) {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float z = transform.localScale.z;
            transform.localScale = new Vector3(x * size, y * size, z * size);
        }

        currentDelayTime = maxDelayTime;
    }

    private void OnTriggerStay(Collider other) {
        GameObject otherGameObject = other.gameObject;
        Player player = otherGameObject.GetComponent<Player>();
        if (player == null) return;
        if (player.IsDead) return;

        player.Heal(heal * Time.deltaTime);

        // 파티클 생성
        currentDelayTime = currentDelayTime < maxDelayTime ? currentDelayTime + Time.deltaTime : maxDelayTime;
        if (currentDelayTime == maxDelayTime) {
            currentDelayTime = 0;
            Vector3 healingEffectPosition = healingEffectTransform.position;
            Quaternion healingEffectRotation = healingEffectTransform.rotation;
            GameObject healingEffectParticle = Instantiate(healingEffect, healingEffectPosition, healingEffectRotation);
            HealingEffect healingEffectScript = healingEffectParticle.GetComponent<HealingEffect>();
            float healingEffectSize = 1;
            healingEffectScript.size = healingEffectSize;
            Destroy(healingEffectParticle, 1);
        }
    }

}
