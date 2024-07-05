using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private const string IS_MOVING = "isMoving";
    [SerializeField] private Player player; 
    private void Awake()
    {
        // Get reference to animator controller
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool( "isMoving", player.IsMoving());
    }
}

