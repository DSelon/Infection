using UnityEngine;

public class Aura02 : MonoBehaviour {

    [Header("Option")]
    public float size = 0.2f;



    public GameObject caster { get; set; }
    public float damage { get; set; }





    private void Start() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Option_SEVolume");
        
        foreach (Transform transform in GetComponentsInChildren<Transform>()) {
            transform.localScale = new Vector3(size, size, size);
        }
    }


    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject == caster) return;

        ILivingEntity livingEntity = otherGameObject.GetComponent<ILivingEntity>();
        if (livingEntity == null) return;

        livingEntity.Damage(damage);
    }

}
