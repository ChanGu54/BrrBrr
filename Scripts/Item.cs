using UnityEngine;

public class Item : MonoBehaviour
{
    private GameManager gm;
    private Vector3 rotation;
    public float rotationSpeed = 300f;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        parent = transform.parent;
        rotation = parent.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        parent.position = new Vector3(parent.position.x, parent.position.y, parent.position.z - (gm.objectVelocity + gm.carVelocity) * Time.deltaTime);
        rotation.y += rotationSpeed * Time.deltaTime;
        parent.rotation = Quaternion.Euler(rotation);

        //if (rotation.y > 360)
        //    rotation.y -= 360;

        if (parent.position.z < -135)
            Destroy(transform.parent.gameObject);
    }
}
