using UnityEngine;

public class XplodeEffect : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroySelf", 1.5f);        
    }

    //Destroys GameObject after playing effect to keep scene clear
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
