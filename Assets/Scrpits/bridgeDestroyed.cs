using UnityEngine;

public class bridgeDestroyed : MonoBehaviour
{
    public static bool isDead = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CoinPickUp.coins > 0)
        {
            Debug.Log("Bridge broke! Player dies.");
            isDead = true;
            Destroy(gameObject);
        }
    }
}
