using UnityEngine;

public class ElectricSlashEffect : MonoBehaviour {

    public float size { get; set; }

    public GameObject caster { get; set; }
    public float damage { get; set; }

    public ElectricWhirlwindAbility electricWhirlwindAbility;



    private void Start() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Option_SEVolume");
        
        foreach (Transform transform in GetComponentsInChildren<Transform>()) {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float z = transform.localScale.z;
            transform.localScale = new Vector3(x * size, y * size, z * size);
        }
    }

    private void OnTriggerStay(Collider other) {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject == caster) return;

        ILivingEntity livingEntity = otherGameObject.GetComponent<ILivingEntity>();
        if (livingEntity == null) return;

        IMonster monster = otherGameObject.GetComponent<IMonster>();
        if ((monster == null) == (caster.GetComponent<IMonster>() == null)) return;

        livingEntity.Damage(damage * Time.deltaTime, Random.Range(0, 5) == 0);



        Player player = caster.GetComponent<Player>();
        if (player == null) return;
        if (player.IsDead) return;

        player.Heal(electricWhirlwindAbility.bloodSuckingTree ? damage * Time.deltaTime * 0.01f : 0);
    }

}
