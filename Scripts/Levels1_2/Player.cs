using System;
using UnityEngine;

public class Player : MonoBehaviour, IObjectOwner {
    // Player Singleton pattern
    public static Player Instance { get; private set; }
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private CameraFollow camera;  
    [SerializeField] private LayerMask objectPlacementLayerMask;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform objectFloatingHoldPoint;
    [SerializeField] private UImanager UImanager;
    [SerializeField] private GameManager gameManager;
    //[SerializeField] private LayerMask playerCollisionMask;
    //[SerializeField] private float swingRange = 2.0f;
    private bool _isSpeaking = false;
    private IObjectOwner _selectedOwner;
    //private float _swingSpeedReduction = 0.5f;
    private bool _isMoving;
    private bool _isCarrying;
    
    private Vector3 _lastInteractDir;
    private GameObjectPickable _gameObject;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    // For event listeners
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }
    
    // VS auto generates these method signatures for event handling
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    { 
        bool justPickedUp = false;
        bool justDropped = false;
        // Drop
        // If the player is carrying an object, and the selected object place/owner doesn't have object
        if (_isCarrying && _selectedOwner != null && !_selectedOwner.HasGameObject())
        {
            _gameObject.SetObjectParent(_selectedOwner);
            justDropped = true;
            // If Player is carrying object and selected object owner has object, then not allowed
        } else if (_isCarrying && _selectedOwner != null && _selectedOwner.HasGameObject())
        {
            UImanager.MakeInteractButtonRed();
        }
        // Pickup
        // If the player is not carrying an object, and the selected object place/owner has an object
        if (!_isCarrying && _selectedOwner != null && _selectedOwner.HasGameObject()) {
            //_selectedOwner.Interact(this);
            Debug.Log("Object picked");
            //SetGameObject(_selectedOwner.GetGameObject());
            //_gameObject.SetObjectParent(this);
            _selectedOwner.GetGameObject().SetObjectParent(this);
            justPickedUp = true;
        }
        
        if (justPickedUp)
        {
            _isCarrying = true;
        } else if (justDropped)
        {
            _isCarrying = false;
        }

    }
    
    private void Update() {
        if (!gameManager.isPaused) {
            //SwingSword();
            if (!_isSpeaking)
                HandleMovement();
            HandleInteractions();
        }
    }

    public bool IsMoving() {
        return _isMoving;
    }
    
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.35f;
        float playerHeight = 1f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove) {
            // Cannot move towards moveDir
            
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = (moveDirection.x < -.5f || moveDirection.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                // Can move only on the X
                moveDirection = moveDirX;
            } else {
                // Cannot move only on the X
                // Attempt to move only on Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = (moveDirection.z < -.5f || moveDirection.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    // Can move only on the Z
                    moveDirection = moveDirZ;
                } // else Cannot move in any direction
            }
        }
        // Can only move it the player is not engaged in conversation
        //canMove = canMove && !is;
        if (canMove) {
            transform.position += moveDirection * moveDistance;
        }

        _isMoving = (moveDirection != Vector3.zero) && !_isSpeaking;

        float rotateSpeed = 10f;
        // Smooth transition
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteractions()
    {
        // Searching for possible ObjectOwner for pick up or drop.
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }

        float maximumInteractDistance = 2f;
        Vector3 characterPosition = transform.position;
        //Ignore the y offset of the cahracter
        Vector3 fixedCharacterPosition = new Vector3(characterPosition.x,0,characterPosition.z);
        // float radius = 0.5f;
        if (Physics.Raycast(fixedCharacterPosition,_lastInteractDir, out RaycastHit raycastHit, maximumInteractDistance))
        {
            ObjectOwnerPillar objOwner = raycastHit.transform.GetComponent<ObjectOwnerPillar>();
            if (objOwner != null)
            {
                if (objOwner != (ObjectOwnerPillar)_selectedOwner)
                {
                    UImanager.ShowInteractElement();
                    SetSelectedObjectOwner(objOwner);
                }
            }
            else
            {
                //Debug.Log("No hit - object owner is null");
                UImanager.HideInteractElement();
                SetSelectedObjectOwner(null);
            }
        }
        else
        {
            //Debug.Log("No hit- no object");
            UImanager.HideInteractElement();
            SetSelectedObjectOwner(null);
        }
    }
    
    private void SetSelectedObjectOwner(IObjectOwner so)
    {
        this._selectedOwner = so;
    }

    public void IsSpeaking()
    {
        camera.ZoomIn();
        _isMoving = false;
        _isSpeaking = true;
    }
    public void StoppedSpeaking()
    {
        camera.ZoomOut();
        _isSpeaking = false;
    }
    
    // Implementing IObjectOwner Interface:
    
    public Transform GetObjectSetpoint()
    {
        return objectFloatingHoldPoint;
    }

    public void SetGameObject(GameObjectPickable gameObjectPickable)
    {
        _gameObject = gameObjectPickable;
    }

    public GameObjectPickable GetGameObject()
    {
        return _gameObject;
    }

    public void ClearGameObject()
    {
        _gameObject = null;
    }

    public bool HasGameObject()
    {
        return (_gameObject != null);
    }
}