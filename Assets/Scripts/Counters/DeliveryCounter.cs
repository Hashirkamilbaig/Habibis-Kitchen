using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter{

	public static DeliveryCounter Instance {get; private set;}

	private void Awake() {
		Instance = this;
	}
    public override void Interact(Player player)
    {
			if(player.HasKitchenObject()){
				if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
					//this means that it only destroys if its a plate
					DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
					
					player.GetKitchenObject().DestroySelf();
				}
			}
    }
}
