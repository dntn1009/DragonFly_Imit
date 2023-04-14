using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class PlayerDataManager : DonDestory<PlayerDataManager>
{
    const int BASIC_GOLD = 10000;
    const int BASIC_GEM = 1000;

    [SerializeField]
    HeroData[] m_herosData; // PlayerData 스크립트에 있는 HeroData는 [System.Serializable] 가 선언되어있어야한다.
    //클래스 객체라 정렬화하여 보내주는 것 같다고 한다.
    PlayerData m_myData;

    public void IncreaseGold(int gold)
    {
        m_myData.m_gemOwned += gold;
    }

    public int GetGold()
    {
        return m_myData.m_goldOwned;
    }
    public int GetGem()
    {
        return m_myData.m_gemOwned;
    }
    public int GetBestRecord()
    {
        return m_myData.m_bestRecord;
    }
    public void SetBestRecord(int score)
    {
        if (score < m_myData.m_bestRecord) return;
        m_myData.m_bestRecord = score;
    }
    public int GetHeroIndex()
    {
        return m_myData.m_heroIndex;
    }
    public bool SetHeroIndex(int index)
    {
        /*if (m_myData.m_heroList[index].m_isOpen)
        {
            m_myData.m_heroIndex = index;
            return true;
        }
        return false;*/
        m_myData.m_heroIndex = index;
        return true;
    }
    public void Save()
    {
       var jsonData = JsonWriter.Serialize(m_myData);
        if(!string.IsNullOrEmpty(jsonData))
        {
            PlayerPrefs.SetString("PLAYER_DATA", jsonData);
            PlayerPrefs.Save();
        }
        //m_mydata를 json형식으로 바꾼거임
        // PlayerPrefs 안드로이드의 프리퍼런스 같은 느낌
        //근데 안좋음 그래서 json을 활용함
    }
    public bool Load()
    {
        //지금 저장된 json은 레지스트리 영역에 저장되어있음
        var jsonData = PlayerPrefs.GetString("PLAYER_DATA", string.Empty);
        if (string.IsNullOrEmpty(jsonData))
        {
            m_myData = JsonReader.Deserialize<PlayerData>(jsonData);
            return true;
        }
        else // 저장된 데이터가 없을경우 기본값만 주기 위해
        {
            m_myData = new PlayerData() { m_goldOwned = BASIC_GOLD, m_gemOwned = BASIC_GEM, m_heroIndex = 0 };
            m_myData.m_heroList.AddRange(m_herosData);
        }
        return false;
    }
    protected override void OnStart()
    {
        //PlayerPrefs.DeleteAll();
        if(!Load())
        {
            Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
