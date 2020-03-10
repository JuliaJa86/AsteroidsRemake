using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Set in Inspector
    public GameObject[]     asteroidPrefabs;
    public GameObject       ufoPrefab;
    public GameObject       shipExplodeEffect;
    public PlayerControl    player;
    public Text             scoreText;
    public Text             gameOverText;
    public Text             finalText;
    public Text             waveText;
    public Image[]          livesArr;

    //Set dynamically
    private int         _highScore = 0;
    private int         _score = 0;
    private int         _asteroidsRemaining = 0;
    private int         _ufoRemaining = 0;
    private int         _initialAsteroidCount = 4;
    private int         _stage = 0;
    private int         _lives = 3;

    //Add score and update interface label
    public void AddScore(int score)
    {
        _score += score;
        scoreText.text = "Score: " + _score;
    }

    //Tracks number of active asteroids. Positive argument adds asteroids, negative substracts
    public void CountAsteroids(int dif)
    {
        _asteroidsRemaining += dif;
        if (_asteroidsRemaining == 2 && _ufoRemaining == 0)
        {
            CreateUFO();
        }
        else if (_asteroidsRemaining == 0 && _ufoRemaining == 0)
        {
            GoToNextWave();
        }
    }

    //Tracks number of active UFOs. Positive argument adds UFO, negative substracts
    public void CountUfo(int dif)
    {
        _ufoRemaining += dif;
        if(_ufoRemaining == 0 && _asteroidsRemaining == 0)
        {
            GoToNextWave();
        }
    }

    //Coroutine. Handle situations when ship is destroyed. Checks if there is spare lives to continue game or not
    public IEnumerator DestroyPlayer()
    {
        player.PlayCrashSound();
        player.gameObject.SetActive(false);
        Instantiate(shipExplodeEffect, player.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        //Small delay to let animation effect play 
        yield return new WaitForSeconds(1.5f);
        if (_lives > 0)
        {
            _lives--;
            livesArr[_lives].gameObject.SetActive(false);            
            player.ResetPlayerAtPoint(); //default position is 0.0.0
            player.gameObject.SetActive(true);
        }
        else
        {
            Destroy(player.gameObject);
            GameOver();
        }
    }

    void Awake()
    {
        gameOverText.gameObject.SetActive(false);
        finalText.gameObject.SetActive(false);
        _highScore = PlayerPrefs.GetInt("highscore", 0);
    }
    
    void Start() {        
        CreateAsteroids();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MenuScene");
        //Try to teleport ship to random position. 60% chance of immediate destroy
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (player.Teleport())
            {
                float camHalfHeight = Camera.main.orthographicSize;
                float camHalfWidth = camHalfHeight * Camera.main.aspect;
                float halfRadius = 0.5f;
                player.ResetPlayerAtPoint(Random.Range(-camHalfWidth + halfRadius, camHalfWidth - halfRadius), Random.Range(-camHalfHeight + halfRadius, camHalfHeight - halfRadius), 0f);
            }
            else
                StartCoroutine(DestroyPlayer());
        }
    }

    //Creates asteroids at random positions and sets directions of movement
    private void CreateAsteroids()
    {
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;
        for (int i = 0; i < _initialAsteroidCount + _stage; i++)
        {
            int asteroidSkin = Random.Range(0, asteroidPrefabs.Length);
            
            //These calculations needed to not spawn asteroids at center of screen where the ship initial position is
            int xSide = Random.Range(0, 2);
            int ySide = Random.Range(0, 2);
            float xCoord = 0f;
            float yCoord = 0f;
            if (xSide == 0) //negative x side of screen
                xCoord = Random.Range(-camHalfWidth, -3f);
            else
                xCoord = Random.Range(3f, camHalfWidth);
            if (ySide == 0) //negative y side of screen
                yCoord = Random.Range(-camHalfHeight, -2f);
            else
                yCoord = Random.Range(2f, camHalfHeight);
            
            Instantiate(asteroidPrefabs[asteroidSkin], new Vector3(xCoord, yCoord, 0), Quaternion.Euler(0, 0, Random.Range(0.0f, 359.0f)));

        }
        _asteroidsRemaining = _initialAsteroidCount + _stage;
    }

    //Creates UFO at random position outside visible area
    private void CreateUFO()
    {
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;
        int xSide = Random.Range(0, 2);
        int ySide = Random.Range(0, 2);
        float xCoord = 0f;
        float yCoord = 0f;
        if (xSide == 0) //negative x side of screen
            xCoord = Random.Range(-camHalfWidth - 2f, -camHalfWidth);
        else
            xCoord = Random.Range(camHalfWidth, camHalfWidth + 2f);
        if (ySide == 0) //negative y side of screen
            yCoord = Random.Range(-camHalfHeight - 2f, -camHalfHeight);
        else
            yCoord = Random.Range(camHalfHeight, camHalfHeight + 2f);
        Instantiate(ufoPrefab, new Vector3(xCoord, yCoord, 0), Quaternion.Euler(0, 0, 0));
        CountUfo(1);
    }

    //Starts next wave of asteroids
    private void GoToNextWave()
    {
        _stage++;
        waveText.text = "Wave: " + (_stage + 1);
        CreateAsteroids();
    }

    //Handles game over situation when ship is destroyed and lives = 0
    private void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        finalText.text = string.Format("Your final score is: {0}!\nPress Esc to return to main menu", _score);
        finalText.gameObject.SetActive(true);
        if (_score > _highScore)
            PlayerPrefs.SetInt("highscore", _score);
    }
}
