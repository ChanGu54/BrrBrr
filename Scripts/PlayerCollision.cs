using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    GameManager gm;

    public GameObject getItemEffect;
    public GameObject boostEffect;
    public GameObject coinText;
    public GameObject fuelText;

    private Vector3 effectPos;

    void Start()
    {
        gm = GameManager.instance;
    }

    void OnTriggerEnter(Collider other)
    {
        effectPos = other.transform.position;

        if (other.gameObject.CompareTag("Item"))
        {
            if (other.gameObject.name == "booster")
            {
                StartCoroutine(OnBoostEffect());
                gm.State = GameState.Boost;
            }

            else if (other.gameObject.name == "fuel")
            {
                StartCoroutine(OnFuelEffect());
            }

            else if (other.gameObject.name == "coin")
            {
                StartCoroutine(OnCoinEffect());
            }

            Destroy(other.transform.parent.gameObject);
        }
    }

    IEnumerator OnFuelEffect()
    {
        float fuelMul = gm.mul;
        var fuelPos = new Vector3(0, 2, 6);

        fuelMul += .1f;
        fuelPos += transform.position;

        GameObject go1 = Instantiate(getItemEffect, effectPos, Quaternion.Euler(0, 0, 0));
        GameObject go2 = Instantiate(fuelText);

        go2.transform.position = fuelPos;
        go2.GetComponent<TextMeshPro>().text = "X " + fuelMul.ToString();
        gm.mul = fuelMul;

        yield return new WaitForSeconds(2);

        Destroy(go1);
        Destroy(go2);
    }


    IEnumerator OnBoostEffect()
    {
        if (GameObject.Find("Speed(Clone)") != null)
            Destroy(GameObject.Find("Speed(Clone)"));
        GameObject go = Instantiate(boostEffect, effectPos, Quaternion.Euler(190, 0, -90), transform);
        yield return new WaitForSeconds(gm.boostTime);
        Destroy(go);
    }

    IEnumerator OnCoinEffect()
    {
        int coinRange = 0;
        var coinPos = new Vector3(0, 2, 6);

        coinPos += transform.position;

        if (gm.State == GameState.Boost)
            coinRange = Random.Range(140, 160);
        else
            coinRange = Random.Range(40, 60);

        GameObject go1 = Instantiate(getItemEffect, effectPos, Quaternion.Euler(0, 0, 0));
        GameObject go2 = Instantiate(coinText);

        go2.transform.position = coinPos;
        go2.GetComponent<TextMeshPro>().text = "+ " + coinRange;
        gm.coin += coinRange;

        yield return new WaitForSeconds(2); 

        Destroy(go1);
        Destroy(go2);
    }
}
