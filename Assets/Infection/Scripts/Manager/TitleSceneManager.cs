using System.Collections;
using UnityEngine;

public class TitleSceneManager : Singleton<TitleSceneManager> {

    public GameObject logo;

    private void Start() {
        StartCoroutine(CStart());
    }

    private IEnumerator CStart() {
        if (PlayerPrefs.GetInt("Option_IsFirstJoin") == 0) {
            PlayerPrefs.SetInt("Option_IsFirstJoin", 1);
            PlayerPrefs.SetFloat("Option_BGMVolume", 0.5f);
            PlayerPrefs.SetFloat("Option_SEVolume", 0.5f);
        }

        string sceneName = "Main";
        yield return StartCoroutine(LoadUtility.CLoadScene(sceneName));

        yield return new WaitForSecondsRealtime(1);

        logo.SetActive(true);

        yield return new WaitForSecondsRealtime(4);

        LoadUtility.operations[sceneName].allowSceneActivation = true;
    }

}
