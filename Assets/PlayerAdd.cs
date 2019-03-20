using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAdd : NetworkManager
{
    public override void OnStartServer()
    {
        CombatHandler handler = GameObject.Find("CombatHandler").GetComponent<CombatHandler>();
        handler.StartCoroutine(handler.Timer(3));
        base.OnStartServer();

    }
}
