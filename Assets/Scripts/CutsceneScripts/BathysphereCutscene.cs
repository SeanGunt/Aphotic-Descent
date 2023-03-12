using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathysphereCutscene : MonoBehaviour
{
    [SerializeField] private GameObject bathysphereCutscene;
    [SerializeField] private Camera bathysphereCam;
    private AudioListener bathysphereListener;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Image fadeToBlackImage;
    private MeshRenderer[] bathysphereRenderers;

    private void Awake()
    {
        bathysphereRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void EndBathysphere()
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

        bathysphereCam.enabled = false;
        bathysphereListener = bathysphereCutscene.GetComponentInChildren<AudioListener>();
        bathysphereListener.enabled = false;
        foreach(MeshRenderer bathysphereRenderer in bathysphereRenderers)
        {
            bathysphereRenderer.enabled = false;
        }
        mainCamera.SetActive(true);
        BGMManager.instance.SwitchBGM(0);

        while (fadeToBlackImage.color.a >= 0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }

        playerMovement.inCutscene = false;
        GameDataHolder.bathysphereCutscenePlayed = true;
        bathysphereCutscene.SetActive(false);
    }
}
