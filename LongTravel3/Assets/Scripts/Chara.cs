using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[System.Serializable]
public class Chara 
{
    public bool m_IsMoveFinished = false;
    public TileCard m_TileCard;
    public CharaState m_CharaState;
    public CharaBackGroundInformation m_charaBackGroundInformation;
    public CharaAttackData m_CharaAttackData;
    public CharaManager m_CharaManager;
    public CharaEmotionState m_EmotionState;


    public void AttackEnemy(Enemy e)
    {
        e.GetHit(m_CharaAttackData.ATK);
    }

    public void GetHit(int num)
    {
        m_CharaState.currentHealth -= num;
        if (m_CharaState.currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        m_TileCard.Cancel();
        m_TileCard = null;
        m_CharaManager.m_AllCharas.Remove(this);
    }


    /// <summary>
    /// 新建函数
    /// </summary>
    /// <param name="c"></param>
    public void Copy(CharaPrefab c)
    {
        m_CharaState = new CharaState(c.m_CharaState);
        m_charaBackGroundInformation = new CharaBackGroundInformation(c.m_charaBackGroundInformation);
        m_CharaAttackData = new CharaAttackData(c.m_CharaAttackData);
        m_CharaState.CurrentEmotion = m_CharaState.MaxEmotion;
    }


    /// <summary>
    /// 获得捡到东西。
    /// </summary>
    public void GetItem(Item i)
    {
        i.BeGet();
        m_CharaManager.m_GameManager.AddCoin(i.m_num);
        IncreaseEmotion(i.m_num);
        ///情绪处理
    }



    public void IncreaseEmotion(int num)
    {
        m_CharaState.CurrentEmotion += num;
        if(m_CharaState.CurrentEmotion > m_CharaState.MaxEmotion)
        {
            m_CharaState.CurrentEmotion = m_CharaState.MaxEmotion;
        }
        ChangeEmotionSprite();
    }

    public void DecreseEmotion()
    {
        m_CharaState.CurrentEmotion -= 1;
        if(m_CharaState.CurrentEmotion <= 0)
        {
            Death();
        }

        ChangeEmotionSprite();
    }



    public void ChangeEmotionSprite()
    {
        if (m_CharaState.CurrentEmotion >= m_CharaState.MaxEmotion / 2 + 3)
        {
            m_EmotionState = CharaEmotionState.Happy;
        }
        else if (m_CharaState.CurrentEmotion>= m_CharaState.MaxEmotion / 2 - 2)
        {
            m_EmotionState = CharaEmotionState.Normal;
        }
        else
        {
            m_EmotionState = CharaEmotionState.Sad;
        }
    }
}


[System.Serializable]
public class CharaAttackData
{
    public int ATK = 5;
    public int attackRange =1;

    public CharaAttackData(CharaAttackData c)
    {
        ATK = c.ATK;
        attackRange = c.attackRange;
    }
}



[System.Serializable]
public class CharaState
{
    public int maxHealth = 10;
    public int currentHealth = 10;

    public int MaxEmotion = 10;
    public int CurrentEmotion = 10;


    public CharaState(CharaState cs)
    {
        maxHealth = cs.maxHealth;
        currentHealth = cs.currentHealth;

        MaxEmotion = cs.maxHealth;
        CurrentEmotion = cs.CurrentEmotion;
    }

}

[System.Serializable]
public class CharaBackGroundInformation
{
    public string name;
    public Sprite body;
    [TextArea(3,4)]
    public string description;
    [TextArea(3, 4)]
    public string helloWord;

    public CharaBackGroundInformation(CharaBackGroundInformation cbi)
    {
        name = cbi.name;
        description = cbi.description;
        helloWord = cbi.helloWord;
        body = cbi.body;
    }
}


public enum CharaEmotionState
{
    Happy,
    Normal,
    Sad
}