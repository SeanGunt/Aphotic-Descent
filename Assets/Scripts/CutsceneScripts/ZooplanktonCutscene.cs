using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZooplanktonCutscene : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private GameObject player;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject zooplanktonCamera;
    [SerializeField] private GameObject hud;
    [SerializeField] private Image fadeToBlackImage;
    [SerializeField] private GameObject zooplanktonCutscene;
    private SkinnedMeshRenderer[] renderersToDisable;

    private void Awake()
    {
        renderersToDisable = GetComponentsInChildren<SkinnedMeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    public void EndCutscene()
    {
        StartCoroutine(FadeToMudMarsh(1f));
    }

    private IEnumerator FadeToMudMarsh(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.0f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        playerVisual.SetActive(true);
        zooplanktonCamera.SetActive(false);
        hud.SetActive(true);
        mainCamera.SetActive(true);
        foreach(SkinnedMeshRenderer rendererToDisable in renderersToDisable)
        {
            rendererToDisable.enabled = false;
        }
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        GameDataHolder.zooplanktonCutscenePlayed = true;
        playerMovement.enabled = true;
        zooplanktonCutscene.SetActive(false);
    }
}
