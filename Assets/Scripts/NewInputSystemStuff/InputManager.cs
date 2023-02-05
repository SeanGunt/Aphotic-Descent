using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static PlayerInputActions inputActions;
    public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        if (inputActions == null)
            inputActions = new PlayerInputActions();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(inputActions.PlayerControls);
    }

    // Update is called once per frame
    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
            return;

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }

    public static void StartRebind(string actionName, int bindingIndex, TMP_Text statusText)
    {
        InputAction action = inputActions.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couldn't find action or binding.");
            return;
        }

        if(action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if(firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                DoRebind(action, bindingIndex, statusText, true);
        }
        else
            DoRebind(action, bindingIndex, statusText, false);
    }
    
    private static void DoRebind(InputAction actionToRebind, int bindingIndex, TMP_Text statusText, bool allCompositeParts)
    {
        if (actionToRebind == null || bindingIndex < 0)
            return;

        statusText.text = $"Press a {actionToRebind.expectedControlType}";

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if(allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if(nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts);
            }
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
        });

        rebind.Start(); // actually starts the rebinding process.
    }
}
