using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pillars;
    [SerializeField] private GameObject npcObject;
    [SerializeField] private GameObject chessboardObject;
    [SerializeField] private PillarsPositionList positionList;
    [SerializeField] private NpcPositionList npcPositionList;
    [SerializeField] private ChessboardPositionList chessboardPositionList;
    [SerializeField] private RockPositionList rockPositionList;
    [SerializeField] private List<GameObject> rocksToSpawn;
    [SerializeField] private List<ObjectOwnerPillar> objectOwners; 
    [SerializeField] private List<GameObjectPickable> pickablesToSpawn; 
    private List<Vector3> _usedPositions = new List<Vector3>();
    private List<Vector3> _aggregatedPositions = new List<Vector3>();
    private List<ObjectOwnerPillar> _availableOwners = new List<ObjectOwnerPillar>();
    
    void Start()
    {
        // TODO refactor code with a common function
        // Create a list that contains all the lists (except for rocks) in order to see which positions remain unused
        _aggregatedPositions.AddRange(positionList.positions);
        _aggregatedPositions.AddRange(npcPositionList.positions);
        _aggregatedPositions.AddRange(chessboardPositionList.positions);
        
        // Randomly select a position from the list and instantiate the object
        // Pillars Spawner
        Vector3 randomPositionPillars = positionList.positions[Random.Range(0, positionList.positions.Length)];
        pillars.transform.position = new Vector3(randomPositionPillars.x, pillars.transform.position.y, randomPositionPillars.z);
        //Instantiate(pillarsPrefab, randomPositionPillars, Quaternion.identity);
        _usedPositions.Add(randomPositionPillars);
        // NPC spawner
        Vector3 randomPositionNPC = npcPositionList.positions[Random.Range(0, npcPositionList.positions.Length)];
        npcObject.transform.position = new Vector3(randomPositionNPC.x, npcObject.transform.position.y, randomPositionNPC.z);
        _usedPositions.Add(randomPositionNPC);
        // Chessboard spawner
        Vector3 randomPositionChessboard = chessboardPositionList.positions[Random.Range(0, chessboardPositionList.positions.Length)];
        chessboardObject.transform.position = new Vector3(randomPositionChessboard.x, 0, randomPositionChessboard.z);
        _usedPositions.Add(randomPositionChessboard);
        
        // Keep only unused position in aggregate
        foreach (Vector3 usedPos in _usedPositions)
        {
            _aggregatedPositions.Remove(usedPos);
        }
        
        // Spawn rocks on the given positions and the unused positions
        int unusedIndexPosition = 0;
        int rockPositionListIndex = 0;
        
        foreach (GameObject rock in rocksToSpawn)
        {
            // Use positions from rockPositionListSo first
            if (rockPositionListIndex < rockPositionList.positions.Length)
            {
                //Debug.Log("should print");
                rock.transform.position = new Vector3(rockPositionList.positions[rockPositionListIndex].x, rock.transform.position.y, rockPositionList.positions[rockPositionListIndex].z);
                rockPositionListIndex++;
            }
            else
            {
                Vector3 rockPosition = _aggregatedPositions[unusedIndexPosition];
                unusedIndexPosition++;
                rock.transform.position = new Vector3(rockPosition.x, rock.transform.position.y, rockPosition.z);
            }
        }
        _availableOwners = new List<ObjectOwnerPillar>(objectOwners);
        
        // Give all the pickable objects to some owner
        foreach (GameObjectPickable pickable in pickablesToSpawn)
        {
            if (pickable == null)
            {
                Debug.LogWarning("Pickable object is null. Skipping.");
                continue;
            }
            //Debug.Log("Pickable selected");
            // Will never occur in our case, but good to have
            if (_availableOwners.Count == 0)
            {
                Debug.LogWarning("Not enough owners for all pickable objects");
                break;
            }
            // Get a random parent from the available owners
            int randomIndex = Random.Range(0, _availableOwners.Count);
            ObjectOwnerPillar randomParent = _availableOwners[randomIndex];
            
            // Set the parent of the pickable object
            pickable.SetObjectParent(randomParent);

            // Remove parent from list/ temp list so he cannot be given another object
            _availableOwners.RemoveAt(randomIndex);
        }
    }
}
