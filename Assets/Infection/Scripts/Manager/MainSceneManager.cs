using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class MainSceneManager : Singleton<MainSceneManager> {

    [SerializeField] private GameObject camera;
    private AudioSource bgmSource;
    private AudioSource seSource;
    [SerializeField] private GameObject blackDisplay;
    [SerializeField] private GameObject gameStartButton;
    [SerializeField] private GameObject loadGaugeObject;
    private GameObject loadGaugeAnimatorObject;
    private Image loadGaugeFillImage;
    [SerializeField] private GameObject controlKeyButton;
    [SerializeField] private GameObject controlKeyObject;
    private Transform controlKeyWindowTransform;
    [SerializeField] private GameObject creatorButton;
    [SerializeField] private GameObject creatorObject;
    private Transform creatorWindowTransform;
    [SerializeField] private GameObject optionButton;
    [SerializeField] private GameObject optionObject;
    private Transform optionWindowTransform;
    private Slider optionBgmSlider;
    private Slider optionSeSlider;
    [SerializeField] private GameObject exitButton;

    [Foldout("Sound")]
    [SerializeField] private AudioClip buttonPointerSound;
    [SerializeField] private AudioClip buttonClickSound;
    [EndFoldout]





    private void Start() {
        bgmSource = camera.GetComponents<AudioSource>()[0];
        seSource = camera.GetComponents<AudioSource>()[1];
        loadGaugeAnimatorObject = loadGaugeObject.transform.GetChild(1).gameObject;
        loadGaugeFillImage = loadGaugeAnimatorObject.transform.GetChild(1).GetComponent<Image>();
        controlKeyWindowTransform = controlKeyObject.transform.GetChild(1);
        creatorWindowTransform = creatorObject.transform.GetChild(1);
        optionWindowTransform = optionObject.transform.GetChild(1);
        optionBgmSlider = optionWindowTransform.GetChild(2).GetComponent<Slider>();
        optionSeSlider = optionWindowTransform.GetChild(3).GetComponent<Slider>();



        StartCoroutine(DisplayUtility.CFadeOutBlackDisplay(blackDisplay)); // 검은 화면 페이드 아웃

        bgmSource.volume = PlayerPrefs.GetFloat("Option_BGMVolume");
        bgmSource.loop = true;
        bgmSource.Play();
        StartCoroutine(BGMUtility.CVolumeUp(bgmSource)); // BGM 볼륨 업

        seSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");
    }
	




    // 게임 시작 버튼 포인터 인
    public void OnGameStartButtonPointerEnter() {
        Animator animator = gameStartButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 게임 시작 버튼 포인터 아웃
    public void OnGameStartButtonPointerExit() {
        Animator animator = gameStartButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 게임 시작 버튼 포인트 다운
    public void OnGameStartButtonPointerDown() {
        StartCoroutine(COnGameStartButtonPointerDown());
    }

    private IEnumerator COnGameStartButtonPointerDown() {
        Animator buttonAnimator = gameStartButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        loadGaugeFillImage.fillAmount = 0;
        
        Animator gaugeAnimator = loadGaugeAnimatorObject.GetComponent<Animator>();
        string sceneName = "Room";
        yield return StartCoroutine(LoadUtility.CLoadScene(sceneName, gaugeAnimator, loadGaugeObject, loadGaugeFillImage, 1));

        yield return new WaitForSecondsRealtime(0.5f);

        StartCoroutine(BGMUtility.CVolumeDown(bgmSource)); // BGM 볼륨 다운
        yield return StartCoroutine(DisplayUtility.CFadeInBlackDisplay(blackDisplay)); // 검은 화면 페이드 인

        LoadUtility.operations[sceneName].allowSceneActivation = true;
    }



    // 조작키 버튼 포인터 인
    public void OnControlKeyButtonPointerEnter() {
        Animator animator = controlKeyButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 조작키 버튼 포인트 아웃
    public void OnControlKeyButtonPointerExit() {
        Animator animator = controlKeyButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 조작키 버튼 포인터 다운
    public void OnControlKeyButtonPointerDown() {
        StartCoroutine(COnControlKeyButtonPointerDown());
    }

    private IEnumerator COnControlKeyButtonPointerDown() {
        Animator buttonAnimator = controlKeyButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();

        

        Animator windowAnimator = controlKeyWindowTransform.GetComponent<Animator>();
        yield return StartCoroutine(AnimationUtility.COpenWindow(controlKeyObject, windowAnimator));
    }
    
    
    
    // 조작키 창 닫기 버튼 포인터 인
    public void OnControlKey_CloseButtonPointerEnter() {
        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 조작키 창 닫기 버튼 포인터 다운
    public void OnControlKey_CloseButtonPointerDown() {
        StartCoroutine(COnControlKey_CloseButtonPointerDown());
    }

    private IEnumerator COnControlKey_CloseButtonPointerDown() {
        seSource.clip = buttonClickSound;
        seSource.Play();



        Animator windowAnimator = controlKeyWindowTransform.GetComponent<Animator>();
        yield return StartCoroutine(AnimationUtility.CCloseWindow(controlKeyObject, windowAnimator));
    }



    // 제작자 버튼 포인터 인
    public void OnCreatorButtonPointerEnter() {
        Animator animator = creatorButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 제작자 버튼 포인트 아웃
    public void OnCreatorButtonPointerExit() {
        Animator animator = creatorButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 제작자 버튼 포인터 다운
    public void OnCreatorButtonPointerDown() {
        StartCoroutine(COnCreatorButtonPointerDown());
    }

    private IEnumerator COnCreatorButtonPointerDown() {
        Animator buttonAnimator = creatorButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();

        

        Animator windowAnimator = creatorWindowTransform.GetComponent<Animator>();
        yield return StartCoroutine(AnimationUtility.COpenWindow(creatorObject, windowAnimator));
    }
    
    
    
    // 제작자 창 닫기 버튼 포인터 인
    public void OnCreator_CloseButtonPointerEnter() {
        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 제작자 창 닫기 버튼 포인터 다운
    public void OnCreator_CloseButtonPointerDown() {
        StartCoroutine(COnCreator_CloseButtonPointerDown());
    }

    private IEnumerator COnCreator_CloseButtonPointerDown() {
        seSource.clip = buttonClickSound;
        seSource.Play();



        Animator windowAnimator = creatorWindowTransform.GetComponent<Animator>();
        yield return StartCoroutine(AnimationUtility.CCloseWindow(creatorObject, windowAnimator));
    }
    
    
    
    // 옵션 버튼 포인터 인
    public void OnOptionButtonPointerEnter() {
        Animator animator = optionButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 옵션 버튼 포인트 아웃
    public void OnOptionButtonPointerExit() {
        Animator animator = optionButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 옵션 버튼 포인터 다운
    public void OnOptionButtonPointerDown() {
        StartCoroutine(COnOptionButtonPointerDown());
    }

    private IEnumerator COnOptionButtonPointerDown() {
        Animator buttonAnimator = optionButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();

        

        optionBgmSlider.value = PlayerPrefs.GetFloat("Option_BGMVolume");
        optionSeSlider.value = PlayerPrefs.GetFloat("Option_SEVolume");

        Animator windowAnimator = optionWindowTransform.GetComponent<Animator>();
        yield return StartCoroutine(AnimationUtility.COpenWindow(optionObject, windowAnimator));
    }



    // 옵션 창 닫기 버튼 포인터 인
    public void OnOption_CloseButtonPointerEnter() {
        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 옵션 창 닫기 버튼 포인터 다운
    public void OnOption_CloseButtonPointerDown() {
        StartCoroutine(COnOption_CloseButtonPointerDown());
    }

    private IEnumerator COnOption_CloseButtonPointerDown() {
        seSource.clip = buttonClickSound;
        seSource.Play();



        Animator windowAnimator = optionWindowTransform.GetComponent<Animator>();
        yield return StartCoroutine(AnimationUtility.CCloseWindow(optionObject, windowAnimator));
    }



    // 옵션 창 BGM 볼륨 슬라이더 수치 변경
    public void OnOption_BGMSliderValueChanged() {
        float volume = (float) Math.Round(optionBgmSlider.value, 1);
        
        optionBgmSlider.value = volume;
        PlayerPrefs.SetFloat("Option_BGMVolume", volume);
        bgmSource.volume = volume;
    }

    // 옵션 창 SE 볼륨 슬라이더 수치 변경
    public void OnOption_SESliderValueChanged() {
        float volume = (float) Math.Round(optionSeSlider.value, 1);
        
        optionSeSlider.value = volume;
        PlayerPrefs.SetFloat("Option_SEVolume", volume);
        seSource.volume = volume;
    }

    
    
    // 나가기 버튼 포인터 인
    public void OnExitButtonPointerEnter() {
        Animator animator = exitButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 나가기 버튼 포인터 아웃
    public void OnExitButtonPointerExit() {
        Animator animator = exitButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 나가기 버튼 포인터 다운
    public void OnExitButtonPointerDown() {
        StartCoroutine(COnExitButtonPointerDown());
    }

    private IEnumerator COnExitButtonPointerDown() {
        Animator animator = exitButton.GetComponent<Animator>();

        if (animator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(animator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        StartCoroutine(BGMUtility.CVolumeDown(bgmSource)); // BGM 볼륨 다운
        yield return StartCoroutine(DisplayUtility.CFadeInBlackDisplay(blackDisplay)); // 검은 화면 페이드 인

        Application.Quit(); // 게임 종료
    }

}
