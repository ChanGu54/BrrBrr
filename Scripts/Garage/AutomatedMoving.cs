using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedMoving : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 endPoint1;
    Vector3 endPoint2;
    Vector3 destination;

    float lerpDis;
    bool ended;

    void Start()
    {
        endPoint1 = GameObject.Find("EndPoint1").transform.position;
        endPoint2 = GameObject.Find("EndPoint2").transform.position;
        destination = endPoint1;

        lerpDis = 0;
        startPoint = transform.position;
        ended = false;
    }

    void Update()
    {
        if (ended)
            return;
        else if (destination.Equals(endPoint1))
            MoveToEndpoint1();
        else if (destination.Equals(endPoint2))
            MoveToEndpoint2();
    }

    void MoveToEndpoint1()
    {
        if (lerpDis <= 1)
            lerpDis += Time.deltaTime * 6;
        else
        {
            lerpDis = 0;
            transform.rotation = Quaternion.Euler(14.35f, 226.876f, 14.822f);
            destination = endPoint2;
            return;
        }

        transform.position = Vector3.Lerp(startPoint, destination, lerpDis);
    }

    void MoveToEndpoint2()
    {
        if (lerpDis <= 1)
            lerpDis += Time.deltaTime * 1.5f;
        else
        {
            ended = true;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, destination, lerpDis);
    }
}
