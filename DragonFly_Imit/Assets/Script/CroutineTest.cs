using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // 네트웤 WWW
public class CroutineTest : MonoBehaviour
{
    [SerializeField]
    UITexture m_uiTexture;
    [SerializeField]
    SpriteRenderer m_spriteRenderer;
    [SerializeField]
    float m_duration = 1f;
    float m_time;
    float m_normalizeTime;
    Texture2D m_texture;
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(Coroutine_LoadTexture("https://mblogthumb-phinf.pstatic.net/MjAxOTEyMjRfNCAg/MDAxNTc3MTkwMTI2MjQ3.XoJdyJgha7sv8WOMFlkTpGwb-h7tNV1EkzNDBJg3W_gg.Vjy68iI-vs6iGR38aGAuXRzXDWfshGC7o7CeqBB5ym4g.JPEG.milkhanger/IMG_6150.JPG?type=w800"));
    }

    /*IEnumerator Coroutine_Event()
    {
        a();// A를 처리하는데 3초가 걸린다 생각 
        yield return new WaitForSeconds(3);//함수 실행시킨후 빠져나옴 3초동안
        b();// B를 처리하는데 3초가 걸린다 생각
        yield return new WaitForSeconds(3);//함수 실행시킨후 빠져나옴 3초동안
    }*/

    IEnumerator Coroutine_LoadTexture(string url)
    {
        /*WWW www = new WWW("https://www.naver.com");
        yield return www; // 네이버에 호출을 받게되면 시작~
        if(www.isDone)
        {

        }*/
        var www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();//요청한 동시에 빠져나오고 다시 진입할때는  return 응답이 올때
        if(www.isDone)//정상적인 신호 시
        {
           m_uiTexture.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            m_uiTexture.MakePixelPerfect();//픽셀 조정
        }
        else if(www.isNetworkError || www.isHttpError)//에러 신호 경우
        {
            Debug.Log(www.error);
        }
    }
    IEnumerator Coroutine_Count(float time)
    {
        int count = 0;
        while (true)
        {
            yield return new WaitForSeconds(time);//null을 하게되면 업데이트문 하나 더만든거임
            Debug.Log(++count);
        }
    }// 이렇게되면 1초마다 업데이트 되는 쓰레드문 하나를 완성한 셈 멀티쓰레드와 유사한 효능을 가짐
    IEnumerator Coroutine_Overlay(float duration)
    {
        while(true)
        {
            m_time += Time.deltaTime;
            m_normalizeTime = m_time / m_duration;
            var color = m_spriteRenderer.color;
            m_spriteRenderer.color = new Color(color.r, color.g, color.b, color.a = 1f - m_normalizeTime);
            if (m_normalizeTime > 1f)
            {
                m_normalizeTime = 0f;
                m_time = 0f;
                yield break;
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
