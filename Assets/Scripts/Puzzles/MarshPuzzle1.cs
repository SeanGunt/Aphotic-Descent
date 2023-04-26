using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarshPuzzle1 : MonoBehaviour
{
    public int ZPFreed;
    [SerializeField] private GameObject hud;
    [SerializeField] private Camera puzzleCam;
    private MarshPuzzle1 scriptFunctionality;
    [SerializeField]private Image fadeToBlackImage;
    private Animator animator;
    private bool cutsceneStarted;

    private void Awake()
    {
        ZPFreed = 0;
        scriptFunctionality = GetComponent<MarshPuzzle1>();
        animator = this.GetComponent<Animator>();
        animator.enabled = false;
    }
    
    private void Update()
    {
        if (ZPFreed == 3 && !cutsceneStarted)
        {
            StartCoroutine(MarshTrenchComplete(1f));
            cutsceneStarted = true;
        }
    }

    public IEnumerator MarshTrenchComplete(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.0f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        hud.gameObject.SetActive(false);
        MarshTransition.instance.StartCutscene();
        animator.enabled = true;
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        yield return new WaitForSeconds(3.1f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        hud.gameObject.SetActive(true);
        puzzleCam.enabled = false;
        yield return new WaitForSeconds(0.1f);
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        MarshTransition.instance.EndCutscene();
        scriptFunctionality.enabled = false;
    }
}
