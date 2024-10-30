using System.Collections;
using UnityEngine;

public class ElectricWhirlwind : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 5f;
    public float currentCooldown { get; set; }
    public float damage { get; set; } = 50.0f;



    // Option
    public float operatingSpeed { get; set; } = 1.0f;
    public float operatingTime { get; set; } = 2.1f;
    public float castingTime { get; set; } = 0.5f;
    public float particleLivingTime { get; set; } = 3.5f;



    // Object
    public GameObject skillEffects;
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



        float time = castingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;
        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);



        time = (operatingTime / operatingSpeed) - (castingTime / operatingSpeed);
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_ElectricWhirlwind", false);

        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        // 파티클 재생
        Vector3 position = playerTransform.transform.position;
        position.y += 1.5f;
        GameObject particle = Instantiate(skillEffects, position, playerTransform.transform.rotation);
        Electric01 electric01 = particle.GetComponent<Electric01>();
        electric01.caster = player.gameObject;
        electric01.damage = damage;
        particle.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(CUseAbility_UpdateParticlePosition(particle.transform, playerTransform));
        Destroy(particle, particleLivingTime);



        float time = 1f;
        yield return new WaitForSeconds(time);

        // 플레이어가 죽었을 경우
        if (player.IsDead) {
            Destroy(particle);
            yield break;
        }

        // 파티클 충돌체 생성
        particle.GetComponent<SphereCollider>().enabled = true;



        time = 2f;
        yield return new WaitForSeconds(time);

        // 파티클 충돌체 제거
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

        // 플레이어가 죽었을 경우
        if (player.IsDead) yield break;

        for (int i = 0; i < 5; i++) {
            time = (operatingTime/ operatingSpeed) / 7;
            yield return new WaitForSeconds(time);

            // 플레이어가 죽었을 경우
            if (player.IsDead) yield break;

            // 효과음 재생
            audioSource.clip = swordSwingSound;
            audioSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
            audioSource.Play();
        }
    }
}
