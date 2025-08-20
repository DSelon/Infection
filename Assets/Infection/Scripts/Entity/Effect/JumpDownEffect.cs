using UnityEngine;

public class JumpDownEffect : MonoBehaviour {

    public float size { get; set; }

    public GameObject caster { get; set; }
    public float damage { get; set; }



    private void Start() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Option_SEVolume");

        foreach (Transform transform in GetComponentsInChildren<Transform>()) {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float z = transform.localScale.z;
            transform.localScale = new Vector3(x * size, y * size, z * size);
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject == caster) return;

        ILivingEntity livingEntity = otherGameObject.GetComponent<ILivingEntity>();
        if (livingEntity == null) return;

        IMonster monster = otherGameObject.GetComponent<IMonster>();
        if ((monster == null) == (caster.GetComponent<IMonster>() == null)) return;

        if (damage > 0) livingEntity.Damage(damage);
    }
	
}
