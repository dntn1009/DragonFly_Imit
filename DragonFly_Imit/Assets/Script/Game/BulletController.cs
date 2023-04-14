using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 10f;
    PlayerController m_player;
    void RemoveBullet()
    {
        m_player.RemoveBullet(this); // 해당 총알의 객체를 넣어줌 queue에
        //Destroy(gameObject);
        //System.GC.Collect();//갈비지데이터를 지워 메모리를 비운다. 매번하면 큰일남 느려짐
        //가용 메모리가 부족해지면 해야됌 왜냐? 삭제하고 갈비지데이터는 삭제되도 남아져있는 데이터이기 때문
    }

    public void InitBullet(PlayerController player)
    {
        m_player = player;
    }
    public void SetBullet(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        if (IsInvoking("RemoveBullet"))//시간이 예약이 걸려 있다면
            CancelInvoke("RemoveBullet");
         Invoke("RemoveBullet", 2f);//3초뒤에 불렛 삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
           var mon = collision.gameObject.GetComponent<MonsterController>();
            m_player.RemoveBullet(this);
            mon.SetDamage(1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * m_speed * Time.deltaTime;
    }
}
