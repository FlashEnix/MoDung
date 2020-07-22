using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimEventController : MonoBehaviour
{
    public AudioClip footSound;
    public AudioClip attackSound;
    private AudioSource audioSource;
    PlayerScript script;
    public List<GameObject> animHips;
    // Start is called before the first frame update
    void Start()
    {
        script = this.GetComponentInParent<PlayerScript>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void dealDamage()
    {
        script.dealDamage();
        if (attackSound != null) audioSource.PlayOneShot(attackSound);
    }

    public void endAttack()
    {
        //MoveScript.instance.endAction(script.gameObject);
        script.endAttack();
    }

    public void shootAttack()
    {
        if (script.shootModel != null)
        {
            Vector3 startPosition = transform.position + new Vector3(0, .5f, .5f);

            foreach(GameObject h in animHips)
            {
                if (h.name == "mixamorig:RightHand")
                {
                    startPosition = h.transform.position;
                }
            }

            GameObject shoot = Instantiate(script.shootModel, startPosition, Quaternion.identity);
            shoot.GetComponent<shootPrefab>().setTarget(script,script.targetAttack.GetComponent<PlayerScript>());

            script.curState = PlayerScript.states.idle;
            script.targetAttack = null;
        }
    }

    public void footStep()
    {
        if (footSound != null) audioSource.PlayOneShot(footSound);
    }
}
