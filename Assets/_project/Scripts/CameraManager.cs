using System.Collections;
using Unity.Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace com.torrenzo.Foundation {
    public class CameraManager : ValidatedMonoBehaviour {
        [Header("References")]
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Anywhere] CinemachineCamera freeLookCam;

        private CinemachineOrbitalFollow orbitalFollow;

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] float speedMultiplier = 1f;

        bool isRMBPressed;
        bool cameraMovementLock;

        void OnEnable() {
            orbitalFollow = freeLookCam.GetComponent<CinemachineOrbitalFollow>();
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        void OnDisable() {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        void OnLook(Vector2 cameraMovement, bool isDeviceMouse) {
            if (cameraMovementLock) return;

            if (isDeviceMouse && !isRMBPressed) return;

            // If the device is mouse use fixedDeltaTime, otherwise use deltaTime
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            // Set the camera axis values
            orbitalFollow.HorizontalAxis.Value = cameraMovement.x * speedMultiplier * deviceMultiplier;
            orbitalFollow.VerticalAxis.Value = cameraMovement.y * speedMultiplier * deviceMultiplier;
        }

        void OnEnableMouseControlCamera() {
            isRMBPressed = true;

            // Lock the cursor to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        void OnDisableMouseControlCamera() {
            isRMBPressed = false;

            // Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Reset the camera axis to prevent jumping when re-enabling mouse control
            orbitalFollow.HorizontalAxis.Value = 0f;
            orbitalFollow.VerticalAxis.Value = 0f;
        }

        IEnumerator DisableMouseForFrame() {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }

    }
}