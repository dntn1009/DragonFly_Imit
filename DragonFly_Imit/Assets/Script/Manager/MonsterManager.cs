using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    public enum MonsterType
    {
        None = -1,
        White,
        Yellow,
        Pink,
        Bomb,
        Max
    }
    GameObject[] m_monstersPrefab;
    Dictionary<MonsterType, GameObjectPool<MonsterController>> m_monsterPool = new Dictionary<MonsterType, GameObjectPool<MonsterController>>(); // Queue
    // Dictionary를 넣는 이유는 화이트몬스터 하나만 생성하지 않고 3마리 특수 몬스터 넣어야되어서
    List<MonsterController> m_monsterList = new List<MonsterController>(); // 활성화된 모든 몬스터들을 넣을 예정 왜냐 폭탄 몬스터를 찾기위해
    Vector2 m_startPos = new Vector2(-5.14f, 5.17f);// 몬스터 시작위치
    float posGap = 1.13f; // 몬스터 간 사이 간격
    public float m_speedScale = 1f;

    public void CancelCreateMonster()
    {
        CancelInvoke("CreateMonsterGroup");
    }

    public void ResetCreateMonster(float scale)
    {
        m_speedScale = scale;
        CancelInvoke("CreateMonsterGroup");
        InvokeRepeating("CreateMonsterGroup", 0.5f, 5f / scale);
    }

    public void RemoveMonster(MonsterController mon)
    {
        mon.gameObject.SetActive(false);
        if (m_monsterList.Remove(mon))// 이 Remove에는 이미 지우고 없어졌으면 false로 반환해주는 값이 있다
        {                              //그래서 if문을 씌워주어서 아직 있으면 지우고 Set하라는 뜻이다.
            m_monsterPool[mon.Type].Set(mon);
        }
    }
    public void DestoryMonsters(MonsterController mon)
    {
        for(int i = 0; i < m_monsterList.Count; i++)
        {
            if(Mathf.Approximately(mon.transform.position.y, m_monsterList[i].transform.position.y))// 실수가 1.2 = 1.21 로 나와서 안될수 있기떄문에 근처오차값은 빼주는 형식으로 설정
            {
                m_monsterList[i].SetDie();
                m_monsterList[i].gameObject.SetActive(false);
                m_monsterPool[m_monsterList[i].Type].Set(m_monsterList[i]);
            }
        }
        m_monsterList.RemoveAll(enemy => enemy.gameObject.activeSelf == false);
        //false인 애들을 찾아서 지워준다.
        //RemoveMonster()을 써버리면 Monsterlist를 지우면서 이용하는거기때문에 오류발생한다.
    }
    void CreateMonsterGroup()
    {
        MonsterType type;
        bool isBomb = false;
        bool isCheck = false;
        for(int i = 0; i < 5; i++)
        {
            do {
                isCheck = false;
                type = (MonsterType)Random.Range((int)MonsterType.White, (int)MonsterType.Max);
                if (!isBomb && type == MonsterType.Bomb)
                {
                    isBomb = true;
                }
                else if (isBomb && type == MonsterType.Bomb)
                {
                    isCheck = true;
                }
            } while (isCheck);
            var mon = m_monsterPool[type].Get();
            m_monsterList.Add(mon);
            mon.gameObject.SetActive(true);
            mon.SetMonster();
            mon.transform.position = m_startPos + Vector2.right * posGap * i;
        }
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_monstersPrefab = Resources.LoadAll<GameObject>("Prefab/Monster/");
        //리소스 몬스터 폴더안의 프리팹을 모두 불러와라.

        for (int i = 0; i < (int)MonsterType.Max; i++)
        {
            var prefab = m_monstersPrefab[i];
            MonsterType type = (MonsterType)i;
           GameObjectPool<MonsterController> pool = new GameObjectPool<MonsterController>(5, () =>
            {
                var obj = Instantiate(prefab);// 배열문제로 인한 prefab 지역변수를 이용하면 x 주소값을 이용하게 바꿈
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                var mon = obj.GetComponent<MonsterController>();
                mon.Type = type;
                return mon;
            });
            m_monsterPool.Add((MonsterType)i, pool);
        }

        /*m_monsterPool = new GameObjectPool<MonsterController>(15, () =>
        {
            var obj = Instantiate(m_monstersPrefab[0]);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var mon = obj.GetComponent<MonsterController>();
            return mon;
        });*///이건 하나의 몹만 만들경우
        InvokeRepeating("CreateMonsterGroup", 3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
