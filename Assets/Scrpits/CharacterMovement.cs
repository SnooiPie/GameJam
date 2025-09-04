using UnityEngine;

public class CharacterMovement : MonoBehaviour

{
    public float speed = 3f;
    public float stopDistance = 0.1f;

    private Transform target;

    void Update()
    {
        if (target == null) return;

        // Move ONLY on Z toward target
        float targetZ = target.position.z;
        float newZ = Mathf.MoveTowards(transform.position.z, targetZ, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        if (Mathf.Abs(transform.position.z - targetZ) <= stopDistance)
        {
            // reached
            target = null;
            // You can trigger “checked” logic here
        }
    }

    // <-- THIS is the method GameManager calls
    public void MoveToSphere(Transform sphere)
    {
        target = sphere;
    }
}