using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player){
        if(!HasKitchenObject()){
            //there is no kitchenObject here
            if(player.HasKitchenObject()){
                //This means player is carrying some object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            //there is a KitchenObject here
            if(player.HasKitchenObject()){
                //this is when player is not carrying any object so do nothing
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                    // then player is holding a plate and we want to add something on top of the plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }
                } else{
                    // player not carrying plate but something else
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                        //this is when Kitchen is holding a plate
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else{
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
