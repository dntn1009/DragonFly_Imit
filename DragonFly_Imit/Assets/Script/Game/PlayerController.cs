using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 2f;
    [SerializeField]
    Vector3 m_dir;
    [SerializeField]
    Transform m_firePos;
    GameObject m_bulletPrefab;
    float m_time; // 총알 발사 자동으로 하기위해 프레임마다 발사하면 너무 많이발사 되니까 시간마다 발사되게 하기

    [SerializeField]
    GameObject m_fxMagnetObj;
    [SerializeField]
    GameObject m_fxInvincibleObj;
    GameObjectPool<BulletController> m_bulletPool; // 총알들을 미리 저장해두는 Pool
    Animator m_animator;
    bool m_isDrag; // 마우스로 캐릭이동
    Vector3 m_startPos;
    float m_moveX;

    public void SetMagnetEffect(int duration)
    {
        m_fxMagnetObj.SetActive(true);
        if (IsInvoking("EndMagnetEffect"))
            CancelInvoke("EndMagnetEffect");
        Invoke("EndMagnetEffect", duration);
    }

    public void SetInvincibleEffect(int duration)
    {
        m_fxInvincibleObj.SetActive(true);
        m_animator.Play("Invincible", 0, 0f);
        CancelInvoke("CreateBullet");
        if (IsInvoking("ChangeStateNormal"))
            CancelInvoke("ChangeStateNormal");
        Invoke("ChangeStateNormal", duration);
    }
    public void EndInvincibleEffect()
    {
        m_fxInvincibleObj.SetActive(false);
        m_animator.Play("idle2", 0, 0f);
        InvokeRepeating("CreateBullet", 1f, 0.1f);
    }
    void ChangeStateNormal()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Normal);
    }

    void CreateBullet()
    {
       var bullet =  m_bulletPool.Get();
       bullet.SetBullet(m_firePos.position);
        /*
          var obj = Instantiate(m_bulletPrefab);
          var bullet = obj.GetComponent<BulletController>();
          bullet.SetBullet(m_firePos.position); 
        */ // 기초 형식
           //Invoke("CreateBullet", 0.2f);//0.2초뒤에 함수 호풀
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Result);
        }
    }

    public void SetDie()
    {
        CancelInvoke("CreateBullet");
        gameObject.SetActive(false);
    }

    public void RemoveBullet(BulletController bullet)
    {
        bullet.gameObject.SetActive(false);
        m_bulletPool.Set(bullet);
    }

    void EndMagnetEffect()
    {
        m_fxMagnetObj.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_bulletPrefab = Resources.Load("Prefab/Bullet/bullet") as GameObject;
        //Invoke("CreateBullet", 3f);//3초뒤에 함수 호풀
        InvokeRepeating("CreateBullet", 3f, 0.1f);// 3뒤에 호출 후 0.2마다 발사
        m_bulletPool = new GameObjectPool<BulletController>(10, () =>
        {
            var obj = Instantiate(m_bulletPrefab);
            obj.SetActive(false);
            var bullet = obj.GetComponent<BulletController>();
            bullet.InitBullet(this);
            return bullet;
        });//총알 10개를 미리 만들어둠

        m_animator = GetComponent<Animator>();
        m_fxMagnetObj.SetActive(false);
        m_fxInvincibleObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* if(Input.GetKeyUp(KeyCode.LeftArrow))
         {
             m_dir = Vector3.zero;
         }
         if (Input.GetKeyUp(KeyCode.RightArrow))
         {
             m_dir = Vector3.zero;
         }
         if (Input.GetKeyDown(KeyCode.LeftArrow))
         {
             m_dir = Vector3.left;
         }
         if (Input.GetKeyDown(KeyCode.RightArrow))
         {
             m_dir = Vector3.right;
         }*/ // 좀 불편함 그래서 GetAxis로 변경(유니티 전용)

        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }*/

      /*  m_time += Time.deltaTime;//프레임단위로만 처리가 가능
        if(m_time > 0.2f)
        {
            CreateBullet();
            m_time = 0f;
        }*/
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f);
        if (m_isDrag)// True일 경우
        {
            var endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = endPos - m_startPos;
            dir.y = 0;
            m_moveX = Mathf.Abs(dir.x);
            var hit = Physics2D.Raycast(transform.position, dir.normalized, m_moveX, 1 << LayerMask.NameToLayer("Wall"));//움직인 거리내에서 Hit한 부분이 있는지 찾는다.
            //레이어마스크를 Wall로 체크해서 Wall만 허용하도록 한다.
            // 1 << 8 은 0000 0000 8비트에서 boolean을 이용하여 판별
            
                if (hit.collider != null)
                {
                    if (!hit.collider.name.Equals("collider_left") && dir.x > 0f || !hit.collider.name.Equals("collider_right") && dir.x < 0f)
                    {
                    m_moveX = hit.distance;
                    }
                }
            transform.position += dir.normalized * m_moveX;
            m_startPos = endPos;
        }
        if (Input.GetMouseButtonDown(0))
        {
            m_isDrag = true;
            // m_startPos = Input.mousePosition;//월드좌표계를 이용중이므로 마우스 조금만이용해도 확움직임
            m_startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButtonUp(0))
        {
            m_isDrag = false;
        }
        transform.position += m_dir * m_speed * Time.deltaTime;
    }
}
