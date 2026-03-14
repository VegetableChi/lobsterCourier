using UnityEngine;

/// <summary>
/// 粒子效果管理器 - 预制粒子效果池
/// </summary>
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }
    
    [Header("粒子预制体")]
    public GameObject deliverySuccessPrefab;
    public GameObject deliveryFailPrefab;
    public GameObject pickupPrefab;
    public GameObject sprintPrefab;
    public GameObject exhaustedPrefab;
    public GameObject coinPrefab;
    public GameObject achievementPrefab;
    public GameObject levelUpPrefab;
    
    [Header("对象池")]
    public int poolSize = 20;
    private GameObject[][] particlePools;
    private bool[][] poolAvailable;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializePools();
    }
    
    void InitializePools()
    {
        int prefabCount = 8; // 预制体数量
        particlePools = new GameObject[prefabCount][];
        poolAvailable = new bool[prefabCount][];
        
        GameObject[] prefabs = new GameObject[]
        {
            deliverySuccessPrefab,
            deliveryFailPrefab,
            pickupPrefab,
            sprintPrefab,
            exhaustedPrefab,
            coinPrefab,
            achievementPrefab,
            levelUpPrefab
        };
        
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] != null)
            {
                particlePools[i] = new GameObject[poolSize];
                poolAvailable[i] = new bool[poolSize];
                
                for (int j = 0; j < poolSize; j++)
                {
                    GameObject particle = Instantiate(prefabs[i], transform);
                    particle.SetActive(false);
                    particlePools[i][j] = particle;
                    poolAvailable[i][j] = true;
                }
            }
        }
    }
    
    public void PlayDeliverySuccess(Vector3 position)
    {
        PlayEffect(0, position);
    }
    
    public void PlayDeliveryFail(Vector3 position)
    {
        PlayEffect(1, position);
    }
    
    public void PlayPickup(Vector3 position)
    {
        PlayEffect(2, position);
    }
    
    public void PlaySprint(Vector3 position)
    {
        PlayEffect(3, position);
    }
    
    public void PlayExhausted(Vector3 position)
    {
        PlayEffect(4, position);
    }
    
    public void PlayCoin(Vector3 position)
    {
        PlayEffect(5, position);
    }
    
    public void PlayAchievement(Vector3 position)
    {
        PlayEffect(6, position);
    }
    
    public void PlayLevelUp(Vector3 position)
    {
        PlayEffect(7, position);
    }
    
    void PlayEffect(int index, Vector3 position)
    {
        if (particlePools[index] == null) return;
        
        for (int i = 0; i < poolSize; i++)
        {
            if (poolAvailable[index][i])
            {
                GameObject particle = particlePools[index][i];
                particle.transform.position = position;
                particle.SetActive(true);
                poolAvailable[index][i] = false;
                
                // 自动回收
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    float duration = ps.main.duration + ps.main.startLifetime.constantMax;
                    Invoke(nameof(RecycleParticle), duration, new object[] { index, i });
                }
                
                return;
            }
        }
    }
    
    void RecycleParticle(int poolIndex, int particleIndex)
    {
        if (particlePools[poolIndex] != null && particleIndex < poolSize)
        {
            GameObject particle = particlePools[poolIndex][particleIndex];
            if (particle != null)
            {
                particle.SetActive(false);
                poolAvailable[poolIndex][particleIndex] = true;
            }
        }
    }
}
