using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarScript : MonoBehaviour
{
    public CreatureStats Creature;
    public RectTransform HealthBar;

    private Camera _camera;

    void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        DamageSystem.instance.OnDealDamage += Instance_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer += Instance_OnDeathPlayer;
        DamageSystem.instance.OnHeal += Instance_OnHeal;
    }

    private void Instance_OnHeal(CreatureStats creature, int hp)
    {
        if (creature == Creature)
        {
            CalcHP();
        }
    }

    private void OnDestroy()
    {
        DamageSystem.instance.OnDealDamage -= Instance_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer -= Instance_OnDeathPlayer;
    }

    private void OnDisable()
    {
        DamageSystem.instance.OnDealDamage -= Instance_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer -= Instance_OnDeathPlayer;
    }

    private void Instance_OnDeathPlayer(CreatureStats creature, IDamage damage)
    {
        if (creature == Creature)
        {
            gameObject.SetActive(false);
        }
    }

    private void Instance_OnDealDamage(CreatureStats creature, IDamage damage)
    {
        if (creature == Creature)
        {
            CalcHP();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CreaturePostition = _camera.WorldToScreenPoint(Creature.transform.position + Vector3.up);
        transform.position = new Vector3(CreaturePostition.x, CreaturePostition.y, 0);
    }

    public void CalcHP()
    {
        float curHp = (float)Creature.HP / (float)Creature.maxHP;
        HealthBar.localScale = new Vector3(curHp, 1, 1);
    }
}
