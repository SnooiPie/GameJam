using UnityEngine;

namespace MusicPuzzle
{
    public class MusicBoxTrigger : MonoBehaviour
    {
        public int boxIndex; // 0, 1, veya 2

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.TryInteractWithMusicBox(boxIndex);
                }
            }
        }
    }
}