using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 2f;
    int m_hp;
    MonsterManager.MonsterType m_type;//자기가 무슨 몬스터인지 확인이 필요
    Animator m_animator;
    public MonsterManager.MonsterType Type {  get { return m_type; } set { m_type = value; } }

    public void SetMonster()
    {
        m_hp = (int)Type + 2;
    }
    public void SetDie()
    {
        EffectManager.Instance.CreateEffect(transform.position);
        GameItemManager.Instance.CreateItem(transform.position);
        GameUIManager.Instance.SetHuntScore(Mathf.RoundToInt(((int)m_type + 1) * 11.3f));
    }
    public void SetDamage(int damage)
    {
        if(damage == -1)
            m_hp = 0;
        else 
            m_hp -= damage;

        m_animator.Play("Hit", 0, 0f);
        if(m_hp <= 0)
        {
            if (Type == MonsterManager.MonsterType.Bomb)
            {
                MonsterManager.Instance.DestoryMonsters(this);
            }
            else
            {
                SetDie();
                MonsterManager.Instance.RemoveMonster(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("collider_bottom"))
        {
            MonsterManager.Instance.RemoveMonster(this);
        }
        else if(collision.CompareTag("Invincible"))
        {
            SetDamage(-1);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * m_speed * MonsterManager.Instance.m_speedScale * Time.deltaTime;
    }
}
