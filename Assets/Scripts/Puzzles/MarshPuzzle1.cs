using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarshPuzzle1 : MonoBehaviour
{
    public int ZPFreed;
    [SerializeField] private GameObject hud, mainCamera, playerVisual, blockade, ZP1, ZP2, ZP3;
    [SerializeField] private GameObject[] destinationPoints1, destinationPoints2, destinationPoints3;
    private Vector3 destinationPoint1, destinationPoint2, destinationPoint3, currentPosition1, cP2, cP3;
    private int positionInPoints;
    private float totalLength;
    [SerializeField] private Camera puzzleCam;
    private MarshPuzzle1 scriptFunctionality;
    [SerializeField]private ObjectiveUpdateHolder objectiveTextTrigger;
    [SerializeField]private Image fadeToBlackImage;
    private Animator animator;
    private bool cutsceneStarted, movingToNextPosition, atFinalDestination;

    private void Awake()
    {
        ZPFreed = 0;
        positionInPoints = 0;
        scriptFunctionality = GetComponent<MarshPuzzle1>();
        animator = this.GetComponent<Animator>();
        animator.enabled = false;
        atFinalDestination = true;
    }
    
    private void Update()
    {
        if (ZPFreed == 3 && !cutsceneStarted)
        {
            StartCoroutine(MarshTrenchComplete(1f));
            cutsceneStarted = true;
        }
        if (!atFinalDestination)
        {
            MovingTheZP();
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
        mainCamera.gameObject.SetActive(false);
        puzzleCam.enabled = true;
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
        mainCamera.gameObject.SetActive(true);
        puzzleCam.enabled = false;
        playerVisual.gameObject.SetActive(true);
        blockade.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        MarshTransition.instance.EndCutscene();
        objectiveTextTrigger.TwelfthObjective();
        animator.enabled = false;
        yield return new WaitForSeconds(0.1f);
        atFinalDestination = false;
    }

    private void MovingTheZP()
    {
        currentPosition1 = ZP1.transform.position;
        cP2 = ZP2.transform.position;
        cP3 = ZP3.transform.position;
        if (destinationPoints1.Length == 0 || destinationPoints2.Length == 0 || destinationPoints3.Length == 0)
        {
            return;
        }
        if(!movingToNextPosition)
        {
            if (positionInPoints == destinationPoints1.Length-1)
            {
                atFinalDestination = true;
                positionInPoints = 0;
                return;
            }
            destinationPoint1 = destinationPoints1[positionInPoints].transform.position;
            destinationPoint2 = destinationPoints2[positionInPoints].transform.position;
            destinationPoint3 = destinationPoints3[positionInPoints].transform.position;
            
            positionInPoints++;
            movingToNextPosition = true;
        }
        ZP1.transform.position = Vector3.MoveTowards(currentPosition1, destinationPoint1, 2 * Time.deltaTime);
        ZP2.transform.position = Vector3.MoveTowards(cP2, destinationPoint2, 2 * Time.deltaTime);
        ZP3.transform.position = Vector3.MoveTowards(cP3, destinationPoint3, 2 * Time.deltaTime);
        totalLength = Vector3.Distance(currentPosition1, destinationPoint1);

        if (totalLength <= 0.2f)
        {
            movingToNextPosition = false;
        }
    }
}
