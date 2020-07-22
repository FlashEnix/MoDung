using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamage : IDamage
{
    public int GetCalculate(CreatureStats creature, int damage)
    {
        int def = Random.Range(creature.minDEF, creature.maxDEF);
        int result = damage - def;
        if (result > 0) return result;
        else return 0;
    }
}
