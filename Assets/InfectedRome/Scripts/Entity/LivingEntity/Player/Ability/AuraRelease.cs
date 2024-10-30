using System.Collections;
using UnityEngine;

public class AuraRelease : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5f;
    public float currentCooldown { get; set; }
    public float damage { get; set; } = 50.0f;



    // Option
    public float operatingSpeed { get; set; } = 1.0f;
    public float operatingTime { get; set; } = 2.15f;
    public float castingTime { get; set; } = 1.4f;
    public float chargeTime { get; set; } = 0.5f;
    public float firstParticleLivingTime { get; set; } = 1.0f;
    public float secondParticleLivingTime { get; set; } = 1.0f;



    // Object
    public GameObject[] skillEffects = new GameObject[2];
    public AudioClip firstSwordSwingSound;
    public AudioClip secondSwordSwingSound;





    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {

        // 시간이 흐르지 않을 경우
        if (Time.timeScale == 0) return;
        
        currentCooldown = currentCooldown < maxCooldown ? currentCooldown + Time.deltaTime : maxCooldown;

    }



    public void UseAbility(Player player) {

        // 재사용 대기 시간일 경우
        if (currentCooldown < maxCooldown) return;

        currentCooldown = 0;

        StartCoroutine(CUseAbility_RunAnimation(player));
        StartCoroutine(CUseAbility_GenerateParticle(player));
        StartCoroutine(CUseAbility_MakeSound(player));

    }
    
    public IEnumerator CUseAbility_RunAnimation(Player player) {
        player.IsOperating = true;

        player.animator.SetBool("isUsingAbility_AuraRelease", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_AuraRelease", false);

        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        float time = chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);



        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 파티클 재생
        Vector3 firstPosition = playerTransform.transform.position;
        firstPosition.y += 1.5f;
        GameObject firstParticle = Instantiate(skillEffects[0], firstPosition, playerTransform.transform.rotation);
        StartCoroutine(CUseAbility_UpdateParticlePosition(firstParticle.transform, playerTransform));
        Destroy(firstParticle, firstParticleLivingTime);



        time = castingTime / operatingSpeed - chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 파티클 재생
        Vector3 secondPosition = playerTransform.transform.position;
        secondPosition.y += 1.5f;
        GameObject secondParticle = Instantiate(skillEffects[1], secondPosition, playerTransform.transform.rotation);
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

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 효과음 재생
        audioSource.clip = firstSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();



        time = castingTime / operatingSpeed - chargeTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 효과음 재생
        audioSource.clip = secondSwordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();
        
    }
}