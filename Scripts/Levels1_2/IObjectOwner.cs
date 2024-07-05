using UnityEngine;

// This interface is like a contract
// Everyone who can pick up objects must follow it 
public interface IObjectOwner {

    Transform GetObjectSetpoint();

    void SetGameObject(GameObjectPickable gameObject);

    GameObjectPickable GetGameObject();

    void ClearGameObject();

    bool HasGameObject();
    
}