using System.Collections;
using UnityEngine;

public class ElectricWhirlwind : MonoBehaviour, IAbility {

    [field: Header("Status")]
    [field: SerializeField] public float cooldown { get; set; } = 5f;
    [field: SerializeField] public float damage { get; set; } = 50.0f;

    public float currentCooldown { get; set; }



    [field: Header("Option")]
    [field: SerializeField] public float operatingSpeed { get; set; } = 1.0f;
    [field: SerializeField] public float operatingTime { get; set; } = 1.8f;
    [field: SerializeField] public float castingTime = 1f;
    [field: SerializeField] public float particleLivingTime = 3.5f;



    [field: Header("Object")]
    [field: SerializeField] public GameObject projectile;
    [field: SerializeField] public AudioClip swordSwingSound;





    private void Start() {
        currentCooldown = cooldown;
    }


    private void Update() {
        if (Time.timeScale == 0) return;
        
        currentCooldown = currentCooldown < cooldown ? currentCooldown + Time.deltaTime : cooldown;
    }



    public void UseAbility(Player player) {
        if (currentCooldown < cooldown) return;
        currentCooldown = 0;

        StartCoroutine(CUseAbility_RunAnimation(player));
        StartCoroutine(CUseAbility_GenerateParticle(player));
        StartCoroutine(CUseAbility_MakeSound(player));
    }
    
    public IEnumerator CUseAbility_RunAnimation(Player player) {

        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;

        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);



        time = (operatingTime / operatingSpeed) - (castingTime / operatingSpeed);
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", false);
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        Vector3 position = playerTransform.transform.position;
        position.y += 1.5f;

        GameObject particle = Instantiate(projectile, position, playerTransform.transform.rotation);
        Electric01 electric01 = particle.GetComponent<Electric01>();
        electric01.caster = player.gameObject;
        electric01.damage = damage;
        particle.GetComponent<SphereCollider>().enabled = false;

        StartCoroutine(CUseAbility_UpdateParticlePosition(particle.transform, playerTransform));

        Destroy(particle, particleLivingTime);



        float time = 1f;
        yield return new WaitForSeconds(time);

        particle.GetComponent<SphereCollider>().enabled = true;



        time = 2f;
        yield return new WaitForSeconds(time);

        particle.GetComponent<SphereCollider>().enabled = false;
    }

    public IEnumerator CUseAbility_UpdateParticlePosition(Transform particleTransform, Transform playerTransform) {
        while (particleTransform != null) {
            Vector3 particlePosition = particleTransform.position;
            Vector3 playerPosition = playerTransform.position;
            particleTransform.position = new Vector3(playerPosition.x, particlePosition.y, playerPosition.z);

            yield return null;
        }
    }

    public IEnumerator CUseAbility_MakeSound(Player player) {
        AudioSource audioSource = player.Sword.GetComponent<AudioSource>();

        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;

        for (int i = 0; i < 5; i++) {

            time = (operatingTime/ operatingSpeed) / 5;
            yield return new WaitForSeconds(time);

            if (player.IsDead) yield break;

            audioSource.clip = swordSwingSound;
            audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
            audioSource.Play();
        }
    }
}
