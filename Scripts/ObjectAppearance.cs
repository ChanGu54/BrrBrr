using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAppearance : MonoBehaviour
{
    public GameObject[] itemPrefab; // 0 -> Fuel, 1 -> Booster, 2 -> Coin
    public GameObject[] enemyPrefab;
    public float itemAppearFreq = 0;
    public float enemyAppearFreq = 0;

    private Vector3[] appearPos;
    private Quaternion itemRotation;
    private Quaternion enemyRotation;
    private float itemTime = 0;
    private float enemyTime = 0;
    private int itemAppearIdx;
    private bool isItemAppeared = false;

    // Start is called before the first frame update
    void Start()
    {
        itemRotation = Quaternion.identity;
        itemRotation.eulerAngles = new Vector3(0, 90, 0);

        enemyRotation = Quaternion.identity;
        enemyRotation.eulerAngles = new Vector3(0, 180, 0);

        appearPos = new Vector3[3];

        for (int i = -1; i < 2; i++)
        {
            appearPos[i + 1] = new Vector3(i * 7, 2, 540);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dtime = Time.deltaTime;

        itemTime += dtime;
        enemyTime += dtime;

        if (itemTime > itemAppearFreq)
        {
            ItemAppear();
            EnemyAppear();
        }

        else if (enemyTime > enemyAppearFreq)
        {
            EnemyAppear();
        }
    }
    
    void EnemyAppear()
    {
        var enemyAppearIdx = Random.Range(0, appearPos.Length);

        while (isItemAppeared && enemyAppearIdx == itemAppearIdx)
            enemyAppearIdx = Random.Range(0, appearPos.Length);

        var enemyIdx = Random.Range(0, enemyPrefab.Length);
        enemyTime = 0;

        Instantiate(enemyPrefab[enemyIdx], appearPos[enemyAppearIdx], enemyRotation, GameObject.Find("Enemies").transform);

        isItemAppeared = false;
    }

    void ItemAppear()
    {
        itemAppearIdx = Random.Range(0, appearPos.Length);
        var itemIdx = Random.Range(0, itemPrefab.Length);
        itemTime = 0;
        isItemAppeared = true;

        Instantiate(itemPrefab[itemIdx], appearPos[itemAppearIdx], itemRotation, GameObject.Find("Items").transform);
    }
}
