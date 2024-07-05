using UnityEngine;

public class GameObjectPickable : MonoBehaviour {
    
    //[SerializeField] private GameObjectSO gameObjectSO;
    
    private IObjectOwner objectParent;
    
    public void SetObjectParent(IObjectOwner objectParent) {
        if (this.objectParent != null) {
            this.objectParent.ClearGameObject();
        }

        this.objectParent = objectParent;

        objectParent.SetGameObject(this);
        // Each parent has a specific point in which they place the object
        transform.parent = objectParent.GetObjectSetpoint();
        // In reference to the new transform the object has 0 deviation in each axis
        transform.localPosition = Vector3.zero;
    }
    
    // Get the IObjectOwner that holds the object
    public IObjectOwner GetObjectParent() {
        return objectParent;
    }

    // Destroy self after cleaning dependancies
    public void DestroySelf() {
        objectParent.ClearGameObject();
        Destroy(gameObject);
    }

}