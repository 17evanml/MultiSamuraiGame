using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class playerController : NetworkBehaviour
{
    public List<Vector2> locations = new List<Vector2>();
    public Vector2 beginning;
    public  Vector2 destination;
    bool tempvar = true;
    //private CombatHandler handler;
    public float speed = 10;
    private int attack;
    private int maxLength = 5;
    private float pathLength = 0;
    float t = 0;
    public bool Moving = false;
    // Start is called before the first frame update
    void Start() {
        if(isServer)
        {
            GameObject.Find("CombatHandler").GetComponent<CombatHandler>().players.Add(this);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0) && Moving != true)
            {   
                Vector2 normalizedMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 tempLoc = new Vector2(normalizedMouse.x, normalizedMouse.y);
                if (!isServer)
                {
                    CmdStoreLocation(tempLoc);
                } 
                StoreLocation(tempLoc);
            }
        }
    }

    void Update()
    {
        if (Moving)
        {
           gameObject.transform.position = Vector2.Lerp(beginning, destination, t);
            t += Time.deltaTime * speed / (destination - beginning).magnitude;
        }
    }

    public CombatBean Post()
    {
        return new CombatBean(attack, locations, this.gameObject);
    }

    [Command]
    void CmdStoreLocation(Vector2 mousePos)
    {
        if (pathLength >= maxLength)
        {
            return;
        }
        Vector2 sizeCalc;
        Vector2 startVec;
        if (locations.Count != 0)
        {
            startVec = locations[locations.Count - 1];
            sizeCalc = mousePos - startVec;
        }
        else
        {
            startVec = toVector2(gameObject.transform.position);
            sizeCalc = mousePos - startVec;
        }
        float mag = sizeCalc.magnitude;
        if (pathLength + mag > maxLength)
        {
            sizeCalc *= (1 / (mag));
            sizeCalc *= maxLength - pathLength;
            mousePos = sizeCalc + startVec;
        }
        pathLength += sizeCalc.magnitude;
        locations.Add(mousePos);
    }

    void StoreLocation(Vector2 mousePos)
    {
        if (pathLength >= maxLength)
        {
            return;
        }
        Vector2 sizeCalc;
        Vector2 startVec;
        if (locations.Count != 0) {
            startVec = locations[locations.Count - 1];
            sizeCalc = mousePos - startVec;
        }
        else {
            startVec = toVector2(gameObject.transform.position);
            sizeCalc = mousePos - startVec;
        }
        float mag = sizeCalc.magnitude;
        if (pathLength + mag > maxLength)
        {
            sizeCalc *= (1 / (mag));
            sizeCalc *= maxLength - pathLength;
            mousePos = sizeCalc + startVec;
        }
        pathLength += sizeCalc.magnitude;
        locations.Add(mousePos);
    }

    public void addLocation(Vector2 location)
    {
        if (!isLocalPlayer)
        {
            {
                locations.Add(location);
            }
        }
    }

    public void setAttack(int attackIn)
    {
        attack = attackIn;
    }


    [ClientRpc]
    public void RpcUpdateLocation(Vector2 location)
    {
        if (!isLocalPlayer)
        {
            addLocation(location);
        }
    }
    [ClientRpc]
    public void RpcUpdateClientAttack(int attack)
    {
        setAttack(attack);
    }
    [ClientRpc]
    public void RpcClearCLient()
    {
            setAttack(0);
            locations.Clear();
            pathLength = 0;
    }
    [ClientRpc]
    public void RpcToggleMoving()
    {
        if(Moving == true)
        {
            Moving = false;
        }
        else
        {
            print("rpc start moving");
            Moving = true;
            StartCoroutine(Navigation());
        }
    }

    IEnumerator Navigation() {
        for (int i = 0; i < locations.Count; i++)
        {
            t = 0;
            if(i == 0)
            {
                beginning = transform.position;
                destination = locations[0]/*-toVector2(transform.position)*/;

            }
            else
            {
                destination = locations[i]/*-locations[i-1]*/;
                beginning = locations[i - 1];
            }
            yield return new WaitUntil(() => toVector2(gameObject.transform.position) == locations[i]);
            print("next locations");
        }
        print("change moving back to false");
        Moving = false;
    }




    Vector2 toVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
    Vector3 toVector3(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}