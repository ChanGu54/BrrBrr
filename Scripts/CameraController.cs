using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    GameManager gm;
    DataManager dm;
    GameObject playerObject;
    public float cameraTrackingSpeed = 0.2f;

    private Vector3 lastTargetPosition = Vector3.zero;
    private Vector3 currentTargetPosition = Vector3.zero;
    private float currentLerpDistance = 0.0f;
    private Vector3 originPosition;

    void Start()
    {
        gm = GameManager.instance;
        dm = DataManager.instance;

        playerObject = Instantiate(gm.playerCarList[dm.carSelected]);
        // 로직에 의문이 생김..

        Vector3 playerPos = playerObject.transform.position;
        Vector3 startTargetPos = playerPos;

        lastTargetPosition = startTargetPos;
        currentTargetPosition = startTargetPos;
        currentLerpDistance = 1.0f;
    }

    void LateUpdate()
    {
        cameraMovement();

        // Continue moving to the current target position
        currentLerpDistance += cameraTrackingSpeed;
        transform.position = Vector3.Lerp(lastTargetPosition, currentTargetPosition, currentLerpDistance);
    }

    void cameraMovement()
    {
        // Get and store the current camera position, and the current player position, in world coordinates.
        Vector3 currentCamPos = transform.position;

        if(gm.State == GameState.Driving)
        {
            playerObject = GameObject.Find("Norm_CameraPos");
        }

        else if(gm.State == GameState.Boost)
        {
            playerObject = GameObject.Find("Boost_CameraPos");
        }

        else if (gm.State == GameState.Crash)
        {
            StartCoroutine(Shake(0.1f, 0.5f));
            gm.State = GameState.GameOver;
            playerObject.transform.SetParent(null);
        }

        Vector3 currentPlayerPos = playerObject.transform.position;

        if (currentCamPos.x == currentPlayerPos.x && currentCamPos.y == currentPlayerPos.y)
        {
            // Positions are the same - tell the camera not to move, then abort.
            currentLerpDistance = 1f;
            lastTargetPosition = currentCamPos;
            currentTargetPosition = currentCamPos;
            return;
        }

        // Reset the travel distance for the lerp
        currentLerpDistance = 0f;

        // Store the current target position so we can lerp from it
        lastTargetPosition = currentCamPos;

        // Store the new target position
        currentTargetPosition = currentPlayerPos;
    }

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            originPosition = transform.position;

            transform.position = (Vector3)Random.insideUnitCircle * _amount + originPosition;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = originPosition;
    }
}