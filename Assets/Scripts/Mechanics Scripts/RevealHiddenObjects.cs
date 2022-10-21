using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealHiddenObjects : MonoBehaviour
{
    public GameObject[] objectsToBeRevealed;
    public bool objRevealed;
    [SerializeField] private float revealedTime, maxRevealed;

    void Awake()
    {
        objRevealed = false;
    }

    void Update()
    {
        revealedTime = Mathf.Clamp(revealedTime, 0f, maxRevealed);
        if (objRevealed)
        {
            foreach (GameObject objectToBeRevealed in objectsToBeRevealed)
            objectToBeRevealed.gameObject.SetActive(true);
            RevelationTimer();
        }
        if (!objRevealed)
        {
            foreach (GameObject objectToBeRevealed in objectsToBeRevealed)
            objectToBeRevealed.gameObject.SetActive(false);
        }
    }

    void RevelationTimer()
    {
        revealedTime -= Time.deltaTime;
        if (revealedTime <= 0)
        {
            objRevealed = false;
            revealedTime = maxRevealed;
        }
    }
}
