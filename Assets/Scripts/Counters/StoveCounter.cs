using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{

		public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
		public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
		public class OnStateChangedEventArgs : EventArgs {
				public State state;
		}

		public enum State {
				Idle,
				Frying,
				Fried,
				Burned,
		}


		private State state;
		[SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
		[SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
		
		private float fryingTimer;
		private FryingRecipeSO fryingRecipeSO;
		private float burningTimer;
		private BurningRecipeSO burningRecipeSO;

		private void Start(){
				state = State.Idle;
		}

		private void Update(){
				if(HasKitchenObject()){
						switch (state){
								case State.Idle:
										break;
								case State.Frying:
										fryingTimer += Time.deltaTime;

										OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
												progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
										});
										
										if(fryingTimer > fryingRecipeSO.fryingTimerMax){
												// this means its fried
												GetKitchenObject().DestroySelf();
												KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
												state = State.Fried;
												burningTimer = 0f;
												
												burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
												OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
														state = state
												});

										}
										break;
								case State.Fried:
										burningTimer += Time.deltaTime;

										OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
												progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
										});
										
										if(burningTimer > burningRecipeSO.burningTimerMax){
												// this means its fried
												GetKitchenObject().DestroySelf();
												KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
												state = State.Burned;

												OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
														state = state
												});

												OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
														progressNormalized = 0f
												});
										}
										break;
								case State.Burned:
										break;
						}
				}
		}

		public override void Interact(Player player)
		{
				if(!HasKitchenObject()){
						//there is no kitchenObject here
						if(player.HasKitchenObject()){
								//This means player is carrying some object
								if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
										//When player is carrying something which can be fried
										player.GetKitchenObject().SetKitchenObjectParent(this);

										fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

										state = State.Frying;
										fryingTimer = 0f;
										OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
												state = state
										});

										OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
												progressNormalized = (float)fryingTimer  / fryingRecipeSO.fryingTimerMax
										});
								}
						}
				} else {
						//there is a KitchenObject here
						if(player.HasKitchenObject()){
								//this is when player is not carrying any object so do nothing
								if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
										// then player is holding a plate and we want to add something on top of the plate
										if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
												GetKitchenObject().DestroySelf();

												state = State.Idle;
												OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
														state = state
												});

												OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
														progressNormalized = 0f
												});
										}
								}
						} else{
								GetKitchenObject().SetKitchenObjectParent(player);

								state = State.Idle;
								OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
										state = state
								});

								OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
										progressNormalized = 0f
								});
						}
				}
		}

		private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO){
				FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
				return cuttingRecipeSO != null;
		}

		private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
				FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
				if (fryingRecipeSO != null){
						return fryingRecipeSO.output;
				}else {
						return null;
				}
		}

		private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
				foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray){
						if(fryingRecipeSO.input == inputKitchenObjectSO){
								return fryingRecipeSO;
						}
				}
				return null;
		}

		private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
				foreach(BurningRecipeSO burningRecipeSO in burningRecipeSOArray){
						if(burningRecipeSO.input == inputKitchenObjectSO){
								return burningRecipeSO;
						}
				}
				return null;
		}
}
