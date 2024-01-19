using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { private set; get; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    public bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Player instance");
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    void Update()
    {
        HandleMove();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVector();
        Vector3 moveVec = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveVec != Vector3.zero)
        {
            lastInteractDir = moveVec;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out BaseCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMove()
    {
        Vector2 inputVector = gameInput.GetMovementVector();
        Vector3 moveVec = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        float playerRadius = .7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveVec, moveDistance);

        if (canMove)
        {
            transform.position += moveVec * moveDistance;
        }

        isWalking = moveVec != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveVec, Time.deltaTime * 10f);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
