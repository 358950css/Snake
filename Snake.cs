using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    //public GameUI;

    //public GameAudio gameAudio;

    Vector3 direction;

    [SerializeField] float speed; //蛇的移動速度

    public Transform bodyPrefab;

    public List<Transform> bodies = new List<Transform>();
    
    public bool canMove;
    public bool canTurn; //可以轉向 

    public AudioSource audioPlayer;

    [SerializeField] Text Game_paused; //遊戲暫停要顯示的text

    [SerializeField] Text scoreText; //分數顯示的text類型
    bool isPaused = false; // 追蹤遊戲是否暫停

    int score; //分數的數值
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = speed;
        ResetStage();
        score = 0;
        Time.timeScale = speed;
        canTurn = true;
        Game_paused.gameObject.SetActive(false); // 確保遊戲開始時暫停文字是隱藏的
    }

    // Update is called once per frame
    void Update()
    {
        // 按下空白鍵時，切換遊戲暫停狀態
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
            Game_paused.gameObject.SetActive(isPaused); // 顯示或隱藏暫停文字
        }
        if (isPaused) return; //如果是暫停這邊要跳掉
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector3.down)
        {
            if (canTurn == true)
            {
                canMove = true;
                canTurn = false;
                direction = Vector3.up;
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && direction != Vector3.up)
        {
            if (canTurn == true)
            {
                canMove = true;
                canTurn = false;
                direction = Vector3.down;
                transform.rotation = Quaternion.Euler(0, 0, 0);

            }
        }
        if (Input.GetKeyDown(KeyCode.A) && direction != Vector3.right)
        {
            if (canTurn == true)
            {
                canMove = true;
                canTurn = false;
                direction = Vector3.left;
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.D) && direction != Vector3.left)
        {
            if (canTurn == true)
            {
                canMove = true;
                canTurn = false;
                direction = Vector3.right;
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        
    }

        // 切換遊戲暫停與恢復
    void TogglePause()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            Time.timeScale = 0; // 暫停遊戲
        }
        else
        {
            Time.timeScale = 1; // 恢復遊戲
            Time.timeScale = speed; //恢復原本遊戲速度
        }
    }

    private void FixedUpdate()
    {
        for (int i = bodies.Count - 1; i > 0; i--) //這裡是貪吃蛇身體部分的移動機制
        {
            // 每一節身體的位置，設為它前一節的位置
            bodies[i].position = bodies[i - 1].position;
            Vector3 newPosition = bodies[i].position; // 先把位置取出來
            newPosition.y = 0.5f;  
            bodies[i].position = newPosition;
        }
        if(canMove)
        {
            transform.Translate(-Vector3.forward); //這邊是條件符合就進行移動
            canTurn = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            bodies.Add(Instantiate(bodyPrefab, transform.position, Quaternion.identity)); //吃到食物生成新身體
            UpdateScore(false);
        }
        if(other.CompareTag("Obstacle"))
        {
            ResetStage();
            ReplayBackgroundMusic();
        }
    }

    void ResetStage() //重新開始
    {
        transform.position = Vector3.zero;
        direction = Vector3.zero;
        canMove = false;

        for (int i = 1; i < bodies.Count; i++)
        {
            Destroy(bodies[i].gameObject);
        }
        bodies.Clear();
        bodies.Add(transform);
        score = 0;
        UpdateScore(true);
    }

    void UpdateScore(bool isReset)
    {
        if (isReset == true) //如果是true就代表是重新開始不要++
        {
            scoreText.text = "現在分數為：" + score.ToString() + "分";
        }
        else
        {
            score++;
            scoreText.text = "現在分數為：" + score.ToString() + "分";
        }
    }

    public void ReplayBackgroundMusic()
    {
        //重新播放音效
        audioPlayer.Play();
    }
}
