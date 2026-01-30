using UnityEngine;

public class DoorArrive : MonoBehaviour
{
    public static bool win = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (CoinPickUp.coins == 0)
        {
            Debug.Log("YOU WIN 🎉");
            win = true;
        }
        else
        {
            Debug.Log("You must reach the door WITHOUT coins!");
        }
    }
}
