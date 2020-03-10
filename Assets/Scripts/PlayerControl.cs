using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Set in Inspector
    public float        speed = 3.0f;
    public float        rotationSpeed = 10.0f;
    public GameObject   bullet;
    public AudioClip    crashSound;
    public Sprite       defaultSkin;
    public Sprite       boostSkin;

    //Set dynamically
    private Rigidbody2D     _rb;
    private SpriteRenderer  _sr;
    private bool            _shootCD = false;
    readonly private float  _shotDelay = 0.1f;

    //Resets player at certain point of screen and makes it invulnerable for 1.5 seconds
    public void ResetPlayerAtPoint(float x = 0f, float y = 0f, float z = 0f)
    {
        _rb.velocity = Vector2.zero;
        _rb.rotation = 0f;//Vector2.zero;
        transform.position = new Vector3(x, y, z);//Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //invulnerability for 1.5 seconds
        //asteroids have kinematic body type, so they won't collide with the ship while it is static
        _rb.bodyType = RigidbodyType2D.Static;
        Invoke("TurnOnPhysics", 1.5f);
    }

    //Plays crash sound when ship is destroyed
    public void PlayCrashSound()
    {
        AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position);
    }

    //Returns result of teleportation try
    public bool Teleport()
    {
        //60% chance to fail
        bool success = Random.Range(0, 100) < 40;
        return success;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _sr.sprite = boostSkin;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            _sr.sprite = defaultSkin;
    }

    void FixedUpdate()
    {
        _rb.AddForce(transform.up * 6f * Input.GetAxis("Vertical"));
        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotationSpeed);
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    //Instantiates bullet. Makes certain delay between shoots
    private void Fire()
    {
        if (_shootCD)
            return;

        _shootCD = true;
        Invoke("ResetShootCooldown", _shotDelay);
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0) + new Vector3(transform.up.x, transform.up.y, 0), transform.rotation);
    }

    //Service function for shoot delay
    private void ResetShootCooldown()
    {
        _shootCD = false;
    }

    //Service function to turn off invulnerability of the ship
    private void TurnOnPhysics()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
