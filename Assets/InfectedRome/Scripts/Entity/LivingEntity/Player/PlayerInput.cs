using UnityEngine;

public class PlayerInput : MonoBehaviour {
    
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public bool[] useAbility { get; private set; } = new bool[4];



    private void Update() {

        // PC
        if (SystemInfo.deviceType.ToString() == "Desktop") {

            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            useAbility[0] = Input.GetKeyDown(KeyCode.U);
            useAbility[1] = Input.GetKeyDown(KeyCode.I);
            useAbility[2] = Input.GetKeyDown(KeyCode.O);
            useAbility[3] = Input.GetKeyDown(KeyCode.P);

        }
        // 모바일
        else if (SystemInfo.deviceType.ToString() == "Handheld") {

        }

    }

}
