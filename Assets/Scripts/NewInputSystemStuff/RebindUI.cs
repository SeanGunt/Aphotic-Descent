using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindUI : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference; //references input action asset

    [SerializeField] private bool excludeMouse = true; //don't accidentally rebind to mouse
    [Range(0,10)] [SerializeField] private int selectedBinding;
    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;

    [Header("Binding Info - Don't edit pls")]
    [SerializeField] private InputBinding inputBinding;
    private int bindingIndex;

    private string actionName;

    [Header("UI stuff")]
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private Button rebindButton;
    [SerializeField] private TMP_Text rebindText;
    [SerializeField] private Button resetButton;

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => DoRebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if(inputActionReference != null)
        {
            InputManager.LoadBindingOverride(actionName);
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.rebindComplete += UpdateUI;
    }

    private void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
    }
    
    private void OnValidate()
    {
        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if (inputActionReference.action != null)
            actionName = inputActionReference.name;

        if (inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if(actionText != null)
            actionText.text = actionName;

        if(rebindText != null)
        {
            if(Application.isPlaying)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
            }
            else
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void DoRebind()
    {
        InputManager.StartRebind(actionName, bindingIndex, rebindText);
    }

    private void ResetBinding()
    {
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }
}
