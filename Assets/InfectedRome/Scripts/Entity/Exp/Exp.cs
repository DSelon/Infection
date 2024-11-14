using UnityEngine;

public class Exp : MonoBehaviour {
	
    public int amount;

    private float rotateSpeed = 100.0f;

    private void Update() {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;
        Player player = otherGameObject.GetComponent<Player>();

        if (player == null) return;

        player.CurrentExp += amount;
        Destroy(gameObject);
    }
    
}
