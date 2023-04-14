using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 0.5f;
    float m_speedScale = 1f;
    SpriteRenderer m_sprRenderer;

    public void SetSpeed(float scale)
    {
        m_speedScale = scale;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_sprRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        m_sprRenderer.material.mainTextureOffset += Vector2.up * m_speed * m_speedScale * Time.deltaTime;
        GameUIManager.Instance.SetDistanceScore(m_sprRenderer.material.mainTextureOffset.y);
    }
}
