using System;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public static DamageSystem instance;
    public event Action<CreatureStats, IDamage> OnDealDamage;
    public event Action<CreatureStats, IDamage> OnDeathPlayer;
    public event Action<CreatureStats, int> OnHeal;

    private void Awake()
    {
        instance = this;
    }

    public void DealDamage(CreatureStats from, CreatureStats to, int number, IDamage damage = null)
    {
        if (damage == null) damage = new BaseDamage();

        int realDamage = damage.GetCalculate(to, number);

        to.HP -= realDamage;

        if (to.HP <= 0)
        {
            OnDeathPlayer?.Invoke(to, damage);
        } 
        Debug.LogFormat("{0} наносит {1} урон {2}", from.name, realDamage, to.name);
        OnDealDamage?.Invoke(to,damage);
    }

    public void Heal(CreatureStats from, CreatureStats to, int number)
    {
        int newHP = to.HP + number;

        if (newHP > to.maxHP)
        {
            to.HP = to.maxHP;
        } else
        {
            to.HP = newHP;
        }
        OnHeal?.Invoke(to, number);
    }
}
