using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecondDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] flashlightMechanic fmech;
    private bool opened;
    private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpeningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameDataHolder.secondDoorOpened)
        {
            door.transform.eulerAngles = new Vector3(0,-90,0);
        }
    }

    private void Update()
    {
        if (GameDataHolder.secondDoorOpened && !opened)
        {
            StartCoroutine("DoorSwingOpen");
        }
    }
    public void Open()
    {
        if (GameDataHolder.secondDoorOpened == false)
        {
            fmech.flashlightText.text = "Door Opened";
            fmech.Invoke("ClearUI", 3);
            audioSource.PlayOneShot(doorOpeningSound);
            GameDataHolder.secondDoorOpened = true;
            DataPersistenceManager.instance.SaveGame();
        }
    }

    private IEnumerator DoorSwingOpen()
    {
        door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), Time.deltaTime * 200);
        yield return new WaitForSeconds(1f);
        opened = true;
    }
}
