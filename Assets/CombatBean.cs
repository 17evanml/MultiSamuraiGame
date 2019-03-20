using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBean
{
    enum AttackType { Null, Attack, Grab, Block };
    private List<Vector2> locations;
    private int attack;
    private GameObject player;
    public CombatBean(int attackIn, List<Vector2> locationsIn, GameObject playerIn)
    {
        attack = attackIn;
        locations = locationsIn;
        player = playerIn;
    }

    public List<Vector2> GetLocations()
    {
        return locations;
    }
    public int GetAttack()
    {
        return attack;
    }
    public GameObject GetPlayer()
    {
        return player;
    }
}