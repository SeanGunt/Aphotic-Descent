using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Interactor : MonoBehaviour
{
    public LayerMask interactableLayerMask = 7;
    public Interactable interactable;
    public Image interactImage;
    public Sprite defaultIcon;
    public Vector2 defaultIconSize;
    public Sprite defaultInteractIcon;
    public Vector2 defaultInteractIconSize;
    UnityEvent onInteract;
   
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, 2, interactableLayerMask))
        {
            if(hit.collider.GetComponent<Interactable>() != false)
            {
                onInteract = hit.collider.GetComponent<Interactable>().onInteract;
                if (Input.GetButtonDown("Interact"))
                {
                onInteract.Invoke();
                }
            }
        }
    }
}