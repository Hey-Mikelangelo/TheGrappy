// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Default Input Actions/XR_InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace XRInput
{
    public class @XRInputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @XRInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""XR_InputActions"",
    ""maps"": [
        {
            ""name"": ""HMD"",
            ""id"": ""a3a4db6d-60a9-403f-af0a-4f631bb9b7df"",
            ""actions"": [
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""555ed2ee-d38c-4e80-a14b-f131a0a09bc4"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""b19fcb82-9246-44d9-b2bd-63c4f92473f8"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bef6b585-7085-427b-a4ae-edcade6ec809"",
                    ""path"": ""<XRHMD>/centerEyePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eca32440-65d1-41ed-a784-cf0db90f8e8d"",
                    ""path"": ""<XRHMD>/centerEyeRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""RightHand"",
            ""id"": ""88147c3d-acf0-4475-9886-9b24bb2a6403"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""fe438135-7b23-4e58-bd41-0d5394c69d37"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""19b8cf94-25f6-4713-9bb8-9ec465a6e586"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GripButton"",
                    ""type"": ""Button"",
                    ""id"": ""89a8e330-27d1-49b2-aa0b-f14e16b05112"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TriggerButton"",
                    ""type"": ""Button"",
                    ""id"": ""20fbe516-a70c-470e-8c4d-f2adb662aca9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryButton"",
                    ""type"": ""Button"",
                    ""id"": ""cdf05bc2-6c6f-40e5-b2a9-9d05b5c911e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryButton"",
                    ""type"": ""Button"",
                    ""id"": ""d0993b25-97b6-4171-81d0-83d24807c611"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAxis"",
                    ""type"": ""Value"",
                    ""id"": ""e947d2e6-3a3f-494b-b999-519d8a13ae3b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAxisClicked"",
                    ""type"": ""Button"",
                    ""id"": ""12c19756-3f10-4e11-b9bc-8a2073fd4a2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff61a971-93b4-4624-af27-b933c84db425"",
                    ""path"": ""<XRController>{RightHand}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""246e384b-9ba7-4a09-affc-dad250139185"",
                    ""path"": ""<XRController>{RightHand}/gripButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""GripButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90fd8901-f5d7-4948-8a06-8293225290dc"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GripButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8c4a537-8a9f-406d-8d20-23b084dfc908"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/grippressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GripButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d97c7ae6-f30f-4eca-80c5-0af3367ebf66"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""TriggerButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dca2ac6a-6c3b-4720-8610-9dfc42d8c86c"",
                    ""path"": ""<XRController>{RightHand}/triggerButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58538949-57dc-4798-96de-c37500a446c8"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40904b55-35c7-43f9-83ef-a1c2a0208aa7"",
                    ""path"": ""<XRController>{RightHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35e065e6-1176-4276-8452-b4e1009ff21f"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ef8b794-08c3-40ff-a097-c4f8414abb17"",
                    ""path"": ""<XRController>{RightHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""600b0604-cf40-4c3d-ae8f-d80163d2184e"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/secondarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93fd947d-ee3f-48f5-8640-1f037ca86957"",
                    ""path"": ""<XRController>{RightHand}/primary2DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60c55d67-142f-4a34-8d88-8f9dbe7c35d3"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f1271c9-6204-421b-8a4e-3058d7a3e5dc"",
                    ""path"": ""<XRController>{RightHand}/primary2DAxisClick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxisClicked"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ff42e33-3f75-43d5-8cbe-1a71bcd32d7a"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/thumbstickclicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxisClicked"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb6061be-24fe-40c6-bc14-6aef3a2d2d29"",
                    ""path"": ""<XRController>{RightHand}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""LeftHand"",
            ""id"": ""fe4cff47-77e3-4fc6-a129-36c7f7fa626c"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""107f3b44-485f-42ee-af6a-e2d4126587b3"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""6308f524-6c01-41a5-aa51-1d8d9720cb77"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GripButton"",
                    ""type"": ""Button"",
                    ""id"": ""19eb737b-866f-4f7c-8617-65545d70079d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TriggerButton"",
                    ""type"": ""Button"",
                    ""id"": ""995b846b-ea0a-4f1a-b59d-ca1109674c6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryButton"",
                    ""type"": ""Button"",
                    ""id"": ""e9a938e8-9f59-4965-8af8-d3b38169c7d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryButton"",
                    ""type"": ""Button"",
                    ""id"": ""7cf26d4d-a597-4073-9bb4-21b7d4ee0fe8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAxis"",
                    ""type"": ""Value"",
                    ""id"": ""e74699b5-9212-4758-a385-38e3b92163c9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAxisClicked"",
                    ""type"": ""Button"",
                    ""id"": ""2136973e-fcab-47f5-9562-81c87e3aa792"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8d2f55a3-0a53-45d0-b191-f8a49ddb4a8e"",
                    ""path"": ""<XRController>{LeftHand}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96448503-fd2b-4dfa-a4b0-c1255231842c"",
                    ""path"": ""<XRController>{RightHand}/gripButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""GripButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b2b8ac0-cb45-4938-893b-b05a92f0636d"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GripButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfb8e6fe-3cd8-4813-af48-c962b0898e84"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/grippressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GripButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a52f93ef-678c-4dcf-9ea5-c887cf57140d"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""TriggerButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""570eef1f-29c5-4957-a815-adb0b8a3fa69"",
                    ""path"": ""<XRController>{LeftHand}/triggerButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15f4925c-c0d1-42e8-8271-3142abdd59d7"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e00d9562-430e-4872-8bf2-2835ccbe6320"",
                    ""path"": ""<XRController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""333064a8-13c7-40f6-899b-ef17e6a50710"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a946ce1c-ce75-4301-86d7-03d00828b343"",
                    ""path"": ""<XRController>{LeftHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b32cdeb-8c05-4303-9f90-35ff37e73750"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/secondarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14428b8d-9a5c-4a1d-8786-48ad12c3dd0a"",
                    ""path"": ""<XRController>{LeftHand}/primary2DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4dd24081-357e-4a05-8330-5ef17e8fb0d8"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64c40549-5c6e-4301-8bee-e487d8f17a0b"",
                    ""path"": ""<XRController>{LeftHand}/primary2DAxisClick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxisClicked"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""916c6f41-cf26-4408-9aa9-ba2865dcd93f"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/thumbstickclicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAxisClicked"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e7b230c-4cd7-475f-bcc1-b1124f3486df"",
                    ""path"": ""<XRController>{LeftHand}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // HMD
            m_HMD = asset.FindActionMap("HMD", throwIfNotFound: true);
            m_HMD_Rotation = m_HMD.FindAction("Rotation", throwIfNotFound: true);
            m_HMD_Position = m_HMD.FindAction("Position", throwIfNotFound: true);
            // RightHand
            m_RightHand = asset.FindActionMap("RightHand", throwIfNotFound: true);
            m_RightHand_Position = m_RightHand.FindAction("Position", throwIfNotFound: true);
            m_RightHand_Rotation = m_RightHand.FindAction("Rotation", throwIfNotFound: true);
            m_RightHand_GripButton = m_RightHand.FindAction("GripButton", throwIfNotFound: true);
            m_RightHand_TriggerButton = m_RightHand.FindAction("TriggerButton", throwIfNotFound: true);
            m_RightHand_PrimaryButton = m_RightHand.FindAction("PrimaryButton", throwIfNotFound: true);
            m_RightHand_SecondaryButton = m_RightHand.FindAction("SecondaryButton", throwIfNotFound: true);
            m_RightHand_PrimaryAxis = m_RightHand.FindAction("PrimaryAxis", throwIfNotFound: true);
            m_RightHand_PrimaryAxisClicked = m_RightHand.FindAction("PrimaryAxisClicked", throwIfNotFound: true);
            // LeftHand
            m_LeftHand = asset.FindActionMap("LeftHand", throwIfNotFound: true);
            m_LeftHand_Position = m_LeftHand.FindAction("Position", throwIfNotFound: true);
            m_LeftHand_Rotation = m_LeftHand.FindAction("Rotation", throwIfNotFound: true);
            m_LeftHand_GripButton = m_LeftHand.FindAction("GripButton", throwIfNotFound: true);
            m_LeftHand_TriggerButton = m_LeftHand.FindAction("TriggerButton", throwIfNotFound: true);
            m_LeftHand_PrimaryButton = m_LeftHand.FindAction("PrimaryButton", throwIfNotFound: true);
            m_LeftHand_SecondaryButton = m_LeftHand.FindAction("SecondaryButton", throwIfNotFound: true);
            m_LeftHand_PrimaryAxis = m_LeftHand.FindAction("PrimaryAxis", throwIfNotFound: true);
            m_LeftHand_PrimaryAxisClicked = m_LeftHand.FindAction("PrimaryAxisClicked", throwIfNotFound: true);
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

        // HMD
        private readonly InputActionMap m_HMD;
        private IHMDActions m_HMDActionsCallbackInterface;
        private readonly InputAction m_HMD_Rotation;
        private readonly InputAction m_HMD_Position;
        public struct HMDActions
        {
            private @XRInputActions m_Wrapper;
            public HMDActions(@XRInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Rotation => m_Wrapper.m_HMD_Rotation;
            public InputAction @Position => m_Wrapper.m_HMD_Position;
            public InputActionMap Get() { return m_Wrapper.m_HMD; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(HMDActions set) { return set.Get(); }
            public void SetCallbacks(IHMDActions instance)
            {
                if (m_Wrapper.m_HMDActionsCallbackInterface != null)
                {
                    @Rotation.started -= m_Wrapper.m_HMDActionsCallbackInterface.OnRotation;
                    @Rotation.performed -= m_Wrapper.m_HMDActionsCallbackInterface.OnRotation;
                    @Rotation.canceled -= m_Wrapper.m_HMDActionsCallbackInterface.OnRotation;
                    @Position.started -= m_Wrapper.m_HMDActionsCallbackInterface.OnPosition;
                    @Position.performed -= m_Wrapper.m_HMDActionsCallbackInterface.OnPosition;
                    @Position.canceled -= m_Wrapper.m_HMDActionsCallbackInterface.OnPosition;
                }
                m_Wrapper.m_HMDActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Rotation.started += instance.OnRotation;
                    @Rotation.performed += instance.OnRotation;
                    @Rotation.canceled += instance.OnRotation;
                    @Position.started += instance.OnPosition;
                    @Position.performed += instance.OnPosition;
                    @Position.canceled += instance.OnPosition;
                }
            }
        }
        public HMDActions @HMD => new HMDActions(this);

        // RightHand
        private readonly InputActionMap m_RightHand;
        private IRightHandActions m_RightHandActionsCallbackInterface;
        private readonly InputAction m_RightHand_Position;
        private readonly InputAction m_RightHand_Rotation;
        private readonly InputAction m_RightHand_GripButton;
        private readonly InputAction m_RightHand_TriggerButton;
        private readonly InputAction m_RightHand_PrimaryButton;
        private readonly InputAction m_RightHand_SecondaryButton;
        private readonly InputAction m_RightHand_PrimaryAxis;
        private readonly InputAction m_RightHand_PrimaryAxisClicked;
        public struct RightHandActions
        {
            private @XRInputActions m_Wrapper;
            public RightHandActions(@XRInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Position => m_Wrapper.m_RightHand_Position;
            public InputAction @Rotation => m_Wrapper.m_RightHand_Rotation;
            public InputAction @GripButton => m_Wrapper.m_RightHand_GripButton;
            public InputAction @TriggerButton => m_Wrapper.m_RightHand_TriggerButton;
            public InputAction @PrimaryButton => m_Wrapper.m_RightHand_PrimaryButton;
            public InputAction @SecondaryButton => m_Wrapper.m_RightHand_SecondaryButton;
            public InputAction @PrimaryAxis => m_Wrapper.m_RightHand_PrimaryAxis;
            public InputAction @PrimaryAxisClicked => m_Wrapper.m_RightHand_PrimaryAxisClicked;
            public InputActionMap Get() { return m_Wrapper.m_RightHand; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(RightHandActions set) { return set.Get(); }
            public void SetCallbacks(IRightHandActions instance)
            {
                if (m_Wrapper.m_RightHandActionsCallbackInterface != null)
                {
                    @Position.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPosition;
                    @Position.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPosition;
                    @Position.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPosition;
                    @Rotation.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnRotation;
                    @Rotation.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnRotation;
                    @Rotation.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnRotation;
                    @GripButton.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnGripButton;
                    @GripButton.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnGripButton;
                    @GripButton.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnGripButton;
                    @TriggerButton.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnTriggerButton;
                    @TriggerButton.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnTriggerButton;
                    @TriggerButton.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnTriggerButton;
                    @PrimaryButton.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryButton;
                    @PrimaryButton.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryButton;
                    @PrimaryButton.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryButton;
                    @SecondaryButton.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnSecondaryButton;
                    @SecondaryButton.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnSecondaryButton;
                    @SecondaryButton.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnSecondaryButton;
                    @PrimaryAxis.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryAxis;
                    @PrimaryAxis.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryAxis;
                    @PrimaryAxis.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryAxis;
                    @PrimaryAxisClicked.started -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.performed -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.canceled -= m_Wrapper.m_RightHandActionsCallbackInterface.OnPrimaryAxisClicked;
                }
                m_Wrapper.m_RightHandActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Position.started += instance.OnPosition;
                    @Position.performed += instance.OnPosition;
                    @Position.canceled += instance.OnPosition;
                    @Rotation.started += instance.OnRotation;
                    @Rotation.performed += instance.OnRotation;
                    @Rotation.canceled += instance.OnRotation;
                    @GripButton.started += instance.OnGripButton;
                    @GripButton.performed += instance.OnGripButton;
                    @GripButton.canceled += instance.OnGripButton;
                    @TriggerButton.started += instance.OnTriggerButton;
                    @TriggerButton.performed += instance.OnTriggerButton;
                    @TriggerButton.canceled += instance.OnTriggerButton;
                    @PrimaryButton.started += instance.OnPrimaryButton;
                    @PrimaryButton.performed += instance.OnPrimaryButton;
                    @PrimaryButton.canceled += instance.OnPrimaryButton;
                    @SecondaryButton.started += instance.OnSecondaryButton;
                    @SecondaryButton.performed += instance.OnSecondaryButton;
                    @SecondaryButton.canceled += instance.OnSecondaryButton;
                    @PrimaryAxis.started += instance.OnPrimaryAxis;
                    @PrimaryAxis.performed += instance.OnPrimaryAxis;
                    @PrimaryAxis.canceled += instance.OnPrimaryAxis;
                    @PrimaryAxisClicked.started += instance.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.performed += instance.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.canceled += instance.OnPrimaryAxisClicked;
                }
            }
        }
        public RightHandActions @RightHand => new RightHandActions(this);

        // LeftHand
        private readonly InputActionMap m_LeftHand;
        private ILeftHandActions m_LeftHandActionsCallbackInterface;
        private readonly InputAction m_LeftHand_Position;
        private readonly InputAction m_LeftHand_Rotation;
        private readonly InputAction m_LeftHand_GripButton;
        private readonly InputAction m_LeftHand_TriggerButton;
        private readonly InputAction m_LeftHand_PrimaryButton;
        private readonly InputAction m_LeftHand_SecondaryButton;
        private readonly InputAction m_LeftHand_PrimaryAxis;
        private readonly InputAction m_LeftHand_PrimaryAxisClicked;
        public struct LeftHandActions
        {
            private @XRInputActions m_Wrapper;
            public LeftHandActions(@XRInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Position => m_Wrapper.m_LeftHand_Position;
            public InputAction @Rotation => m_Wrapper.m_LeftHand_Rotation;
            public InputAction @GripButton => m_Wrapper.m_LeftHand_GripButton;
            public InputAction @TriggerButton => m_Wrapper.m_LeftHand_TriggerButton;
            public InputAction @PrimaryButton => m_Wrapper.m_LeftHand_PrimaryButton;
            public InputAction @SecondaryButton => m_Wrapper.m_LeftHand_SecondaryButton;
            public InputAction @PrimaryAxis => m_Wrapper.m_LeftHand_PrimaryAxis;
            public InputAction @PrimaryAxisClicked => m_Wrapper.m_LeftHand_PrimaryAxisClicked;
            public InputActionMap Get() { return m_Wrapper.m_LeftHand; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(LeftHandActions set) { return set.Get(); }
            public void SetCallbacks(ILeftHandActions instance)
            {
                if (m_Wrapper.m_LeftHandActionsCallbackInterface != null)
                {
                    @Position.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPosition;
                    @Position.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPosition;
                    @Position.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPosition;
                    @Rotation.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnRotation;
                    @Rotation.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnRotation;
                    @Rotation.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnRotation;
                    @GripButton.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnGripButton;
                    @GripButton.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnGripButton;
                    @GripButton.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnGripButton;
                    @TriggerButton.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnTriggerButton;
                    @TriggerButton.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnTriggerButton;
                    @TriggerButton.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnTriggerButton;
                    @PrimaryButton.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryButton;
                    @PrimaryButton.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryButton;
                    @PrimaryButton.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryButton;
                    @SecondaryButton.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnSecondaryButton;
                    @SecondaryButton.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnSecondaryButton;
                    @SecondaryButton.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnSecondaryButton;
                    @PrimaryAxis.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryAxis;
                    @PrimaryAxis.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryAxis;
                    @PrimaryAxis.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryAxis;
                    @PrimaryAxisClicked.started -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.performed -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.canceled -= m_Wrapper.m_LeftHandActionsCallbackInterface.OnPrimaryAxisClicked;
                }
                m_Wrapper.m_LeftHandActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Position.started += instance.OnPosition;
                    @Position.performed += instance.OnPosition;
                    @Position.canceled += instance.OnPosition;
                    @Rotation.started += instance.OnRotation;
                    @Rotation.performed += instance.OnRotation;
                    @Rotation.canceled += instance.OnRotation;
                    @GripButton.started += instance.OnGripButton;
                    @GripButton.performed += instance.OnGripButton;
                    @GripButton.canceled += instance.OnGripButton;
                    @TriggerButton.started += instance.OnTriggerButton;
                    @TriggerButton.performed += instance.OnTriggerButton;
                    @TriggerButton.canceled += instance.OnTriggerButton;
                    @PrimaryButton.started += instance.OnPrimaryButton;
                    @PrimaryButton.performed += instance.OnPrimaryButton;
                    @PrimaryButton.canceled += instance.OnPrimaryButton;
                    @SecondaryButton.started += instance.OnSecondaryButton;
                    @SecondaryButton.performed += instance.OnSecondaryButton;
                    @SecondaryButton.canceled += instance.OnSecondaryButton;
                    @PrimaryAxis.started += instance.OnPrimaryAxis;
                    @PrimaryAxis.performed += instance.OnPrimaryAxis;
                    @PrimaryAxis.canceled += instance.OnPrimaryAxis;
                    @PrimaryAxisClicked.started += instance.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.performed += instance.OnPrimaryAxisClicked;
                    @PrimaryAxisClicked.canceled += instance.OnPrimaryAxisClicked;
                }
            }
        }
        public LeftHandActions @LeftHand => new LeftHandActions(this);
        public interface IHMDActions
        {
            void OnRotation(InputAction.CallbackContext context);
            void OnPosition(InputAction.CallbackContext context);
        }
        public interface IRightHandActions
        {
            void OnPosition(InputAction.CallbackContext context);
            void OnRotation(InputAction.CallbackContext context);
            void OnGripButton(InputAction.CallbackContext context);
            void OnTriggerButton(InputAction.CallbackContext context);
            void OnPrimaryButton(InputAction.CallbackContext context);
            void OnSecondaryButton(InputAction.CallbackContext context);
            void OnPrimaryAxis(InputAction.CallbackContext context);
            void OnPrimaryAxisClicked(InputAction.CallbackContext context);
        }
        public interface ILeftHandActions
        {
            void OnPosition(InputAction.CallbackContext context);
            void OnRotation(InputAction.CallbackContext context);
            void OnGripButton(InputAction.CallbackContext context);
            void OnTriggerButton(InputAction.CallbackContext context);
            void OnPrimaryButton(InputAction.CallbackContext context);
            void OnSecondaryButton(InputAction.CallbackContext context);
            void OnPrimaryAxis(InputAction.CallbackContext context);
            void OnPrimaryAxisClicked(InputAction.CallbackContext context);
        }
    }
}
