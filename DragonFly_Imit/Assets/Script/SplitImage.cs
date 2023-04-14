using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SplitImage : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_sprites;
    [SerializeField]
    UITexture m_texture;
    // Start is called before the first frame update
    void Start()
    {
        m_sprites = Resources.LoadAll<Sprite>("Fonts/");
        for(int i = 0; i < m_sprites.Length; i++)
        {
            var spr = m_sprites[i];
            Texture2D texture = new Texture2D((int)spr.rect.width, (int)spr.rect.height, TextureFormat.ARGB32, false);
            //텍스처포맷의 확장자는 PC 안드로이드(etc) 마다 다르므로 설정이 필요함.
            //근데 이건 이미지로  뽑을거라 손실이 적은 ARGB32로 설정

            texture.SetPixels(spr.texture.GetPixels((int)spr.rect.x, (int)spr.rect.y, (int)spr.rect.width, (int)spr.rect.height));
            //위의 setpixels를 풀어놓은 함수가 밑에 주석이다.
            /*for(int y = 0; y < texture.height; y++)
            {
                for(int x = 0; x < texture.width; x++)
                {
                    Color color = spr.texture.GetPixel((int)spr.rect.x + x, (int)spr.rect.y + y);
                    texture.SetPixel(x, y, color);
                }
            }*/
            texture.Apply();
            m_texture.mainTexture = texture;
            m_texture.MakePixelPerfect();
            var image = texture.EncodeToPNG();
            var path = string.Format(Application.dataPath + "/Images/Fonts/FontImage_{0:00}.png", i + 1);
            File.WriteAllBytes(path, image);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
