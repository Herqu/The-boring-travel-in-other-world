using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

[System.Serializable]
public class Enemy
{
    public TileCard m_TileCard;
    public CharaState m_CharaState;
    public CharaAttackData m_CharaAttackData;
    public CharaBackGroundInformation m_charaBackGroundInformation;
    public CharaManager m_CharaManager;

    public void GetHit(int num)
    {
        m_CharaState.currentHealth -= num;
        if (m_CharaState.currentHealth <= 0)
        {
            Death();
        }
    }
    public void AttackChara(Chara c)
    {
        c.GetHit(m_CharaAttackData.ATK);

    }

    private void Death()
    {
        m_TileCard.Cancel();
        m_TileCard = null;
        m_CharaManager.m_AllEnemy.Remove(this);

    }

    public void Copy(EnemyType enemy)
    {
        m_CharaState = new CharaState(enemy.m_CharaState);
        m_charaBackGroundInformation = new CharaBackGroundInformation(enemy.m_charaBackGroundInformation);
        m_CharaAttackData = new CharaAttackData(enemy.m_CharaAttackData);
    }

}
