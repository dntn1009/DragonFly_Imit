using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxController : MonoBehaviour
{
    ParticleSystem[] m_particles;
    // Start is called before the first frame update
    void Start()
    {
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlaying = false;
        for(int i = 0; i < m_particles.Length; i++)
        {
            if(m_particles[i].isPlaying)
            {
                isPlaying = true;
                break;
            }
        }
        if(!isPlaying)
        {
            EffectManager.Instance.RemoveEffect(this);
        }
    }
}
