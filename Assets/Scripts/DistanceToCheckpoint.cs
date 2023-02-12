using UnityEngine;
using UnityEngine.UI; 
using TMPro;


public class DistanceToCheckpoint : MonoBehaviour {

    // Reference to checkpoint position
    [SerializeField]
    private Transform checkpoint;

    // Reference to UI text that shows the distance value
    [SerializeField]
    private TextMeshProUGUI distanceText;

    // Calculated distance value
    private float distance;

    // Update is called once per frame
    private void Update()
    {

        // Calculate distance value between character and checkpoint
        distance = (checkpoint.transform.position - transform.position).magnitude;
        distanceText.text = "Distance: " + distance.ToString("F1") + " meters";
    }

}
