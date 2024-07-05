using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    public class NPC : MonoBehaviour
    {
        // Give empty points as patch
        [SerializeField] private Transform[] points;
        [SerializeField] private float npcSpeed = 0.7f;
        [SerializeField] private float chaseSpeed = 2.5f;
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private float chaseThreshold = 10f;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Player player;
        
        private bool _isAlive;
        private int _destPoint = 0;
        private bool _isChasing = false;
        private Vector3 _lastPathPosition;
        private bool _wounded; // equivalent to 2 hp
        void Start()
        {
            _isAlive = true;
            if (points.Length == 0)
            {
                Debug.LogError("No patrol points assigned.");
                return;
            }

            transform.position = points[_destPoint].position;
            ToNextPoint();
        }

        public void TakeDamage()
        {
            if (_wounded)
                Kill();
            else
                _wounded = true;
        }
        
        // Method to "kill" the enemy
        private void Kill()
        {
            _isAlive = false;
            DestroyImmediate(gameObject);
            //GetComponent<Renderer>().enabled = false;
        }

        // Method to check if the enemy is alive
        public bool Alive()
        {
            return _isAlive;
        }
        
        private void ToNextPoint()
        {
            if (points.Length == 0)
                return;

            // Modulo for cyclical traversal of a list
            _destPoint = (_destPoint + 1) % points.Length;
        }

        public void Update()
        {
            if (!gameManager.isPaused) {
                if (!_isAlive || points.Length == 0)
                    return;

                if (_isChasing) {
                    ChasePlayer();
                }
                // Patrol
                else {
                    Vector3 target = points[_destPoint].position;
                    // Atempt to move towards target
                    Vector3 moveDirection = target - transform.position;
                    transform.position += moveDirection.normalized * (npcSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, target) < 0.2f) {
                        ToNextPoint();
                    }
                }

                DetectPlayer();
            }

        }
        
        private void ChasePlayer()
        {
            Vector3 target = player.gameObject.transform.position;
            Vector3 moveDirection = target - transform.position;
            transform.position += moveDirection.normalized * (chaseSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                player.GetComponent<Player>().TakeDamage();
                _isChasing = false; // Stop chasing after damaging the player
            }
            if (Vector3.Distance(transform.position, _lastPathPosition) > chaseThreshold)
            {
                _isChasing = false; // Stop chasing if too far from patrol path
            }
        }

        private void DetectPlayer()
        {
            Vector3 target = player.gameObject.transform.position;
            if (Vector3.Distance(transform.position, target) <= detectionRadius)
            {
                _isChasing = true;
            }
        }
    }
}