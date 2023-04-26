using UnityEngine;
using System.Collections;
using TMPro;

public class ObjectiveUpdateHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private GameObject objectiveObj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip objectiveSound;

    private void Awake()
    {
        if(GameDataHolder.eelIsDead && !GameDataHolder.hermitCaveObjectiveTriggered)
        {
            Invoke("EighthObjective", 2f);
        }
        objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 0);
        objectiveObj.SetActive(true);
    }

    public void FirstObjective()
    {
        objectiveText.text = "Escape The Submarine";
        GameDataHolder.objectiveId = 1;
        StartCoroutine(FadeText(7f, objectiveText));
    }
    public void SecondObjective()
    {
        objectiveText.text = "Find A Way Through The Kelp Maze";
        GameDataHolder.objectiveId = 2;
        GameDataHolder.kelpMazeObjectiveTriggerd = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void ThirdObjective()
    {
        objectiveText.text = "Proceed To The Lab";
        GameDataHolder.objectiveId = 3;
        GameDataHolder.kelpMazeEndTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));

    }

    public void FourthObjective()
    {
        objectiveText.text = "Investigate The Lab";
        GameDataHolder.objectiveId = 4;
        GameDataHolder.labStartObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void FifthObjective()
    {
        objectiveText.text = "Neutralize The Eel";
        GameDataHolder.objectiveId = 5;
        GameDataHolder.eelObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void SixthObjective()
    {
        objectiveText.text = "End Its Suffering";
        GameDataHolder.objectiveId = 6;
        GameDataHolder.eelObjective2Triggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void SeventhObjective()
    {
        objectiveText.text = "Find The Exit To The Cave";
        GameDataHolder.objectiveId = 7;
        GameDataHolder.ridgeObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void EighthObjective()
    {
        BGMManager.instance.SwitchBGMFade(7);
        audioSource.PlayOneShot(objectiveSound);
        objectiveText.text = "Descend Deeper Into The Cave";
        GameDataHolder.objectiveId = 8;
        GameDataHolder.hermitCaveObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void NinthObjective()
    {
        objectiveText.text = "Reach The Cage Before It Closes";
        GameDataHolder.objectiveId = 9;
        GameDataHolder.pistolShrimpObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void TenthObjective()
    {
        objectiveText.text = "Find A Way To Destroy The Biolamps";
        GameDataHolder.objectiveId = 10;
        GameDataHolder.biolampsObjectivetriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void EleventhObjective()
    {
        objectiveText.text = "Repel The Creature";
        GameDataHolder.objectiveId = 11;
        GameDataHolder.marshObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void TwelfthObjective()
    {
        objectiveText.text = "Find Your Way Out of The Marsh";
        GameDataHolder.objectiveId = 12;
        GameDataHolder.marshObjective2Triggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void ThirteenthObjective()
    {
        objectiveText.text = "Find A Way To Fix The Submarine";
        GameDataHolder.objectiveId = 13;
        GameDataHolder.trenchObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }
    public IEnumerator FadeText(float t, TextMeshProUGUI i)
    {
        objectiveObj.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
