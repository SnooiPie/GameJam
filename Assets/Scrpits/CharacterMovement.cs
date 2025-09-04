using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 3f;
    public float stopDistance = 0.1f;
    public float waitTime = 1.5f; // her sphere’de bekleme süresi

    private Queue<Transform> sphereQueue = new Queue<Transform>();
    private Transform currentTarget;
    private bool isMoving = false;

    void Update()
    {
        if (currentTarget == null || !isMoving) return;

        // Z ekseninde hareket
        float targetZ = currentTarget.position.z;
        float newZ = Mathf.MoveTowards(transform.position.z, targetZ, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        if (Mathf.Abs(transform.position.z - targetZ) <= stopDistance)
        {
            StartCoroutine(WaitAndNext());
        }
    }

    IEnumerator WaitAndNext()
    {
        isMoving = false;
        yield return new WaitForSeconds(waitTime);

        if (sphereQueue.Count > 0)
        {
            currentTarget = sphereQueue.Dequeue();
            isMoving = true;
        }
        else
        {
            currentTarget = null;
        }
    }

    // Ghost'tan çağrılacak method
    public void MoveThroughSpheres(List<Transform> spheres, Transform finalSphere)
    {
        sphereQueue.Clear();

        // önce diğer sphere’leri sıraya ekle
        foreach (Transform s in spheres)
        {
            if (s != finalSphere)
                sphereQueue.Enqueue(s);
        }

        // en sona doğru sphere’i ekle
        sphereQueue.Enqueue(finalSphere);

        if (sphereQueue.Count > 0)
        {
            currentTarget = sphereQueue.Dequeue();
            isMoving = true;
        }
    }
}
