using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

    public static int GetPriority(float[] table)//아이템 확률표 외부데이터를 이용할 경우
    {
        if (table == null || table.Length == 0) return -1;
        //오류값으로 -1을 설정
        float sum = 0f;
        for (int i = 0; i < table.Length; i++)
        {
            sum += table[i];
        }
        float num = Random.Range(1.0f, sum);//확률 랜덤
        //만약 91이 뜨면 90퍼까지의 코인은 안뜨고 91부터 1퍼인 아이템이 뜨는거임 젬_레드가 뜨는거임
        sum = 0f;

        for (int i = 0; i < table.Length; i++)
        {
            if (num > sum && num <= sum + table[i])
                return i;
            sum += table[i];
        }
        return -1;
    }

    public static int GetPriority(int[] table)//아이템 확률표 외부데이터를 이용할 경우
    {
        if (table == null || table.Length == 0) return -1;
        //오류값으로 -1을 설정
        int sum = 0;
        for(int i = 0; i < table.Length; i++)
        {
            sum += table[i];
        }
        int num = Random.Range(0, sum) + 1;//확률 랜덤
        //만약 91이 뜨면 90퍼까지의 코인은 안뜨고 91부터 1퍼인 아이템이 뜨는거임 젬_레드가 뜨는거임
        sum = 0;

        for(int i = 0; i < table.Length; i++)
        {
            if (num > sum && num <= sum + table[i])
                return i;
            sum += table[i];
        }
        return -1;
    }
}
