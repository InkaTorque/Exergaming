using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageUIAnimator : MonoBehaviour {

    public Sprite[] AnimationFrames;
    public float frameChangeTime;

    private bool animationEnabled=false;
    private int currentImageIndex;

    private bool timeToChangeFrame;
    private float frameChangeTimer;

	void OnEnable()
    {
    }

    void Update()
    {
        if(animationEnabled==true)
        {
            AnimateTutorialSection();
        }
    }

    public void ActivateAnimation(Sprite[] images)
    {
        AnimationFrames = images;
        animationEnabled = true;
        currentImageIndex = 0;
        timeToChangeFrame = true;
    }

    void AnimateTutorialSection()
    {
        if(timeToChangeFrame)
        {
            GetComponent<Image>().sprite = AnimationFrames[currentImageIndex];
            timeToChangeFrame = false;
            frameChangeTimer = 0f;
        }
        else
        {
            frameChangeTimer = frameChangeTimer + Time.deltaTime;
            if(frameChangeTimer>=frameChangeTime)
            {
                currentImageIndex++;
                if(currentImageIndex>=AnimationFrames.Length)
                {
                    currentImageIndex = 0;
                }
                timeToChangeFrame = true;
            }
        }
    }
}
