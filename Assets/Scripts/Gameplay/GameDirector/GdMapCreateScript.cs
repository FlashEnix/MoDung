using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GdMapCreateScript : MonoBehaviour
{

    public GameObject tail;

    private int[,] testMap = new int[13, 13]
    {
        {0,0,0,0,0,1,1,1,1,0,0,0,0},
        {0,1,1,0,1,1,1,1,1,0,0,0,0},
        {0,1,1,0,1,1,1,1,1,0,0,0,0},
        {0,1,1,0,0,1,1,1,1,0,0,0,0},
        {1,1,1,0,0,0,1,1,0,0,1,1,0},
        {1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,0,0,1,1,0,0,1,1,0},
        {0,0,0,0,0,0,1,1,0,0,0,0,0},
        {0,0,0,0,0,1,1,1,1,0,0,0,0},
        {0,0,0,0,1,1,1,1,1,1,0,0,0},
        {0,0,0,0,1,1,1,1,1,1,0,0,0},
        {0,0,0,0,0,1,1,1,1,0,0,0,0}
    };

    void Start()
    {
        for (int a = 0; a < 13; a++)
        {
            for (int b = 0; b < 13; b++)
            {
                int item = testMap[a, b];
                if (item == 1)
                {
                    Instantiate(tail, new Vector3(b, 0, -a), Quaternion.Euler(90,0,0));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
