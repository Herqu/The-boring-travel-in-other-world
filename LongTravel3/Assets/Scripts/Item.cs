
using UnityEngine;

[System.Serializable]
public class Item
{
    public TileCard m_TileCard;
    public string m_name;
    public int m_num;
    public Sprite m_sprite;


    private void Death()
    {
        m_TileCard.Cancel();
        m_TileCard = null;
        //m_CharaManager.m_AllEnemy.Remove(this);

    }

    public void Copy(EnemyType enemy)
    {
        //m_CharaState = new CharaState(enemy.m_CharaState);
        //m_charaBackGroundInformation = new CharaBackGroundInformation(enemy.m_charaBackGroundInformation);
        //m_CharaAttackData = new CharaAttackData(enemy.m_CharaAttackData);
    }


    public Item(ItemType i)
    {
        m_name = i.name;
        m_sprite = i.sprite;
        m_num = i.itemNum;
    }



    /// <summary>
    /// 物体道具被拾捡。
    /// </summary>
    public void BeGet()
    {
        Death();
    }
}
