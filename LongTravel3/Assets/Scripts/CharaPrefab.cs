using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chara", menuName = "MyOBJ/NewChara")]
public class CharaPrefab : ScriptableObject
{
    public TileCard m_TileCard;
    public CharaState m_CharaState;
    public CharaBackGroundInformation m_charaBackGroundInformation;
    public CharaAttackData m_CharaAttackData;

}
