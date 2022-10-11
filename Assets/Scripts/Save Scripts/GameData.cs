using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public bool noCharges;
    public GameData()
    {
        playerPosition = new Vector3(0,1,0);
        playerRotation = new Quaternion(0,0,0,0);
        noCharges = true;
    }
}
