using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaManager : MonoBehaviour
{
    public GameManager m_GameManager;


    [Space]
    public List<Chara> m_AllCharas = new List<Chara>();
    public List<Enemy> m_AllEnemy = new List<Enemy>();

    public EnemyType m_NormalZombie;
    public CharaPrefab m_CharaPrefab;

    private void Awake()
    {
        foreach(Enemy e in m_AllEnemy)
        {
            e.Copy(m_NormalZombie);
            e.m_CharaManager = this;
        }

        foreach (Chara c in m_AllCharas)
        {
            c.Copy(m_CharaPrefab);
            c.m_CharaManager = this;
        }

        m_GameManager = GetComponent<GameManager>();
    }

    public bool IsAnyCharaUnMove()
    {
        bool b = false;
        foreach(Chara c in m_AllCharas)
        {
            if(c.m_IsMoveFinished  == false)
            {
                b = true;
            }
        }

        return b;
    }



    public void AllCharaEmotionCost()
    {
        foreach(Chara c in m_AllCharas)
        {
            c.DecreseEmotion();
        }
    }

}
