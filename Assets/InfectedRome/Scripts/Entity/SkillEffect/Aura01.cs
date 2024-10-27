using UnityEngine;

public class Aura01 : MonoBehaviour {
	
    [Header("Option")]
    public float size = 0.4f;

    private void Start() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Option_SEVolume");

        foreach (Transform transform in GetComponentsInChildren<Transform>()) {
            transform.localScale = new Vector3(size, size, size);
        }
    }

}
