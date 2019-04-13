using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CameraFollow : NetworkBehaviour
{
    GameObject player;
    bool moving = true;
    //public GameObject[] players;
    public List<GameObject> players = new List<GameObject>();

    Vector3 center = new Vector3(0.0f, 0.0f, -10.0f);
    public float borderSize = 1.0f;
    float maxX = 0.0f, minX = 0.0f, maxY = 0.0f, minY = 0.0f;
    float size = 5.0f;
    float minSize = 5.0f;
    public Camera ourCamera;
    private Vector3 velocity;
    public float smoothTime = 0.3f;

    private float velocity2;
    public float smoothTime2 = 0.3f;

    private void Start()
    {
        //Find all players in game
        //players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        if (moving)
        {
            //1. Calculating size of boundary box from origin
            //Reset max and Min
            maxX = 0.0f;
            minX = 0.0f;
            maxY = 0.0f;
            minY = 0.0f;

            //Figure out furthest player on x+, x-, y+, and y-
            foreach (GameObject player in players)
            {
                if (player.transform.position.x > maxX) { maxX = player.transform.position.x; }
                if (player.transform.position.x < minX) { minX = player.transform.position.x; }
                if (player.transform.position.y > maxY) { maxY = player.transform.position.y; }
                if (player.transform.position.y < minY) { minY = player.transform.position.y; }
            }

            //Find center of boundary box
            center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, center.z);


            //check for bigger maxX * 1/aspect or maxY
            if ((maxX - center.x + borderSize) * 0.5625f < minSize && maxY + borderSize - center.y < minSize)
            {
                ourCamera.orthographicSize = Mathf.SmoothDamp(ourCamera.orthographicSize, minSize, ref velocity2, smoothTime2);
            }
            else if ((maxX - center.x + borderSize) * 0.5625f > maxY + borderSize - center.y)
            {
                ourCamera.orthographicSize = Mathf.SmoothDamp(ourCamera.orthographicSize, ((maxX - center.x + borderSize) * 0.5625f), ref velocity2, smoothTime2);
            }
            else
            {
                ourCamera.orthographicSize = Mathf.SmoothDamp(ourCamera.orthographicSize, (maxY + borderSize - center.y), ref velocity2, smoothTime2);
            }

            transform.position = Vector3.SmoothDamp(transform.position, center, ref velocity, smoothTime);
        }
    }

    [ClientRpc]
    public void RpcToggleMoving()
    {
        if (moving == true)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }
    }
}