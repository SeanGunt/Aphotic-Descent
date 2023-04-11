using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField]private GameObject cameraHintCutscene;
    [SerializeField]private Camera hintCam;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField]private GameObject mainCamera;
    [SerializeField]private Image fadeToBlackImage;
    [SerializeField]private GameObject hud;

    private void Awake()
    {
        if (GameDataHolder.eelIsDead && GameDataHolder.inEelCave && !GameDataHolder.hintCamPlayed)
        {
            cameraHintCutscene.SetActive(true);
            playerMovement.enabled = false;
            hud.SetActive(false);
            Invoke("EndCutscene", 3.01f);
        }
    }

    private void EndCutscene()
    {
        StartCoroutine(FadeToBlack(2f));
    }

    private IEnumerator FadeToBlack(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.3f);
        while (fadeToBlackImage.color.a < 1f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        yield return new WaitForSeconds(2f);

        hintCam.enabled = false;
        hud.SetActive(true);

        while (fadeToBlackImage.color.a >= 0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }

        playerMovement.enabled = true;
        GameDataHolder.hintCamPlayed = true;
        mainCamera.SetActive(true);
        cameraHintCutscene.SetActive(false);
    }
}
