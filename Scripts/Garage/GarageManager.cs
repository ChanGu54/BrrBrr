using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageManager : MonoBehaviour
{
    DataManager dm;
    public Text coinText;
    public GameObject failPanel;
    CarSelection cs;
    List<int> carObtained;
    public GameObject buyBtn;

    int coin;

    void Start()
    {
        cs = GetComponent<CarSelection>();
        dm = DataManager.instance;
        carObtained = dm.carObtained;
        coin = dm.coin;
        coinText.text = coin.ToString();
    }

    public void BuyCar()
    {
        if (coin >= 1000)
        {
            StartCoroutine(MoneyDown(1000, 0.2f));
            buyBtn.SetActive(false);
            coin -= 1000;
            dm.coin = coin;
            dm.carObtained.Add(cs.idx);
            dm.SaveGameData();
            GameObject.Find("BuyBtnClicked").GetComponent<AudioSource>().Play();
        }

        else
            failPanel.SetActive(true);
    }

    public void FailOkButton()
    {
        failPanel.SetActive(false);
    }


    IEnumerator MoneyDown(int amount, float seconds)
    {
        float detail = 100f;
        int prev_coin = coin;
        float lerp_val = 0;

        while (lerp_val <= 1) {
            coinText.text = ((int)Mathf.Lerp(prev_coin, prev_coin - amount, lerp_val)).ToString();
            lerp_val += 1 / (seconds * detail);

            yield return new WaitForSeconds(1/ detail);
        }
        coinText.text = (prev_coin - amount).ToString();
    }
}
