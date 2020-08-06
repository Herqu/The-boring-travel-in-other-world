using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InputManager m_InputManager;
    public TileCardMap m_TileCardMap;
    public CharaManager m_CharaManager;
    public int MaxZombieNum = 3;
    [Space]
    public int m_CurrentCoinNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        m_InputManager = GetComponent<InputManager>();
        m_InputManager.StartByGameManager(this);
        m_TileCardMap = GetComponent<TileCardMap>();
        m_CharaManager = GetComponent<CharaManager>();



        m_TileCardMap.RandomPutChara(m_CharaManager.m_AllCharas);
        m_TileCardMap.RandomPutEnemy(m_CharaManager.m_AllEnemy);
        m_TileCardMap.PutCoin();
    }


    private void Update()
    {
        ///这里可以添加一个数，确定当总的enemy的数量少于多少的时候就添加一个enemy进入网格。
        if(m_CharaManager.m_AllEnemy.Count < MaxZombieNum)
        {
            ;
        }
    }




    public void AddCoin(int num)
    {
        Debug.Log("Get coin: "+ num.ToString());
        m_CurrentCoinNum += num;
    }
}
