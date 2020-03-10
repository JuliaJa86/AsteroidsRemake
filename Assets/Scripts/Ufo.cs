using UnityEngine;

public class Ufo : MonoBehaviour
{
    //Set in Inspector
    public AudioClip    explodeSound;
    public GameObject   explodeEffect;

    //Set dynamically
    private GameController  _gameController = null;
    private GameObject      _objectToChase;
    private float           _speed = 0.8f;
    private Vector3         _targetPosition;
    private Vector3         _ufoPosition;
    private Vector3         _route;
    readonly private int    _score = 200;

    void Awake()
    {
        _gameController = Camera.main.GetComponent<GameController>();
        _objectToChase = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Calculate route for ufo to fly towards player and translate it
        _targetPosition = _objectToChase.transform.position;
        _ufoPosition = transform.position;
        _route = _targetPosition - _ufoPosition;
        if (Vector3.Distance(_ufoPosition, _targetPosition) > 0f)
        {
            transform.Translate(_route.x * _speed * Time.deltaTime, _route.y * _speed * Time.deltaTime, _route.z * _speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            Destroy(coll.gameObject);
            _gameController.AddScore(_score);
        }
        else if (coll.gameObject.tag == "Player")
        {
            StartCoroutine(_gameController.DestroyPlayer());
        }
        //Common collision behavior, including Asteroids
        AudioSource.PlayClipAtPoint(explodeSound, Camera.main.transform.position);
        _gameController.CountUfo(-1);
        Instantiate(explodeEffect, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
