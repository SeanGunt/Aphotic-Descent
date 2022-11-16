using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightMovement : MonoBehaviour, IDataPersistence
{
    private Vector3 vectOffset;
    [SerializeField]private GameObject goFollow;
    [SerializeField] private float speed = 3.0f;
    
    public void LoadData(GameData data)
  {
    this.transform.position = data.flashlightPosition;
  }

  public void SaveData(GameData data)
  {
    data.flashlightPosition = this.transform.position;
  }
    
    // Start is called before the first frame update
    void Start()
    {
        vectOffset = transform.position - goFollow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = goFollow.transform.position + vectOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }
}
