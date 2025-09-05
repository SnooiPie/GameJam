using UnityEngine;

public class MusicBoxTrigger : MonoBehaviour
{
    public int boxIndex; // 0, 1, veya 2
    public MusicBoxPuzzle puzzleManager;

    void OnMouseDown()
    {
        if (puzzleManager != null)
        {
            puzzleManager.OnMusicBoxClicked(boxIndex);
        }
    }
}