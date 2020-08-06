using UnityEngine;
using UnityEngine.UI;

public class CardInfoUI : MonoBehaviour
{
    public Slider m_slider;
    public Image m_faceEmotion;

    public TileCard m_currentTilecard;
    public Sprite[] m_facesSprite;


    // Update is called once per frame
    void Update()
    {
        if(m_currentTilecard && m_currentTilecard.tag =="Chara")
        {
            m_slider.value = m_currentTilecard.m_CurrentChara.m_CharaState.currentHealth/ m_currentTilecard.m_CurrentChara.m_CharaState.maxHealth;
            //m_faceEmotion.sprite = 
        }
    }
}

public enum CardInfoUIState
{
    Chara,
    Enemy,
    Item
}


