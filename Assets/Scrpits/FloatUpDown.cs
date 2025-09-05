using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatAmount = 0.5f;
    private Vector3 startPos;
    private bool hasRecordedStartPos = false;

    void Start()
    {
        // Başlangıç pozisyonunu kaydet
        startPos = transform.position;
        hasRecordedStartPos = true;
    }

    void Update()
    {
        // Eğer başlangıç pozisyonu kaydedilmemişse kaydet
        if (!hasRecordedStartPos)
        {
            startPos = transform.position;
            hasRecordedStartPos = true;
        }
        
        // Sadece Y ekseninde salınım yap
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    // Eğer başka bir script tarafından pozisyon değiştirilirse, başlangıç pozisyonunu sıfırla
    public void ResetStartPosition()
    {
        startPos = transform.position;
    }
}