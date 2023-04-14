using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float m_duration = 1f;
    float m_power = 0.3f;
    float m_time;
    bool m_isStart;
    
    public void Play(float duration, float power)
    {
        m_duration = duration;
        m_power = power;
        m_isStart = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isStart)
        {
            var value = Random.insideUnitCircle * m_power;
            transform.localPosition = new Vector3(value.x, value.y, transform.position.z);
            m_time += Time.deltaTime;
            if(m_time > m_duration)
            {
                m_time = 0f;
                m_isStart = false;
                transform.localPosition = new Vector3(0f, 0f, transform.position.z);
            }
        }
    }
}
