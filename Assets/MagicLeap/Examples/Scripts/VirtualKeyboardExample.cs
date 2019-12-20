// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2019 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Developer Agreement, located
// here: https://id.magicleap.com/terms/developer
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// Updates the parent for the InputField & VirtualKeyboard at runtime.
    /// This is done to prevent modifying or breaking the Prefab connection of the UserInterface.
    /// The UI Status panel is also updated each frame with relative controller information.
    /// </summary>
    public class VirtualKeyboardExample : MonoBehaviour
    {
        [SerializeField, Tooltip("A reference to the controller connection handler in the scene.")]
        private MLControllerConnectionHandlerBehavior _controllerConnectionHandler = null;

        [SerializeField, Tooltip("The status text field within the UserInterface.")]
        private Text _statusText = null;

        [SerializeField, Tooltip("A reference to the example UI canvas placement script.")]
        private PlaceFromCamera _examplePlacement = null;

        private void Start()
        {
            #if PLATFORM_LUMIN
            MLInput.OnControllerButtonDown += HandleOnButtonDown;
            #endif
        }

        private void OnDestroy()
        {
            #if PLATFORM_LUMIN
            MLInput.OnControllerButtonDown -= HandleOnButtonDown;
            #endif
        }

        private void Update()
        {
            UpdateStatus();
        }

        /// <summary>
        /// Updates the status text.
        /// </summary>
        private void UpdateStatus()
        {
            _statusText.text = string.Format("<color=#dbfb76><b>{0} {1}</b></color>\n{2}: {3}\n",
                  LocalizeManager.GetString("Controller"),
                  LocalizeManager.GetString("Data"),
                  LocalizeManager.GetString("Status"),
                  LocalizeManager.GetString(ControllerStatus.Text));
        }

        private void HandleOnButtonDown(byte controllerId, MLInput.Controller.Button button)
        {
            if (_controllerConnectionHandler.IsControllerValid(controllerId))
            {
                if (button == MLInput.Controller.Button.Bumper)
                {
                    StartCoroutine("SingleFrameUpdate");
                }
            }
        }

        private IEnumerator SingleFrameUpdate()
        {
            _examplePlacement.PlaceOnUpdate = true;
            yield return new WaitForEndOfFrame();
            _examplePlacement.PlaceOnUpdate = false;
        }
    }
}