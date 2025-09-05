using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerHighlight : MonoBehaviour
{
    [Tooltip("Highlight rengi (default: sarı)")]
    public Color highlightColor = Color.yellow;

    // Orijinal materyalleri kaydetmek için renderer -> materials
    private static Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    // Hangi renderer kaç trigger tarafından highlight edilmiş (ref count)
    private static Dictionary<Renderer, int> highlightRefCount = new Dictionary<Renderer, int>();

    void Reset()
    {
        // Kolay kullanım: otomatik olarak trigger yap
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Bu trigger'ın parent'ı
        if (transform.parent == null) return;
        var parent = transform.parent.gameObject;

        // Önce renderer'ı bul: parent'ta yoksa çocuklarda ara
        var rend = parent.GetComponent<Renderer>() ?? parent.GetComponentInChildren<Renderer>();
        if (rend == null) return;

        // Eğer ilk defa highlight edilecekse orijinal materyalleri sakla
        if (!originalMaterials.ContainsKey(rend))
        {
            // clone the materials array so we can restore them later
            originalMaterials[rend] = rend.materials;
        }

        // Artır ref count
        if (!highlightRefCount.ContainsKey(rend)) highlightRefCount[rend] = 0;
        highlightRefCount[rend]++;

        // Eğer zaten highlight'lıysa tekrar oluşturma (zaten sarı)
        if (highlightRefCount[rend] > 1) return;

        // Yeni materyaller oluştur (orijinal sharedMaterial üzerinden kopyala) ve renklerini sarı yap
        var newMats = new Material[rend.materials.Length];
        for (int i = 0; i < newMats.Length; i++)
        {
            // sharedMaterial kullanarak ham template al, sonra instance
            var template = rend.sharedMaterials.Length > i ? rend.sharedMaterials[i] : null;
            if (template != null)
                newMats[i] = new Material(template);
            else
                newMats[i] = new Material(Shader.Find("Standard"));

            // Eğer materyalin _Color propertysi varsa set et, yoksa fallback yapma
            if (newMats[i].HasProperty("_Color"))
                newMats[i].color = highlightColor;
        }

        rend.materials = newMats;
    }

    void OnTriggerExit(Collider other)
    {
        if (transform.parent == null) return;
        var parent = transform.parent.gameObject;

        var rend = parent.GetComponent<Renderer>() ?? parent.GetComponentInChildren<Renderer>();
        if (rend == null) return;

        // Eğer hiç kayıt yoksa çık
        if (!highlightRefCount.ContainsKey(rend)) return;

        highlightRefCount[rend]--;
        if (highlightRefCount[rend] > 0) return; // hâlâ başka trigger'lar aktif

        // ref sıfırlandıysa orijinal materyalleri geri koy
        if (originalMaterials.ContainsKey(rend))
        {
            rend.materials = originalMaterials[rend];
            originalMaterials.Remove(rend);
        }

        highlightRefCount.Remove(rend);
    }

    // sahne değişimlerinde temiz tutmak için (opsiyonel)
    void OnDestroy()
    {
        // Eğer obje yok edilirse referanslar düzgün düşsün
        if (transform.parent == null) return;
        var parent = transform.parent.gameObject;
        var rend = parent.GetComponent<Renderer>() ?? parent.GetComponentInChildren<Renderer>();
        if (rend == null) return;

        if (highlightRefCount.ContainsKey(rend))
        {
            highlightRefCount[rend]--;
            if (highlightRefCount[rend] <= 0)
            {
                if (originalMaterials.ContainsKey(rend))
                {
                    rend.materials = originalMaterials[rend];
                    originalMaterials.Remove(rend);
                }
                highlightRefCount.Remove(rend);
            }
        }
    }
}
