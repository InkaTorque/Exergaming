using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinigameOverseer : MonoBehaviour {
    [HideInInspector]
    public int time, reps;
    [HideInInspector]
    public string name;

    public GameObject miniGameBoss;
    public Vector3 camPos;
    public Quaternion camAngle;

    public GameObject tutoPanel,scoreGO,timeGO,resultsPanel,starPannel;
    public Image tutoProgressBar,resultsProgressBar;
    public Text timeUI, scoreUI;
    public RectTransform signBackground,sign3, sign2, sign1, signGO;

    public float camTranslationTime,textTranslationTime,tutoWaitTime;
    public Vector2 initialTextPos,iniitalBackgroundPos;

    private bool onGameplay,gameActivated,initialAnimSetUpDone,arrivedAtDestination,onTutoShowing,initialTutSetUpDone,onResults,resultsSetUPdONE;
    private int animationStage;

    private float animStartTime,holdTimer,tutoHoldTimer,resultsHoldTimer;
    private Vector3 initialCamPos;
    private Quaternion initialCamRotation;

    private float currentTime;

    public Text tutoGameName, tutoGameDescription , tutoGameShadow , resultsScoreText,resultNameTex;
    public string gameName, GameDescription;
    public Sprite[] gameTutFrames;
    public ImageUIAnimator iuia;
    public GameObject[] stars;

    [HideInInspector]
    public int currentScore;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(gameActivated)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentTime = 0;
                ShowResultsScreen();
            }
            if(Input.GetKeyDown(KeyCode.G))
            {
                GestureDone();
            }
	        if(!onGameplay)
            {
                if(onResults)
                {
                    if(resultsSetUPdONE)
                    {
                        resultsHoldTimer = resultsHoldTimer + Time.deltaTime;
                        if (resultsProgressBar != null)
                        {
                            resultsProgressBar.fillAmount = (resultsHoldTimer / tutoWaitTime);
                        }
                        if (resultsHoldTimer >= tutoWaitTime)
                        {
                            if (resultsPanel != null)
                            {
                                resultsPanel.SetActive(false);
                            }
                            gameActivated = false;
                            LaunchNewActivity();
                        }
                    }
                    else
                    {
                        FillResultsPanel();
                    }
                }
                else
                {
                    switch (animationStage)
                    {
                        case 0: AnimateCam(); break;
                        case 1: ShowTut(); break;
                        case 2: AnimBackground(); break;
                        case 3: anim3Text(); break;
                        case 4: anim2Text(); break;
                        case 5: anim1Text(); break;
                        case 6: animGOText(); break;
                    }

                }
            }
            else
            {
                CountDown();
            }

        }
	}

    private void LaunchNewActivity()
    {
        miniGameBoss.SendMessage("EndMinigame");
        GameManager.instance.stratRandomGame();
    }

    private void ShowTut()
    {
        if(initialTutSetUpDone)
        {
            tutoHoldTimer = tutoHoldTimer + Time.deltaTime;
            if(tutoProgressBar!=null)
            {
                tutoProgressBar.fillAmount = (tutoHoldTimer / tutoWaitTime);
            }
            if(tutoHoldTimer>=tutoWaitTime)
            {
                if(tutoPanel!=null)
                {
                    tutoPanel.SetActive(false);
                }
                animationStage++;
            }
        }
        else
        {
            if(tutoPanel!=null)
            {
                tutoPanel.SetActive(true);
            }
            tutoGameName.text = gameName;
            tutoGameShadow.text = gameName;
            iuia.ActivateAnimation(gameTutFrames);
            tutoGameDescription.text = GameDescription;
            tutoHoldTimer = 0f;
            initialTutSetUpDone = true;
        }
    }
    

    private void CountDown()
    {
        currentTime -= Time.deltaTime;
        timeUI.text = Mathf.Round(currentTime).ToString();
        if (currentTime <= 0)
        {
            ShowResultsScreen();
        }
    }

    private void ShowResultsScreen()
    {
        onGameplay = false;
        onResults = true;
        resultsPanel.SetActive(true);
        timeGO.SetActive(false);
        scoreGO.SetActive(false);
        resultsHoldTimer = 0f;
        resultsSetUPdONE = false;

    }
    private void FillResultsPanel()
    {
        for (int i = 0; i < 5; i++)
        {
            stars[i].SetActive(false);
        }
        resultNameTex.text = gameName;
        resultsScoreText.text = "" + currentScore.ToString() + "/" + reps.ToString();
        float ratio = currentScore / reps;
        float startNumber=0;
        Debug.Log("RATO " + ratio + "CS "+currentScore + "REPS "+reps);
        if(ratio>=0 && ratio<=0.2)
        {
            startNumber = 1;
        }
        else if (ratio > 0.2 && ratio <= 0.4)
        {
            startNumber = 2;
        }else if (ratio > 0.4 && ratio <= 0.6)
        {
            startNumber = 3;
        }else if (ratio > 0.6 && ratio <= 0.8)
        {
            startNumber = 4;
        }else 
        {
            startNumber = 5;
        }
        for(int j =0;j<startNumber;j++)
        {
            stars[j].SetActive(true);
        }
        resultsSetUPdONE = true;
    }

    private void AnimateCam()
    {
        if(initialAnimSetUpDone)
        {
            float time;
            time  = (Time.time - animStartTime) / camTranslationTime;
            Camera.main.transform.position = Vector3.Lerp(initialCamPos, camPos, time);
            Camera.main.transform.rotation = Quaternion.Lerp(initialCamRotation,camAngle,time);
            if(time>=1.0f)
            {
                animationStage++;
                animStartTime = Time.time;
                initialAnimSetUpDone = false;
            }
        }
        else
        {
            animStartTime = Time.time;
            initialCamPos = Camera.main.transform.position;
            initialCamRotation = Camera.main.transform.rotation;
            initialAnimSetUpDone = true;
        }
    }
    private void AnimBackground()
    {
        if (initialAnimSetUpDone)
        {
            float time;
            time = (Time.time - animStartTime) / textTranslationTime;
            signBackground.anchoredPosition = Vector2.Lerp(iniitalBackgroundPos, Vector2.zero, time);
            if (time >= 1.0f)
            {
                holdTimer = 0f;
                arrivedAtDestination = false;
                initialAnimSetUpDone = false;
                animationStage++;
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
            arrivedAtDestination = false;
        }
    }

    private void anim3Text()
    {
        if (initialAnimSetUpDone)
        {
            if (!arrivedAtDestination)
            {
                float time;
                time = (Time.time - animStartTime) / textTranslationTime;
                sign3.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
                if (time >= 1.0f)
                {
                    holdTimer = 0f;
                    arrivedAtDestination = true;
                }

            }
            else
            {
                holdTimer = holdTimer + Time.deltaTime;
                if (holdTimer >= 0.5f)
                {
                    animationStage++;
                    sign3.anchoredPosition = initialTextPos;
                    initialAnimSetUpDone = false;
                    animStartTime = Time.time;
                }
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
            arrivedAtDestination = false;
        }
    }
    private void anim2Text()
    {
        
        if (initialAnimSetUpDone)
        {
            if (!arrivedAtDestination)
            {
                float time;
                time = (Time.time - animStartTime) / textTranslationTime;
                sign2.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
                if (time >= 1.0f)
                {
                    holdTimer = 0f;
                    arrivedAtDestination = true;
                }

            }
            else
            {
                holdTimer = holdTimer + Time.deltaTime;
                if (holdTimer >= 0.5f)
                {
                    initialAnimSetUpDone = false;
                    sign2.anchoredPosition = initialTextPos;
                    animationStage++;
                    animStartTime = Time.time;
                }
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
            arrivedAtDestination = false;
        }
    }
    private void anim1Text()
    {
        if (initialAnimSetUpDone)
        {
            if (!arrivedAtDestination)
            {
                float time;
                time = (Time.time - animStartTime) / textTranslationTime;
                sign1.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
                if (time >= 1.0f)
                {
                    holdTimer = 0f;
                    arrivedAtDestination = true;
                }

            }
            else
            {
                holdTimer = holdTimer + Time.deltaTime;
                if (holdTimer >= 0.5f)
                {
                    initialAnimSetUpDone = false;
                    sign1.anchoredPosition = initialTextPos;
                    animationStage++;
                    animStartTime = Time.time;
                }
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
            arrivedAtDestination = false;
        }
    }
    private void animGOText()
    {
        
        if (initialAnimSetUpDone)
        {
            if (!arrivedAtDestination)
            {
                float time;
                time = (Time.time - animStartTime) / textTranslationTime;
                signGO.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
                if (time >= 1.0f)
                {
                    holdTimer = 0f;
                    arrivedAtDestination = true;
                }

            }
            else
            {
                holdTimer = holdTimer + Time.deltaTime;
                if (holdTimer >= 0.5f)
                {
                    signGO.anchoredPosition = initialTextPos;
                    StartGameplayTime();
                }
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
            arrivedAtDestination = false;
        }
    }

    private void StartGameplayTime()
    {
        signBackground.anchoredPosition = iniitalBackgroundPos;
        timeGO.SetActive(true);
        scoreGO.SetActive(true);
        timeUI.text = time.ToString();
        scoreUI.text = " 0/" + reps;
        currentTime = time;
        onGameplay = true;
        miniGameBoss.SendMessage("StartMinigame");
        GameManager.instance.allowInput = true;
    }

    public void StartGame(int _time , int _reps)
    {
        onResults = false;
        animationStage = 0;
        onGameplay = false;
        gameActivated = true;
        time = _time;
        reps = _reps;
        initialAnimSetUpDone = false;
        arrivedAtDestination = false;
        onTutoShowing = true;
        initialTutSetUpDone = false;
        tutoHoldTimer = 0f;
        resultsSetUPdONE = false;
    }

    public void SaveCameraState()
    {
        camAngle = Camera.main.transform.rotation;
        camPos = Camera.main.transform.position;
    }

    public void GestureDone()
    {
        if(onGameplay)
        {
            miniGameBoss.SendMessage("GestureDone");
            StartCoroutine(disableInputTemp());
        }
    }

    IEnumerator disableInputTemp()
    {
        GameManager.instance.allowInput = false;
         yield return new WaitForSeconds(0.5f);
        GameManager.instance.allowInput = true;
    }

    public void ProcessEvaluation(bool validity)
    {
        if(validity)
        {
            GameManager.instance.PlaySuccess();
            UPdateScore();
        }
        else
        {
            GameManager.instance.PlayFail();
            //play negative sound
        }
    }

    private void UPdateScore()
    {
        currentScore++;
        scoreUI.text = currentScore+"/" + reps;
    }
}
