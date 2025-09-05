// UI i�in ayr� bir s�n�f
using UnityEngine;

public class MusicBoxUI : MonoBehaviour
{
    // Renk se�im UI elementi
    public GameObject colorSelectionUI;

    // Renk butonlar�
    public void RedButton() { ColorSelected("K�rm�z�"); }
    public void GreenButton() { ColorSelected("Ye�il"); }
    public void BlueButton() { ColorSelected("Mavi"); }

    private int currentBoxIndex;
    private MusicBoxPuzzle puzzleManager;

    // UI'� a�ar
    public void OpenColorSelection(int boxIndex, MusicBoxPuzzle manager)
    {
        currentBoxIndex = boxIndex;
        puzzleManager = manager;
        colorSelectionUI.SetActive(true);
    }

    // Renk se�ildi�inde
    private void ColorSelected(string color)
    {
        puzzleManager.ColorSelected(currentBoxIndex, color);
        colorSelectionUI.SetActive(false);
    }
}