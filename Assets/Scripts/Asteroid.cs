using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //Set in Inspector
    public AudioClip    explodeSound;
    public GameObject   explodeEffect;
    
    private enum EAsteroidType
    {
        large = 0,
        medium,
        small
    }

    //Set dynamically
    private EAsteroidType   _aType = EAsteroidType.large;
    private GameController  _gameController = null;
    readonly private int[]  _scoresAmount = { 20, 50, 100 };

    private void Awake()
    {
        _gameController = Camera.main.GetComponent<GameController>();
    }

    void Start()
    {
        //Set start impulse according to up vector and rotation velocity
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity += new Vector2(transform.up.x, transform.up.y) * 5f;
        rb.angularVelocity = Random.Range(0.0f, 90.0f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            Destroy(coll.gameObject);
            AudioSource.PlayClipAtPoint(explodeSound, Camera.main.transform.position);
            if (_aType == EAsteroidType.large || _aType == EAsteroidType.medium)
            {
                //Create 2 new asteroids
                for (int i = 0; i < 2; i++)
                {
                    GameObject astGO = Instantiate(gameObject, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 359.0f)));
                    Asteroid ast = astGO.GetComponent<Asteroid>();
                    if (_aType == EAsteroidType.large)
                        ast.SetAsteroidProperties(EAsteroidType.medium);
                    else
                        ast.SetAsteroidProperties(EAsteroidType.small);
                }
                _gameController.CountAsteroids(1); // -1 destroyed + 2 new
            }
            else
            {
                _gameController.CountAsteroids(-1); // -1 small asteroid
            }
            _gameController.AddScore(_scoresAmount[(int)_aType]);
            Instantiate(explodeEffect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
        else if (coll.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(_gameController.DestroyPlayer());
        }
    }

    //Set asteroid type and scale, according type
    private void SetAsteroidProperties(EAsteroidType type)
    {
        _aType = type;
        //set scale
        switch (_aType)
        {
            case EAsteroidType.large:
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case EAsteroidType.medium:
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case EAsteroidType.small:
                transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                break;
        }
    }
}
