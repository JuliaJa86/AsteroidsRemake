using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Set in Inspector
    public AudioClip fireSound;

    void Start()
    {
        AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position);
        Destroy(gameObject, 3.0f);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 500);
    }
}
