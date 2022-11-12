using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 flashlightPosition;
    public int invisCharges;
    public bool knifeHasBeenPickedUp;

    public GameData()
    {
        playerPosition = new Vector3(-60f,-20.5f,-90f);
        playerRotation = new Quaternion(0,0,0,0);
        flashlightPosition = new Vector3(-59.75f, -20.14f, -89.62f);
        invisCharges = 3;
        knifeHasBeenPickedUp = false;
    }
}
