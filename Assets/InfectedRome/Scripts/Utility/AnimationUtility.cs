using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationUtility : MonoBehaviour {
	
    // 애니메이션 클립 얻기
    public static AnimationClip GetAnimationClipInAnimator(Animator animator, string name) {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
            if (clip.name == name) return clip;
        }

        return null;
    }

    // 애니메이터의 모든 상태 얻기
    public static List<AnimatorState> GetAnimatorStates(Animator animator) {
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] animatorControllerLayers = animatorController.layers;
        List<AnimatorState> states = new List<AnimatorState>(); 

        foreach (AnimatorControllerLayer layer in animatorControllerLayers) {

            ChildAnimatorState[] childAnimatorStates = layer.stateMachine.states;
            foreach (ChildAnimatorState childAnimatorState in childAnimatorStates) {
                states.Add(childAnimatorState.state);
            }

        }

        return states;
    }

    // 애니메이터 상태 얻기
    public static AnimatorState GetAnimatorState(Animator animator, string name) {
        foreach (AnimatorState state in GetAnimatorStates(animator)) {
            if (state.name == name) return state;
        }

        return null;
    }
    




    // 버튼 애니메이션 실행
    public static IEnumerator CPlayButtonPointerDownAnimation(Animator animator) {
        animator.SetBool("isPointerDown", true);

        AnimationClip clip = GetAnimationClipInAnimator(animator, "PointerDown");
        AnimatorState state = GetAnimatorState(animator, "PointerDown");
        yield return new WaitForSecondsRealtime(clip.length / state.speed);

        animator.SetBool("isPointerDown", false);
    }


    // 창 열기
    public static IEnumerator COpenWindow(GameObject gameObject, Animator animator) {
        gameObject.SetActive(true);

        animator.SetBool("isWindowOpen", true);

        AnimationClip clip = GetAnimationClipInAnimator(animator, "Open");
        AnimatorState state = GetAnimatorState(animator, "Open");
        yield return new WaitForSecondsRealtime(clip.length / state.speed);
    }

    // 창 닫기
    public static IEnumerator CCloseWindow(GameObject gameObject, Animator animator) {
        animator.SetBool("isWindowOpen", false);

        AnimationClip clip = GetAnimationClipInAnimator(animator, "Close");
        AnimatorState state = GetAnimatorState(animator, "Close");
        yield return new WaitForSecondsRealtime(clip.length / state.speed);

        gameObject.SetActive(false);
    }
}
