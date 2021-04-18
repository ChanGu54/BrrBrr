using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public GameManager gm;
    public Transform[] map;
    private int flag;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        flag = 0;
    }

    void Update()
    {
        for (int i = 0; i < map.Length; i++)
        {
            map[i].position = new Vector3(0, 0, map[i].position.z - gm.carVelocity * Time.deltaTime);
        }

        if (map[flag].position.z < -135)
        {
            map[flag].position = new Vector3(0, 0, map[(flag + map.Length - 1) % map.Length].position.z + 135);
            flag = (flag + 1) % map.Length;
        }
    }
}
