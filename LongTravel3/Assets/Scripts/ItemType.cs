
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "MyOBJ/NewItem")]
public class ItemType : ScriptableObject
{
    public TileCard m_TileCard;
    public string name;
    public Sprite sprite;
    public int itemNum;

    //public void Copy(EnemyType enemy)
    //{
    //    m_CharaState = new CharaState(enemy.m_CharaState);
    //    m_charaBackGroundInformation = new CharaBackGroundInformation(enemy.m_charaBackGroundInformation);
    //    m_CharaAttackData = new CharaAttackData(enemy.m_CharaAttackData);
    //}

}
