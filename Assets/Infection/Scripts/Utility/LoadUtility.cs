using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadUtility : MonoBehaviour {

    public static Dictionary<string, AsyncOperation> operations {
        get; private set;
    } = new Dictionary<string, AsyncOperation>();





    // 씬 로드
    public static IEnumerator CLoadScene(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // 비동기 씬 호출
        operation.allowSceneActivation = false;

        operations[sceneName] = operation;

        yield return operation.isDone;
    }
    
    public static IEnumerator CLoadScene(string sceneName, Animator animator, GameObject gaugeObject, Image fillImage, float slack = 2) {
        yield return CoroutineUtility.StartStaticCoroutine(CShowLoadGauge(animator, gaugeObject));

        yield return new WaitForSecondsRealtime(1.0f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // 비동기 씬 호출
        operation.allowSceneActivation = false;

        operations[sceneName] = operation;

        yield return CoroutineUtility.StartStaticCoroutine(CRunLoadGauge(sceneName, fillImage, slack));

        yield return new WaitForSecondsRealtime(1.0f);

        yield return CoroutineUtility.StartStaticCoroutine(CHideLoadGauge(animator, gaugeObject));
    }





    // 로딩 게이지 작동
    private static IEnumerator CRunLoadGauge(string sceneName, Image fillImage, float slack = 2) {
        float timer = 0;
        float currentProgress; // 현재 로딩 게이지 진행률
        
        fillImage.fillAmount = 0;
        AsyncOperation operation = operations[sceneName];
        while (!operation.isDone) {
            yield return null;

            timer += Time.unscaledDeltaTime;
            currentProgress = operation.progress; // 현재 로딩 게이지 진행률

            // 로딩 게이지가 90% 미만일 경우
            if (currentProgress < 0.9f) {
                fillImage.fillAmount = Mathf.Lerp(currentProgress, 1, timer / slack); // 로딩 게이지 증가
            }
            // 로딩 게이지가 90% 이상일 경우
            else {
                fillImage.fillAmount = Mathf.Lerp(0, 1, timer / slack); // 로딩 게이지 증가

                // 로딩 게이지가 100%일 경우
                if (fillImage.fillAmount == 1) {
                    yield break;
                }
            }
        }

    }


    // 로딩 게이지 드러내기
    private static IEnumerator CShowLoadGauge(Animator animator, GameObject gaugeObject) {
        gaugeObject.SetActive(true);

        animator.SetBool("isShow", true);

        AnimationClip clip = AnimationUtility.GetAnimationClipInAnimator(animator, "Show");
        // AnimatorState state = AnimationUtility.GetAnimatorState(animator, "Show");
        float speed = 4;
        yield return new WaitForSecondsRealtime(clip.length / speed);
    }

    // 로딩 게이지 숨기기
    private static IEnumerator CHideLoadGauge(Animator animator, GameObject gaugeObject) {
        animator.SetBool("isShow", false);

        AnimationClip clip = AnimationUtility.GetAnimationClipInAnimator(animator, "Hide");
        // AnimatorState state = AnimationUtility.GetAnimatorState(animator, "Hide");
        float speed = 4;
        yield return new WaitForSecondsRealtime(clip.length / speed);

        gaugeObject.SetActive(false);
    }
}
