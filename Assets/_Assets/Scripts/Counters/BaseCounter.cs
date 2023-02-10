using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;


    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter Interact ! nao deve ser chamado nunca. esta aqui para termos o metodo descrito, como virtual, deixando a implementacao para o caller");
    }

    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("BaseCounter InteractAlternate ! nao deve ser chamado nunca. esta aqui para termos o metodo descrito, como virtual, deixando a implementacao para o caller");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject _kitchenObject)
    {
        this.kitchenObject = _kitchenObject;
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

