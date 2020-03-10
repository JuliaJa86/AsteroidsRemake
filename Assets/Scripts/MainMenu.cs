using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Set in Inspector
    public Text             highScoreText;
    public GameObject[]     asteroidPrefabs;

    //Set dynamically
    private int             _highScore;

    //Button click handler assigned in Inspector
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    void Awake()
    {
        _highScore = PlayerPrefs.GetInt("highscore", 0);
        highScoreText.text = string.Format("High Score: {0}", _highScore);
        CreateDecorations();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    //Create some asteroids just for fun and visual effect
    private void CreateDecorations()
    {
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;
        for (int i = 0; i < 6; i++)
        {
            int asteroidSkin = Random.Range(0, asteroidPrefabs.Length);
            float xCoord = Random.Range(-camHalfWidth, camHalfWidth);
            float yCoord = Random.Range(-camHalfHeight, camHalfHeight);
            Instantiate(asteroidPrefabs[asteroidSkin], new Vector3(xCoord, yCoord, 0), Quaternion.Euler(0, 0, Random.Range(0.0f, 359.0f)));

        }
    }
}
