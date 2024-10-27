using UnityEngine;
using VFolders.Libs;

public class Exp : MonoBehaviour {
	
    public int amount = 10;

    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;
        Player player = otherGameObject.GetComponent<Player>();

        if (player == null) return;

        player.CurrentExp += amount;
        gameObject.Destroy();
    }
    
}
