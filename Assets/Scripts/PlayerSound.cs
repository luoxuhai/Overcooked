using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (player.isWalking)
            {
                SoundManager.Instance.PlayFootstepSound(player.transform.position, 1f);
             }
        }
    }
}
