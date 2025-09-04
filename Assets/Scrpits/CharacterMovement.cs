using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        // Eğer Rigidbody varsa etkisizleştirelim
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        // A/D tuşları = Horizontal input
        float move = Input.GetAxis("Horizontal"); 

        // Ama hareketi X yerine Z eksenine uygula
        Vector3 movement = new Vector3(0, 0, move) * speed * Time.deltaTime;

        transform.Translate(movement, Space.World);
    }
}