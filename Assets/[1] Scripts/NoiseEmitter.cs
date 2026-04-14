using UnityEngine;
using NaughtyAttributes;

public class NoiseEmitter : MonoBehaviour
{
    public float noiseRadius = 18f;
    public Transform GF;
    public GirlfriendAI GF_Script;

    public bool _test;

    private void Update()
    {
        if (_test)
        {
            _test = false;
            EmitNoise();
        }
    }
    [Button] public void EmitNoise() 
    {
        if(noiseRadius >= Vector3.Distance(transform.position, GF.position)) 
        {
            GF_Script.HearNoise(transform.position);
        }
    }

    public void EmitNoiseAtRange(float range) 
    {
        if (range >= Vector3.Distance(transform.position, GF.position)) 
        {
            GF_Script.HearNoise(transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}