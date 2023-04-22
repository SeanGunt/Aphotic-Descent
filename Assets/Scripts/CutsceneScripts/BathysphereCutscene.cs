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
    private flashlightMechanic flashlightmechanic;
    private WeaponController weaponController;
    private GameObject player;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Image fadeToBlackImage;
    [SerializeField] private GameObject dumbyPlayer;
    private MeshRenderer[] bathysphereRenderers;
    [SerializeField] private SkinnedMeshRenderer mantaRenderer;
    [SerializeField] private AudioClip bathosphereAudio;
    private AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flashlightmechanic = player.GetComponent<flashlightMechanic>();
        weaponController = player.GetComponent<WeaponController>();
        bathysphereRenderers = GetComponentsInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bathosphereAudio);
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
        dumbyPlayer.SetActive(false);
        bathysphereListener = bathysphereCutscene.GetComponentInChildren<AudioListener>();
        bathysphereListener.enabled = false;
        foreach(MeshRenderer bathysphereRenderer in bathysphereRenderers)
        {
            bathysphereRenderer.enabled = false;
        }
        mantaRenderer.enabled = false;
        mainCamera.SetActive(true);
        BGMManager.instance.SwitchBGM(0);

        while (fadeToBlackImage.color.a >= 0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }

        playerMovement.inCutscene = false;
        GameDataHolder.bathysphereCutscenePlayed = true;
        flashlightmechanic.enabled = true;
        weaponController.enabled = true;
        bathysphereCutscene.SetActive(false);
    }
}
