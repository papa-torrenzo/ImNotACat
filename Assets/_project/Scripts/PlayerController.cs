using System.Collections.Generic;
using Unity.Cinemachine;
using KBCore.Refs;
using Unity.VisualScripting;
using UnityEngine;


namespace com.torrenzo.Foundation {
    public class PlayerController : ValidatedMonoBehaviour {
        [Header("References")]
        [SerializeField, Self] CharacterController characterController;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineCamera freeLookCam;
        [SerializeField, Anywhere] InputReader input;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        /*[Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;*/

        const float ZeroF = 0f;

        Transform mainCam;

        float currentSpeed;
        float velocity;
        // float jumpVelocity;
        // float dashVelocity = 1f;

        Vector3 movement;

        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");

        void Awake() {
            mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookCam.OnTargetObjectWarped(transform, transform.position - freeLookCam.transform.position - Vector3.forward);



        }

        void Start() => input.EnablePlayerActions();

        void OnEnable() {
            input.Jump += OnJump;
            //input.Move += OnMove;
        }

        void OnDisable() {
            input.Jump -= OnJump;
            //input.Move -= OnMove;

        }



        void OnJump(bool performed) {
           // noop
        }


        void Update() {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            HandleMovement();
            UpdateAnimator();
        }

        void UpdateAnimator() {
            animator.SetFloat(Speed, currentSpeed);
        }

        public void HandleMovement() {
            // Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

            if (adjustedDirection.magnitude > ZeroF) {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            } else {
                SmoothSpeed(ZeroF);

                // Reset horizontal velocity for a snappy stop
                characterController.Move(Vector3.zero);
                //new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
            }
        }

        void HandleHorizontalMovement(Vector3 adjustedDirection) {
            // Move the player
            Vector3 velocity = adjustedDirection * (moveSpeed * Time.fixedDeltaTime);
            characterController.Move(velocity);
            Debug.Log(velocity);
        }

        void HandleRotation(Vector3 adjustedDirection) {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void SmoothSpeed(float value) {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }
}