using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WariningBlink : MonoBehaviour
{
    private new Light light;
    private float lerpValue;
    private Color red = new Color(1, 0, 0);
    private Color blue = new Color(0, 0, 1);
    private Color blank = new Color(1, 1, 1, 0);
    // Start is called before the first frame update
    void Start()
    {
        lerpValue = - 4f;
        light = gameObject.GetComponent<Light>();
        light.color = new Color(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        lerpValue += Time.deltaTime * 8;

        if (lerpValue > 4f)
            lerpValue = -4f;

        float x = Lerp_Equation(Mathf.Abs(lerpValue));

        if (lerpValue < 0)
        {
            light.color = Color.Lerp(red, blank, x);
        }
        else
        {
            light.color = Color.Lerp(blank, blue, x);
        }
    }

    // Input must be 0 ~ 4
    float Lerp_Equation(float x)
    {
        return (1 / 2.0f) * Mathf.Sqrt(x);
    }
}
