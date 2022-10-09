using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisibilityMechanic : MonoBehaviour
{
    public float timeInvisible = 5.0f;
    public float invisibleTimer;
    public float invisibilityCharges = 3f;
    public Text invisText;
    [SerializeField]
    private bool isInvisible;
    public bool isSafe;
    [SerializeField]
    Image invisibilityBar;
    public Material[] mat;
    Renderer rend;

    void Awake()
    {
      isInvisible = false;
      isSafe = false;
      invisText.text = "";
      rend = GetComponent<Renderer>();
      rend.enabled = true;
      rend.sharedMaterial = mat[0];
      invisibilityBar.enabled = false;
    }
    void Update()
    {
        invisibleTimer = Mathf.Clamp(invisibleTimer, 0f, timeInvisible);
        if (Input.GetKeyDown(KeyCode.E))
        {
          if (invisibilityCharges > 0 && !isSafe)
          {
            isInvisible = true;
            rend.sharedMaterial = mat[1];
            invisibilityBar.enabled = true;
            invisibleTimer = timeInvisible;
            isSafe = true;
            invisibilityCharges--;
            invisText.text = "You are invisible!";
          }
        }
        if (isInvisible)
        {
          invisibleTimer -= Time.deltaTime;
          invisibilityBar.fillAmount = invisibleTimer/timeInvisible;
          if (invisibleTimer <= 0)
          {
            isInvisible = false;
            rend.sharedMaterial = mat[0];
            invisibilityBar.enabled = false;
            isSafe = false;
            invisText.text = "You are no longer invisible!";
          }
        }

    }
}
