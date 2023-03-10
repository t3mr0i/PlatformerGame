using UnityEngine;

public class CoinController : MonoBehaviour
{
    public int pointValue = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Füge die Punkte zum Score hinzu und zerstöre die Münze
            other.GetComponentInParent<GameController>().AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}