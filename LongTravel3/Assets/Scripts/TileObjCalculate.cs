using UnityEditor.UI;
using UnityEngine;

public class TileObjCalculate : MonoBehaviour
{
    [Header("复制本")]
    public CharaPrefab m_CharaType;
    public EnemyType m_EnemyType;
    public ItemType m_Coin;



    public void CalculateCardOBJFromRatio(TileCard t)
    {
        int seed = Random.Range(0, 100);
        CalculateOBJType nextType = CalculateOBJType.Empty;
        if(seed > 50)
        {
            nextType = CalculateOBJType.Coin;
        }else if(seed >= 0)
        {
            nextType = CalculateOBJType.Zombie;
        }else if(seed >= 0)
        {
            nextType = CalculateOBJType.Chara;
        }
        
        switch (nextType)
        {
            case CalculateOBJType.Empty:
                break;
            case CalculateOBJType.Coin:
                Item coin = new Item(m_Coin);
                t.RegistCoin(coin);
                break;
            case CalculateOBJType.Zombie:
                Enemy e = new Enemy();
                e.Copy(m_EnemyType);
                t.RegistEnemy(e);
                break;
            case CalculateOBJType.Chara:
                break;
        }


        t.m_CurrentChara = null;


    }
}


public enum CalculateOBJType
{
    Empty,
    Coin,
    Chara,
    Zombie,
}
