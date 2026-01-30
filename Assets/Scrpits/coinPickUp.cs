using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public static int coins = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            coins++;
            Debug.Log("Coins: " + coins);
            Destroy(gameObject);
        }
    }
}
