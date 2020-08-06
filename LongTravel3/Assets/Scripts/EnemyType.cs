using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "MyOBJ/NewEnemy")]
public class EnemyType : ScriptableObject
{
    public TileCard m_TileCard;
    public CharaState m_CharaState;
    public CharaAttackData m_CharaAttackData;
    public CharaBackGroundInformation m_charaBackGroundInformation;

    //public void Copy(EnemyType enemy)
    //{
    //    m_CharaState = new CharaState(enemy.m_CharaState);
    //    m_charaBackGroundInformation = new CharaBackGroundInformation(enemy.m_charaBackGroundInformation);
    //    m_CharaAttackData = new CharaAttackData(enemy.m_CharaAttackData);
    //}


}
