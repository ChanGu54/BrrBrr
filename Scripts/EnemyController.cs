using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3[] fly_ememy; // 0 -> Left, 1 -> Right
    private Vector3[] fly_player;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;

        fly_ememy = new Vector3[2];
        fly_ememy[0] = new Vector3(-100, 100, 1000);
        fly_ememy[1] = new Vector3(100, 100, 1000);

        fly_player = new Vector3[2];
        fly_player[0] = new Vector3(-100, 100, -300);
        fly_player[1] = new Vector3(100, 100, -300);
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            transform.position.z - (gm.objectVelocity + gm.carVelocity) * Time.deltaTime);

        //if (rotation.y > 360)
        //    rotation.y -= 360;

        if (transform.position.z < -135)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gm.State == GameState.Driving)
            {
                StartCoroutine(PlayerCarFlying(other.gameObject));
                gm.State = GameState.Crash;
            }
            else if (gm.State == GameState.Boost)
            {
                StartCoroutine(EnemyCarFlying(gameObject));
            }
        }
    }

    public IEnumerator EnemyCarFlying(GameObject go)
    {
        int index = Random.Range(0, fly_ememy.Length);
        go.GetComponent<Rigidbody>().AddForce(fly_ememy[index]);
        go.GetComponent<Rigidbody>().AddTorque(fly_ememy[index]);
        gm.score += 30;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public IEnumerator PlayerCarFlying(GameObject go)
    {
        int index = Random.Range(0, fly_player.Length);
        go.GetComponent<Rigidbody>().AddTorque(fly_player[index]);
        go.GetComponent<Rigidbody>().AddForce(fly_player[index]);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
