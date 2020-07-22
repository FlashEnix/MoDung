using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public event Action OnMoveFinish;
    public event Action OnAttackFinish;
    public event Action OnDealDamage;

    public enum states
    {
        idle, walk, attack, death, impact, block, cast
    }

    public states curState = states.idle;

    public Animator anim;
    public float speed = 3;
    public GameObject shootModel;

    public List<GameObject> targetsMove = new List<GameObject>();
    public GameObject targetAttack = null;
    private CharacterController ch;
    private PlayerSounds sound;
    
    void Start()
    {
        ch = GetComponent<CharacterController>();
        sound = GetComponent<PlayerSounds>();
        if (!anim) anim = GetComponentInChildren<Animator>();

        DamageSystem.instance.OnDealDamage += Instance_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer += Instance_OnDeathPlayer;
    }

    private void Instance_OnDeathPlayer(CreatureStats c, IDamage damage)
    {
        if (c.playerScript == this)
        {
            GameHelper.instance.PlaySoundShot(sound.death);
            gameObject.tag = "DeathPlayer";
            gameObject.layer = 2;
            ch.enabled = false;
            setState(states.death);
        }
    }

    private void Instance_OnDealDamage(CreatureStats c, IDamage damage)
    {
        if (curState == states.death) return;
        if (c.playerScript == this)
        {
            setState(states.impact);
            GameHelper.instance.PlaySoundShot(sound.impact);
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target.GetComponent<CreatureStats>() != null)
        {
            targetAttack = target;
        }
        else
        {
            targetsMove.Add(target);
        }   
    }

    public void setState(states state)
    {
        curState = state;
    }

    void Update()
    {
        animUpdate();

        if (targetsMove.Count > 0)
        {
            curState = states.walk;
            Vector3 moveVector = targetsMove[0].transform.position - transform.position;
            ch.Move(moveVector.normalized * speed * Time.deltaTime);
            transform.LookAt(targetsMove[0].transform.position);

            float dist = Mathf.Abs(Vector3.Distance(transform.position, targetsMove[0].transform.position));

            if (dist < 0.05f)
            {
                GameObject toDestroy = targetsMove[0];
                targetsMove.Remove(toDestroy);
                Destroy(toDestroy);
                if (targetsMove.Count == 0)
                {
                    OnMoveFinish?.Invoke();
                }
            }
        }
        else
        {
            if (targetAttack != null)
            {
                if (curState != states.attack)
                {
                    //Поворачиваем игроков друг к другу
                    transform.LookAt(targetAttack.transform.position);
                    targetAttack.transform.LookAt(transform.position);
                    curState = states.attack;
                }
            } 
            else
            {
                if (curState == states.attack)
                {
                    curState = states.idle;
                }
            }
            //if (curState == states.walk || curState == states.impact || curState == states.block)
            if (curState != states.attack && curState != states.death)
            {
                curState = states.idle;
            }
        }
    }

    void animUpdate()
    {
        switch(curState)
        {
            case states.walk:
                anim.SetBool("walk", true);
                break;
            case states.attack:
                anim.SetBool("attack", true);
                anim.SetBool("walk", false);
                break;
            case states.idle:
                anim.SetBool("walk", false);
                anim.SetBool("attack", false);
                anim.SetBool("impact", false);
                anim.SetBool("block", false);
                break;
            case states.impact:
                anim.SetBool("impact", true);
                break;
            case states.block:
                anim.SetBool("block", true);
                break;
            case states.death:
                anim.SetBool("death", true);
                break;
            default:
                break;
        }
    }

    public void SetAnim(string name)
    {
        anim.SetTrigger(name);
    }

    public void dealDamage()
    {
        targetAttack = null;
        OnDealDamage?.Invoke();
    }

    public void impactDamage()
    {
        curState = states.impact;
    }

    public void blockDamage()
    {
        curState = states.block;
    }

    public void endAttack()
    {
        OnAttackFinish?.Invoke();
    }
}
