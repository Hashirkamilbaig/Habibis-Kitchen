using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

	public event EventHandler OnRecipeSpawned;
	public event EventHandler OnRecipeCompleted;
	public event EventHandler OnRecipeSuccess;
	public event EventHandler OnRecipeFailed;
	public static DeliveryManager Instance {get; private set;}
	[SerializeField] private RecipeListSO recipeListSO;
	private List<RecipeSO> waitingRecipeSOList;
	private float spawnRecipeTimer;
	private float spawnRecipeTimerMax = 4f;
	private int waitingRecipesMax = 4;
	private int successfulRecipesAmount;

	private void Awake() {
		Instance = this;
		waitingRecipeSOList = new List<RecipeSO>();
	}
	
	private void Update(){
		spawnRecipeTimer -= Time.deltaTime;
		if(spawnRecipeTimer <= 0f){
			spawnRecipeTimer = spawnRecipeTimerMax;

			if(waitingRecipeSOList.Count < waitingRecipesMax){
					RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
					Debug.Log(waitingRecipeSO.recipeName);
					waitingRecipeSOList.Add(waitingRecipeSO);

					OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public void DeliverRecipe(PlateKitchenObject plateKitchenObject){
		for (int i = 0; i< waitingRecipeSOList.Count; i++){
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

			if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count){
				// Has same number of ingredients
				bool plateContentsMatchesRecipe = true;
				foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList){
					// Going through all the ingredients in the Recipe
					bool ingredientFound = false;
					foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()){
						//going through all the ingredients currently in the plate to deliver
						if(plateKitchenObjectSO == recipeKitchenObjectSO){
							//this means ingredient is found
							ingredientFound = true;
							break;
						}
					}
					if(!ingredientFound){
						plateContentsMatchesRecipe = false;
					}
				}
				if(plateContentsMatchesRecipe){
					//player delivered the recipe
					Debug.Log("Player successfully delivered the ordered");
					waitingRecipeSOList.RemoveAt(i);
					successfulRecipesAmount++;

					OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
					OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
					return;
				}
			}
		}

		//player did not deliver the right recipe
		OnRecipeFailed?.Invoke(this, EventArgs.Empty);
		
	}

	public List<RecipeSO> GetWaitingRecipeSOList(){
		return waitingRecipeSOList;
	}

	public int GetSuccessfulRecipesAmount(){
		return successfulRecipesAmount;
	}
}
