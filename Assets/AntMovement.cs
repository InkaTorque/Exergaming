using UnityEngine;
using System.Collections;

public class AntMovement : MonoBehaviour {

    public GameObject goodParticle;
    public float speed,deathTime;
    public Transform target;

    private bool stop, launchToDeath;
    private float time,startTime;

    private Vector3 positionAtDeath,deathPos;
    private GameObject particle;

    // Use this for initialization
    void Start()
    {
        stop = false; launchToDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToCake();
    }

    private void MoveToCake()
    {
        if(!stop)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position,target.position)<=0.1)
            {
                stop = true;
            }

        }
        else
        {
            if(launchToDeath)
            {
                time = (Time.time-startTime) / deathTime;
                transform.position = Vector3.Lerp(positionAtDeath, deathPos, time);
                if(time>=1)
                {
                    particle = GameObject.Instantiate(goodParticle);
                    particle.transform.position = gameObject.transform.position;
                    Destroy(particle, 1.5f);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Eliminate()
    {
        stop = true;
        launchToDeath = true; 
        startTime = Time.time;
        positionAtDeath = transform.position;
        deathPos.x = positionAtDeath.x;
        deathPos.y = positionAtDeath.y + 6f;
        deathPos.z = positionAtDeath.z + 3.5f;

    }
}
