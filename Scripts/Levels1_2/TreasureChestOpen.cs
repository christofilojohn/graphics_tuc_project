using UnityEngine;
using System.Collections;

public class TreasureChestOpen : MonoBehaviour
{
    // Reference to the chest lid GameObject
    [SerializeField] private GameObject chestLid;
    
    private float zMovementDistance = 1.0f;
    private float yMovementDistance = -0.4f;
    private float moveDuration = 1.0f;
    private float pauseDuration = 0.5f;
    private bool _opened = false;
    
    void Update()
    {
        //Cheat for debugging purposes
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    StartCoroutine(OpenLid());
        //}
    }

    public bool IsOpen()
    {
        return _opened; 
    }
    public void OpenChest()
    {
        _opened = true;
        StartCoroutine(OpenLid());
    }
    
    private IEnumerator OpenLid()
    {
        Vector3 startPosition = chestLid.transform.localPosition;

        // Move on the z-axis
        Vector3 zTargetPosition = startPosition + new Vector3(0, 0, zMovementDistance);
        yield return MoveOverTime(chestLid, startPosition, zTargetPosition, moveDuration);

        // Short pause
        yield return new WaitForSeconds(pauseDuration);

        // Move on the y-axis
        Vector3 yTargetPosition = zTargetPosition + new Vector3(0, yMovementDistance, 0);
        yield return MoveOverTime(chestLid, zTargetPosition, yTargetPosition, moveDuration);
        
    }

    private IEnumerator MoveOverTime(GameObject obj, Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            obj.transform.localPosition = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        obj.transform.localPosition = end;
    }
}