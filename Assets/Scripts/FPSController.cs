using System;
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
   // [SerializeField] public GameEventGameObject onModelPointing;
   
   private Vector3 _moveDirection = Vector3.zero;
   private float _rotationX = 0;
   
   private CharacterController _characterController;

   private bool _clipRecordIsEnabled;
   //private AudioSourceManager _currentAudioSourceManager;
   private ElementManager _currentElemManager;
   private long _timeStartRecord;
   private bool _isRecording;
   private Ray _rayOrigin;
   private RaycastHit _hitInfo;
   void Start()
   {
      _characterController = GetComponent<CharacterController>();
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
      float movementDirectionY = _moveDirection.y;
      _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

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

      _characterController.Move(_moveDirection * Time.deltaTime);
      if (canMove)
      {
         _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
         _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
         playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
         transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
      }
      #endregion

      #region HandlesClipSave
      long currentTime = new DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
      if (Input.GetKeyDown(KeyCode.R)  && _clipRecordIsEnabled && !_isRecording)
      {
         
            AudioRecorder.Instance.StartRecording(source);
            _timeStartRecord =  currentTime;
            _isRecording = true;
        
      }
      else if (Input.GetKeyDown(KeyCode.R) && _isRecording)
      {
         _isRecording = false;
         // _currentAudioSourceManager.SetNewAudioClip(AudioRecorder.Instance.EndRecording(), currentTime - _timeStartRecord);
         _currentElemManager.SetRecording(AudioRecorder.Instance.EndRecording(), currentTime - _timeStartRecord);
      }
      
      if (Input.GetKeyUp(KeyCode.R) && _isRecording  && _timeStartRecord + 2 < currentTime)
      {
            _isRecording = false;
            _currentElemManager.SetRecording(AudioRecorder.Instance.EndRecording(), currentTime - _timeStartRecord);
            // _currentAudioSourceManager.SetNewAudioClip(AudioRecorder.EndRecording(), currentTime - _timeStartRecord);
            // onClipRecordingStop.Raise();
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
         // _currentAudioSourceManager = _hitInfo.transform.gameObject.GetComponentInParent<AudioSourceManager>();
         if (_currentElemManager is not null)
         {
            _currentElemManager.NotPointedAt();
            _currentElemManager = null;
         }
         _currentElemManager = _hitInfo.transform.gameObject.GetComponentInParent<ElementManager>();
         if (_currentElemManager is null)
         {
            _currentElemManager = _hitInfo.transform.gameObject.GetComponent<ElementManager>();

         }
         _currentElemManager.PointedAt();
         _clipRecordIsEnabled = true;
      
      }
      else if (!raycastHit && !_isRecording && _clipRecordIsEnabled)
      {
         _currentElemManager.NotPointedAt();
         _clipRecordIsEnabled = false;
         _currentElemManager = null;
      }

      #endregion
   }
}
