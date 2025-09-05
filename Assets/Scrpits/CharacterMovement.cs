using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float waitAtSphereTime = 2f;
    
    private bool isMoving = false;
    private Coroutine movementCoroutine;
    private int currentTargetSphereID = -1;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed * 100f;
        agent.stoppingDistance = 0.1f;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    public void MoveToSphere(List<Transform> pathWaypoints, SphereInteraction sphere, int sphereID)
    {
        if (isMoving) 
        {
            Debug.Log("AI zaten hareket ediyor!");
            return;
        }
        
        if (sphere == null)
        {
            Debug.LogError("Hedef sphere null!");
            return;
        }

        // Yeni hedef için önceki path'i sıfırla
        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = false;
        }

        waypoints = pathWaypoints;
        currentTargetSphereID = sphereID;
        
        // Hareketi başlat
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);
        
        movementCoroutine = StartCoroutine(MoveToSphereCoroutine(sphere));
    }

    private IEnumerator MoveToSphereCoroutine(SphereInteraction sphere)
    {
        isMoving = true;
        Debug.Log($"AI, {sphere.id} ID'li sphere'e gidiyor (Hedef ID: {currentTargetSphereID})...");

        // Hedef sphere'in hala var olduğundan ve doğru ID'ye sahip olduğundan emin ol
        if (sphere == null || sphere.id != currentTargetSphereID)
        {
            Debug.LogError("Hedef sphere kayboldu veya değişti!");
            isMoving = false;
            yield break;
        }

        // 1. En uygun waypoint'e git (varsa)
        Transform nearestWaypoint = FindNearestWaypoint(sphere.transform.position);
        if (nearestWaypoint != null)
        {
            Debug.Log($"Seçilen waypoint: {nearestWaypoint.name} (pos: {nearestWaypoint.position})");
            yield return StartCoroutine(MoveToTarget(nearestWaypoint.position));
        }

        // 2. Sphere'e git (yeniden kontrol)
        if (sphere != null && sphere.id == currentTargetSphereID)
        {
            yield return StartCoroutine(MoveToTarget(sphere.transform.position));

            // 3. Bekle
            yield return new WaitForSeconds(waitAtSphereTime);

            // 4. Sphere'i topla (son kontrol)
            if (sphere != null && sphere.id == currentTargetSphereID)
            {
                sphere.Collect();
                Debug.Log($"Sphere {sphere.id} toplandı!");
            }
        }

        // Hareket tamamlandı - tamamen temizle
        StopMovement();
        Debug.Log("Hareket tamamlandı!");
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent bulunamadı!");
            yield break;
        }

        // HARD RESET - Agent'ı disable/enable ederek tamamen sıfırla
        agent.enabled = false;
        yield return null;
        agent.enabled = true;
        yield return null;

        // Eğer ajan NavMesh üzerinde değilse, en yakın geçerli pozisyona warp et
        if (!agent.isOnNavMesh)
        {
            NavMeshHit agentHit;
            if (NavMesh.SamplePosition(transform.position, out agentHit, 5.0f, NavMesh.AllAreas))
            {
                Debug.Log($"Agent NavMesh üzerinde değildi. Warp ile NavMesh'e yerleştiriliyor: {agentHit.position}");
                agent.Warp(agentHit.position);
            }
            else
            {
                Debug.LogError("Agent NavMesh üzerinde değil ve yakınında NavMesh bulunamadı!");
                yield break;
            }
        }

        // Agent'ı yeniden yapılandır
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed * 100f;
        agent.stoppingDistance = 0.1f;
        agent.updatePosition = true;
        agent.updateRotation = true;

        // Hedef pozisyonu NavMesh üzerinde örnekle
        Vector3 navTarget = GetValidNavMeshPosition(targetPosition);
        Vector3 directionToTarget = (navTarget - agent.transform.position).normalized;
        Debug.Log($"Agent pos: {agent.transform.position}");
        Debug.Log($"Hedef world pos: {targetPosition} -> NavTarget: {navTarget}");
        Debug.Log($"Hedefe yön vektörü: {directionToTarget}");
        Debug.Log($"Agent'ın baktığı yön: {agent.transform.forward}");

        // Önce agent'ı hedefe doğru çevir
        transform.LookAt(new Vector3(navTarget.x, transform.position.y, navTarget.z));
        yield return new WaitForSeconds(0.1f);

        // Basit SetDestination kullan
        agent.isStopped = false;
        agent.SetDestination(navTarget);

        // Path hesaplanana kadar bekle
        while (agent.pathPending)
        {
            yield return null;
        }

        // Path geçerli değilse iptal et
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogWarning("Geçersiz path hesaplandı!");
            yield break;
        }
        
        // PathPartial durumunda da devam et, ancak uyar
        if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Partial path hesaplandı - hedefe tam olarak ulaşılamayabilir, mümkün olduğu kadar yaklaşılacak.");
        }

        Debug.Log($"Path durumu: {agent.pathStatus}, Kalan mesafe: {agent.remainingDistance}");

        // Hedefe ulaşana kadar bekle
        float stuckTimer = 0f;
        float stuckThreshold = 2f;
        Vector3 lastPosition = transform.position;
        float positionCheckInterval = 0.5f;
        float positionCheckTimer = 0f;

        while (agent.remainingDistance > agent.stoppingDistance && agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            // Takılma kontrolü - pozisyon değişimi ile
            positionCheckTimer += Time.deltaTime;
            if (positionCheckTimer >= positionCheckInterval)
            {
                float distanceMoved = Vector3.Distance(transform.position, lastPosition);
                if (distanceMoved < 0.1f) // Çok az hareket ettiyse
                {
                    stuckTimer += positionCheckInterval;
                }
                else
                {
                    stuckTimer = 0f;
                }
                
                lastPosition = transform.position;
                positionCheckTimer = 0f;
            }

            // Eğer takılmışsa yeniden path hesapla
            if (stuckTimer > stuckThreshold)
            {
                Debug.LogWarning("Agent takılmış görünüyor, path yeniden hesaplanıyor...");
                agent.ResetPath();
                agent.SetDestination(navTarget);
                stuckTimer = 0f;
                
                // Path yeniden hesaplanana kadar bekle
                while (agent.pathPending)
                {
                    yield return null;
                }
            }

            yield return null;
        }

        // Hedefe ulaştığında durmasını sağla
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            Debug.Log("Hedefe başarıyla ulaşıldı!");
        }
    }

    private Vector3 GetValidNavMeshPosition(Vector3 targetPosition)
    {
        NavMeshHit hit;
        
        // Önce 1 metre yarıçapında ara
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        // Sonra 3 metre yarıçapında ara
        else if (NavMesh.SamplePosition(targetPosition, out hit, 3.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        // Son olarak 10 metre yarıçapında ara
        else if (NavMesh.SamplePosition(targetPosition, out hit, 10.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning($"Hedef pozisyona yakın NavMesh bulunamadı: {targetPosition}");
            return targetPosition;
        }
    }

    private Transform FindNearestWaypoint(Vector3 targetPosition)
    {
        if (waypoints == null || waypoints.Count == 0) return null;

        Transform bestWaypoint = null;
        float bestScore = float.MinValue;

        Vector3 currentPos = transform.position;
        Vector3 directionToTarget = (targetPosition - currentPos).normalized;

        foreach (Transform waypoint in waypoints)
        {
            if (waypoint == null) continue;

            Vector3 directionToWaypoint = (waypoint.position - currentPos).normalized;
            float distance = Vector3.Distance(currentPos, waypoint.position);
            
            // Hedef yönüne doğru olan waypoint'leri tercih et (dot product > 0)
            float directionScore = Vector3.Dot(directionToWaypoint, directionToTarget);
            
            // Mesafe skoru (yakın olanlar daha iyi)
            float distanceScore = 1f / (1f + distance * 0.1f);
            
            // Toplam skor: yön önemli, mesafe ikincil
            float totalScore = directionScore * 2f + distanceScore;
            
            if (totalScore > bestScore)
            {
                bestScore = totalScore;
                bestWaypoint = waypoint;
            }
        }

        // Eğer hiçbir waypoint hedef yönünde değilse, en yakın olanı seç
        if (bestWaypoint == null || bestScore <= 0)
        {
            float closestDistance = float.MaxValue;
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint == null) continue;
                
                float distance = Vector3.Distance(currentPos, waypoint.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestWaypoint = waypoint;
                }
            }
        }

        return bestWaypoint;
    }

public void TeleportInstantly(Vector3 targetPosition)
{
    if (agent == null) return;

    // Mevcut coroutine veya path varsa durdur
    StopMovement();

    // NavMesh üzerinde geçerli bir pozisyon bul
    NavMeshHit hit;
    if (NavMesh.SamplePosition(targetPosition, out hit, 2f, NavMesh.AllAreas))
    {
        agent.Warp(hit.position);
    }
    else
    {
        agent.Warp(targetPosition); // fallback
    }
}

    public void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        isMoving = false;
        currentTargetSphereID = -1;
        Debug.Log("Hareket durduruldu!");
    }
}