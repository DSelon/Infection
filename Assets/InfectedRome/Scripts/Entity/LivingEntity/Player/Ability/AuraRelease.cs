using System.Collections;
using UnityEngine;

public class AuraRelease : MonoBehaviour, IAbility {

    [field: Header("Status")]
    [field: SerializeField] public float cooldown { get; set; } = 3f;
    [field: SerializeField] public float damage { get; set; } = 20.0f;

    public float currentCooldown { get; set; }



    [field: Header("Option")]
    [field: SerializeField] public float operatingSpeed { get; set; } = 1.0f;
    [field: SerializeField] public float operatingTime { get; set; } = 2.15f;
    [field: SerializeField] public float chargeTime = 0.5f;
    [field: SerializeField] public float castingTime = 1.4f;
    [field: SerializeField] public float firstParticleLivingTime = 1.0f;
    [field: SerializeField] public float secondParticleLivingTime = 1.0f;



    [field: Header("Object")]
    [field: SerializeField] public GameObject[] projectiles = new GameObject[2];
    [field: SerializeField] public AudioClip firstSwordSwingSound;
    [field: SerializeField] public AudioClip secondSwordSwingSound;





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
        player.animator.SetBool("isUsingAbility_AuraRelease", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_AuraRelease", false);
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        float time = chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;



        Vector3 firstPosition = playerTransform.transform.position;
        firstPosition.y += 1.5f;

        GameObject firstParticle = Instantiate(projectiles[0], firstPosition, playerTransform.transform.rotation);

        StartCoroutine(CUseAbility_UpdateParticlePosition(firstParticle.transform, playerTransform));

        Destroy(firstParticle, firstParticleLivingTime);



        time = castingTime / operatingSpeed - chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;



        Vector3 secondPosition = playerTransform.transform.position;
        secondPosition.y += 1.5f;

        GameObject secondParticle = Instantiate(projectiles[1], secondPosition, playerTransform.transform.rotation);
        Aura02 aura02 = secondParticle.GetComponent<Aura02>();
        aura02.caster = player.gameObject;
        aura02.damage = damage;

        /*
        Rigidbody rigidbody = secondParticle.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.AddForce(playerTransform.transform.forward * moveSpeed);
        */

        StartCoroutine(CUseAbility_UpdateParticlePosition(secondParticle.transform, playerTransform));

        Destroy(secondParticle, secondParticleLivingTime);

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

        float time = chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;



        audioSource.clip = firstSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();



        time = castingTime / operatingSpeed - chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        if (player.IsDead) yield break;



        audioSource.clip = secondSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();
        
    }
}