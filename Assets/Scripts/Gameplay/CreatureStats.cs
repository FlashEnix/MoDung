using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScript))]
public class CreatureStats : MonoBehaviour, ITailObject
{
    public string creatureName = "Персонаж";
    public Sprite avatar;

    public int HP = 5;
    public int MP = 0;
    public int minATK = 1;
    public int maxATK = 3;
    public int minDEF = 0;
    public int maxDEF = 2;
    public int SPD = 3;
    public int rangeAttack = 0;
    public int team;

    public int maxHP { get; private set; }

    public PlayerScript playerScript;
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        maxHP = HP;
    }
}
