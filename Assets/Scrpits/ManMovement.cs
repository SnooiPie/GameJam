using UnityEngine;

public class ManMovement : MonoBehaviour
{
    public float speed = 3f;
    private Transform target;

    void Update()
    {
        if (target != null)
        {
            // Only move on Z-axis
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += new Vector3(0, 0, direction.z) * speed * Time.deltaTime;

            // Stop when close
            if (Mathf.Abs(transform.position.z - target.position.z) < 0.1f)
            {
                Debug.Log("Character reached " + target.name);
                target = null;
            }
        }
    }

    // 👇 This is the missing method
    public void MoveToSphere(Transform sphere)
    {
        target = sphere;
    }
}