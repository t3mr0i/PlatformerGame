using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int pointValue = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Verringere die Anzahl der Leben und aktualisiere die UI
            collision.collider.GetComponentInParent<GameController>().currentLives--;
            collision.collider.GetComponentInParent<GameController>().UpdateGreenCoinText();

            // Zerstöre das Hindernis
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Platform"))
        {
            // Füge die Punkte zum Score hinzu und zerstöre das Hindernis
            collision.collider.GetComponentInParent<GameController>().AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}