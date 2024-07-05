using System;
using UnityEngine;

namespace Level3.scripts
{
    public class Player : MonoBehaviour
    {
        // Player Singleton pattern
        public static Player Instance { get; private set; }
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private HealthBarManager hbManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private float moveSpeed;
        private const float increasedSpeed = 12f;
        private const float reducedSpeed = 3f;
        private const float defaultSpeed = 7f;
        [SerializeField] private GameInput gameInput;
        public float attackRange = 2.9f;
        //[SerializeField] private UImanager UImanager;
        private Rigidbody rb;
        private const float COOLDOWN_TIME = 1f;
        private const float ATTACK_COOLDOWN_TIME = 1.5f;
        private const float MEAT_COOLDOWN_TIME = 6f;
        private float cooldownTimer = 0f;
        private float attackCooldownTimer = 0f;
        private float meatCooldownTimer = 0f;
        
        private bool _isMoving;

        private void Awake()
        {
            Instance = this;
        }

        // For event listeners
        private void Start()
        {
            gameInput.OnInteractAction += GameInput_OnInteractAction;
            ResetSpeed();
        }
        

        private void Update()
        {
            if (!gameManager.isPaused) {
                // Update cooldown timer
                if (cooldownTimer > 0)
                    cooldownTimer -= Time.deltaTime;
                if (attackCooldownTimer > 0)
                    attackCooldownTimer -= Time.deltaTime;
                if (meatCooldownTimer > 0)
                    meatCooldownTimer -= Time.deltaTime;
                else
                    ResetSpeed();
                HandleMovement();
            }
        }

        public bool IsMoving()
        {
            return _isMoving;
        }

        private void GameInput_OnInteractAction(object sender, System.EventArgs e)
        { 
            //var ss = gameInput.OnSwingSword();
            if (attackCooldownTimer <= 0)
            {
                RaycastHit hit;
                // Instead of initial raycast, better for diagonals
                //float radius = 0.7f;
                if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
                {
                    NPC npc = hit.collider.GetComponent<NPC>();
                    if (npc != null)
                    {
                        audioManager.PlayOuch();
                        npc.TakeDamage();
                    }
                    else
                    {
                        IBooster booster = hit.collider.GetComponent<IBooster>();
                        if (booster != null)
                        {
                            string boosterClassName = booster.GetType().Name;
                            ApplyBooster(boosterClassName);
                            booster.Consume();
                            
                        }
                    }
                }

                //audioManager.PlaySuccessSound();
                attackCooldownTimer = ATTACK_COOLDOWN_TIME;
            }
            //Debug.Log(ss);
        }

        public void ApplyBooster(string boosterName)
        {
            if (boosterName.Equals("Heart"))
            {
                hbManager.Heal();
                audioManager.PlaySuccessSound();
            }
            else if (boosterName.Equals("Meat"))
            {
                hbManager.ShowMeatBooster();
                audioManager.PlaySuccessSound();
                moveSpeed = increasedSpeed;
                meatCooldownTimer = MEAT_COOLDOWN_TIME;
            } 
            else if (boosterName.Equals("BadMeat"))
            {
                hbManager.ShowRottenMeatCanvas();
                audioManager.PlayErrorSound();
                moveSpeed = reducedSpeed;
                meatCooldownTimer = MEAT_COOLDOWN_TIME;
            }
        }

        private void ResetSpeed()
        {
            hbManager.HideMeatBooster();
            hbManager.HideRottenMeatCanvas();
            moveSpeed = defaultSpeed;
        }
        private void HandleMovement()
        {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
            float moveDistance = moveSpeed * Time.deltaTime;
            float playerRadius = 0.35f;
            float playerHeight = 1f;
            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirection, moveDistance);

            if (!canMove)
            {
                // Cannot move towards moveDir

                // Attempt only X movement
                Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
                canMove = (moveDirection.x < -.5f || moveDirection.x > +.5f) && !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                if (canMove)
                {
                    // Can move only on the X
                    moveDirection = moveDirX;
                }
                else
                {
                    // Cannot move only on the X
                    // Attempt to move only on Z
                    Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                    canMove = (moveDirection.z < -.5f || moveDirection.z > +.5f) && !Physics.CapsuleCast(
                        transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ,
                        moveDistance);

                    if (canMove)
                    {
                        // Can move only on the Z
                        moveDirection = moveDirZ;
                    } // else Cannot move in any direction
                }
            }

            // Can only move it the player is not engaged in conversation
            //canMove = canMove;
            if (canMove)
            {
                transform.position += moveDirection * moveDistance;
            }
            _isMoving = (moveDirection != Vector3.zero) ;

            float rotateSpeed = 10f;
            // Smooth transition
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }

        public void TakeDamage()
        {
            if (cooldownTimer <= 0)
            {
                audioManager.PlayErrorSound();
                hbManager.TakeDamage();
                cooldownTimer = COOLDOWN_TIME;
            }
        }

        public bool Alive()
        {
            return hbManager.Alive();
        }
        
        //https://gamedev.stackexchange.com/questions/87584/how-do-i-tell-if-my-enemy-can-see-my-player
    }
}