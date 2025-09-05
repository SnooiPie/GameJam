// UI için ayrý bir sýnýf
using UnityEngine;

public class MusicBoxUI : MonoBehaviour
{
    // Renk seçim UI elementi
    public GameObject colorSelectionUI;

    // Renk butonlarý
    public void RedButton() { ColorSelected("Kýrmýzý"); }
    public void GreenButton() { ColorSelected("Yeþil"); }
    public void BlueButton() { ColorSelected("Mavi"); }

    private int currentBoxIndex;
    private MusicBoxPuzzle puzzleManager;

    // UI'ý açar
    public void OpenColorSelection(int boxIndex, MusicBoxPuzzle manager)
    {
        currentBoxIndex = boxIndex;
        puzzleManager = manager;
        colorSelectionUI.SetActive(true);
    }

    // Renk seçildiðinde
    private void ColorSelected(string color)
    {
        puzzleManager.ColorSelected(currentBoxIndex, color);
        colorSelectionUI.SetActive(false);
    }
}