using UnityEngine;
using System.Collections;
using TMPro;

public class ObjectiveUpdateHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private GameObject objectiveObj;

    private void Awake()
    {
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
        objectiveText.text = "Defeat The Eel";
        GameDataHolder.objectiveId = 5;
        GameDataHolder.eelObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }

    public void SixthObjective()
    {
        objectiveText.text = "Find The Exit To The Cave";
        GameDataHolder.objectiveId = 6;
        GameDataHolder.ridgeObjectiveTriggered = true;
        StartCoroutine(FadeText(7f, objectiveText));
    }
    public IEnumerator FadeText(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
