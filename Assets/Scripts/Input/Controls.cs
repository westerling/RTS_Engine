// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Camera"",
            ""id"": ""d1b94d21-7bbf-48e8-8a64-6404f7ddf49e"",
            ""actions"": [
                {
                    ""name"": ""Move Camera"",
                    ""type"": ""Value"",
                    ""id"": ""a24ab013-3640-4e07-b966-f7189e9896d8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseScrollY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""60f17761-5487-4409-8804-7e835cda1dcb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""5f1f4465-62b0-4c42-9d08-b0cfe688824b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""99dca889-744c-48a9-bd2e-a80c8a9be244"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3c9dc2d5-16de-4d83-888a-c504d4c2ee04"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""16979659-2ef2-49a8-b18c-5b7c137968de"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""05810057-9b7d-4f12-a0c9-e9347d02d9c0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1f038465-8684-4bc9-aef2-ad523261081f"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseScrollY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Normal"",
            ""id"": ""a2449f98-a92b-43a8-a10c-05040fd796cf"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""c44bb1ec-a463-4eaa-89f7-734c63617d92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""eeae66d3-1264-4417-8a74-2159020a4ba0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Chat"",
                    ""type"": ""Button"",
                    ""id"": ""6d9c4813-8c4c-413a-a043-0255a4d1e3fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""67d17c90-bf86-42e6-afe8-537958dd9a27"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e94dbc7-4a73-448d-81b5-b7d9802dcd31"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85395b42-3516-4ab5-a49f-9ccccc0e107c"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Chat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Selected"",
            ""id"": ""253c8656-3461-4144-912f-6b7a35063c8c"",
            ""actions"": [
                {
                    ""name"": ""House"",
                    ""type"": ""Button"",
                    ""id"": ""cbf8899d-3542-42e7-b096-bc0fd0ab95fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d5d44f85-fa0a-48d4-a409-bd5c261d45db"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""House"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Build"",
            ""id"": ""afe1c248-bd16-4da1-85fe-241062fa7aad"",
            ""actions"": [
                {
                    ""name"": ""Stop"",
                    ""type"": ""Button"",
                    ""id"": ""3fa9b561-6a5c-456a-8d00-74e41c194d4e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Build"",
                    ""type"": ""Button"",
                    ""id"": ""275fb5d8-0011-4c87-aee3-d5c50034f9e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ecc3ad17-f5d8-4279-ae57-f1808f58554f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Stop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91d8ac91-d502-491e-993e-0cc19af0b407"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""9c2abbb2-e1f6-48bc-a8fa-b087cc31454c"",
            ""actions"": [
                {
                    ""name"": ""Resume"",
                    ""type"": ""Button"",
                    ""id"": ""bb9ddd0f-2ad8-4320-aef9-2c54a0585f53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Type"",
                    ""type"": ""Button"",
                    ""id"": ""ece348d0-54a6-4c01-913e-2ee879790f6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Send"",
                    ""type"": ""Button"",
                    ""id"": ""d4710b46-a025-410a-b590-a9947c2b6ef5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0c67d659-3356-4ce8-b7e0-e36be0ef611e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Resume"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dece3c37-f251-4bc1-bbe0-bc5b0aff2c3a"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Type"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4eb8af91-91bf-4f1c-a9fe-dd050920f053"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Send"",
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
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_MoveCamera = m_Camera.FindAction("Move Camera", throwIfNotFound: true);
        m_Camera_MouseScrollY = m_Camera.FindAction("MouseScrollY", throwIfNotFound: true);
        // Normal
        m_Normal = asset.FindActionMap("Normal", throwIfNotFound: true);
        m_Normal_Pause = m_Normal.FindAction("Pause", throwIfNotFound: true);
        m_Normal_Shift = m_Normal.FindAction("Shift", throwIfNotFound: true);
        m_Normal_Chat = m_Normal.FindAction("Chat", throwIfNotFound: true);
        // Selected
        m_Selected = asset.FindActionMap("Selected", throwIfNotFound: true);
        m_Selected_House = m_Selected.FindAction("House", throwIfNotFound: true);
        // Build
        m_Build = asset.FindActionMap("Build", throwIfNotFound: true);
        m_Build_Stop = m_Build.FindAction("Stop", throwIfNotFound: true);
        m_Build_Build = m_Build.FindAction("Build", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Resume = m_Menu.FindAction("Resume", throwIfNotFound: true);
        m_Menu_Type = m_Menu.FindAction("Type", throwIfNotFound: true);
        m_Menu_Send = m_Menu.FindAction("Send", throwIfNotFound: true);
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

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_MoveCamera;
    private readonly InputAction m_Camera_MouseScrollY;
    public struct CameraActions
    {
        private @Controls m_Wrapper;
        public CameraActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveCamera => m_Wrapper.m_Camera_MoveCamera;
        public InputAction @MouseScrollY => m_Wrapper.m_Camera_MouseScrollY;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @MoveCamera.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveCamera;
                @MouseScrollY.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMouseScrollY;
                @MouseScrollY.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMouseScrollY;
                @MouseScrollY.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMouseScrollY;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveCamera.started += instance.OnMoveCamera;
                @MoveCamera.performed += instance.OnMoveCamera;
                @MoveCamera.canceled += instance.OnMoveCamera;
                @MouseScrollY.started += instance.OnMouseScrollY;
                @MouseScrollY.performed += instance.OnMouseScrollY;
                @MouseScrollY.canceled += instance.OnMouseScrollY;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // Normal
    private readonly InputActionMap m_Normal;
    private INormalActions m_NormalActionsCallbackInterface;
    private readonly InputAction m_Normal_Pause;
    private readonly InputAction m_Normal_Shift;
    private readonly InputAction m_Normal_Chat;
    public struct NormalActions
    {
        private @Controls m_Wrapper;
        public NormalActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Normal_Pause;
        public InputAction @Shift => m_Wrapper.m_Normal_Shift;
        public InputAction @Chat => m_Wrapper.m_Normal_Chat;
        public InputActionMap Get() { return m_Wrapper.m_Normal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NormalActions set) { return set.Get(); }
        public void SetCallbacks(INormalActions instance)
        {
            if (m_Wrapper.m_NormalActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Shift.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnShift;
                @Shift.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnShift;
                @Shift.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnShift;
                @Chat.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChat;
                @Chat.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChat;
                @Chat.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChat;
            }
            m_Wrapper.m_NormalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Shift.started += instance.OnShift;
                @Shift.performed += instance.OnShift;
                @Shift.canceled += instance.OnShift;
                @Chat.started += instance.OnChat;
                @Chat.performed += instance.OnChat;
                @Chat.canceled += instance.OnChat;
            }
        }
    }
    public NormalActions @Normal => new NormalActions(this);

    // Selected
    private readonly InputActionMap m_Selected;
    private ISelectedActions m_SelectedActionsCallbackInterface;
    private readonly InputAction m_Selected_House;
    public struct SelectedActions
    {
        private @Controls m_Wrapper;
        public SelectedActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @House => m_Wrapper.m_Selected_House;
        public InputActionMap Get() { return m_Wrapper.m_Selected; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SelectedActions set) { return set.Get(); }
        public void SetCallbacks(ISelectedActions instance)
        {
            if (m_Wrapper.m_SelectedActionsCallbackInterface != null)
            {
                @House.started -= m_Wrapper.m_SelectedActionsCallbackInterface.OnHouse;
                @House.performed -= m_Wrapper.m_SelectedActionsCallbackInterface.OnHouse;
                @House.canceled -= m_Wrapper.m_SelectedActionsCallbackInterface.OnHouse;
            }
            m_Wrapper.m_SelectedActionsCallbackInterface = instance;
            if (instance != null)
            {
                @House.started += instance.OnHouse;
                @House.performed += instance.OnHouse;
                @House.canceled += instance.OnHouse;
            }
        }
    }
    public SelectedActions @Selected => new SelectedActions(this);

    // Build
    private readonly InputActionMap m_Build;
    private IBuildActions m_BuildActionsCallbackInterface;
    private readonly InputAction m_Build_Stop;
    private readonly InputAction m_Build_Build;
    public struct BuildActions
    {
        private @Controls m_Wrapper;
        public BuildActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Stop => m_Wrapper.m_Build_Stop;
        public InputAction @Build => m_Wrapper.m_Build_Build;
        public InputActionMap Get() { return m_Wrapper.m_Build; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BuildActions set) { return set.Get(); }
        public void SetCallbacks(IBuildActions instance)
        {
            if (m_Wrapper.m_BuildActionsCallbackInterface != null)
            {
                @Stop.started -= m_Wrapper.m_BuildActionsCallbackInterface.OnStop;
                @Stop.performed -= m_Wrapper.m_BuildActionsCallbackInterface.OnStop;
                @Stop.canceled -= m_Wrapper.m_BuildActionsCallbackInterface.OnStop;
                @Build.started -= m_Wrapper.m_BuildActionsCallbackInterface.OnBuild;
                @Build.performed -= m_Wrapper.m_BuildActionsCallbackInterface.OnBuild;
                @Build.canceled -= m_Wrapper.m_BuildActionsCallbackInterface.OnBuild;
            }
            m_Wrapper.m_BuildActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Stop.started += instance.OnStop;
                @Stop.performed += instance.OnStop;
                @Stop.canceled += instance.OnStop;
                @Build.started += instance.OnBuild;
                @Build.performed += instance.OnBuild;
                @Build.canceled += instance.OnBuild;
            }
        }
    }
    public BuildActions @Build => new BuildActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Resume;
    private readonly InputAction m_Menu_Type;
    private readonly InputAction m_Menu_Send;
    public struct MenuActions
    {
        private @Controls m_Wrapper;
        public MenuActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Resume => m_Wrapper.m_Menu_Resume;
        public InputAction @Type => m_Wrapper.m_Menu_Type;
        public InputAction @Send => m_Wrapper.m_Menu_Send;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Resume.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnResume;
                @Resume.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnResume;
                @Resume.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnResume;
                @Type.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnType;
                @Type.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnType;
                @Type.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnType;
                @Send.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSend;
                @Send.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSend;
                @Send.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSend;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Resume.started += instance.OnResume;
                @Resume.performed += instance.OnResume;
                @Resume.canceled += instance.OnResume;
                @Type.started += instance.OnType;
                @Type.performed += instance.OnType;
                @Type.canceled += instance.OnType;
                @Send.started += instance.OnSend;
                @Send.performed += instance.OnSend;
                @Send.canceled += instance.OnSend;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface ICameraActions
    {
        void OnMoveCamera(InputAction.CallbackContext context);
        void OnMouseScrollY(InputAction.CallbackContext context);
    }
    public interface INormalActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
        void OnChat(InputAction.CallbackContext context);
    }
    public interface ISelectedActions
    {
        void OnHouse(InputAction.CallbackContext context);
    }
    public interface IBuildActions
    {
        void OnStop(InputAction.CallbackContext context);
        void OnBuild(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnResume(InputAction.CallbackContext context);
        void OnType(InputAction.CallbackContext context);
        void OnSend(InputAction.CallbackContext context);
    }
}
