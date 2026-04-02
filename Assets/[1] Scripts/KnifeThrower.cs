using UnityEngine;

public class KnifeThrower : MonoBehaviour
{
    [SerializeField] private GameObject knifeProjectilePrefab;

    public static KnifeThrower instance;
    void Awake(){ Setup(); }
    void Setup()
    {
            if (instance != null){ Destroy(gameObject); return; }
        instance = this;
    }

    public static void ThrowKnife()
    {
            if (!instance.isActiveAndEnabled){ return; }
        Quaternion spawnRotation = FindAnyObjectByType<TAG_3DCamera>().transform.rotation;
        Instantiate(instance.knifeProjectilePrefab, instance.transform.position, spawnRotation);
    }
}