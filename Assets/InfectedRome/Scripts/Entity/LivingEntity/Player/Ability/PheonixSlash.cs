using System.Collections;
using UnityEngine;

public class PheonixSlash : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5f;
    public float currentCooldown { get; set; }
    public float damage { get; set; } = 50.0f;



    // Option
    public float operatingSpeed { get; set; } = 1.0f;
    public float operatingTime { get; set; } = 2.0f;
    public float castingTime { get; set; } = 0.8f;
    public float firstParticleLivingTime { get; set; } = 1f;
    public float secondParticleLivingTime { get; set; } = 2.5f;



    // Object
    public GameObject[] skillEffects = new GameObject[2];
    public AudioClip swordSwingSound;





    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {
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

        player.animator.SetBool("isUsingAbility_PheonixSlash", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_PheonixSlash", false);

        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;



        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        Vector3 position = playerTransform.transform.position;
        position.y += 1.5f;

        // 파티클 재생
        GameObject firstParticle = Instantiate(skillEffects[0], position, playerTransform.transform.rotation);
        StartCoroutine(CUseAbility_UpdateParticlePosition(firstParticle.transform, playerTransform));
        Destroy(firstParticle, firstParticleLivingTime);

        // 파티클 재생
        GameObject secondParticle = Instantiate(skillEffects[1], position, playerTransform.transform.rotation);
        Pheonix02 pheonix02 = secondParticle.GetComponent<Pheonix02>();
        pheonix02.caster = player.gameObject;
        pheonix02.damage = damage;
        Destroy(secondParticle, secondParticleLivingTime);



        time = 1.0f;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
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

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        // 효과음 재생
        audioSource.clip = swordSwingSound;
        audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
        audioSource.Play();
    }
}
