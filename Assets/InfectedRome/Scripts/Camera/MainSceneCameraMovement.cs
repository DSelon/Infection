using UnityEngine;

public class MainSceneCameraMovement : MonoBehaviour {

    [Header("")]
    [SerializeField] private float maxAngle = 5f; // 최대 회전 각도
    [SerializeField] private float smoothTime = 2f; // 부드러운 정도(회전 속도 반비례)

    private Camera camera;
    private float startRotation;
    private float targetRotation;
    private float currentVelocity;
    private int direction = 1;



    private void Start() {
        camera = GetComponent<Camera>();
        startRotation = camera.transform.localEulerAngles.y;
        targetRotation = startRotation + maxAngle * direction;
    }

    private void Update() {
        // 현재 회전 각도 계산
        float currentRotation = camera.transform.localEulerAngles.y;
        currentRotation = currentRotation > 180 ? currentRotation - 360 : currentRotation;

        // 목표 회전 각도로 부드럽게 이동
        float smoothRotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref currentVelocity, smoothTime);

        // 카메라 회전 적용
        camera.transform.localEulerAngles = new Vector3(camera.transform.localEulerAngles.x, smoothRotation, camera.transform.localEulerAngles.z);

        // 목표 각도에 도달하면 방향 전환
        if (Mathf.Abs(smoothRotation - targetRotation) < 0.1f) {
            direction *= -1;
            targetRotation = startRotation + maxAngle * direction;
        }
    }

}
