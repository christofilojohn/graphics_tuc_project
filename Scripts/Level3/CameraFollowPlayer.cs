using UnityEngine;

namespace Level3.scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player; // Reference to the player's transform

        [SerializeField]
        private Vector3 offset = new Vector3(0, 5, -10); // Offset value between the player and the camera

        [SerializeField] private float smoothSpeed = 0.125f; // Smooth speed factor
        [SerializeField] private float heightOffset = 5f; // Height offset for the camera
        [SerializeField] private float zoomedOffsetMultiplier = 0.5f;
        [SerializeField] private KeyCode zoomKey = KeyCode.Tab;

        private Vector3 _initialOffset; // keep initial offset for zooming

        //[SerializeField] private float angle = 45f; // Angle to tilt the camera downwards
        private bool _isZoomed = false;

        void Start()
        {
            // Set the initial offset if not set in the Inspector
            if (offset == Vector3.zero)
            {
                offset = transform.position - player.position;
            }

            _initialOffset = offset;
        }

        void LateUpdate()
        {
            if (Input.GetKeyDown(zoomKey))
            {
                _isZoomed = !_isZoomed;
            }

            // Adjust the offset based on the zoom state
            Vector3 currentOffset = _isZoomed ? _initialOffset * zoomedOffsetMultiplier : _initialOffset;

            // Desired position based on the player's position and the offset
            Vector3 desiredPosition = player.position + currentOffset;
            // Adjust the height of the camera
            desiredPosition.y += heightOffset;
            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            Vector3 lookAtPosition = player.position;
            transform.LookAt(lookAtPosition);
        }

        public void ZoomIn()
        {
            if (!_isZoomed)
            {
                _isZoomed = true;
            }
        }

        public void ZoomOut()
        {
            if (_isZoomed)
            {
                _isZoomed = false;
            }
        }
    }
}