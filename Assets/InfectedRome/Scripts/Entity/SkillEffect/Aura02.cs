using UnityEngine;

public class Aura02 : MonoBehaviour {

    public float size { get; set; } = 0.6f;



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
        GetComponent<SphereCollider>().radius *= size;
    }


    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject == caster) return;

        ILivingEntity livingEntity = otherGameObject.GetComponent<ILivingEntity>();
        if (livingEntity == null) return;

        livingEntity.Damage(damage);
    }

}
