using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemManager : SingletonMonobehaviour<GameItemManager>
{
    public struct ItemData
    {
        public ItemType m_type;
        public int m_value;
    }//해당 아이템의 정보를 담기위해 젬은 돈을 더주고 골드는 1원 이므로 int
    public enum ItemType
    {
        Coin,
        Gem_Red,
        Gem_Green,
        Gem_Blue,
        Invincible,
        Magnet,
        Max
    }
    [SerializeField]
    Sprite[] m_iconSprites;
    [SerializeField]
    GameObject m_itemPrefab;
    [SerializeField]
    PlayerController m_player;
    GameObjectPool<GameItemController> m_itemPool;
    Dictionary<ItemType, ItemData> m_itemDatas = new Dictionary<ItemType, ItemData>();

    float m_basePosY = 5.17f;
    float m_endPosY = -10f;
    float m_maxDuration = 3f;
    int[] m_itemTable = { 3, 3, 2, 1, 46, 45 };//아이템 확률표
    //외부데이터에서 확률표를 받느냐, 여기서 수정하느냐.
    //그래서 Util이란 스크립트를 만듬.
    float[] m_itemTableSingle = { 97.0f, 1.5f, 0.7f, 0.3f, 0.1f, 1.0f };

    public void RemoveItem(GameItemController item)
    {
        item.gameObject.SetActive(false);
        m_itemPool.Set(item);
    }

    public void CreateItem(Vector3 pos)
    {
        ItemType type;
        var dir = (m_player.transform.position - pos).normalized;
        var item = m_itemPool.Get();
        dir.y = 0f;
        item.m_from = pos;
        item.m_to = new Vector3(pos.x, m_endPosY) + dir * 2f;
        item.m_duration = (item.m_from - item.m_to).magnitude / Mathf.Abs(m_endPosY - m_basePosY) * m_maxDuration;
        do
        {
            type = (ItemType)Util.GetPriority(m_itemTable);
        } while (GameStateManager.Instance.State == GameStateManager.GameState.Invincible && type == ItemType.Invincible);
        item.SetItem(pos, m_itemDatas[type], m_iconSprites[(int)type]);
    }

    void InitItemData()
    {
        m_itemDatas.Add(ItemType.Coin, new ItemData() { m_type = ItemType.Coin, m_value = 1 });
        //struct 이용해서 함수사용
        m_itemDatas.Add(ItemType.Gem_Red, new ItemData() { m_type = ItemType.Gem_Red, m_value = 10 });
        m_itemDatas.Add(ItemType.Gem_Green, new ItemData() { m_type = ItemType.Gem_Green, m_value = 20 });
        m_itemDatas.Add(ItemType.Gem_Blue, new ItemData() { m_type = ItemType.Gem_Blue, m_value = 30 });
        m_itemDatas.Add(ItemType.Invincible, new ItemData() { m_type = ItemType.Invincible, m_value = 3 });
        m_itemDatas.Add(ItemType.Magnet, new ItemData() { m_type = ItemType.Magnet, m_value = 10 });//여기서의 밸류는 지속시간으로 설정
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_itemPool = new GameObjectPool<GameItemController>(10, () =>
        {
            var obj = Instantiate(m_itemPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var item = obj.GetComponent<GameItemController>();
            item.InitItem(m_player);
            return item;
        });
        InitItemData();
    }
}
