using UnityEngine;
using System.Collections;

public class LadderTeleport : MonoBehaviour
{
    public Transform topPoint;        // Merdivenin üst noktası
    public Transform bottomPoint;     // Merdivenin alt noktası
    public float thresholdY = 1f;     // Yüksekliğe göre karar verilecek değer
    public KeyCode teleportKey = KeyCode.T;
    public float moveDuration = 1f;   // Kaç saniyede geçiş yapsın

    private bool isNearLadder = false;
    private Transform player;
    private bool isMoving = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isNearLadder = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == player)
        {
            isNearLadder = false;
            player = null;
        }
    }

    void Update()
    {
        if (isNearLadder && player != null && !isMoving && Input.GetKeyDown(teleportKey))
        {
            if (player.position.y > thresholdY)
            {
                StartCoroutine(SmoothMove(player, bottomPoint.position));
            }
            else
            {
                StartCoroutine(SmoothMove(player, topPoint.position));
            }
        }
    }

    IEnumerator SmoothMove(Transform obj, Vector3 targetPos)
    {
        isMoving = true;

        Vector3 startPos = obj.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;

            // Daha yumuşak geçiş için easing
            t = t * t * (3f - 2f * t);

            obj.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        obj.position = targetPos; // tam yerine oturt
        isMoving = false;
    }
}
