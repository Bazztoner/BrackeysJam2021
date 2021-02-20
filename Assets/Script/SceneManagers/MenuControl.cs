// GENERATED AUTOMATICALLY FROM 'Assets/Script/SceneManagers/MenuControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MenuControl : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MenuControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MenuControl"",
    ""maps"": [
        {
            ""name"": ""Navigation"",
            ""id"": ""ee67fd91-5953-4f0a-b396-c642ca1841f3"",
            ""actions"": [
                {
                    ""name"": ""NavigateUp"",
                    ""type"": ""Button"",
                    ""id"": ""b027b843-2a14-4b76-9584-5dba7d41a84d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NavigateDown"",
                    ""type"": ""Button"",
                    ""id"": ""8564bc9b-605f-4842-9991-8424ee185cad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""cacc735e-8c35-4e4e-8af6-0e002880bc6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""81ae81fe-b50c-4a9e-a564-53fef98f4f37"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigateUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09028af4-e06f-4c73-b641-4e4cbcfcc05b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigateDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51389a9e-861e-450b-bfc8-43139b799c5b"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Navigation
        m_Navigation = asset.FindActionMap("Navigation", throwIfNotFound: true);
        m_Navigation_NavigateUp = m_Navigation.FindAction("NavigateUp", throwIfNotFound: true);
        m_Navigation_NavigateDown = m_Navigation.FindAction("NavigateDown", throwIfNotFound: true);
        m_Navigation_Select = m_Navigation.FindAction("Select", throwIfNotFound: true);
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

    // Navigation
    private readonly InputActionMap m_Navigation;
    private INavigationActions m_NavigationActionsCallbackInterface;
    private readonly InputAction m_Navigation_NavigateUp;
    private readonly InputAction m_Navigation_NavigateDown;
    private readonly InputAction m_Navigation_Select;
    public struct NavigationActions
    {
        private @MenuControl m_Wrapper;
        public NavigationActions(@MenuControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @NavigateUp => m_Wrapper.m_Navigation_NavigateUp;
        public InputAction @NavigateDown => m_Wrapper.m_Navigation_NavigateDown;
        public InputAction @Select => m_Wrapper.m_Navigation_Select;
        public InputActionMap Get() { return m_Wrapper.m_Navigation; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NavigationActions set) { return set.Get(); }
        public void SetCallbacks(INavigationActions instance)
        {
            if (m_Wrapper.m_NavigationActionsCallbackInterface != null)
            {
                @NavigateUp.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateUp;
                @NavigateUp.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateUp;
                @NavigateUp.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateUp;
                @NavigateDown.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateDown;
                @NavigateDown.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateDown;
                @NavigateDown.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateDown;
                @Select.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_NavigationActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NavigateUp.started += instance.OnNavigateUp;
                @NavigateUp.performed += instance.OnNavigateUp;
                @NavigateUp.canceled += instance.OnNavigateUp;
                @NavigateDown.started += instance.OnNavigateDown;
                @NavigateDown.performed += instance.OnNavigateDown;
                @NavigateDown.canceled += instance.OnNavigateDown;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public NavigationActions @Navigation => new NavigationActions(this);
    public interface INavigationActions
    {
        void OnNavigateUp(InputAction.CallbackContext context);
        void OnNavigateDown(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
