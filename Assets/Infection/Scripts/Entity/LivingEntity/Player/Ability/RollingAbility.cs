using System;
using System.Collections;
using UnityEngine;

public class RollingAbility : MonoBehaviour, IAbility {

    // Status
    public float maxCooldown { get; set; } = 4;
    public float currentCooldown { get; set; }

    // Option
    public float operatingSpeed { get; set; } = 1;

    // Object
    [field: SerializeField] public Sprite icon { get; set; }
    [Header("Jump Up Effect")]
    public GameObject jumpUpEffect;
    public Transform jumpUpEffectTransform;
    [Header("High Speed Effect")]
    public GameObject highSpeedEffect;
    public Transform highSpeedEffectTransform;
    [Header("Jump Down Effect")]
    public GameObject jumpDownEffect;
    public Transform jumpDownEffectTransform;

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

        StartCoroutine(CUseAbility(player));

    }

    private IEnumerator CUseAbility(Player player) {
        player.IsOperating = true; // 동작 시작
        player.IsCasting = true; // 캐스팅 시작
        player.InvincibilityTime = player.InvincibilityTime > 0.48f / operatingSpeed ? player.InvincibilityTime : 0.48f / operatingSpeed; // 무적 시간 적용

        player.Heal(recoveryHealthTree ? player.MaxHealth * 0.05f : 0);

        // 애니메이션 실행
        player.animator.SetBool("isUsingAbility_Rolling", true);
        player.animator.SetFloat("usingAbilitySpeed", operatingSpeed);

        // 위치 이동
        StartCoroutine(CUseAbility_Move(player));

        // 파티클 생성
        Vector3 jumpUpEffectPosition = jumpUpEffectTransform.position;
        Quaternion jumpUpEffectRotation = jumpUpEffectTransform.rotation;
        GameObject jumpUpEffectParticle = Instantiate(jumpUpEffect, jumpUpEffectPosition, jumpUpEffectRotation);
        JumpUpEffect jumpUpEffectScript = jumpUpEffectParticle.GetComponent<JumpUpEffect>();
        float jumpUpEffectSize = 1;
        jumpUpEffectScript.size = jumpUpEffectSize;
        Destroy(jumpUpEffectParticle, 1);



        float time = 0.24f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 파티클 생성
        Vector3 highSpeedEffectPosition = highSpeedEffectTransform.position;
        Quaternion highSpeedEffectRotation = highSpeedEffectTransform.rotation;
        GameObject highSpeedEffectParticle = Instantiate(highSpeedEffect, highSpeedEffectPosition, highSpeedEffectRotation);
        HighSpeedEffect highSpeedEffectScript = highSpeedEffectParticle.GetComponent<HighSpeedEffect>();
        float highSpeedEffectSize = 2;
        highSpeedEffectScript.size = highSpeedEffectSize;
        Destroy(highSpeedEffectParticle, 1);



        time = 0.24f / operatingSpeed;
        yield return new WaitForSeconds(time);

        // 파티클 생성
        Vector3 jumpDownEffectPosition = jumpDownEffectTransform.position;
        Quaternion jumpDownEffectRotation = jumpDownEffectTransform.rotation;
        GameObject jumpDownEffectParticle = Instantiate(jumpDownEffect, jumpDownEffectPosition, jumpDownEffectRotation);
        JumpDownEffect jumpDownEffectScript = jumpDownEffectParticle.GetComponent<JumpDownEffect>();
        float jumpDownEffectSize = 1;
        jumpDownEffectScript.size = jumpDownEffectSize;
        GameObject jumpDownEffectCaster = player.gameObject;
        jumpDownEffectScript.caster = jumpDownEffectCaster;
        float jumpDownEffectDamage = 0;
        jumpDownEffectScript.damage = jumpDownEffectDamage;
        Destroy(jumpDownEffectParticle, 1);

        // 애니메이션 종료
        player.animator.SetBool("isUsingAbility_Rolling", false);

        player.IsCasting = false; // 캐스팅 종료
        player.IsOperating = false; // 동작 종료

        if (incresedMoveSpeedTree) {
            float moveSpeed = player.MoveSpeed;
            player.MoveSpeed += moveSpeed * 0.3f; // 속도 증가 효과 적용



            time = 2 / operatingSpeed;
            yield return new WaitForSeconds(time);

            player.MoveSpeed = moveSpeed; // 속도 증가 효과 적용 해제
        }
    }

    public IEnumerator CUseAbility_Move(Player player) {

        player.transform.Rotate(new Vector3(0, 180, 0));

        float horizontal = player.input.horizontal;
        float vertical = player.input.vertical;

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction.Normalize();

        float deltaTime = 0;
        while (deltaTime < 0.48f / operatingSpeed) {
            deltaTime += Time.deltaTime;

            player.controller.SimpleMove(direction * 15.0f); // 이동

            yield return null;
        }

        player.transform.Rotate(new Vector3(0, 180, 0));

    }

}
