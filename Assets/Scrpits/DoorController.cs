using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openSpeed = 2f;
    private Quaternion closedRot;
    private Quaternion openRot;
    private bool isOpening = false;

    void Start()
    {
        closedRot = Quaternion.Euler(0f, -90f, 0f); // kapalı pozisyon
        openRot = Quaternion.Euler(0f, 0f, 0f);     // açık pozisyon
        transform.rotation = closedRot;
    }

    void Update()
    {
        if (isOpening)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, openRot, openSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, openRot) < 0.5f)
            {
                transform.rotation = openRot;
                isOpening = false;
            }
        }
    }

    public void Open()
    {
        isOpening = true;
    }
}