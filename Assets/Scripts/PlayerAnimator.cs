using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player player;
    private Animator animator;
    private readonly string IS_WALKING = "IsWalking";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update() {
        animator.SetBool(IS_WALKING, player.isWalking);
    }
}
