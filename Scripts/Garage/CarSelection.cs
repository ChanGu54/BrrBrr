using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    DataManager dm;
    public GameObject[] carList;
    public Transform startPoint;
    public GameObject carSeeing;
    public TextMeshPro carName;

    public int idx;
    bool isSwiping;
    bool alreadySwiped;
    Vector3 mouse;
    Vector3 startPos;
    Vector3 endPos;
    Touch t;

    public GameObject buyButton;
    int coin;
    List<int> carObtained;

    // Start is called before the first frame update
    void Start()
    {
        dm = DataManager.instance;
        coin = dm.coin;
        carObtained = dm.carObtained;
        idx = dm.carSelected;
        carSeeing = Instantiate(carList[idx], startPoint.position, startPoint.rotation);
        carName.text = carSeeing.name.Split('(')[0];
    }

    void Update()
    {
#if UNITY_EDITOR
        Pc_SwipeCtrl();
#elif UNITY_ANDROID
        Andorid_SwipeCtrl();
#endif
        BackKeyCtrl();
    }

    void Andorid_SwipeCtrl()
    {
        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);
            if (isSwiping == false && t.phase == TouchPhase.Began)
            {
                isSwiping = true;
                startPos = t.position;
            }
            if (isSwiping == true)
            {
                Andorid_SwipeCheck();
            }
            if (t.phase == TouchPhase.Ended)
            {
                isSwiping = false;
            }
        }
    }

    void Pc_SwipeCtrl()
    {
        if (Input.GetMouseButton(0) && !alreadySwiped)
        {
            mouse = Input.mousePosition;
            if (isSwiping)
            {
                Pc_SwipeCheck();
            }
            else if (!isSwiping)
            {
                isSwiping = true;
                startPos = mouse;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
            alreadySwiped = false;
        }
    }

    void Andorid_SwipeCheck()
    {
        endPos = t.position;
        Vector3 swipeDirection = endPos - startPos;
        float swipeDetection = swipeDirection.x / Screen.width;
        if (swipeDetection < -1 / 10.0f)
        {
            isSwiping = false;
            ChangeCar(-1);
        }

        else if (swipeDetection > 1 / 10.0f)
        {
            isSwiping = false;
            ChangeCar(1);
        }
    }

    void Pc_SwipeCheck()
    {
        endPos = Input.mousePosition;
        Vector3 swipeDirection = endPos - startPos;
        float swipeDetection = swipeDirection.x / Screen.width;
        Debug.Log(swipeDirection);
        if (swipeDetection < -1 / 5.0f)
        {
            isSwiping = false;
            ChangeCar(1);
            alreadySwiped = true;
        }

        else if (swipeDetection > 1 / 5.0f)
        {
            isSwiping = false;
            ChangeCar(-1);
            alreadySwiped = true;
        }
    }

    void ChangeCar(int dir = 0) // -1 : Left, 1 : Right
    {
        Destroy(carSeeing);
        idx += dir;

        if (idx < 0)
            idx += carList.Length;
        else if (idx > carList.Length - 1)
            idx -= carList.Length;

        carSeeing = Instantiate(carList[idx], startPoint.position, startPoint.rotation);
        carName.text = carSeeing.name.Split('(')[0];
        BuyButtonVisualizer();
    }

    void BuyButtonVisualizer()
    {
        carObtained = dm.carObtained;

        if (carObtained.Contains(idx))
            buyButton.SetActive(false);
        else
            buyButton.SetActive(true);
    }


    void BackKeyCtrl()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space))
        {
            carObtained = dm.carObtained;

            if (carObtained.Contains(idx))
                dm.carSelected = idx;
            dm.SaveGameData();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
