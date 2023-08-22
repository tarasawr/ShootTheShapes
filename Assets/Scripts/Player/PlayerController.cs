using UnityEngine;

namespace Taras.Player
{
    public class PlayerController : MonoBehaviour
    {
        public VariableJoystick variableJoystick;
        public Transform cameraTransform;

        [SerializeField] private float rotationSpeed = 70.0f;
        [SerializeField] private float rotationSmoothing = 5.0f;

        private float _xRotation = 0f;
        private Vector3 _targetObject;
        private bool _isLooking = false;

        private void Update()
        {
            if (_isLooking)
            {
                LookAtTargetObject();
            }
            else
            {
                float horizontalInput = variableJoystick.Horizontal * rotationSpeed * Time.deltaTime;
                float verticalInput = variableJoystick.Vertical * rotationSpeed * Time.deltaTime;

                _xRotation -= verticalInput;
                _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

                cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
                transform.Rotate(Vector3.up * horizontalInput);
            }
        }

        private void LookAtTargetObject()
        {
            Vector3 direction = _targetObject - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                _isLooking = false;
            }
        }

        public void SetTargetObject(Vector3 target)
        {
            _targetObject = target;
            _isLooking = true;
        }
    }
}