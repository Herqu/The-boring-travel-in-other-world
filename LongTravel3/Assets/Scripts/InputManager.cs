using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class InputManager : MonoBehaviour
{
    public GameManager m_GameManager;
    [Space]
    public Vector2 m_CurrentMousePosition;//鼠标永远的当前世界位置。
    public Vector2 m_LastMousePosition;//鼠标最后一次按下的
    public Vector2 m_Distance;//两个距离的世界距离

    [Space]
    public Camera m_camera;
    public TileCardMap m_TileCardMap;
    public CharaManager m_CharaManager;
    public TileCard m_CurrentHoldCard;
    public TileCard m_CurrentChangeCard;

    [Space]
    public CardChessTurnState m_TurnState = CardChessTurnState.CharaMove;
    public TileCard m_CurrentAttackCharaTile;

    private void Start()
    {
        m_camera = Camera.main;
        m_CharaManager = GetComponent<CharaManager>();
        m_TileCardMap = GetComponent<TileCardMap>();
    }

    public void StartByGameManager(GameManager gm)
    {
        m_GameManager = gm;
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_TurnState)
        {
            case CardChessTurnState.CharaMove:
                CharaMoveInUpdate();
                break;
            case CardChessTurnState.CharaAttack:
                CharaAttackInUpdate();
                ///计算当前还剩余的没有活动的Chara的量。然后改变。
                break;
            case CardChessTurnState.EnemyAttack:
                EnemyAttackInUpdate();
                ///这里还要查一条，就是重新布置僵尸。
                ///关闭所有卡片中没有empty的卡片。
                m_CharaManager.AllCharaEmotionCost();
                m_TileCardMap.AllTileCardCheck();////这里要在tilemap里面根据很多东西重新布局。确定范围。
                m_TileCardMap.AllTildCardRegenerate();
                break;
        }
    }


    public void CharaMoveInUpdate()
    {

        m_CurrentMousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);

        //整体初始位置 
        RaycastHit2D hit = Physics2D.Raycast(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            m_LastMousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);
            if (hit && hit.transform.tag == "Chara" &&hit.transform.GetComponent<TileCard>().m_CurrentChara.m_IsMoveFinished == false)
            {
                m_CurrentHoldCard = hit.collider.gameObject.GetComponent<TileCard>();
                m_CurrentHoldCard.StartMove();
            }
            else
            {
                m_CurrentHoldCard = null;
            }
        }


        if (m_CurrentHoldCard)
        {
            RaycastHit2D[] hit2 = Physics2D.RaycastAll(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (RaycastHit2D r in hit2)
            {
                if (r.transform != m_CurrentHoldCard.transform)
                {
                    if (m_CurrentHoldCard.m_neigbborCard.FindInNeibor(r.transform))
                    {
                        m_CurrentChangeCard = r.transform.GetComponent<TileCard>();//再加一个检测，要保证这个新的 卡牌在原来currenthold卡牌的周围。
                        m_CurrentChangeCard.HighLight();
                    }
                    else
                    {
                        m_CurrentChangeCard = null;
                    }
                }

            }

            m_Distance = m_CurrentMousePosition - m_LastMousePosition;

            if (Input.GetMouseButton(0))
            {

                m_CurrentHoldCard.Move(m_Distance);

            }

            ///当鼠标抬起的时候，调整角色返回或者更换卡牌。
            if (Input.GetMouseButtonUp(0))
            {
                if (m_CurrentChangeCard)
                {
                    m_TileCardMap.ChangeCard(m_CurrentHoldCard, m_CurrentChangeCard);
                    m_TurnState = CardChessTurnState.CharaAttack;
                    m_CurrentAttackCharaTile = m_CurrentHoldCard;
                    ///这里的设计，让角色不影响内容…就是说，确定当前卡片已经活跃过了。这点放在卡片里面。不要放在chara类里面。因为上面还要检测。
                    m_CurrentHoldCard.m_CurrentChara.m_IsMoveFinished = true;
                    m_CurrentHoldCard = null;
                }
                else
                {
                    m_CurrentHoldCard.StopMove();
                    m_CurrentAttackCharaTile = null;
                    m_CurrentHoldCard = null;
                }
                m_CurrentChangeCard = null;
                m_CurrentHoldCard = null;
            }
            m_CurrentChangeCard = null;///这里的设计稍微有问题。但是影响不大。

        }
    }

    public void CharaAttackInUpdate()
    {

        ///首先是屏幕显示攻击范围。设定周围的卡片都是闪光效果
        m_TileCardMap.SetFlashByChara(m_CurrentAttackCharaTile);
        //tcs.Add(NeighborCard)
        ///然后射线确定点击内容。
        ///如果是攻击范围。则发动技能。并且浸入下一个阶段。所有的敌人的一遍流程。
        ///如果不是，那么就灭有回应
        /// 
        /// 
        /// 
        ///
                //整体初始位置 
        RaycastHit2D hit = Physics2D.Raycast(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetMouseButtonDown(0))
        {
            if(hit && hit.transform.tag == "Enemy" && hit.transform.GetComponent<TileCard>().isFlash && hit.transform.GetComponent<TileCard>().m_IsOpen)
            {
                Debug.Log("Attack function");
                ///确认是不是能够攻击对象。
                m_CurrentAttackCharaTile.Attack(hit.transform.GetComponent<TileCard>());
                ///对对象使用应该使用的m_cuurentattackcharatile的 攻击部分。
                m_CurrentAttackCharaTile = null;
                ///跳到下一阶段
                CharaAfterAttackAnyUnMoveCard();
            }
            else if (hit && hit.transform.tag == "Item" && hit.transform.GetComponent<TileCard>().isFlash && hit.transform.GetComponent<TileCard>().m_IsOpen)///如果碰到一张卡片，但是这张卡片没有打开。那么就跳过。
            {
                ///当前卡片交互
                m_CurrentAttackCharaTile.Interact(hit.transform.GetComponent<TileCard>());
                

                m_CurrentAttackCharaTile = null;
                CharaAfterAttackAnyUnMoveCard();

            }else if (hit && hit.transform.GetComponent<TileCard>().isFlash)
            {

                m_CurrentAttackCharaTile = null;
                CharaAfterAttackAnyUnMoveCard();
            }

        }
    }
    public void CharaAfterAttackAnyUnMoveCard()
    {
        if (m_CharaManager.IsAnyCharaUnMove())
        {
            m_TurnState = CardChessTurnState.CharaMove;
        }
        else
        {
            m_TurnState = CardChessTurnState.EnemyAttack;

        }
    }



    public void EnemyAttackInUpdate()
    {
        m_TileCardMap.AllEnemyAttack();
        m_TurnState = CardChessTurnState.CharaMove;
        m_CurrentHoldCard = null;
        EnemyAttackAfterCheck();
    }
    public void EnemyAttackAfterCheck()
    {
        if(m_CharaManager.m_AllCharas.Count == 0)
        {
            Debug.Log("YOU LOSE");
        }
    }





    public void CallByButton()
    {
        m_TurnState = CardChessTurnState.CharaMove;
        m_TileCardMap.AllTileCardCheck();
    }


}




public enum MoveDirection
{
    u,
    d,
    l,
    r
}


public enum CardChessTurnState
{
    CharaMove,
    CharaAttack,
    EnemyAttack,
    CardCheck
}