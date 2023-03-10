
using UnityEngine;
using UnityEngine.UI;

public class StoreController
{
    private GameObject storeObject;
    private Button buyButton;
    private Text greenCoinText;
    private int greenCoins;
    private GameObject[] outfitPrefabs;
    private CharacterCustomizationUI customizationUI;
    private int selectedOutfitIndex = 0;
    
    public StoreController(GameObject storeObject, Button buyButton, Text greenCoinText, int greenCoins, GameObject[] outfitPrefabs, CharacterCustomizationUI customizationUI)
    {
        this.storeObject = storeObject;
        this.buyButton = buyButton;
        this.greenCoinText = greenCoinText;
        this.greenCoins = greenCoins;
        this.outfitPrefabs = outfitPrefabs;
        this.customizationUI = customizationUI;

        // Setze den Buy-Button-Text
        buyButton.GetComponentInChildren<Text>().text = "Buy (" + greenCoins + ")";

        // Deaktiviere das Store-Objekt
        storeObject.SetActive(false);
    }

    public void ShowStore()
    {
        // Aktiviere das Store-Objekt
        storeObject.SetActive(true);

        // Setze den Standard-Outfit-Index
        selectedOutfitIndex = 0;

        // Zeige das erste Outfit an
        customizationUI.SetOutfit(outfitPrefabs[selectedOutfitIndex]);

        // Deaktiviere den Buy-Button, wenn nicht genügend Green Coins vorhanden sind
        buyButton.interactable = greenCoins >= 10;
    }

    public void HideStore()
    {
        // Deaktiviere das Store-Objekt
        storeObject.SetActive(false);
    }

    public void BuyOutfit()
    {
        // Reduziere die Anzahl der Green Coins
        greenCoins -= 10;

        // Aktualisiere den Green-Coin-Text
        greenCoinText.text = "Green Coins: " + greenCoins;

        // Aktiviere den Buy-Button, wenn genügend Green Coins vorhanden sind
        buyButton.interactable = greenCoins >= 10;

        // Schalte das Outfit frei
        selectedOutfitIndex++;
        customizationUI.SetOutfit(outfitPrefabs[selectedOutfitIndex]);

        // Aktualisiere den Buy-Button-Text
        buyButton.GetComponentInChildren<Text>().text = "Buy (" + greenCoins + ")";
    }
}