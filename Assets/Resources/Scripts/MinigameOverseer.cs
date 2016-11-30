using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinigameOverseer : MonoBehaviour {
    [HideInInspector]
    public int time, reps;
    [HideInInspector]
    public string name;

    public Vector3 camPos;
    public Quaternion camAngle;

    public GameObject tutoPanel,scoreGO,timeGO;
    public Text timeUI, scoreUI;
    public RectTransform sign3, sign2, sign1, signGO;

    public float camTranslationTime,textTranslationTime;
    public Vector2 initialTextPos;

    private bool onGameplay,gameActivated,initialAnimSetUpDone;
    private int animationStage;

    private float animStartTime;
    private Vector3 initialCamPos;
    private Quaternion initialCamRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(gameActivated)
        {
	        if(!onGameplay)
            {
                switch(animationStage)
                {
                    case 0: AnimateCam(); break;
                    case 1: anim3Text(); break;
                    case 2: anim2Text(); break;
                    case 3: anim1Text(); break;
                    case 4: animGOText(); break;
                }
            }

        }
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

    private void anim3Text()
    {
        if (initialAnimSetUpDone)
        {
            float time;
            time = (Time.time - animStartTime) / textTranslationTime;
            sign3.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
            if (time >= 1.0f)
            {
                animationStage++;
                sign3.anchoredPosition = initialTextPos;
                animStartTime = Time.time;
                initialAnimSetUpDone = false;
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
        }
    }
    private void anim2Text()
    {
        if (initialAnimSetUpDone)
        {
            float time;
            time = (Time.time - animStartTime) / textTranslationTime;
            sign2.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
            if (time >= 1.0f)
            {
                animationStage++;
                sign2.anchoredPosition = initialTextPos;
                animStartTime = Time.time;
                initialAnimSetUpDone = false;
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
        }
    }
    private void anim1Text()
    {
        if (initialAnimSetUpDone)
        {
            float time;
            time = (Time.time - animStartTime) / textTranslationTime;
            sign1.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
            if (time >= 1.0f)
            {
                animationStage++;
                sign1.anchoredPosition = initialTextPos;
                animStartTime = Time.time;
                initialAnimSetUpDone = false;
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
        }
    }
    private void animGOText()
    {
        if (initialAnimSetUpDone)
        {
            float time;
            time = (Time.time - animStartTime) / textTranslationTime;
            signGO.anchoredPosition = Vector2.Lerp(initialTextPos, Vector2.zero, time);
            if (time >= 1.0f)
            {
                signGO.anchoredPosition = initialTextPos;
                StartGameplayTime();
            }
        }
        else
        {
            animStartTime = Time.time;
            initialAnimSetUpDone = true;
        }
    }

    private void StartGameplayTime()
    {
        timeGO.SetActive(true);
        scoreGO.SetActive(true);
    }

    public void StartGame(int time , int reps)
    {
        animationStage = 0;
        onGameplay = false;
        gameActivated = true;
        initialAnimSetUpDone = false;
    }
}
