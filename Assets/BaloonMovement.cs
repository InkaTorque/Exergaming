using UnityEngine;
using System.Collections;

public class BaloonMovement : MonoBehaviour {

    public GameObject  goodParticle, badParticle;
    public float speed;

    public BaloonMinigame bm;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        GoUp();
	}

    private void GoUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed*Time.deltaTime, transform.position.z);
        if(transform.position.y>=-176)
        {
            bm.SpawnNextBalloon();
            Destroy(gameObject);
        }
    }
    
    public void Eliminate(bool valid)
    {
        GameObject particle=null;
        if(valid)
        {
            particle = GameObject.Instantiate(goodParticle);
        }
        else
        {
            particle = GameObject.Instantiate(badParticle);
        }
        particle.transform.position = gameObject.transform.position;
        bm.SpawnNextBalloon();
        Destroy(gameObject);
        Destroy(particle, 1.5f);
    }
}
