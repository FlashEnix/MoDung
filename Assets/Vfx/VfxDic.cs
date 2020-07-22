using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxDic : MonoBehaviour
{
    public static VfxDic instance;
    public GameObject[] list;

    private void Awake()
    {
        instance = this;
    }

    public GameObject CreateEffect(string name)
    {
        foreach(GameObject effect in list)
        {
            if (effect.name == name)
            {
                return Instantiate(effect);
            }
        }
        return null;
    }
}
