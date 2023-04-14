using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroData
{
    public string m_name;
    public string m_className;
    public int m_level;
    public bool m_isOpen;

}

public class PlayerData
{
    public int m_goldOwned; // 골드획득량
    public int m_gemOwned; // 잼 획득량
    public int m_heroIndex;
    public int m_bestRecord;
    public List<HeroData> m_heroList = new List<HeroData>();
}
