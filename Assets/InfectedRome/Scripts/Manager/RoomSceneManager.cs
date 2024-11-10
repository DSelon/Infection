using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class RoomSceneManager : Singleton<RoomSceneManager> {
    
	[SerializeField] private GameObject camera;
    private AudioSource bgmSource;
    private AudioSource seSource;
    [SerializeField] private Player player;
    [SerializeField] private Creep creep;
    [SerializeField] private GameObject blackDisplay;
    [SerializeField] private GameObject treeObject;
    private GameObject treeCardBundle;
    private GameObject treeCard1Button;
    private GameObject treeCard2Button;
    private GameObject treeCard3Button;
    [SerializeField] private GameObject pauseObject;
    private Transform pauseWindowTransform;
    private GameObject pauseBackToGameButton;
    private GameObject pauseExitToMainButton;
    [SerializeField] private GameObject scoreObject;
    private Transform scoreWindowTransform;
    private Image scoreWindowImage;
    private Text scoreTitleText;
    private Text scoreScoreText;
    private GameObject scoreReplayButton;
    private Text scoreReplayButtonText;
    private GameObject scoreQuitButton;
    private Text scoreQuitButtonText;

    [Foldout("Image")]
    [SerializeField] private Sprite victoryWindowImage;
    [SerializeField] private Sprite defeatWindowImage;
    [EndFoldout]

    [Foldout("Color")]
    [SerializeField] private Color victoryFontColor;
    [SerializeField] private Color defeatFontColor;
    [EndFoldout]
    
    [Foldout("Sound")]
    [SerializeField] private AudioClip buttonPointerSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip[] victoryMusics;
    [SerializeField] private AudioClip[] defeatMusics;
    [EndFoldout]





    private void Start() {
        bgmSource = camera.GetComponents<AudioSource>()[0];
        seSource = camera.GetComponents<AudioSource>()[1];
        player.PlayerLevelUp += OnPlayerLevelUp;
        player.PlayerDeath += OnPlayerDeath;
        creep.CreepDeath += OnCreepDeath;
        treeCardBundle = treeObject.transform.GetChild(1).gameObject;
        treeCard1Button = treeCardBundle.transform.GetChild(0).gameObject;
        treeCard2Button = treeCardBundle.transform.GetChild(1).gameObject;
        treeCard3Button = treeCardBundle.transform.GetChild(2).gameObject;
        pauseWindowTransform = pauseObject.transform.GetChild(1);
        pauseBackToGameButton = pauseWindowTransform.GetChild(0).gameObject;
        pauseExitToMainButton = pauseWindowTransform.GetChild(1).gameObject;
        scoreWindowTransform = scoreObject.transform.GetChild(1);
        scoreWindowImage = scoreWindowTransform.GetComponent<Image>();
        scoreTitleText = scoreWindowTransform.GetChild(0).GetComponent<Text>();
        scoreScoreText = scoreWindowTransform.GetChild(1).GetComponent<Text>();
        scoreReplayButton = scoreWindowTransform.GetChild(2).gameObject;
        scoreReplayButtonText = scoreReplayButton.transform.GetChild(2).GetComponent<Text>();
        scoreQuitButton = scoreWindowTransform.GetChild(3).gameObject;
        scoreQuitButtonText = scoreQuitButton.transform.GetChild(2).GetComponent<Text>();



        StartCoroutine(DisplayUtility.CFadeOutBlackDisplay(blackDisplay)); // 검은 화면 페이드 아웃

        bgmSource.volume = PlayerPrefs.GetFloat("Option_BGMVolume");
        bgmSource.loop = true;
        bgmSource.Play();
        StartCoroutine(BGMUtility.CVolumeUp(bgmSource)); // BGM 볼륨 업

        seSource.volume = PlayerPrefs.GetFloat("Option_SEVolume");



        CoroutineUtility.CallWaitForSeconds(1, OnPlayerLevelUp);
    }

    private void Update() {
        if (player.IsDead
        || creep.IsDead) return;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Animator pauseAnimator = pauseWindowTransform.GetComponent<Animator>();
            if (pauseObject.activeSelf) {
                StartCoroutine(AnimationUtility.CCloseWindow(pauseObject, pauseAnimator));
                CoroutineUtility.CallWaitForSecondsRealtime(0.25f, () => { Time.timeScale = 1; });
            }
            else {
                StartCoroutine(AnimationUtility.COpenWindow(pauseObject, pauseAnimator));
                Time.timeScale = 0;
            }
        }
    }





    // 트리 창 1 버튼 포인터 인
    public void OnTree_1ButtonPointerEnter() {
        Animator animator = treeCard1Button.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 트리 창 1 버튼 포인터 아웃
    public void OnTree_1ButtonPointerExit() {
        Animator animator = treeCard1Button.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 트리 창 1 버튼 포인터 다운
    public void OnTree_1ButtonPointerDown() {
        StartCoroutine(COnTree_1ButtonPointerDown());
    }

    private IEnumerator COnTree_1ButtonPointerDown() {
        Animator buttonAnimator = treeCard1Button.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;

        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        OnTree_ButtonPointerDown(0);
    }

    // 트리 창 2 버튼 포인터 인
    public void OnTree_2ButtonPointerEnter() {
        Animator animator = treeCard2Button.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 트리 창 2 버튼 포인터 아웃
    public void OnTree_2ButtonPointerExit() {
        Animator animator = treeCard2Button.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 트리 창 2 버튼 포인터 다운
    public void OnTree_2ButtonPointerDown() {
        StartCoroutine(COnTree_2ButtonPointerDown());
    }

    private IEnumerator COnTree_2ButtonPointerDown() {
        Animator buttonAnimator = treeCard2Button.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;

        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        OnTree_ButtonPointerDown(1);
    }

    // 트리 창 3 버튼 포인터 인
    public void OnTree_3ButtonPointerEnter() {
        Animator animator = treeCard3Button.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 트리 창 3 버튼 포인터 아웃
    public void OnTree_3ButtonPointerExit() {
        Animator animator = treeCard3Button.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 트리 창 3 버튼 포인터 다운
    public void OnTree_3ButtonPointerDown() {
        StartCoroutine(COnTree_3ButtonPointerDown());
    }

    private IEnumerator COnTree_3ButtonPointerDown() {
        Animator buttonAnimator = treeCard3Button.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;

        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        OnTree_ButtonPointerDown(2);
    }

    // 트리 창 버튼 포인터 다운
    private void OnTree_ButtonPointerDown(int number) {

        // 트리 능력 활성화
        player.ability.PickCard(number, player);

        // 트리 창 닫기
        Animator treeAnimator = treeCardBundle.GetComponent<Animator>();
        StartCoroutine(AnimationUtility.CCloseWindow(treeObject, treeAnimator));
        CoroutineUtility.CallWaitForSecondsRealtime(0.25f, () => { Time.timeScale = 1; });

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

    // 일시정지 창 게임으로 돌아가기 버튼 포인터 다운
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

    // 일시정지 창 메인으로 나가기 버튼 포인터 다운
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



    // 점수 창 다시하기 버튼 포인터 인
    public void OnScore_ReplayButtonPointerEnter() {
        Animator animator = scoreReplayButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 점수 창 다시하기 버튼 포인터 아웃
    public void OnScore_ReplayButtonPointerExit() {
        Animator animator = scoreReplayButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 점수 창 다시하기 버튼 포인터 다운
    public void OnScore_ReplayButtonPointerDown() {
        StartCoroutine(COnScore_ReplayButtonPointerDown());
    }

    private IEnumerator COnScore_ReplayButtonPointerDown() {
        Animator buttonAnimator = scoreReplayButton.GetComponent<Animator>();

        if (buttonAnimator.GetBool("isPointerDown")) yield break;



        StartCoroutine(AnimationUtility.CPlayButtonPointerDownAnimation(buttonAnimator));

        seSource.clip = buttonClickSound;
        seSource.Play();



        string sceneName = "Room";
        yield return StartCoroutine(LoadUtility.CLoadScene(sceneName));

        StartCoroutine(BGMUtility.CVolumeDown(bgmSource)); // BGM 볼륨 다운
        yield return StartCoroutine(DisplayUtility.CFadeInBlackDisplay(blackDisplay)); // 검은 화면 페이드 인

        LoadUtility.operations[sceneName].allowSceneActivation = true;
        Time.timeScale = 1;
    }



    // 점수 창 나가기 버튼 포인터 인
    public void OnScore_QuitButtonPointerEnter() {
        Animator animator = scoreQuitButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", true);

        seSource.clip = buttonPointerSound;
        seSource.Play();
    }

    // 점수 창 나가기 버튼 포인터 아웃
    public void OnScore_QuitButtonPointerExit() {
        Animator animator = scoreQuitButton.GetComponent<Animator>();
        animator.SetBool("isPointerEnter", false);
    }

    // 점수 창 나가기 버튼 포인터 다운
    public void OnScore_QuitButtonPointerDown() {
        StartCoroutine(COnScore_QuitButtonPointerDown());
    }

    private IEnumerator COnScore_QuitButtonPointerDown() {
        Animator buttonAnimator = scoreQuitButton.GetComponent<Animator>();

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



    // 플레이어 레벨 업 이벤트
    private void OnPlayerLevelUp() {

        if (creep.IsDead) return;

        Time.timeScale = 0;
        
        // 트리 창 열기
        Animator treeAnimator = treeCardBundle.GetComponent<Animator>();
        StartCoroutine(AnimationUtility.COpenWindow(treeObject, treeAnimator));
        player.ability.PrintCard(player);
        treeCard1Button.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = player.ability.cards[player.ability.selectedCards[0]].Item1;
        treeCard1Button.transform.GetChild(2).GetComponent<Text>().text = player.ability.cards[player.ability.selectedCards[0]].Item2;
        treeCard1Button.transform.GetChild(3).GetComponent<Text>().text = player.ability.cards[player.ability.selectedCards[0]].Item3;
        treeCard2Button.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = player.ability.cards[player.ability.selectedCards[1]].Item1;
        treeCard2Button.transform.GetChild(2).GetComponent<Text>().text = player.ability.cards[player.ability.selectedCards[1]].Item2;
        treeCard2Button.transform.GetChild(3).GetComponent<Text>().text = player.ability.cards[player.ability.selectedCards[1]].Item3;
        treeCard3Button.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = player.ability.cards[player.ability.selectedCards[2]].Item1;
        treeCard3Button.transform.GetChild(2).GetComponent<Text>().text = player.ability.cards[player.ability.selectedCards[2]].Item2;
        treeCard3Button.transform.GetChild(3).GetComponent<Text>().text = player.ability.cards[player.ability.selectedCards[2]].Item3;

    }

    
    
    // 플레이어 사망 이벤트
    private void OnPlayerDeath() {
        if (creep.IsDead) return;

        StartCoroutine(COnPlayerDeath());
    }

    private IEnumerator COnPlayerDeath() {
        StartCoroutine(CTimeExpansionEffect());

        yield return new WaitForSecondsRealtime(2.25f);

        StartCoroutine(BGMUtility.CVolumeDown(bgmSource, 2.0f));

        yield return new WaitForSecondsRealtime(0.5f);

        bgmSource.clip = defeatMusics[Random.Range(0, defeatMusics.Length)];
        // bgmSource.clip = victoryMusics[Random.Range(0, victoryMusics.Length)];
        StartCoroutine(BGMUtility.CVolumeUp(bgmSource, 2.0f));
        bgmSource.Play();

        yield return new WaitForSecondsRealtime(0.25f);

        scoreWindowImage.sprite = defeatWindowImage;
        scoreTitleText.text = "DEFEAT";
        scoreTitleText.color = defeatFontColor;
        scoreScoreText.text = "Score: " + (player.Level + 1).ToString();
        scoreReplayButtonText.color = defeatFontColor;
        scoreQuitButtonText.color = defeatFontColor;
        // scoreWindowImage.sprite = victoryWindowImage;
        // scoreTitleText.text = "VICTORY";
        // scoreTitleText.color = victoryFontColor;
        // scoreScoreText.text = "Score: " + (player.Level + 1).ToString();
        // scoreReplayButtonText.color = victoryFontColor;
        // scoreQuitButtonText.color = victoryFontColor;
        Animator scoreAnimator = scoreWindowTransform.GetComponent<Animator>();
        StartCoroutine(AnimationUtility.COpenWindow(scoreObject, scoreAnimator));
    }



    // Creep 사망 이벤트
    private void OnCreepDeath() {
        if (player.IsDead) return;

        StartCoroutine(COnCreepDeath());
    }

    private IEnumerator COnCreepDeath() {
        StartCoroutine(CTimeExpansionEffect());

        yield return new WaitForSecondsRealtime(2.25f);

        StartCoroutine(BGMUtility.CVolumeDown(bgmSource, 2.0f));

        yield return new WaitForSecondsRealtime(0.5f);

        // bgmSource.clip = defeatMusics[Random.Range(0, defeatMusics.Length)];
        bgmSource.clip = victoryMusics[Random.Range(0, victoryMusics.Length)];
        StartCoroutine(BGMUtility.CVolumeUp(bgmSource, 2.0f));
        bgmSource.Play();

        yield return new WaitForSecondsRealtime(0.25f);

        // scoreWindowImage.sprite = defeatWindowImage;
        // scoreTitleText.text = "DEFEAT";
        // scoreTitleText.color = defeatFontColor;
        // scoreScoreText.text = "Score: " + (player.Level + 1).ToString();
        // scoreReplayButtonText.color = defeatFontColor;
        // scoreQuitButtonText.color = defeatFontColor;
        scoreWindowImage.sprite = victoryWindowImage;
        scoreTitleText.text = "VICTORY";
        scoreTitleText.color = victoryFontColor;
        scoreScoreText.text = "Score: " + (player.Level + 1).ToString();
        scoreReplayButtonText.color = victoryFontColor;
        scoreQuitButtonText.color = victoryFontColor;
        Animator scoreAnimator = scoreWindowTransform.GetComponent<Animator>();
        StartCoroutine(AnimationUtility.COpenWindow(scoreObject, scoreAnimator));
    }



    private IEnumerator CTimeExpansionEffect() {
        float timeScaleChangeSpeed = 0.3f;
        float differenceTimeScale = Time.timeScale;
        float currentTimeScale = Time.timeScale;
        while (currentTimeScale > 0.2) {
            currentTimeScale -= differenceTimeScale * timeScaleChangeSpeed * Time.unscaledDeltaTime;
            Time.timeScale = currentTimeScale;

            yield return null;
        }
        Time.timeScale = 0.2f;
    }

}
