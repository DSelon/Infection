using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {
    
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public bool[] useAbility { get; private set; } = new bool[4];

    public Player player;
    public GameObject mobileUI;
    public FixedJoystick joystick;
    public GameObject abilityButtonBundle;
    [NonSerialized] public GameObject[] abilityButtons = new GameObject[4];



    private void Start() {
        for (int i = 0; i < abilityButtons.Length; i++) abilityButtons[i] = abilityButtonBundle.transform.GetChild(i).gameObject;

        if (SystemInfo.deviceType.ToString() == "Handheld") mobileUI.SetActive(true);
    }
    
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

            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;

            for (int i = 0; i < abilityButtons.Length; i++) {
                abilityButtons[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = player.abilityIcons[i].sprite;
                abilityButtons[i].transform.GetChild(2).GetComponent<Image>().fillAmount = player.abilityCoolFills[i].fillAmount;
            }

        }

    }



    public void OnUseAbility0() {
        int number = 0;

        useAbility[number] = true;
        CoroutineUtility.CallWaitForOneFrame(() => { useAbility[number] = false; });
    }

    public void OnUseAbility1() {
        int number = 1;

        useAbility[number] = true;
        CoroutineUtility.CallWaitForOneFrame(() => { useAbility[number] = false; });
    }

    public void OnUseAbility2() {
        int number = 2;

        useAbility[number] = true;
        CoroutineUtility.CallWaitForOneFrame(() => { useAbility[number] = false; });
    }

    public void OnUseAbility3() {
        int number = 3;

        useAbility[number] = true;
        CoroutineUtility.CallWaitForOneFrame(() => { useAbility[number] = false; });
    }

}
