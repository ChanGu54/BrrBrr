using System.Collections;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    

    private bool isSwiping;
    private bool isMoving;
    private Vector3 startPos;
    private Vector3 endPos;
    private Touch t;
    private Vector3 mouse;
    private int carCurPos; // 0 -> Left, 1 -> Center, 2 -> Right

    private Vector3[] roadPos;  // 0 -> Left, 1 -> Center, 2 -> Right

    private float lerpDist;

    enum CarPos  // 0 -> Left, 1 -> Center, 2 -> Right
    {
        Left,
        Center,
        Right
    }

    enum MoveCar
    {
        left = -1,
        right = 1
    }

    void Start()
    {
        if (GameObject.Find("Score"))
            GameObject.Find("Score").transform.SetParent(transform);
        isSwiping = false;
        carCurPos = (int)CarPos.Center;
        isMoving = false;
        lerpDist = 0;

        roadPos = new Vector3[3];

        for(int i = -1; i < 2; i++)
        {
            roadPos[i + 1] = gameObject.transform.position;
            roadPos[i + 1].x += 7 * i;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        Pc_SwipeCtrl();
#elif UNITY_ANDROID
        Andorid_SwipeCtrl();
#endif
    }

    void Andorid_SwipeCtrl()
    {
        if(Input.touchCount > 0 && !isMoving)
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
        if (Input.GetMouseButton(0) && !isMoving)
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
            isSwiping = false;
    }

    void Andorid_SwipeCheck()
    {
        endPos = t.position;
        Vector3 swipeDirection = endPos - startPos;
        float swipeDetection = swipeDirection.x / Screen.width;
        if (swipeDetection < - 1 / 10.0f)
        {
            isSwiping = false;
            if(carCurPos != (int)CarPos.Left)
                Moving_car(MoveCar.left);
        }

        else if (swipeDetection > 1 / 10.0f)
        {
            isSwiping = false;
            if (carCurPos != (int)CarPos.Right)
                Moving_car(MoveCar.right);
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
            if (carCurPos != (int)CarPos.Left)
                Moving_car(MoveCar.left);
        }

        else if (swipeDetection > 1 / 5.0f)
        {
            isSwiping = false;
            if (carCurPos != (int)CarPos.Right)
                Moving_car(MoveCar.right);
        }
    }

    void Moving_car(MoveCar direction) //-1 : Left, 1 : Right
    {
        isMoving = true;

        if (direction == MoveCar.left)
        {
            StartCoroutine("MoveLeft");
        }
        else if(direction == MoveCar.right)
        { 
            StartCoroutine("MoveRight");
        }
    }

    IEnumerator MoveLeft()
    {
        var go = transform.Find("LeftRearLight").gameObject;
        go.SetActive(true);
        lerpDist = 0;
        --carCurPos;

        while ((lerpDist += Time.deltaTime * 4) < 1)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, roadPos[carCurPos], lerpDist);
            yield return null;
        }

        gameObject.transform.position = roadPos[carCurPos];
        go.SetActive(false);

        isMoving = false;
    }

    IEnumerator MoveRight()
    {
        var go = transform.Find("RightRearLight").gameObject;
        go.SetActive(true);
        lerpDist = 0;
        ++carCurPos;

        while ((lerpDist += Time.deltaTime * 4) < 1)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, roadPos[carCurPos], lerpDist);
            yield return null;
        }

        gameObject.transform.position = roadPos[carCurPos];
        go.SetActive(false);

        isMoving = false;
    }
}
