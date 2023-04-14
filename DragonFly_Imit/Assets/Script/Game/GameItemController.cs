using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer m_icon;
    [SerializeField]
    AnimationCurve m_baseYCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // x=y그래프
    [SerializeField]
    AnimationCurve m_posYCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    TweenRotation m_tweenRot;
    PlayerController m_player;

    bool m_isMagnet; //
    bool m_isResume;
    public Vector3 m_from;
    public Vector3 m_to;
    public float m_duration = 2f;
    float m_time;
    GameItemManager.ItemData m_itemData;
    IEnumerator Coroutine_PlayCurve()
    {
        yield return new WaitForEndOfFrame();
        float value = 0f;
        Vector3 moveVector = Vector3.zero;
        m_time = 0f;
        while(m_time < 1f)
        {
            yield return null;
            m_time += Time.deltaTime / m_duration;
            if (m_isResume)
            {
                m_isResume = false;
                m_from = transform.position;
                m_to = new Vector3(m_from.x, m_to.y);
            }
            if (!m_isMagnet)
            {
                value = m_baseYCurve.Evaluate(m_time);
                moveVector = m_from * (1f - value) + m_to * value;
                value = m_posYCurve.Evaluate(m_time);

                transform.position = moveVector + Vector3.up * value * 2f;
            }
        }
    }
    public void InitItem(PlayerController player)
    {
        m_player = player;
    }
    public void SetItem(Vector3 pos, GameItemManager.ItemData itemData, Sprite icon)
    {
        m_itemData = itemData;
        m_isMagnet = false;
        m_isResume = false;
        m_icon.sprite = icon;
        transform.position = pos;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine("Coroutine_PlayCurve");
        transform.localRotation = Quaternion.identity;
        if(m_itemData.m_type >= GameItemManager.ItemType.Gem_Red && m_itemData.m_type <= GameItemManager.ItemType.Gem_Blue)
        {
            m_tweenRot.ResetToBeginning();
            m_tweenRot.PlayForward();
        }
        else
        {
            m_tweenRot.enabled = false;
        }
    }

    void SetInvincibleEffectOnFinish()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Normal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("collider_bottom"))
        {
            GameItemManager.Instance.RemoveItem(this);
        }
        else if(collision.CompareTag("Magnet"))
        {
            m_isMagnet = true;
        }
        else if(collision.CompareTag("Player"))
        {
            GameItemManager.Instance.RemoveItem(this);
            switch (m_itemData.m_type)
            {
                case GameItemManager.ItemType.Coin:
                    GameUIManager.Instance.SetGoldCount(1);
                    break;
                case GameItemManager.ItemType.Gem_Red:
                    break;
                case GameItemManager.ItemType.Gem_Green:
                    break;
                case GameItemManager.ItemType.Gem_Blue:
                    GameUIManager.Instance.SetGoldCount((int)m_itemData.m_type * 10);
                    break;
                case GameItemManager.ItemType.Invincible:
                    GameStateManager.Instance.SetState(GameStateManager.GameState.Invincible, m_itemData.m_value);
                    break;
                case GameItemManager.ItemType.Magnet:
                    m_player.SetMagnetEffect(m_itemData.m_value);
                    break;

            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))
        {
            m_isMagnet = false;
            m_isResume = true;
        }
    }
    void Update()
    {
     if(m_isMagnet)
     {
            transform.position += (m_player.transform.position + transform.position).normalized * 15f * Time.deltaTime;
     }
    }
}
