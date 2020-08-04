using System;
using System.Linq;
using System.Collections;
using UnityEngine;
public class GameHelper: MonoBehaviour
{
    public static GameHelper instance;

    private void Awake()
    {
        instance = this;
    }

    public bool CheckAttack(CreatureStats from, CreatureStats to)
    {
        AttackAction action = from.GetComponent<AttackAction>();
        action.Init(to);
        return action.Check();
    }

    public Tail GetTailFromObject(GameObject obj)
    {
        return FindObjectsOfType<Tail>().OrderBy(x => Vector3.Distance(obj.transform.position, x.transform.position)).FirstOrDefault();
    }

    public void PlaySoundShot(AudioClip clip, float volume = 1)
    {
        if (clip == null) return;
        GetComponent<AudioSource>().PlayOneShot(clip, volume);
    }

    public GameObject InstantiateObject(string name,Vector3 position)
    {
        GameObject obj = FindObjectOfType<ObjectsDic>().objects.Where(x => x.name == name).FirstOrDefault();

        if (obj != null)
        {
            obj.transform.position = position;
            return Instantiate(obj);
        } else
        {
            return null;
        }
        
    }
    public void DelayMethod(Action method, float seconds = 1)
    {
        StartCoroutine(_delayMethod(method,seconds));
    }

    private IEnumerator _delayMethod(Action method, float seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
        method.Invoke();
    }
}
