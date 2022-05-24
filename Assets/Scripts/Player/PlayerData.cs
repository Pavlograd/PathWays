using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/PlayerData")]
public class PlayerData : ScriptableObject
{
    public InventoryObject[] objects;
    public float startSize;
    public float speed = 0.1f;
    public float timBtnNwElmts;
    public float timBtnNwPssnt;
    public float timerBuilding; // Max time before a passant arrives or game over
}
