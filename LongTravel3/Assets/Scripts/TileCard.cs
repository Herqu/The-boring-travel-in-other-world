using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class TileCard : MonoBehaviour
{
    public Sprite m_BackGroundSprites;
    public Sprite m_FrontGroundSprites;
    public SpriteRenderer m_FrontSpriteRenderer;
    public SpriteRenderer m_BackSpriteRenderer;
    public SpriteRenderer m_FlashSpriteRenderer;
    public SpriteRenderer m_OBJSpriteRenderer;
    [Space]
    public Chara m_CurrentChara = null;
    public Enemy m_CurrentEnemy = null;
    public Item m_CurrentItem = null;
    [Space]
    public Vector2 m_currentLocation;
    public NeighborCard m_neigbborCard;
    public TileCardAnchor m_Anchor;
    public bool m_IsOpen = false;
    public float m_moveSpeed = 0.5f;
    public Animator m_Animator;


    private void Awake()
    {
        m_FrontSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        m_BackSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        m_OBJSpriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();         
        m_FlashSpriteRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateFlash();
    }


    public void StartByTileCardManger(TileCardAnchor tca)
    {
        m_BackSpriteRenderer.sprite = m_BackGroundSprites;
        m_FrontSpriteRenderer.sprite = m_FrontGroundSprites;
        m_currentLocation = (Vector2)transform.position;
        m_Anchor = tca;
    }

    public void StartByTileCardMangerSetNeighbor(NeighborCard n)
    {
        m_neigbborCard = new NeighborCard(n);
    }

    public bool RegistChara(Chara c)
    {
        if(tag == "Empty")
        {
            c.m_TileCard = this;
            m_CurrentChara = c;
            m_OBJSpriteRenderer.sprite = c.m_charaBackGroundInformation.body;
            this.tag = "Chara";
            OpenCard();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RegistEnemy(Enemy c)
    {
        if (tag == "Empty")
        {
            c.m_TileCard = this;
            m_CurrentEnemy = c;
            m_OBJSpriteRenderer.sprite = c.m_charaBackGroundInformation.body;
            this.tag = "Enemy";
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RegistCoin(Item c)
    {
        if (tag == "Empty")
        {
            c.m_TileCard = this;
            m_CurrentItem= c;
            m_OBJSpriteRenderer.sprite = c.m_sprite;
            this.tag = "Item";
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Cancel()
    {
        m_CurrentEnemy = null;
        m_CurrentChara = null;
        m_OBJSpriteRenderer.sprite = null;
        this.tag = "Empty";
    }//注销掉当前的东西


    public void StartMove()
    {
        //卡片浮起来。并且给周围的卡片一个准备移动的信号。
    }
    public void StopMove()
    {
        transform.position = m_currentLocation;
        //下面是更加详细的文件，通过
        if (!m_IsOpen)
        {
            OpenCard();
        }
    }

    public void OpenCard()
    {
        m_IsOpen = true;
        m_Animator.SetBool("IsOpen", true);
    }

    public void CloseCard()
    {
        m_IsOpen = false;
        m_Animator.SetBool("IsOpen", false);
    }

    public void Move(Vector2 dis)
    {
        transform.position = dis + m_currentLocation;
    }

    public void HighLight()//当鼠标按住卡片的时候，卡片浮动或者发光。
    {
        ;//高光
    }


    public void TileCardCheck()
    {
        if(tag =="Empty") {
            CloseCard();
        }
        if (m_CurrentChara != null)
        {
            m_CurrentChara.m_IsMoveFinished = false;
        }
    }


    public bool isFlash = false;
    public void Flash()
    {
        isFlash = true;

    }

    public void UpdateFlash()
    {
        if (isFlash)
        {
            m_FlashSpriteRenderer.enabled = true;
        }else
        {
            m_FlashSpriteRenderer.enabled = false;
        }
        isFlash = false;

    }




    /// <summary>
    /// 攻击和交互相关
    /// </summary>
    /// <param name="tileCard"></param>
    public void Attack(TileCard tileCard)
    {
        ///对tilecadr用本tilcard的角色的血量的攻击方式。
        ///应该有概率，但是我不想用了。
        m_CurrentChara.AttackEnemy(tileCard.m_CurrentEnemy);
    }

    public void Interact(TileCard tileCard)
    {
        ///对tilecadr用本tilcard的角色的血量的攻击方式。
        ///应该有概率，但是我不想用了。
        m_CurrentChara.GetItem(tileCard.m_CurrentItem);
    }



    public void EnemyAttack()
    {
        foreach(TileCard t in m_neigbborCard.allTileCard)
        {
            if (t.m_CurrentChara != null && t.m_IsOpen && t.tag =="Chara")
            {
                m_CurrentEnemy.AttackChara(t.m_CurrentChara);
                Debug.Log("僵尸攻击了");
                return;
            }
        }
        Debug.Log("没有攻击");
        return;
    }






}

















[System.Serializable]
public class NeighborCard
{
    public TileCard up;
    public TileCard down;
    public TileCard left;
    public TileCard right;

    public List<TileCard> allTileCard  = new List<TileCard>();

    public NeighborCard(NeighborCard n)
    {
        up = n.up;
        down= n.down;
        left= n.left;
        right= n.right;

        if (up)
        {
            allTileCard.Add(up);
        }
        if (down)
        {
            allTileCard.Add(down);
        }
        if (left)
        {
            allTileCard.Add(left);
        }
        if (right)
        {
            allTileCard.Add(right);
        }

    }

    public NeighborCard(TileCard u, TileCard d, TileCard l, TileCard r)
    {

        if (u)
        {
            up = u;
            allTileCard.Add(up);
        }
        if (d)
        {
            down = d;
            allTileCard.Add(down);
        }
        if (l)
        {
            left = l;
            allTileCard.Add(left);
        }
        if (r)
        {
            right = r;
            allTileCard.Add(right);
        }



    }

    public bool FindInNeibor(Transform t)
    {
        if (up && up.transform == t)
            return true;
        if (down && down.transform == t)
            return true;
        if (left && left.transform == t)
            return true;
        if (right && right.transform == t)
            return true;

        return false;
    }
}