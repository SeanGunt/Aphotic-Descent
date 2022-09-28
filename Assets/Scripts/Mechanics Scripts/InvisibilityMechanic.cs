using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisibilityMechanic : MonoBehaviour
{
    public float timeInvisible = 5.0f;
    public float invisibleTimer;
    public float invisibilityCharges = 3.0f;

    public Text invisText;

    [SerializeField]
    private bool isInvisible;
    public bool isSafe;

    public Material[] mat;
    Renderer rend;

    // Start is called before the first frame update
    void Awake()
    {
      isInvisible = false;
      isSafe = false;
      invisText.text = "";
      rend = GetComponent<Renderer>();
      rend.enabled = true;
      rend.sharedMaterial = mat[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
          if (invisibilityCharges > 0 && !isSafe)
          {
            isInvisible = true;
            rend.sharedMaterial = mat[1];
            invisibleTimer = timeInvisible;
            isSafe = true;
            invisibilityCharges--;
            invisText.text = "You are invisible!";
          }
        }
        if (isInvisible)
        {
          invisibleTimer -= Time.deltaTime;
          if (invisibleTimer <= 0)
          {
            isInvisible = false;
            rend.sharedMaterial = mat[0];
            isSafe = false;
            invisText.text = "You are no longer invisible!";
          }
        }
    }
}
