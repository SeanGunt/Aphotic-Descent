using UnityEngine;

public class SonarPingManager : MonoBehaviour
{
    [SerializeField]private GameObject ping;
    [SerializeField]private SpherePings managedPing;
    private bool doneOnce;
    private bool instantiated;

    public void InstantiatePings()
    {
        if (!instantiated)
        {
            ping.gameObject.SetActive(true);
            instantiated = true;
        }
    }

    public void SetPingTimer(float time)
    {
        managedPing.SetDisappearTimer(time);
        Invoke("ResetBools", time);
    }

    private void ResetBools()
    {
        instantiated = false;
    }
}
