using UnityEngine;

public class Aura01 : MonoBehaviour {
	
    public float size { get; set; } = 0.4f;

    private void Start() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Option_SEVolume");

        foreach (Transform transform in GetComponentsInChildren<Transform>()) {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float z = transform.localScale.z;
            transform.localScale = new Vector3(x * size, y * size, z * size);
        }
    }

}
