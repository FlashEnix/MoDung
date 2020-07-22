using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AvatarScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CreatureStats Creature;
    [SerializeField] private GameObject _tooltip = null;

    private Image _image;
    private bool _live = true;
    
    void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void DamageSystem_OnDealDamage(CreatureStats creature, IDamage damage)
    {
        if (creature == Creature && _live == true)
        {
            _image.color = Color.red;
            GameHelper.instance.DelayMethod(() => { _image.color = Color.white; }, 0.3f);
        }
    }

    private void DamageSystem_OnDeathPlayer(CreatureStats creature, IDamage damage)
    {
        if (creature == Creature)
        {
            SetDeath();
        }
    }

    public void SetDeath()
    {
        _live = false;
        _image.color = Color.gray;
        GameHelper.instance.DelayMethod(() => { Destroy(this.gameObject); }, 1);
    }

    private void OnEnable()
    {
        DamageSystem.instance.OnDealDamage += DamageSystem_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer += DamageSystem_OnDeathPlayer;
    }



    private void OnDisable()
    {
        DamageSystem.instance.OnDealDamage -= DamageSystem_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer -= DamageSystem_OnDeathPlayer;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.SetActive(true);
        _tooltip.GetComponent<TooltipCreatureScript>().GenerateInfo(Creature);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.SetActive(false);
    }
}
