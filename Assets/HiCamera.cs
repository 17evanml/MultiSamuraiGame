using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiCamera : MonoBehaviour
{
    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");
        //add itself to the camera follow players array       
        camera.GetComponent<CameraFollow>().players.Add(gameObject);
    }


}
