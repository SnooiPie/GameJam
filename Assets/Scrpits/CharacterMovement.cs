using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Transform targetSphere;
    private SphereInteraction targetInteraction;

    void Update()
    {
        if (targetSphere != null)
        {
            Vector3 targetPos = new Vector3(targetSphere.position.x, transform.position.y, targetSphere.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                targetInteraction.TriggerAction();
                targetSphere = null;
                targetInteraction = null;
            }
        }
    }

    public void MoveToSphere(Transform sphere, SphereInteraction interaction)
    {
        targetSphere = sphere;
        targetInteraction = interaction;
    }
}
