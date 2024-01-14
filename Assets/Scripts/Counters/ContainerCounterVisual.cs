using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
