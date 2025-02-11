using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlateKitchenObject : KitchenObject{

	public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
	public class OnIngredientAddedEventArgs : EventArgs{
		public KitchenObjectSO kitchenObjectSO;
	}
	[SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
	private List<KitchenObjectSO> kitchenObjectSOList;

	private void Awake(){
		kitchenObjectSOList = new List<KitchenObjectSO>();
	}
	public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO){
		if(!validKitchenObjectSOList.Contains(kitchenObjectSO)){
			return false;
			//not a valid kitchen object to be added to the plate
		}

		if (kitchenObjectSOList.Contains(kitchenObjectSO)){
			//this means that it already has the kitchen object
			return false;
		}else {
			kitchenObjectSOList.Add(kitchenObjectSO);
			OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{
				kitchenObjectSO = kitchenObjectSO
			});

			return true;
		}
	}

	public List<KitchenObjectSO> GetKitchenObjectSOList(){
		return kitchenObjectSOList;
	}
}
