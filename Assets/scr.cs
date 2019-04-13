using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr : MonoBehaviour {
    //Character Variables
    public float startHealth = 100.0f;
    float health; 
    public Image healthbar;


    Vector3 Pos1 = new Vector3(0, 1);
    Vector3 Pos2 = new Vector3(2, 4);

    public bool canMove = true;

    //Need this for alphaLerp DONT TOUCH
    Vector3 netVector = Vector3.zero;
    Vector3 oldPosition = Vector3.zero;
    public float velocity = 0.2f;
    float goldenRatio = 0.0f;

    //Need this for hit/hurt-boxes
    enum State { None, Attack, Block, Grab };
    public int activeState = (int)State.None;
    public float hitSize = 2.0f;
    public float hurtSize = 2.0f;
    int hitState = (int)State.None;
    bool Updated = false;
    Collider2D ownCircle;

    private void Start()
    {
    //    Debug.Log("z = attack, x = block, c = grab");
        health = startHealth;

        //Finding ownCollider
        ownCircle = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (canMove) { shitMovement(); }
        
        shitState();
        
        if (Input.GetKeyDown("2"))
        {
            getHit();
        }

        if (Input.GetKeyDown("1"))
        {
            health -= 10;
            healthbar.fillAmount = health / startHealth;
        }

        alphaLerp(ref netVector, velocity);
        hurtBox();
       // Debug.Log("Active: " + (State)activeState + "| Hit: " + (State)hitState);
    }

    //delet this
    //====================================================================================
    void shitMovement()
    {
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            netVector += Vector3.right;
        }
        if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {
            netVector += Vector3.left;
        }
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            netVector += Vector3.up;
        }
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
        {
            netVector += Vector3.down;
        }
        if (Input.GetKeyDown("g"))
        {
            netVector += new Vector3(-2, 2, 0);
        }
    }

    void shitState()
    {
        if (Input.GetKeyDown("z"))
        {
            activeState = 1;
        }
        else if (Input.GetKeyDown("x"))
        {
            activeState = 2;
        }
        else if (Input.GetKeyDown("c"))
        {
            activeState = 3;
        }

    }
    //====================================================================================

    void alphaLerp(ref Vector3 netVector, float velocity)
    {
        //fuck floats tbh
        if (goldenRatio <= 0.001f)
        {
            oldPosition = transform.position;
        }

        //INITIALIZE PARAMETERS
        //only on x and y dont know if that matters at all
        float xDist = Mathf.Abs(netVector.x);
        float yDist = Mathf.Abs(netVector.y);
        float totDist = Mathf.Sqrt(xDist * xDist + yDist * yDist);
        //calc time of travel
        float totTime = totDist / velocity;
        
        //increase the ratio and clamp it from 0 to 100
        goldenRatio += Time.deltaTime/totTime;
        goldenRatio = Mathf.Clamp(goldenRatio, 0.0f, 1.0f);

        transform.position = oldPosition + goldenRatio * netVector;

        //If time taken is larger than total time, then stop running, reset timeTaken, and set netVector to Vector3.zero
        if (goldenRatio >= 0.999f)
        {
            netVector = Vector3.zero;
            goldenRatio = 0.0f;
        }

    }


    void hurtBox()
    {
        //Check all colliders hit
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hurtSize);
        if (colliders.Length > 1)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                //Skip ownCircle
                if (colliders[i] != ownCircle)
                {
                    //Get information from objects about what is touching it
                    scr thisObject = colliders[i].gameObject.GetComponent<scr>();
                    hitState = thisObject.activeState;
                }
                
            }
        }
        else
        {
            hitState = (int)State.None;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, hurtSize);
    }


    void getHit()
    {
        //To do: based on hitState and activeState make a decision on what to do
        if (activeState == (int)State.None)
        {
            if (hitState == (int)State.Attack)
            {

            }
            else if (hitState == (int)State.Block)
            {

            }
            else if (hitState == (int)State.Grab)
            {

            }
        }
        else if (activeState == (int)State.Attack)
        {
            if (hitState == (int)State.Block)
            {

            }
            else if (hitState == (int)State.Grab)
            {

            }
        }

        else if (activeState == (int)State.Block)
        {
            if (hitState == (int)State.Attack)
            {

            }
            else if (hitState == (int)State.Grab)
            {

            }
        }
        else if (activeState == (int)State.Grab)
        {
            if (hitState == (int)State.Attack)
            {

            }
            else if (hitState == (int)State.Block)
            {

            }
        }

    }
}
    

