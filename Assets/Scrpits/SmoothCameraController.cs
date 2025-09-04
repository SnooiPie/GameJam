using UnityEngine;

public class FollowYZ_LockX : MonoBehaviour
{
    public Transform target;       // Takip edilecek nesne
    public float camSpeed = 5f;    // Yaklaşma hızı (unit/s)

    private float lockX;

    void Start()
    {
        // Kameranın başladığı X'i kilitle
        lockX = transform.position.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 p = transform.position;

        // hedef Y ve Z'ye yaklaş
        float newY = Mathf.MoveTowards(p.y, target.position.y, camSpeed * Time.deltaTime);
        float newZ = Mathf.MoveTowards(p.z, target.position.z, camSpeed * Time.deltaTime);

        transform.position = new Vector3(lockX, newY, newZ);
    }
}
