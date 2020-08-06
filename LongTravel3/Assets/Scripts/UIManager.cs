using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputManager m_InputManager;
    public GameManager m_GameManager;
    public Text m_CharaName;
    public Text m_Description;
    public string noOBJtext = "???";

    [Header("点击的卡片")]
    public Camera m_camera;
    public TileCard m_UICard;


    [Header("硬币和UI")]
    public Text m_text;
    public StringBuilder sb = new StringBuilder();

    [Header("小标签UI")]
    public GameObject m_CardInfo;
    public Transform m_InfoUITransform;
    public Vector2 m_CardInfoOffset;

    private List<TileCard> m_AllInfoCards = new List<TileCard>();
    private List<CardInfoUI> m_AllCardInfos = new List<CardInfoUI>();


    // Start is called before the first frame update
    void Start()
    {
        m_InputManager = GetComponent<InputManager>();
        m_GameManager = GetComponent<GameManager>();
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ///卡牌面板更新
        UIholdCheck();
        UIShowCurrentCard();
        
        
        ///硬币更新。
        CoinUpdate();


        ///卡牌单个UI更新。
        ShowCurrentCardCheck();
        ShowCurrentAllTileCardInfo();
    }


    public void UIholdCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            m_UICard = hit.transform.GetComponent<TileCard>();
        }
    }

    public void UIShowCurrentCard()
    {

        if (m_UICard)
        {
            if (m_UICard.tag == "Chara")
            {
                m_CharaName.text = m_UICard.m_CurrentChara.m_charaBackGroundInformation.name;
                m_Description.text = m_UICard.m_CurrentChara.m_charaBackGroundInformation.description;

            //}
            //else if (m_UICard.tag == "Enemy")
            //{
            //    m_CharaName.text = m_UICard.m_CurrentEnemy.m_charaBackGroundInformation.name;
            //    m_Description.text = m_UICard.m_CurrentEnemy.m_charaBackGroundInformation.description;

            //}
            //else if (m_UICard.tag == "Item")
            //{
            //    m_CharaName.text = noOBJtext;
            //    m_Description.text = noOBJtext;
            }else if (!m_UICard.m_IsOpen)
            {
                m_CharaName.text = noOBJtext;
                m_Description.text = noOBJtext;

            }

        }

    }

    public void CoinUpdate()
    {
        //StringBuilder sb = "Coin:" + m_GameManager.m_CurrentCoinNum.ToString();
        sb.Clear();
        sb.AppendFormat("Coin{0}", m_GameManager.m_CurrentCoinNum);
        m_text.text = sb.ToString(); 
    }



    /// <summary>
    /// 关于角色小标签部分
    /// </summary>
    public void ShowCurrentCardCheck()
    {

        m_AllInfoCards.Clear();
        RaycastHit2D hit = Physics2D.Raycast(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            TileCard tc = hit.transform.GetComponent<TileCard>();
            if (!m_AllInfoCards.Exists(t => t.transform == tc.transform))
            {
                m_AllInfoCards.Add(tc);

            }
        }

    }

    /// <summary>
    /// 单个的卡片捕捉
    /// </summary>
    public void ShowCurrentAllTileCardInfo()
    {
        foreach(CardInfoUI g in m_AllCardInfos)
        {
            Destroy(g.gameObject);
        }
        m_AllCardInfos.Clear();

        foreach(TileCard t in m_AllInfoCards)
        {
            Vector2 position = (Vector2)m_camera.WorldToScreenPoint(t.transform.position)+ m_CardInfoOffset;

            CardInfoUI cardInfo = Instantiate(m_CardInfo,position, Quaternion.identity, m_InfoUITransform).GetComponent<CardInfoUI>();
            cardInfo.m_currentTilecard = t;
            ///这里还要调整UI的数据。倒也没什么特别要考虑的地方。
            m_AllCardInfos.Add(cardInfo);
        }



    }
}
