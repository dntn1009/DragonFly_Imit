using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class GameUIManager : SingletonMonobehaviour<GameUIManager>
{
    [SerializeField]
    UILabel m_distScoreLabel;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_goldCountLabel;
    int m_distScore;
    int m_huntScore;
    int m_goldCount;
    StringBuilder m_sb = new StringBuilder(); // string 보관하는곳

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetDistanceScore(float dist)
    {
        m_distScore = Mathf.RoundToInt(dist * 100);
       m_distScoreLabel.text = m_sb.AppendFormat("{0:n0}", dist * 100).ToString();
        m_sb.Clear();
    }

    public int GetDistanceScore()
    {
        return m_distScore;
    }

    public void SetHuntScore(int score)
    {
        m_huntScore += score;
        m_huntScoreLabel.text = m_sb.AppendFormat("{0:n0}", m_huntScore).ToString();
        m_sb.Clear();
    }

    public int GetHuntScore()
    {
        return m_huntScore;
    }

    public void SetGoldCount(int gold)
    {
        m_goldCount += gold;
        m_goldCountLabel.text = m_sb.AppendFormat("{0:n0}", m_goldCount).ToString();
        m_sb.Clear();
    }

    public int GetGoldCount()
    {
        return m_goldCount;
    }

    public void OnPressPause()
    {
       Time.timeScale = Time.timeScale == 0 ? 1 : 0; // 0이면 1 반환 아니면 0 변환
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_distScore = 0;
        m_huntScore = 0;
        m_goldCount = 0;
        SetDistanceScore(m_distScore);
        SetHuntScore(m_huntScore);
        SetGoldCount(m_goldCount);
    }
}
