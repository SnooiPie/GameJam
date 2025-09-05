using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour
{
    [Header("Oda Kilitleri")]
    public bool[] isRoomUnlocked;

    [Header("Odalar (Collider lazım, Is Trigger = true)")]
    public GameObject[] rooms;

    [Header("Ayarlar")]
    public float lockDuration = 0.1f; // kilitliyse player ne kadar süre hareket edemez

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player bulunamadı! Player objesine 'Player' tag'ini ekle.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.transform.position;

        int count = Mathf.Min(isRoomUnlocked.Length, rooms.Length);

        for (int i = 0; i < count; i++)
        {
            Collider roomCol = rooms[i].GetComponent<Collider>();

            if (roomCol.bounds.Contains(playerPos))
            {
                if (!isRoomUnlocked[i])
                {
                    // En yakın nokta hesapla (player’ı oda dışında en yakın yüzeye atar)
                    Vector3 nearestPoint = roomCol.ClosestPoint(playerPos);
                    nearestPoint.z -= .1f; // biraz dışarı atmak için z ekseninde kaydır

                    StartCoroutine(BlockPlayerMovement(nearestPoint));
                }
            }
        }
    }

    private IEnumerator BlockPlayerMovement(Vector3 newPos)
    {
        // Player’ı en yakın noktaya ışınla
        player.transform.position = newPos;

        // Hareket scriptini kapat
        var moveScript = player.GetComponent<GhostController>(); // kendi scriptinin adını yaz
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }

        // Bekle
        yield return new WaitForSeconds(lockDuration);

        // Hareket scriptini geri aç
        if (moveScript != null)
        {
            moveScript.enabled = true;
        }
    }
}
