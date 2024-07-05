using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOwnerPillar : MonoBehaviour, IObjectOwner
{
    [SerializeField] private GameObjectPickable ownedObject;
    [SerializeField] private Transform objectHoldPosition;

    // If an object is given in the start by the editor, then initialize
    public void Awake()
    {
        if (ownedObject != null)
        {
            ownedObject.SetObjectParent(this);
        }
            
    }
    
    // IObjectOwner interface implementation
    public Transform GetObjectSetpoint()
    {
        return objectHoldPosition;
    }
    public void SetGameObject(GameObjectPickable gameObjectPickable)
    {
        ownedObject = gameObjectPickable;
    }

    public GameObjectPickable GetGameObject()
    {
        return ownedObject;
    }

    public void ClearGameObject()
    {
        ownedObject = null;
    }

    public bool HasGameObject()
    {
        return (ownedObject!=null);
    }
}
