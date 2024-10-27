using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // 카메라 추적
    public void FollowCamera(Player player, Vector3 cameraPosition) {
        Vector3 position = player.transform.position;

        player.Camera.transform.position = Vector3.Lerp(cameraPosition, new Vector3(
            position.x + player.cameraOffsetPosition.x,
            position.y + player.cameraOffsetPosition.y,
            position.z + player.cameraOffsetPosition.z
            ), player.CameraFollowSpeed * Time.deltaTime);
    }


    // 이동
    public void Move(Player player, float horizontal, float vertical) {
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction.Normalize();

        bool inputKey = direction != Vector3.zero;


        player.animator.SetBool("isRunning", inputKey);
        player.controller.SimpleMove(direction * player.MoveSpeed); // 이동


        // 키 입력이 없을 경우
        if (!inputKey) {
            return;
        }


        if (Mathf.Sign(player.transform.forward.x) != Mathf.Sign(direction.x)
        || Mathf.Sign(player.transform.forward.z) != Mathf.Sign(direction.z)) {
            player.transform.Rotate(0, 1, 0);
        }
        player.transform.forward = Vector3.Lerp(player.transform.forward, direction, player.RotateSpeed * Time.deltaTime); // 회전
	}

}
