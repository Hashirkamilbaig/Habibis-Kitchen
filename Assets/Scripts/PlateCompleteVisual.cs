using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable] public struct KitchenObjectSO_GameObject{
      public KitchenObjectSO kitchenObjectSO;
      public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;

    private void Start(){
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList){
          kitchenObjectSO_GameObject.gameObject.SetActive(false);
      }

    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e){
      foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList){
        if(kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO){
          kitchenObjectSO_GameObject.gameObject.SetActive(true);
        }
      }
    }
}
