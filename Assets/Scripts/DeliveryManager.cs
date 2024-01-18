using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManger : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManger Instance { get; private set; }

    [SerializeField] private RecipeSOList recipeSOList;
    private List<RecipeSO> waitingRecipeSOList = new();
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO recipeSO = recipeSOList.recipeSOList[UnityEngine.Random.Range(0, recipeSOList.recipeSOList.Count)];
                waitingRecipeSOList.Add(recipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO recipeSO = waitingRecipeSOList[i];

            if (recipeSO.kitchenObjectSOList.Count != plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                continue;
            }

            bool plateContentMatchesRecipe = true;
            foreach (KitchenObjectSO recipeKitchenObjectSO in recipeSO.kitchenObjectSOList)
            {
                bool ingredientFound = false;
                foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                {
                    if (recipeKitchenObjectSO == kitchenObjectSO)
                    {
                        ingredientFound = true;
                        break;
                    }
                }

                if (!ingredientFound)
                {
                    plateContentMatchesRecipe = false;
                    break;
                }
            }

            if (plateContentMatchesRecipe)
            {
                waitingRecipeSOList.RemoveAt(i);
                OnCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }
}
