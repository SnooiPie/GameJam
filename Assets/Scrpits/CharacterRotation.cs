using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public float rotationSpeed = 360f; // derece/saniye
    public bool useLocalRotation = false; // parent rotasyonu varsa true yap

    private float initialY;   // modelin "0" yönüne ofset
    private float targetY;

    void Start()
    {
        // Başlangıçtaki global/local Y açısını kaydet
        initialY = useLocalRotation ? transform.localEulerAngles.y : transform.eulerAngles.y;
        targetY = initialY;
    }

    void Update()
    {
        // Inputları al (diagonalleri de destekle)
        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);

        // Hedef açıyı initialY'ye göre belirle (negatif açı değil, 0..360 aralığı kullan)
        if (w && !a && !s && !d)           targetY = initialY + 0f;
        else if (d && !w && !s && !a)      targetY = initialY + 90f;
        else if (s && !w && !a && !d)      targetY = initialY + 180f;
        else if (a && !w && !s && !d)      targetY = initialY + 270f; // -90 yerine 270 kullanıyoruz
        else if (w && d)                   targetY = initialY + 45f;
        else if (d && s)                   targetY = initialY + 135f;
        else if (s && a)                   targetY = initialY + 225f;
        else if (a && w)                   targetY = initialY + 315f;
        // else: tuşa basılmıyorsa targetY değişmez (korur)

        // Mevcut açıyı hedefe doğru düzgün döndür (sarılma/360 sınırını doğru ele alır)
        float currentY = useLocalRotation ? transform.localEulerAngles.y : transform.eulerAngles.y;
        float newY = Mathf.MoveTowardsAngle(currentY, targetY, rotationSpeed * Time.deltaTime);

        if (useLocalRotation)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
        else
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
    }
}
