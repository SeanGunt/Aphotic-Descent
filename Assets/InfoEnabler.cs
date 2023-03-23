using UnityEngine;

public class InfoEnabler : MonoBehaviour
{
    private LoreSaver loreSaver;
    private GameObject myself;

    void Awake()
    {
        loreSaver = this.GetComponent<LoreSaver>();
        myself = this.gameObject;
    }
    void OnTriggerEnter()
    {
        loreSaver.CollectLore();
        myself.GetComponent<Collider>().enabled = false;
    }
}
