using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUtility : MonoBehaviour {

	// 검은 화면 페이드 인
    public static IEnumerator CFadeInBlackDisplay(GameObject blackDisplay) {
        blackDisplay.SetActive(true);

        Animator animator = blackDisplay.GetComponent<Animator>();
        
        blackDisplay.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        animator.SetBool("isFadeIn", true);

        AnimationClip clip = AnimationUtility.GetAnimationClipInAnimator(animator, "FadeIn");
        AnimatorState state = AnimationUtility.GetAnimatorState(animator, "FadeIn");
        yield return new WaitForSecondsRealtime(clip.length / state.speed);

        animator.SetBool("isFadeIn", false);
        blackDisplay.GetComponent<Image>().color = new Color(0, 0, 0, 1);
    }

    // 검은 화면 페이드 아웃
    public static IEnumerator CFadeOutBlackDisplay(GameObject blackDisplay) {
        blackDisplay.SetActive(true);
        
        Animator animator = blackDisplay.GetComponent<Animator>();
        
        blackDisplay.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        animator.SetBool("isFadeOut", true);

        AnimationClip clip = AnimationUtility.GetAnimationClipInAnimator(animator, "FadeOut");
        AnimatorState state = AnimationUtility.GetAnimatorState(animator, "FadeOut");
        yield return new WaitForSecondsRealtime(clip.length / state.speed);

        blackDisplay.SetActive(false);
        
        animator.SetBool("isFadeOut", false);
        blackDisplay.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

}
