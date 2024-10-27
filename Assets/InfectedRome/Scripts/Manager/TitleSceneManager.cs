using System.Collections;

public class TitleSceneManager : Singleton<TitleSceneManager> {

    private void Start() {
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene() {
        string sceneName = "Main";
        yield return StartCoroutine(LoadUtility.CLoadScene(sceneName));

        LoadUtility.operations[sceneName].allowSceneActivation = true;
    }

}
