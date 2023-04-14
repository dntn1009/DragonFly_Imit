using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonMonobehaviour<GameStateManager>
{

    public enum GameState
    {
        Normal,
        Invincible,
        Result,
        max
    }
    [SerializeField]
    BgController m_bgCtr;
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    CameraShake m_camShake;
    [SerializeField]
    Result m_result;
    GameState m_state;
    public GameState State {  get { return m_state; } }
    public void SetState(GameState state, int duration = 0)
    {
        m_state = state;
        switch(m_state)
        {
            case GameState.Normal:
                m_player.EndInvincibleEffect();
                m_bgCtr.SetSpeed(1f);
                MonsterManager.Instance.ResetCreateMonster(1f);
                break;
            case GameState.Invincible:
                m_camShake.Play(duration, 0.1f);
                m_player.SetInvincibleEffect(duration);
                m_bgCtr.SetSpeed(5f);
                MonsterManager.Instance.ResetCreateMonster(8f);
                break;
            case GameState.Result:
                m_result.SetUI();
                GameUIManager.Instance.Hide();
                MonsterManager.Instance.CancelCreateMonster();
                m_player.SetDie();
                break;
        }
    }


    protected override void OnStart()
    {

    }
}
