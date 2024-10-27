using System.Collections;
using UnityEngine;

public class PheonixSlash : MonoBehaviour, IAbility {

    [field: Header("Status")]
    [field: SerializeField] public float cooldown { get; set; } = 5f;
    [field: SerializeField] public float damage { get; set; } = 50.0f;

    public float currentCooldown { get; set; }



    [field: Header("Option")]
    [field: SerializeField] public float operatingSpeed { get; set; } = 1.0f;
    [field: SerializeField] public float operatingTime { get; set; } = 2.0f;
    [field: SerializeField] public float castingTime = 0.8f;
    [field: SerializeField] public float firstParticleLivingTime = 1f;
    [field: SerializeField] public float secondParticleLivingTime = 2.5f;



    [field: Header("Object")]
    [field: SerializeField] public GameObject[] projectiles = new GameObject[2];
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
        player.animator.SetBool("isUsingAbility_PheonixSlash", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_PheonixSlash", false);
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;



        Vector3 position = playerTransform.transform.position;
        position.y += 1.5f;

        GameObject firstParticle = Instantiate(projectiles[0], position, playerTransform.transform.rotation);
        GameObject secondParticle = Instantiate(projectiles[1], position, playerTransform.transform.rotation);
        Pheonix02 pheonix02 = secondParticle.GetComponent<Pheonix02>();
        pheonix02.caster = player.gameObject;
        pheonix02.damage = damage;

        StartCoroutine(CUseAbility_UpdateParticlePosition(firstParticle.transform, playerTransform));

        Destroy(firstParticle, firstParticleLivingTime);
        Destroy(secondParticle, secondParticleLivingTime);



        time = 1.0f;
        yield return new WaitForSeconds(time);

        secondParticle.GetComponent<BoxCollider>().enabled = false;
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



        audioSource.clip = swordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();
    }
}
