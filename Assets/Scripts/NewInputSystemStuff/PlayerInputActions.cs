//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/NewInputSystemStuff/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""84e1c5a9-70b8-4009-a63c-816147ea1e4c"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""e8c30073-cbf9-407b-a18a-d7bb648b0b99"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""02fc2855-40f8-4808-a996-ee46ec03e581"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Ascend"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f4e1c6c4-3d15-48cb-9279-29e11fac9a75"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Descend"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5317fcfc-e44d-4406-a8ea-b8ed80c7a34b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Knife"",
                    ""type"": ""Button"",
                    ""id"": ""b56856f5-d5e2-48b6-a4c9-27f28e84cced"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""c9c94502-c10c-443f-a7f4-e562828b22a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Invisibility"",
                    ""type"": ""Button"",
                    ""id"": ""5129cb5f-3123-4b6a-907f-ec42559f16b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Flashlight"",
                    ""type"": ""Button"",
                    ""id"": ""f9a63426-8976-49d8-9e5b-b0f4b2b77ae6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Blacklight"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c4fea956-7221-43e0-b400-8ea2c484b368"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""ea6a0224-8f74-4d8c-9f7b-f30d071a0077"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""1d98650e-7ca2-4eeb-b831-444189a4cd37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""9614dab0-5873-4fed-8d8b-ab6ffcc5ee95"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""555dd61b-5331-4b9b-a8eb-98d430305321"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2650ea97-5c9c-4fc4-973e-d60a9aeb2af2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2ad18d18-8c87-4401-ad70-51f33e67549a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""94ed9a5d-75bb-4fa2-91a7-3a445ad102cc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""62fd0e1c-d301-467d-aa0b-f2ec6eecdcef"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""359aa7b8-4777-496b-8fec-53ede8aec216"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63adfdbc-c96d-4b3d-8714-16da1696aff8"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22810e92-4121-4d0c-8c4f-83cfa8f6f080"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ascend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1dccbc9b-3283-4689-a815-b32c08f0c4c1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ascend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87ee3193-d909-43d2-9a47-0ebd8cf30181"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Descend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e9b4dd9-b13b-4b51-b831-d5d259370e9d"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Descend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b86a6df2-a771-4b87-a21f-ec153c91d8e8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Knife"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d43f6eba-3a49-408d-91fd-1df53c7b0ac2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Knife"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3e97efb-e73e-431d-b55b-026d5c6039ff"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""732599a1-cb73-449d-abf4-df1b9d36c66f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6e00f4b-025e-4198-b4cf-bcd566a5da73"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Invisibility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0787787d-508b-4bfd-a99d-0fab394b184b"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Invisibility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4cb9382-cdbe-4f9e-87e8-d0f7a2d73cc2"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dee7bfa2-8457-42cc-9e77-24ba6621c0fd"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a46ea437-66b7-424d-8ff4-44928e59b71a"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Blacklight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74bd2b9a-edbb-4305-8d00-3ad7da9cba15"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Blacklight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4f89e76-7844-423f-b8e9-5c20142fdc20"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6aae9c27-0871-4269-a5a9-48ad10f4755a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MenuControls"",
            ""id"": ""e844ca54-3af5-4d4b-a26b-a42db1fbf1d5"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""8410d9f4-0dbe-4de0-9a7b-06cf13a9b877"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5b939a9f-53fc-4ec7-b6c5-59ad73618f79"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Movement = m_PlayerControls.FindAction("Movement", throwIfNotFound: true);
        m_PlayerControls_Look = m_PlayerControls.FindAction("Look", throwIfNotFound: true);
        m_PlayerControls_Ascend = m_PlayerControls.FindAction("Ascend", throwIfNotFound: true);
        m_PlayerControls_Descend = m_PlayerControls.FindAction("Descend", throwIfNotFound: true);
        m_PlayerControls_Knife = m_PlayerControls.FindAction("Knife", throwIfNotFound: true);
        m_PlayerControls_Interact = m_PlayerControls.FindAction("Interact", throwIfNotFound: true);
        m_PlayerControls_Invisibility = m_PlayerControls.FindAction("Invisibility", throwIfNotFound: true);
        m_PlayerControls_Flashlight = m_PlayerControls.FindAction("Flashlight", throwIfNotFound: true);
        m_PlayerControls_Blacklight = m_PlayerControls.FindAction("Blacklight", throwIfNotFound: true);
        m_PlayerControls_Pause = m_PlayerControls.FindAction("Pause", throwIfNotFound: true);
        m_PlayerControls_Escape = m_PlayerControls.FindAction("Escape", throwIfNotFound: true);
        // MenuControls
        m_MenuControls = asset.FindActionMap("MenuControls", throwIfNotFound: true);
        m_MenuControls_Newaction = m_MenuControls.FindAction("New action", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Movement;
    private readonly InputAction m_PlayerControls_Look;
    private readonly InputAction m_PlayerControls_Ascend;
    private readonly InputAction m_PlayerControls_Descend;
    private readonly InputAction m_PlayerControls_Knife;
    private readonly InputAction m_PlayerControls_Interact;
    private readonly InputAction m_PlayerControls_Invisibility;
    private readonly InputAction m_PlayerControls_Flashlight;
    private readonly InputAction m_PlayerControls_Blacklight;
    private readonly InputAction m_PlayerControls_Pause;
    private readonly InputAction m_PlayerControls_Escape;
    public struct PlayerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerControls_Movement;
        public InputAction @Look => m_Wrapper.m_PlayerControls_Look;
        public InputAction @Ascend => m_Wrapper.m_PlayerControls_Ascend;
        public InputAction @Descend => m_Wrapper.m_PlayerControls_Descend;
        public InputAction @Knife => m_Wrapper.m_PlayerControls_Knife;
        public InputAction @Interact => m_Wrapper.m_PlayerControls_Interact;
        public InputAction @Invisibility => m_Wrapper.m_PlayerControls_Invisibility;
        public InputAction @Flashlight => m_Wrapper.m_PlayerControls_Flashlight;
        public InputAction @Blacklight => m_Wrapper.m_PlayerControls_Blacklight;
        public InputAction @Pause => m_Wrapper.m_PlayerControls_Pause;
        public InputAction @Escape => m_Wrapper.m_PlayerControls_Escape;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Ascend.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAscend;
                @Ascend.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAscend;
                @Ascend.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAscend;
                @Descend.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDescend;
                @Descend.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDescend;
                @Descend.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDescend;
                @Knife.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnKnife;
                @Knife.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnKnife;
                @Knife.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnKnife;
                @Interact.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Invisibility.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInvisibility;
                @Invisibility.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInvisibility;
                @Invisibility.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInvisibility;
                @Flashlight.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFlashlight;
                @Flashlight.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFlashlight;
                @Flashlight.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFlashlight;
                @Blacklight.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnBlacklight;
                @Blacklight.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnBlacklight;
                @Blacklight.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnBlacklight;
                @Pause.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Escape.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnEscape;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Ascend.started += instance.OnAscend;
                @Ascend.performed += instance.OnAscend;
                @Ascend.canceled += instance.OnAscend;
                @Descend.started += instance.OnDescend;
                @Descend.performed += instance.OnDescend;
                @Descend.canceled += instance.OnDescend;
                @Knife.started += instance.OnKnife;
                @Knife.performed += instance.OnKnife;
                @Knife.canceled += instance.OnKnife;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Invisibility.started += instance.OnInvisibility;
                @Invisibility.performed += instance.OnInvisibility;
                @Invisibility.canceled += instance.OnInvisibility;
                @Flashlight.started += instance.OnFlashlight;
                @Flashlight.performed += instance.OnFlashlight;
                @Flashlight.canceled += instance.OnFlashlight;
                @Blacklight.started += instance.OnBlacklight;
                @Blacklight.performed += instance.OnBlacklight;
                @Blacklight.canceled += instance.OnBlacklight;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);

    // MenuControls
    private readonly InputActionMap m_MenuControls;
    private IMenuControlsActions m_MenuControlsActionsCallbackInterface;
    private readonly InputAction m_MenuControls_Newaction;
    public struct MenuControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public MenuControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_MenuControls_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_MenuControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuControlsActions set) { return set.Get(); }
        public void SetCallbacks(IMenuControlsActions instance)
        {
            if (m_Wrapper.m_MenuControlsActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_MenuControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public MenuControlsActions @MenuControls => new MenuControlsActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnAscend(InputAction.CallbackContext context);
        void OnDescend(InputAction.CallbackContext context);
        void OnKnife(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnInvisibility(InputAction.CallbackContext context);
        void OnFlashlight(InputAction.CallbackContext context);
        void OnBlacklight(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
    }
    public interface IMenuControlsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
