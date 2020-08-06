using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TileCardMap : MonoBehaviour
{
    public TileCardAnchor[,] m_tileCardAnchorMap = new TileCardAnchor[4, 4];
    public TileCard[,] m_tileCardMap = new TileCard[4, 4];
    public GameObject m_TileCardPrefab;
    public TileObjCalculate m_Calculate;

    [Space]
    public ItemType m_coin;

    private void Awake()
    {
        m_Calculate = GetComponent<TileObjCalculate>();

        for(int x = 0; x<4; x++)
        {
            for (int y= 0; y< 4; y++)
            {
                TileCardAnchor tca= new TileCardAnchor(x, y);
                m_tileCardAnchorMap[x, y] = tca;
            }
        }

        foreach(TileCardAnchor tca in m_tileCardAnchorMap)
        {
            GameObject obj = Instantiate(m_TileCardPrefab, tca.position, transform.rotation);
            m_tileCardMap[tca.x, tca.y] = obj.GetComponent<TileCard>();
            StringBuilder sb = new StringBuilder(tca.x + "," + tca.y);
            obj.name = sb.ToString();
            obj.GetComponent<TileCard>().StartByTileCardManger(tca);
        }


        ///测定计算周围的卡片
        foreach (TileCard tc in m_tileCardMap)
        {
            TileCard up = null;
            TileCard down = null;
            TileCard left = null;
            TileCard right = null;

            TileCardAnchor upAnchor = new TileCardAnchor(tc.m_Anchor.x, tc.m_Anchor.y + 1);
            TileCardAnchor downAnchor= new TileCardAnchor(tc.m_Anchor.x, tc.m_Anchor.y - 1);
            TileCardAnchor leftAnchor= new TileCardAnchor(tc.m_Anchor.x - 1, tc.m_Anchor.y );
            TileCardAnchor rightAnchor = new TileCardAnchor(tc.m_Anchor.x+1, tc.m_Anchor.y );

            if (upAnchor.x >= 0&& upAnchor.x <4&& upAnchor.y >= 0 && upAnchor.y < 4)
            {
                up = m_tileCardMap[upAnchor.x, upAnchor.y];
            }

            if (downAnchor.x >= 0 && downAnchor.x < 4 && downAnchor.y >= 0 && downAnchor.y < 4)
            {
                down = m_tileCardMap[downAnchor.x, downAnchor.y];
            }

            if (leftAnchor.x >= 0 && leftAnchor.x < 4 && leftAnchor.y >= 0 && leftAnchor.y < 4)
            {
                left = m_tileCardMap[leftAnchor.x, leftAnchor.y];
            }

            if (rightAnchor.x >= 0 && rightAnchor.x < 4 && rightAnchor.y >= 0 && rightAnchor.y < 4)
            {
                right= m_tileCardMap[rightAnchor.x, rightAnchor.y];
            }

            NeighborCard nc = new NeighborCard(up,down, left,right);

            tc.StartByTileCardMangerSetNeighbor(nc);

        }
    }




    /// <summary>
    /// 随机把所有的chara放进来。chara的数据不变。
    /// </summary>
    /// <param name="chara"></param>
    public void RandomPutChara(List<Chara> chara)
    {
        //m_tileCardMap[1, 1].RegistChara(chara);
        //m_tileCardMap[1, 2].RegistEnemy(enemy);
        foreach(Chara c in chara)
        {
            while (true)
            {
                int x = UnityEngine.Random.Range(0, 3);
                int y = UnityEngine.Random.Range(0, 3);

                if (m_tileCardMap[x, y].RegistChara(c))
                {
                    break;
                }

            }
            
        }
    }
    /// <summary>
    /// 随机放进怪物。怪物的种类更具原来的种类新建。
    /// </summary>
    /// <param name="enemies"></param>
    public void RandomPutEnemy(List<Enemy> enemies)
    {
        foreach (Enemy e in enemies)
        {
        
            while (true)
            {
                int x = UnityEngine.Random.Range(0, 3);
                int y = UnityEngine.Random.Range(0, 3);

                if (m_tileCardMap[x, y].RegistEnemy(e))
                {
                    break;
                }

            }

        }
    }

    public void PutCoin()///设计还 不够完整。暂时就这样吧
    {
        foreach(TileCard t in m_tileCardMap)
        {
            
            Item i = new Item(m_coin);
            t.RegistCoin(i);
        }
    }


    public void ChangeCard(TileCard a,TileCard b)
    {
        //tilemap素组内的调整
        TileCard c = a;
        m_tileCardMap[a.m_Anchor.x, a.m_Anchor.y] = b;
        m_tileCardMap[b.m_Anchor.x, b.m_Anchor.y] = c;

        //tilecard内的anchor的调整。
        TileCardAnchor ca = a.m_Anchor;
        a.m_Anchor = b.m_Anchor;
        b.m_Anchor = ca;

        //tilecard内的currentlocation的调整。
        a.m_currentLocation = a.m_Anchor.position;
        b.m_currentLocation = b.m_Anchor.position;



        //回到具体的位置。
        a.StopMove();
        b.StopMove();

        AlltileGetSurroundCard();
    }

    public void AlltileGetSurroundCard()
    {
        ///测定计算周围的卡片
        foreach (TileCard tc in m_tileCardMap)
        {
            TileCard up = null;
            TileCard down = null;
            TileCard left = null;
            TileCard right = null;

            TileCardAnchor upAnchor = new TileCardAnchor(tc.m_Anchor.x, tc.m_Anchor.y + 1);
            TileCardAnchor downAnchor = new TileCardAnchor(tc.m_Anchor.x, tc.m_Anchor.y - 1);
            TileCardAnchor leftAnchor = new TileCardAnchor(tc.m_Anchor.x - 1, tc.m_Anchor.y);
            TileCardAnchor rightAnchor = new TileCardAnchor(tc.m_Anchor.x + 1, tc.m_Anchor.y);

            if (upAnchor.x >= 0 && upAnchor.x < 4 && upAnchor.y >= 0 && upAnchor.y < 4)
            {
                up = m_tileCardMap[upAnchor.x, upAnchor.y];
            }

            if (downAnchor.x >= 0 && downAnchor.x < 4 && downAnchor.y >= 0 && downAnchor.y < 4)
            {
                down = m_tileCardMap[downAnchor.x, downAnchor.y];
            }

            if (leftAnchor.x >= 0 && leftAnchor.x < 4 && leftAnchor.y >= 0 && leftAnchor.y < 4)
            {
                left = m_tileCardMap[leftAnchor.x, leftAnchor.y];
            }

            if (rightAnchor.x >= 0 && rightAnchor.x < 4 && rightAnchor.y >= 0 && rightAnchor.y < 4)
            {
                right = m_tileCardMap[rightAnchor.x, rightAnchor.y];
            }

            NeighborCard nc = new NeighborCard(up, down, left, right);

            tc.StartByTileCardMangerSetNeighbor(nc);

        }
    }
    
    /// <summary>
    /// 一个回合结束后。关闭所有没有打开的图片。并且设置为未操作过。
    /// </summary>
    public void AllTileCardCheck()
    {
        foreach(TileCard tc in m_tileCardMap)
        {
            if (tc.m_IsOpen)
            {
                tc.TileCardCheck();
            }
        }
    }

    /// <summary>
    /// 所有enemyCard攻击一次。
    /// </summary>
    public void AllEnemyAttack()
    {
        foreach(TileCard t in m_tileCardMap)
        {
            if (t.tag == "Enemy"&& t.m_IsOpen)
            {
                t.EnemyAttack();
            }
        }
    }

    public void SetFlashByChara(TileCard tc)///这里还可以改变，更具tc中具有的chara攻击类型，决定flash的那几个。
    {
        if (tc.m_neigbborCard.up )
            tc.m_neigbborCard.up.Flash();
        if (tc.m_neigbborCard.down )
            tc.m_neigbborCard.down.Flash();
        if (tc.m_neigbborCard.left )
            tc.m_neigbborCard.left.Flash();
        if (tc.m_neigbborCard.right)
            tc.m_neigbborCard.right.Flash();
    }

    /// <summary>
    /// 一个便利所有的卡片。并且注册卡片物体的工具。
    /// </summary>
    public void AllTildCardRegenerate()
    {
        foreach (TileCard tc in m_tileCardMap)
        {
            if (tc.tag == "Empty")
            {
                m_Calculate.CalculateCardOBJFromRatio(tc);
            }
        }
    }
}


[System.Serializable]
public class TileCardAnchor
{
    public int x;
    public int y;

    public Vector2 position;

    public TileCardAnchor(int t_x,int t_y)
    {
        x = t_x;
        y = t_y;

        position = new Vector2(t_x, t_y);
    }
}
