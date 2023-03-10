using UnityEngine;

public class CharacterCustomizationUI : MonoBehaviour
{
    public GameObject characterObject;
    public GameObject[] outfitPrefabs;
    
    
    private GameObject currentOutfitObject;

    void Start()
    {
        // Setze das Standard-Outfit
        SetOutfit(outfitPrefabs[0]);
    }

    public void SetOutfit(GameObject outfitPrefab)
    {
        // Entferne das aktuelle Outfit
        if (currentOutfitObject != null)
        {
            Destroy(currentOutfitObject);
        }

        // Erstelle das neue Outfit und setze es als Kind des Charakter-Objekts
        currentOutfitObject = Instantiate(outfitPrefab, characterObject.transform);
        currentOutfitObject.transform.localPosition = Vector3.zero;
        currentOutfitObject.transform.localRotation = Quaternion.identity;
    }
}