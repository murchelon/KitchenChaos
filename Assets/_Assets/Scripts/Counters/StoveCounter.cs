using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgressChangedEventsArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State argState;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;


    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {

        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Frying:

                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            argState = state
                        });

                    }
                    break;

                case State.Fried:

                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer >= burningRecipeSO.burningTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            argState = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;

                case State.Burned:
                    break;

                default:
                    break;
            }

            Debug.Log(state);
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());


                    fryingTimer = 0f;
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        argState = state
                    });
            
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    argState = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOwithInput(inputKitchenObjectSO);

        return fryingRecipeSO != null;
    }


    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOwithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }


    private FryingRecipeSO GetFryingRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }


    private BurningRecipeSO GetBurningRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

        return null;
    }

    
}
