using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleManangerScript : MonoBehaviour
{
    [SerializeField] private UIParticleAttractor mobileParticleAttractor;

    private ParticleSystem _particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        ClearParticles();
    }

    public void ParticlesToTarget(Vector3 from, Vector3 to)
    {
        ClearParticles();
        
        mobileParticleAttractor.transform.position = to;
        _particleSystem.transform.position = from;
        
        mobileParticleAttractor.gameObject.SetActive(true);
        _particleSystem.Play();
    }

    public void ParticlesSphere(Vector3 at)
    {
        ClearParticles();
        _particleSystem.Play();
    }
    

    void ClearParticles()
    {
        _particleSystem.Stop();
        mobileParticleAttractor.gameObject.SetActive(false);
    }
}
