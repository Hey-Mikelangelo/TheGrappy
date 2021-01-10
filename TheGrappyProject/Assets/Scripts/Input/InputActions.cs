// GENERATED AUTOMATICALLY FROM 'Assets/Settings/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""BaseGameplay"",
            ""id"": ""c4a7c13e-d58a-4abb-9a99-a51085c2ceef"",
            ""actions"": [
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7f6b1e46-6a8c-4349-a37a-dd83516ffabb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""ee75e7ac-e4e6-46d6-ba40-258f2291245f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a2b661b7-47d5-4232-aab2-eb45bbc4d37a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""wasd"",
                    ""id"": ""be4bf801-a9fa-4f67-b071-82782d78d7da"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3b5516f2-de32-44e1-8ccd-4e33e463ec22"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""243502fd-370d-41d3-989a-9c038abc0611"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""14ac56b1-f106-479f-afe8-ed145c3618f7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""52c193e5-6331-4988-ba09-bbdc57f6d6c2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""arrowKeys"",
                    ""id"": ""9d839af3-af81-4aee-9081-cd34ebaec01c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4e367b58-9762-4343-9310-fdafa6846e5a"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""35b7379b-a7ab-4a36-8b88-a8a844d70544"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9bc6048b-f18d-4a73-8415-961d4a210ab6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""129911b7-1bb9-4e7c-9634-87494fdf8cb8"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2ae245b1-cb27-4574-a889-e922b9ce05e0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2d07d59-d56e-4854-9fd5-b35cff6ea005"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BaseGameplay
        m_BaseGameplay = asset.FindActionMap("BaseGameplay", throwIfNotFound: true);
        m_BaseGameplay_Aim = m_BaseGameplay.FindAction("Aim", throwIfNotFound: true);
        m_BaseGameplay_Action = m_BaseGameplay.FindAction("Action", throwIfNotFound: true);
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

    // BaseGameplay
    private readonly InputActionMap m_BaseGameplay;
    private IBaseGameplayActions m_BaseGameplayActionsCallbackInterface;
    private readonly InputAction m_BaseGameplay_Aim;
    private readonly InputAction m_BaseGameplay_Action;
    public struct BaseGameplayActions
    {
        private @InputActions m_Wrapper;
        public BaseGameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Aim => m_Wrapper.m_BaseGameplay_Aim;
        public InputAction @Action => m_Wrapper.m_BaseGameplay_Action;
        public InputActionMap Get() { return m_Wrapper.m_BaseGameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BaseGameplayActions set) { return set.Get(); }
        public void SetCallbacks(IBaseGameplayActions instance)
        {
            if (m_Wrapper.m_BaseGameplayActionsCallbackInterface != null)
            {
                @Aim.started -= m_Wrapper.m_BaseGameplayActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_BaseGameplayActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_BaseGameplayActionsCallbackInterface.OnAim;
                @Action.started -= m_Wrapper.m_BaseGameplayActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_BaseGameplayActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_BaseGameplayActionsCallbackInterface.OnAction;
            }
            m_Wrapper.m_BaseGameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
            }
        }
    }
    public BaseGameplayActions @BaseGameplay => new BaseGameplayActions(this);
    public interface IBaseGameplayActions
    {
        void OnAim(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
    }
}
