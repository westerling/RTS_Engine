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
            ""name"": ""Player"",
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
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""cede1708-048d-4005-9108-a2cd30a78429"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Chat"",
                    ""type"": ""Button"",
                    ""id"": ""f5abbab8-55ff-46b6-ac81-56a37a55ad8c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""08ecb5e6-b910-49e7-9187-1b909ff69a8a"",
                    ""expectedControlType"": ""Button"",
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
                    ""id"": ""c2895991-4a64-4bbb-8b39-6fdaf80501ab"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0686128d-87a5-4111-8267-8cc2ebeca9de"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Chat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c10019eb-bcdd-4e70-a781-203bbe3716aa"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
            ""name"": ""UnitsSelected"",
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
            ""name"": ""BuildingSelected"",
            ""id"": ""5804008d-0040-4736-a1a4-2fc325e2451f"",
            ""actions"": [
                {
                    ""name"": ""Chat"",
                    ""type"": ""Button"",
                    ""id"": ""574c01b8-405a-4802-a9ff-4b1f2a310010"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Build Villager"",
                    ""type"": ""Button"",
                    ""id"": ""58a2b78a-cb7f-40f3-ae94-8d512a831d16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a26230be-18c2-4544-9ff6-dc5349f132d5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Chat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df36f000-f8af-498e-a9b3-dcb255a91e45"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Build Villager"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BuildMode"",
            ""id"": ""afe1c248-bd16-4da1-85fe-241062fa7aad"",
            ""actions"": [
                {
                    ""name"": ""Stop"",
                    ""type"": ""Button"",
                    ""id"": ""3fa9b561-6a5c-456a-8d00-74e41c194d4e"",
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
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""9c2abbb2-e1f6-48bc-a8fa-b087cc31454c"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""bb9ddd0f-2ad8-4320-aef9-2c54a0585f53"",
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
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveCamera = m_Player.FindAction("Move Camera", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Chat = m_Player.FindAction("Chat", throwIfNotFound: true);
        m_Player_Shift = m_Player.FindAction("Shift", throwIfNotFound: true);
        m_Player_MouseScrollY = m_Player.FindAction("MouseScrollY", throwIfNotFound: true);
        // UnitsSelected
        m_UnitsSelected = asset.FindActionMap("UnitsSelected", throwIfNotFound: true);
        m_UnitsSelected_House = m_UnitsSelected.FindAction("House", throwIfNotFound: true);
        // BuildingSelected
        m_BuildingSelected = asset.FindActionMap("BuildingSelected", throwIfNotFound: true);
        m_BuildingSelected_Chat = m_BuildingSelected.FindAction("Chat", throwIfNotFound: true);
        m_BuildingSelected_BuildVillager = m_BuildingSelected.FindAction("Build Villager", throwIfNotFound: true);
        // BuildMode
        m_BuildMode = asset.FindActionMap("BuildMode", throwIfNotFound: true);
        m_BuildMode_Stop = m_BuildMode.FindAction("Stop", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Newaction = m_Menu.FindAction("New action", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MoveCamera;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Chat;
    private readonly InputAction m_Player_Shift;
    private readonly InputAction m_Player_MouseScrollY;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveCamera => m_Wrapper.m_Player_MoveCamera;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @Chat => m_Wrapper.m_Player_Chat;
        public InputAction @Shift => m_Wrapper.m_Player_Shift;
        public InputAction @MouseScrollY => m_Wrapper.m_Player_MouseScrollY;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MoveCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Chat.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChat;
                @Chat.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChat;
                @Chat.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChat;
                @Shift.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShift;
                @Shift.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShift;
                @Shift.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShift;
                @MouseScrollY.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseScrollY;
                @MouseScrollY.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseScrollY;
                @MouseScrollY.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseScrollY;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveCamera.started += instance.OnMoveCamera;
                @MoveCamera.performed += instance.OnMoveCamera;
                @MoveCamera.canceled += instance.OnMoveCamera;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Chat.started += instance.OnChat;
                @Chat.performed += instance.OnChat;
                @Chat.canceled += instance.OnChat;
                @Shift.started += instance.OnShift;
                @Shift.performed += instance.OnShift;
                @Shift.canceled += instance.OnShift;
                @MouseScrollY.started += instance.OnMouseScrollY;
                @MouseScrollY.performed += instance.OnMouseScrollY;
                @MouseScrollY.canceled += instance.OnMouseScrollY;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UnitsSelected
    private readonly InputActionMap m_UnitsSelected;
    private IUnitsSelectedActions m_UnitsSelectedActionsCallbackInterface;
    private readonly InputAction m_UnitsSelected_House;
    public struct UnitsSelectedActions
    {
        private @Controls m_Wrapper;
        public UnitsSelectedActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @House => m_Wrapper.m_UnitsSelected_House;
        public InputActionMap Get() { return m_Wrapper.m_UnitsSelected; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UnitsSelectedActions set) { return set.Get(); }
        public void SetCallbacks(IUnitsSelectedActions instance)
        {
            if (m_Wrapper.m_UnitsSelectedActionsCallbackInterface != null)
            {
                @House.started -= m_Wrapper.m_UnitsSelectedActionsCallbackInterface.OnHouse;
                @House.performed -= m_Wrapper.m_UnitsSelectedActionsCallbackInterface.OnHouse;
                @House.canceled -= m_Wrapper.m_UnitsSelectedActionsCallbackInterface.OnHouse;
            }
            m_Wrapper.m_UnitsSelectedActionsCallbackInterface = instance;
            if (instance != null)
            {
                @House.started += instance.OnHouse;
                @House.performed += instance.OnHouse;
                @House.canceled += instance.OnHouse;
            }
        }
    }
    public UnitsSelectedActions @UnitsSelected => new UnitsSelectedActions(this);

    // BuildingSelected
    private readonly InputActionMap m_BuildingSelected;
    private IBuildingSelectedActions m_BuildingSelectedActionsCallbackInterface;
    private readonly InputAction m_BuildingSelected_Chat;
    private readonly InputAction m_BuildingSelected_BuildVillager;
    public struct BuildingSelectedActions
    {
        private @Controls m_Wrapper;
        public BuildingSelectedActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Chat => m_Wrapper.m_BuildingSelected_Chat;
        public InputAction @BuildVillager => m_Wrapper.m_BuildingSelected_BuildVillager;
        public InputActionMap Get() { return m_Wrapper.m_BuildingSelected; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BuildingSelectedActions set) { return set.Get(); }
        public void SetCallbacks(IBuildingSelectedActions instance)
        {
            if (m_Wrapper.m_BuildingSelectedActionsCallbackInterface != null)
            {
                @Chat.started -= m_Wrapper.m_BuildingSelectedActionsCallbackInterface.OnChat;
                @Chat.performed -= m_Wrapper.m_BuildingSelectedActionsCallbackInterface.OnChat;
                @Chat.canceled -= m_Wrapper.m_BuildingSelectedActionsCallbackInterface.OnChat;
                @BuildVillager.started -= m_Wrapper.m_BuildingSelectedActionsCallbackInterface.OnBuildVillager;
                @BuildVillager.performed -= m_Wrapper.m_BuildingSelectedActionsCallbackInterface.OnBuildVillager;
                @BuildVillager.canceled -= m_Wrapper.m_BuildingSelectedActionsCallbackInterface.OnBuildVillager;
            }
            m_Wrapper.m_BuildingSelectedActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Chat.started += instance.OnChat;
                @Chat.performed += instance.OnChat;
                @Chat.canceled += instance.OnChat;
                @BuildVillager.started += instance.OnBuildVillager;
                @BuildVillager.performed += instance.OnBuildVillager;
                @BuildVillager.canceled += instance.OnBuildVillager;
            }
        }
    }
    public BuildingSelectedActions @BuildingSelected => new BuildingSelectedActions(this);

    // BuildMode
    private readonly InputActionMap m_BuildMode;
    private IBuildModeActions m_BuildModeActionsCallbackInterface;
    private readonly InputAction m_BuildMode_Stop;
    public struct BuildModeActions
    {
        private @Controls m_Wrapper;
        public BuildModeActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Stop => m_Wrapper.m_BuildMode_Stop;
        public InputActionMap Get() { return m_Wrapper.m_BuildMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BuildModeActions set) { return set.Get(); }
        public void SetCallbacks(IBuildModeActions instance)
        {
            if (m_Wrapper.m_BuildModeActionsCallbackInterface != null)
            {
                @Stop.started -= m_Wrapper.m_BuildModeActionsCallbackInterface.OnStop;
                @Stop.performed -= m_Wrapper.m_BuildModeActionsCallbackInterface.OnStop;
                @Stop.canceled -= m_Wrapper.m_BuildModeActionsCallbackInterface.OnStop;
            }
            m_Wrapper.m_BuildModeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Stop.started += instance.OnStop;
                @Stop.performed += instance.OnStop;
                @Stop.canceled += instance.OnStop;
            }
        }
    }
    public BuildModeActions @BuildMode => new BuildModeActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Newaction;
    public struct MenuActions
    {
        private @Controls m_Wrapper;
        public MenuActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Menu_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
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
    public interface IPlayerActions
    {
        void OnMoveCamera(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnChat(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
        void OnMouseScrollY(InputAction.CallbackContext context);
    }
    public interface IUnitsSelectedActions
    {
        void OnHouse(InputAction.CallbackContext context);
    }
    public interface IBuildingSelectedActions
    {
        void OnChat(InputAction.CallbackContext context);
        void OnBuildVillager(InputAction.CallbackContext context);
    }
    public interface IBuildModeActions
    {
        void OnStop(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
