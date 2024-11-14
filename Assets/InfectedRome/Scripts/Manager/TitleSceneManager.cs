using System.Collections;
using UnityEngine;

public class TitleSceneManager : Singleton<TitleSceneManager> {

    public GameObject logo;

    private void Start() {
        StartCoroutine(CStart());
    }

    private IEnumerator CStart() {
        string sceneName = "Main";
        yield return StartCoroutine(LoadUtility.CLoadScene(sceneName));

        yield return new WaitForSecondsRealtime(1);

        logo.SetActive(true);

        yield return new WaitForSecondsRealtime(4);

        LoadUtility.operations[sceneName].allowSceneActivation = true;
    }

}
