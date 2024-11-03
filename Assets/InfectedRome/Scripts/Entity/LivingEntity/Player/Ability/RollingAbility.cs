using System;
using System.Collections;
using UnityEngine;

public class RollingAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 4;
    public float currentCooldown { get; set; }



    // Option
    public float operatingSpeed { get; set; } = 1;
    public float operatingTime { get; set; } = 0.48f;
    public float[] particleLivingTimes { get; set; } = { 10, 1, 1 };



    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    public GameObject[] skillEffects = new GameObject[3];



    // Tree
    [NonSerialized] public bool cooldownTree = false;
    [NonSerialized] public bool recoveryHealthTree = false;
    [NonSerialized] public bool incresedMoveSpeedTree = false;





    private void Start() {
        currentCooldown = maxCooldown;
    }


    private void Update() {

        // 시간이 흐르지 않을 경우
        if (Time.timeScale == 0) return;
        
        currentCooldown = currentCooldown < maxCooldown ? currentCooldown + Time.deltaTime * (cooldownTree ? 1.4f : 1) : maxCooldown;

    }



    public void UseAbility(Player player) {

        // 재사용 대기 시간일 경우
        if (currentCooldown < maxCooldown) return;

        // 다른 능력이 동작 중일 경우
        if (player.IsOperating) return;

        currentCooldown = 0;

        StartCoroutine(CUseAbility_RunAnimation(player));
        StartCoroutine(CUseAbility_Move(player));
        StartCoroutine(CUseAbility_GenerateParticle(player));

    }
    
    public IEnumerator CUseAbility_RunAnimation(Player player) {
        player.IsOperating = true;
        player.IsCasting = true;
        player.InvincibilityTime = player.InvincibilityTime > operatingTime / operatingSpeed ? player.InvincibilityTime : operatingTime / operatingSpeed;

        player.Heal(recoveryHealthTree ? player.MaxHealth * 0.05f : 0);

        player.animator.SetBool("isUsingAbility_Rolling", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        float time = operatingTime / operatingSpeed;
        yield return new WaitForSeconds(time);

        player.animator.SetBool("isUsingAbility_Rolling", false);

        player.IsCasting = false;
        player.IsOperating = false;

        if (incresedMoveSpeedTree) StartCoroutine(CUseAbility_IncreasedMoveSpeed(player));
    }

    public IEnumerator CUseAbility_IncreasedMoveSpeed(Player player) {
        float moveSpeed = player.MoveSpeed;
        player.MoveSpeed += moveSpeed * 0.3f;

        float time = 2;
        yield return new WaitForSeconds(time);

        player.MoveSpeed = moveSpeed;
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
        Destroy(firstParticle, particleLivingTimes[0]);



        float time = operatingTime / 2 / operatingSpeed;
        yield return new WaitForSeconds(time);

        Vector3 secondPosition = playerTransform.transform.position;
        secondPosition.y += 0.5f;
        GameObject secondParticle = Instantiate(skillEffects[1], secondPosition, playerTransform.rotation);
        Destroy(secondParticle, particleLivingTimes[1]);



        time = operatingTime / 2 / operatingSpeed;
        yield return new WaitForSeconds(time);

        Vector3 thirdPosition = playerTransform.transform.position;
        thirdPosition.y += 0f;
        Quaternion thirdRotation = skillEffects[2].transform.rotation;
        GameObject thirdParticle = Instantiate(skillEffects[2], thirdPosition, thirdRotation);
        Destroy(thirdParticle, particleLivingTimes[2]);
    }

}
