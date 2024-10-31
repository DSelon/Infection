using System.Collections;
using UnityEngine;

public class Rolling : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 3f;
    public float currentCooldown { get; set; }



    // Option
    public float operatingSpeed { get; set; } = 1.0f;
    public float operatingTime { get; set; } = 0.48f;
    public float[] particleLivingTime { get; set; } = { 10.0f, 1.0f, 1.0f };



    // Object
    public GameObject[] skillEffects = new GameObject[3];
    public AudioClip rollingSound;





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
        StartCoroutine(CUseAbility_Move(player));
        StartCoroutine(CUseAbility_GenerateParticle(player));

    }
    
    public IEnumerator CUseAbility_RunAnimation(Player player) {
        player.IsOperating = true;
        player.IsCasting = true;
        player.IsInvincibility = true;

        player.animator.SetBool("isUsingAbility_Rolling", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_Rolling", false);

        player.IsInvincibility = false;
        player.IsCasting = false;
        player.IsOperating = false;
    }

    public IEnumerator CUseAbility_Move(Player player) {

        player.transform.Rotate(new Vector3(0, 180, 0));

        float horizontal = player.input.horizontal;
        float vertical = player.input.vertical;

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction.Normalize();

        float deltaTime = 0;
        while (deltaTime < operatingTime / operatingSpeed) {
            deltaTime += Time.deltaTime;

            player.controller.SimpleMove(direction * 15.0f); // 이동

            yield return null;
        }

        player.transform.Rotate(new Vector3(0, 180, 0));

    }

    public IEnumerator CUseAbility_GenerateParticle(Player player) {
        Transform playerTransform = player.transform;

        // 파티클 재생
        Vector3 firstPosition = playerTransform.transform.position;
        firstPosition.y += 0f;
        Quaternion firstRotation = skillEffects[0].transform.rotation;
        GameObject firstParticle = Instantiate(skillEffects[0], firstPosition, firstRotation);
        Destroy(firstParticle, particleLivingTime[0]);



        float time = operatingTime / 2 / operatingSpeed;
        yield return new WaitForSeconds(time);

        Vector3 secondPosition = playerTransform.transform.position;
        secondPosition.y += 0.5f;
        GameObject secondParticle = Instantiate(skillEffects[1], secondPosition, playerTransform.rotation);
        Destroy(secondParticle, particleLivingTime[1]);



        time = operatingTime / 2 / operatingSpeed;
        yield return new WaitForSeconds(time);

        Vector3 thirdPosition = playerTransform.transform.position;
        thirdPosition.y += 0f;
        Quaternion thirdRotation = skillEffects[2].transform.rotation;
        GameObject thirdParticle = Instantiate(skillEffects[2], thirdPosition, thirdRotation);
        Destroy(thirdParticle, particleLivingTime[2]);
    }

}
