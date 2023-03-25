using UnityEngine;

public class SonarPingManager : MonoBehaviour
{
    [SerializeField]private GameObject ping;
    [SerializeField]private SonarPings managedPing;
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

    public void ColorManager(int colorCase)
    {
        if (colorCase == 1)
        {
            managedPing.SetColor(new Color(0, 0, 1, 1));
        }
        if (colorCase == 2)
        {
            managedPing.SetColor(new Color(1, 0, 0, 1));
        }
    }

    private void ResetBools()
    {
        instantiated = false;
    }
}
