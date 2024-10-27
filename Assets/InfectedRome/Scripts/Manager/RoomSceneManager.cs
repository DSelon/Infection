using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using VInspector;

public class RoomSceneManager : Singleton<RoomSceneManager> {
	[SerializeField] private GameObject camera;
    private AudioSource bgmSource;
    private AudioSource seSource;
    [SerializeField] private GameObject blackDisplay;
    [SerializeField] private GameObject pauseObject;
    private Transform pauseWindowTransform;
    private GameObject pauseBackToGameButton;
    private GameObject pauseExitToMainButton;
    
    [Foldout("Sound")]
    [SerializeField] private AudioClip buttonPointerSound;
    [SerializeField] private AudioClip buttonClickSound;
    [EndFoldout]





    private void Start() {
        bgmSource = camera.GetComponents<AudioSource>()[0];
        seSource = camera.GetComponents<AudioSource>()[1];
        pauseWindowTransform = pauseObject.transform.GetChild(0);
        pauseBackToGameButton = pauseWindowTransform.GetChild(0).gameObject;
        pauseExitToMainButton = pauseWindowTransform.GetChild(1).gameObject;



        StartCoroutine(DisplayUtility.CFadeOutBlackDisplay(blackDisplay)); // 검은 화면 페이드 아웃

        bgmSource.volume = PlayerPrefs.GetFloat("Option_BGMVolume");
        bgmSource.loop = true;
        bgmSource.Play();
        StartCoroutine(BGMUtility.CVolumeUp(bgmSource)); // BGM 볼륨 업

        seSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Animator pauseAnimator = pauseWindowTransform.GetComponent<Animator>();
            if (Time.timeScale == 0) {
                Time.timeScale = 1;
                StartCoroutine(AnimationUtility.CCloseWindow(pauseObject, pauseAnimator));
            }
            else {
                StartCoroutine(AnimationUtility.COpenWindow(pauseObject, pauseAnimator));
                Time.timeScale = 0;
            }
        }
    }





    // 일시정지 창 게임으로 돌아가기 버튼 포인터 인
    public void OnPause_BackToGameButtonPointerEnter() {
        Animator animator = pauseBackToGameButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 일시정지 창 게임으로 돌아가기 버튼 포인터 아웃
    public void OnPause_BackToGameButtonPointerExit() {
        Animator animator = pauseBackToGameButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 일시정지 창 게임으로 돌아가기 버튼 포인트 다운
    public void OnPause_BackToGameButtonPointerDown() {
        StartCoroutine(COnPause_BackToGameButtonPointerDown());
    }

    private IEnumerator COnPause_BackToGameButtonPointerDown() {
        Animator buttonAnimator = pauseBackToGameButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        Animator windowAnimator = pauseWindowTransform.GetComponent<Animator>();

        windowAnimator.SetBool("isWindowOpen", false);

        AnimationClip clip = AnimationUtility.GetAnimationClipInAnimator(windowAnimator, "Close");
        AnimatorState state = AnimationUtility.GetAnimatorState(windowAnimator, "Close");
        yield return new WaitForSecondsRealtime(clip.length / state.speed);

        pauseObject.SetActive(false);
        Time.timeScale = 1;
    }



    // 일시정지 창 메인으로 나가기 버튼 포인터 인
    public void OnPause_ExitToMainButtonPointerEnter() {
        Animator animator = pauseExitToMainButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 일시정지 창 메인으로 나가기 버튼 포인터 아웃
    public void OnPause_ExitToMainButtonPointerExit() {
        Animator animator = pauseExitToMainButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 일시정지 창 메인으로 나가기 버튼 포인트 다운
    public void OnPause_ExitToMainButtonPointerDown() {
        StartCoroutine(COnPause_ExitToMainButtonPointerDown());
    }

    private IEnumerator COnPause_ExitToMainButtonPointerDown() {
        Animator buttonAnimator = pauseExitToMainButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        string sceneName = "Main";
        yield return StartCoroutine(LoadUtility.CLoadScene(sceneName));

        StartCoroutine(BGMUtility.CVolumeDown(bgmSource)); // BGM 볼륨 다운
        yield return StartCoroutine(DisplayUtility.CFadeInBlackDisplay(blackDisplay)); // 검은 화면 페이드 인

        LoadUtility.operations[sceneName].allowSceneActivation = true;
        Time.timeScale = 1;
    }

}
