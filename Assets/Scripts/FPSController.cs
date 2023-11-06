using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
   [SerializeField]private Camera playerCamera;
   [SerializeField]private float walkSpeed = 6f;
   [SerializeField]private float runSpeed = 12f;
   [SerializeField] private AudioClip source;
   
   [SerializeField]private float lookSpeed = 2f;
   [SerializeField]private float lookXLimit = 45f;
   [SerializeField]private bool canMove = true;
   
   
   private Vector3 moveDirection = Vector3.zero;
   private float rotationX = 0;

   
   private CharacterController characterController;

   private bool _clipRecordIsEnabled;
   private AudioSourceManager _currentAudioSourceManager;
   private long _timeStartRecord;
   private bool _isRecording;
   Ray _rayOrigin;
   RaycastHit _hitInfo;
   void Start()
   {
      characterController = GetComponent<CharacterController>();
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
   }

   private void Update()
   {
      #region Handles Movement

      Vector3 forward = transform.TransformDirection(Vector3.forward);
      Vector3 right = transform.TransformDirection(Vector3.right);
      
      //Press Left Shift to run
      bool isRunning = Input.GetKey(KeyCode.LeftShift);
      float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
      float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
      float movementDirectionY = moveDirection.y;
      moveDirection = (forward * curSpeedX) + (right * curSpeedY);

      #endregion

      #region Handles Jumping

      // if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
      // {
      //    moveDirection.y = jumpPower;
      // }
      // else
      // {
      //    moveDirection.y = movementDirectionY;
      // }
      //
      // if (!characterController.isGrounded)
      // {
      //    moveDirection.y = gravity * Time.deltaTime;
      // }

      #endregion

      #region Handles Rotation

      characterController.Move(moveDirection * Time.deltaTime);
      if (canMove)
      {
         rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
         rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
         playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
         transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
      }
      #endregion

      #region HandlesClipSave
      long currentTime = new DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
      if (Input.GetKeyDown(KeyCode.R)  && _clipRecordIsEnabled && !_isRecording)
      {
         
            AudioRecorder.StartRecording(source);
            _timeStartRecord =  currentTime;
            _isRecording = true;
        
      }
      else if (Input.GetKeyDown(KeyCode.R) && _isRecording)
      {
         _isRecording = false;
         _currentAudioSourceManager.SetNewAudioClip(AudioRecorder.EndRecording(), currentTime - _timeStartRecord);
      }
      
      if (Input.GetKeyUp(KeyCode.R) && _isRecording  && _timeStartRecord + 2 < currentTime)
      {
            _isRecording = false;
            _currentAudioSourceManager.SetNewAudioClip(AudioRecorder.EndRecording(), currentTime - _timeStartRecord);
      }

      #endregion

      #region Raycast test

      int layerMask = LayerMask.GetMask("SoundSensitive");
      bool raycastHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out _hitInfo,
         5f,
         layerMask);
      if (raycastHit && !_clipRecordIsEnabled)
      {
         Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 5f, Color.yellow);
         _currentAudioSourceManager = _hitInfo.transform.gameObject.GetComponentInParent<AudioSourceManager>();
         _clipRecordIsEnabled = true;
         Debug.Log(_currentAudioSourceManager);
         Debug.Log( _hitInfo.transform.gameObject);
      }
      else if (!raycastHit && !_isRecording && _clipRecordIsEnabled)
      {
         _clipRecordIsEnabled = false;
         _currentAudioSourceManager = null;
      }

      #endregion
   }
}
