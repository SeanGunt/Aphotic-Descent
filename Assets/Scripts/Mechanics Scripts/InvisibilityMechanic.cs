using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvisibilityMechanic : MonoBehaviour, IDataPersistence
{
    public float timeInvisible = 5.0f;
    public float invisibleTimer;
    public int invisibilityCharges = 3;
    public int maxCharges = 3;
    public Text interactionText;
    [SerializeField] public bool isInvisible;
    public bool isSafe;
    [SerializeField] private Image invisibilityBar, invisibilityCharge;
    [SerializeField] private Image fullInvisBar, fullInvisCharge;
    [SerializeField] private GameObject Player, PlayerMesh, invisibilityUI;
    [SerializeField] private TextMeshProUGUI invisibilityChargesText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip invisSound;
    public static event Action OnChargeUsed;
    [SerializeField] private ParticleSystem invisParticle;

    private void Start()
    {
      if (GameDataHolder.invisibilityAcquired)
      {
        invisibilityUI.SetActive(true);
        invisibilityChargesText.text = ": " + invisibilityCharges.ToString();
      }
      else
      {
        invisibilityUI.SetActive(false);
      }
    }
    void Awake()
    {
      isInvisible = false;
      isSafe = false;
      interactionText.text = "";
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
        if (Input.GetButtonDown("Invisibility") && GameDataHolder.invisibilityAcquired)
        {
          if (invisibilityCharges > 0 && !isSafe)
          {
            CancelInvoke("ClearUI");
            isInvisible = true;
            audioSource.PlayOneShot(invisSound);
            fullInvisCharge.enabled = true;
            fullInvisCharge.fillAmount = 1;
            //invisibilityBar.enabled = true;
            //fullInvisBar.enabled = true;
            invisibleTimer = timeInvisible;
            invisParticle.gameObject.SetActive(true);
            invisParticle.Play();
            isSafe = true;
            invisibilityCharges--;
            OnChargeUsed?.Invoke();
            invisibilityChargesText.text = ": " + invisibilityCharges.ToString();
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
          fullInvisCharge.fillAmount = invisibleTimer/timeInvisible;
          if (invisibleTimer <= 0)
          {
            invisParticle.gameObject.SetActive(false);
            invisParticle.Stop();
            isInvisible = false;
            fullInvisCharge.enabled = false;
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
        OnChargeUsed?.Invoke();
        invisibilityChargesText.text = ": " + invisibilityCharges.ToString();
        interactionText.text = $"You picked up a charge! Invis charges: {invisibilityCharges}";
        Debug.Log("InvisPicked Up");
        Invoke ("ClearUI", 3);
      }
    }

    public void SetInvisUIActive()
    {
      invisibilityUI.SetActive(true);
      invisibilityChargesText.text = ": " + invisibilityCharges.ToString();
    }

    void ClearUI()
    {
      interactionText.text = "";
      fullInvisBar.enabled = false;
      invisibilityBar.enabled = false;
    }
}
