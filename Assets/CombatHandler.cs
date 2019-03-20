using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CombatHandler : NetworkBehaviour {
public List<playerController> players = new List<playerController>();
    private List<CombatBean> attackFrame = new List<CombatBean>();
    private List<List<CombatBean>> history = new List<List<CombatBean>>();

    void updateClients()
    {
        for (int i = 0; i < players.Count; i++)
        {
            List<Vector2> locations = attackFrame[i].GetLocations();
            playerController player = attackFrame[i].GetPlayer().GetComponent<playerController>();
            for (int j = 0; j < locations.Count; j++)
            {
                UpdateClientLocation(locations[j], player);
            }
            UpdateClientAttack(attackFrame[i].GetAttack(), player);
        }
    }

    public void register(playerController player)
    {
        players.Add(player);
    }


    private void Start()
    {
        StartCoroutine(Timer(3));
    }

    void collect()
    {
        attackFrame.Clear();
        for (int i = 0; i < players.Count; i++)
        {
            attackFrame.Add(players[i].Post());
        }
    }
    private void Update()
    {
        //print(PlayersComplete());
    }


    void UpdateClientLocation(Vector2 location, playerController player)
    {
        if (!isServer)
        {
            player.RpcUpdateLocation(location);
        }
    }
    void UpdateClientAttack(int attack, playerController player)
    {
        player.RpcUpdateClientAttack(attack);
    }

    public IEnumerator Timer(int seconds)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].RpcClearCLient();
        }
        yield return new WaitForSeconds(seconds);
        StartCoroutine(startMoving());

    }

    IEnumerator startMoving()
    {
        collect();
        updateClients();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].RpcToggleMoving();
        }
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(PlayersComplete);
      
        StartCoroutine(Timer(3));
    }

    public bool PlayersComplete()
    {
        for(int i = 0; i < players.Count; i++)
        {
            print("I: " + i);
            print("IsMoving: "  + players[i].Moving);
            if(players[i].Moving == true)
            {
                print("returning false");
                return false;
            }
        }
        print("returning true");
        return true;
    }





}