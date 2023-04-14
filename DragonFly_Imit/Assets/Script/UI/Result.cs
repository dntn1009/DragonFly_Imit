using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField]
    UI2DSprite m_sdCharSprite;
    [SerializeField]
    GameObject m_bestRecordObj;
    [SerializeField]
    UILabel m_totalScoreLabel;
    [SerializeField]
    UILabel m_distScoreLabel;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_goldCountLabel;
    [SerializeField]
    UILabel m_bestRecordLabel;

    public void SetUI()
    {
        bool isBest = false;
        gameObject.SetActive(true);
        m_bestRecordObj.SetActive(false);
        int totalScore = GameUIManager.Instance.GetDistanceScore() + GameUIManager.Instance.GetHuntScore();
        if(totalScore > PlayerDataManager.Instance.GetBestRecord())
        {
            isBest = true;
            m_bestRecordObj.SetActive(true);
        }
        m_totalScoreLabel.text = string.Format("{0:n0}", totalScore);
        m_distScoreLabel.text = string.Format("{0:n0}", GameUIManager.Instance.GetDistanceScore());
        m_huntScoreLabel.text = string.Format("{0:n0}", GameUIManager.Instance.GetHuntScore());
        m_goldCountLabel.text = string.Format("{0:n0}", GameUIManager.Instance.GetGoldCount());
        m_bestRecordLabel.text = string.Format("{0:n0}", isBest ? totalScore : PlayerDataManager.Instance.GetBestRecord());

        m_sdCharSprite.sprite2D = Resources.Load<Sprite>(string.Format("SD/sd_{0:00}{1}", PlayerDataManager.Instance.GetHeroIndex() + 1, isBest ? "_highscore" : string.Empty));
        m_sdCharSprite.MakePixelPerfect();

        PlayerDataManager.Instance.IncreaseGold(GameUIManager.Instance.GetGoldCount());
        PlayerDataManager.Instance.SetBestRecord(totalScore);
        PlayerDataManager.Instance.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
