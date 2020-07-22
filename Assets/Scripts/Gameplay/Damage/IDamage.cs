using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    int GetCalculate(CreatureStats creature,int damage);
}
