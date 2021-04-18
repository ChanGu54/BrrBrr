using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState // -1 : Start, 0 : Driving, 1 : Boost, 2 : Crash, 3 : GameOver, 4: Paused
{
    Start = -1,
    Driving,
    Boost,
    Crash,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour
{
    readonly string highscoreText = "HighScore : ";

    GameState _State;

    public GameState State
    {
        get
        {
            return _State;
        }
        set
        {
            if (_State == value && value == GameState.Boost)
            {
                boostingTime -= boostTime; // 부스트 상태에서 부스터 아이템 먹었을 때 부스트 시간 늘림
            }

            _State = value;

            switch (_State)
            {
                case GameState.Start:
                    StartCoroutine(AtStart());
                    break;
                case GameState.Driving:
                    // Update 구문에서 처리
                    break;
                case GameState.Boost:
                    max_Velocity = boost_Velocity;
                    break;
                case GameState.Crash:
                    carVelocity = 0;
                    //scoreText.transform.SetParent(null);
                    PlayerBroken.SetActive(true);
                    break;
                case GameState.GameOver:
                    StartCoroutine(GameOver());
                    break;
                case GameState.Paused:
                    exitPanel.SetActive(true);
                    if (SceneManager.GetActiveScene().name == "GameScene")
                        Time.timeScale = 0;
                    break;
                default:
                    Debug.LogError("상태 정보 인풋 오류! : " + _State.ToString());
                    break;
            }

            Debug.Log("상태 변경 : " + _State.ToString());
        }
    }

    static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager 인스턴스가 Hierarcy에 존재하지 않습니다!");
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    DataManager dm;
    public GameObject[] playerCarList;

    GameObject Score;
    TextMeshPro scoreText;

    private float max_Velocity;
    public float vel_Variation = 20f;
    public float carVelocity = 0;
    public float objectVelocity = 50f;

    public float common_Velocity = 100f;
    public float boost_Velocity = 200f;

    float _score;
    public float score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;

            if(_score < 0)
            {
                Debug.LogError("Score 세팅 에러!");
                _score = 0;
            }    
        }
    }
    public float mul = 1f;

    int _coin;
    public int coin
    {
        get
        {
            return _coin;
        }
        set
        {
            _coin = value;

            if(_coin < 0)
            {
                Debug.LogError("Coin값 세팅 에러!");
                _coin = 0;
            }
        }
    }
    public float boostTime = 10f;
    private float boostingTime = 0;

    public GameObject exitPanel;
    public GameObject gameOverPanel;
    public GameObject PlayerBroken;

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    void Start()
    {
        dm = DataManager.instance;
        Score = GameObject.Find("Score");

        if (Score)
        {
            scoreText = Score.GetComponent<TextMeshPro>();
            var highScore = Score.transform.Find("HighScore").GetComponent<TextMeshPro>();
            highScore.text = highscoreText + dm.highScore.ToString();
        }
        boostingTime = 0;
    }

    void ActivateGameOverPanel()
    {
        Debug.Log("점수 : " + score);
        Debug.Log("배수 : " + mul);
        score *= mul;
        if (score > dm.highScore)
            dm.highScore = (int)score;
        dm.coin += coin;
        dm.SaveGameData();
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.Find("FinalScore").GetComponent<TextMeshPro>().text = ((int)score).ToString();
        gameOverPanel.transform.Find("HighScore").GetComponent<TextMeshPro>().text = highscoreText + dm.highScore;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            State = GameState.Paused;
        }

        switch (State)
        {
            case GameState.Driving:
                common_Velocity = common_Velocity + Time.deltaTime;
                boost_Velocity = common_Velocity * 2;
                max_Velocity = common_Velocity;
                break;
            case GameState.Boost:
                if (boostingTime < boostTime)
                {
                    boostingTime += Time.deltaTime;
                }
                else
                {
                    boostingTime = 0;
                    State = GameState.Driving;
                }
                break;
            default:
                break;
        }

        if (State != GameState.GameOver)
        {
            if (carVelocity > max_Velocity)
                carVelocity -= vel_Variation * Time.deltaTime;

            else if (carVelocity < max_Velocity)
                carVelocity += vel_Variation * Time.deltaTime;

            score += max_Velocity * Time.deltaTime / 100;

            if (scoreText != null)
                scoreText.text = ((int)score).ToString();
        }
    }


    public IEnumerator AtStart()
    {
        yield return new WaitForSeconds(1f);
        State = GameState.Driving;
        yield return null;
    }

    public IEnumerator GameOver()
    {
        common_Velocity = 0;
        max_Velocity = common_Velocity;

        yield return new WaitForSeconds(3f);

        scoreText.gameObject.SetActive(false);
        ActivateGameOverPanel();

        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene("MainMenu");
    }
}

