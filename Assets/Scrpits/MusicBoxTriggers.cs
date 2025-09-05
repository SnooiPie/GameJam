using UnityEngine;

public class MusicBoxTriggers : MonoBehaviour
{
    public MusicBoxPuzzle musicBoxPuzzle; // Inspector'dan atayabilirsin

    void OnTriggerEnter(Collider other)
    {
        // Disk veya kaset AI tarafından getirildiyse
        if (other.CompareTag("Disk") || other.CompareTag("Kaset"))
        {
            // Diskin rengini veya ID'sini al
            SphereInteraction disk = other.GetComponent<SphereInteraction>();
            if (disk != null)
            {
                // Örneğin disk.id veya disk.color bilgisini kullan
                // Eğer renk ile çalışıyorsan:
                string color = disk.isCorrectSphere ? "Yeşil" : "Kırmızı"; // örnek
                // MusicBoxPuzzle'a bildir
                int boxIndex = 0; // Kutunun indexini uygun şekilde belirleyin
                musicBoxPuzzle.ColorSelected(boxIndex, color);

                // Disk/kaset kutuya yerleştirildiğinde yok edebilirsin
                Destroy(other.gameObject);
            }
        }
    }
}