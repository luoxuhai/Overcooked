using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManger : MonoBehaviour
{

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
        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count > waitingRecipeMax)
            {
                RecipeSO recipeSO = recipeSOList.recipeSOList[Random.Range(0, recipeSOList.recipeSOList.Count)];
                waitingRecipeSOList.Add(recipeSO);
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
                return;
            } else {
                // 交付错误
            }
        }
    }
}
