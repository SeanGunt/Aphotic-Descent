using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int invisCharges;
    public bool knifeHasBeenPickedUp;

    public GameData()
    {
        playerPosition = new Vector3(-60f,-20.5f,-90f);
        playerRotation = new Quaternion(0,0,0,0);
        invisCharges = 3;
        knifeHasBeenPickedUp = false;
    }
}
