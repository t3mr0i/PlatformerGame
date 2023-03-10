using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int[] scoreThresholds;
    public GameObject levelUpEffectPrefab;
    public AudioClip levelUpSound;
    public int currentLevel = 1;
    public int maxLevel = 
    private bool hasLevelUpEffectPlayed = false;

    public bool TryAdvanceLevel(int score)
    {
        // Überprüfe, ob das nächste Level erreicht wurde
        if (currentLevel < scoreThresholds.Length && score >= scoreThresholds[currentLevel])
        {
            currentLevel++;
            hasLevelUpEffectPlayed = false;
            return true;
        }

        return false;
    }

    public void ShowLevelUpEffect()
    {
        // Spiele den Level-Up-Sound ab und zeige den Effekt an, wenn dieser noch nicht abgespielt wurde
        if (!hasLevelUpEffectPlayed)
        {
            AudioSource.PlayClipAtPoint(levelUpSound, Camera.main.transform.position);
            Instantiate(levelUpEffectPrefab, Camera.main.transform.position, Quaternion.identity);
            hasLevelUpEffectPlayed = true;
        }
    }
}