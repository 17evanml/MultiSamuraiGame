using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {
    enum attack { Null, Attack, Defend, Grab };
    public Click(float inX, float inY, int inType)
    {
        xPos = inX;
        yPos = inY;
        attackType = inType;
    }
    
    float xPos;
    float yPos;
    int attackType;
}
