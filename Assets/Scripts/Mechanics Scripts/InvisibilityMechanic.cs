using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvisibilityMechanic : MonoBehaviour, IDataPersistence
{
    public float timeInvisible = 5.0f;
    public float invisibleTimer;
    public int invisibilityCharges = 3;
    public Text interactionText;
    [SerializeField]private bool isInvisible;
    public bool isSafe;
    [SerializeField] private Image invisibilityBar;
    [SerializeField] private Image fullInvisBar;
    [SerializeField] private GameObject Player, PlayerMesh;
    public Material[] mat;
    Renderer rend;
    [SerializeField] private TextMeshProUGUI invisibilityChargesText;

    private void Start()
    {
      invisibilityChargesText.text = "Invisibility Charges: " + invisibilityCharges.ToString();
    }
    void Awake()
    {
      isInvisible = false;
      isSafe = false;
      interactionText.text = "";
      rend = PlayerMesh.GetComponent<Renderer>();
      rend.enabled = true;
      rend.sharedMaterial = mat[0];
      invisibilityBar.enabled = false;
      fullInvisBar.enabled = false;
    }

    public void LoadData(GameData data)
    {
      invisibilityCharges = data.invisCharges;
    }

    public void SaveData(GameData data)
    {
      data.invisCharges = invisibilityCharges;
    }
    void Update()
    {
        invisibleTimer = Mathf.Clamp(invisibleTimer, 0f, timeInvisible);
        if (Input.GetButtonDown("Invisibility"))
        {
          if (invisibilityCharges > 0 && !isSafe)
          {
            CancelInvoke("ClearUI");
            isInvisible = true;
            rend.sharedMaterial = mat[1];
            invisibilityBar.enabled = true;
            fullInvisBar.enabled = true;
            invisibleTimer = timeInvisible;
            isSafe = true;
            invisibilityCharges--;
            invisibilityChargesText.text = "Invisibility Charges: " + invisibilityCharges.ToString();
            interactionText.text = "You are invisible!";
          }
          else if (invisibilityCharges <= 0)
          {
            interactionText.text = "You have no charges!";
            Invoke ("ClearUI", 3);
          }
        }
        if (isInvisible)
        {
          invisibleTimer -= Time.deltaTime;
          fullInvisBar.fillAmount = invisibleTimer/timeInvisible;
          if (invisibleTimer <= 0)
          {
            isInvisible = false;
            rend.sharedMaterial = mat[0];
            fullInvisBar.enabled = false;
            isSafe = false;
            interactionText.text = $"You are no longer invisible! Charges: {invisibilityCharges}";
            Invoke ("ClearUI", 4);
          }
        }
        if (Player.GetComponent<PlayerHealthController>().isBleeding)
        {
          isSafe = false;
        }
    }

    void OnTriggerStay(Collider col)
    {
      if (col.gameObject.tag == ("InvisPickup"))
      {
        CancelInvoke("ClearUI");
        invisibilityCharges++;
        invisibilityChargesText.text = "Invisibility Charges: " + invisibilityCharges.ToString();
        interactionText.text = $"You picked up a charge! Invis charges: {invisibilityCharges}";
        Debug.Log("InvisPicked Up");
        Invoke ("ClearUI", 3);
      }
    }

    void ClearUI()
    {
      interactionText.text = "";
      fullInvisBar.enabled = false;
      invisibilityBar.enabled = false;
    }
}
