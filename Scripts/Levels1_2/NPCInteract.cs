using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour 
{
    private const float INTERACT_COOLDOWN = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private Player playerObject;
    [SerializeField] private UImanager UImanager;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject invisibleOwnerPrefab;
    [SerializeField] private GameObject bishopPrefab;
    [SerializeField] private Transform bishopOwnerSpawnPoint;
    // Player Detection
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float rotationSpeed = 2f; 
    private float _nextInteractTime = 0f;
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        dialogueManager.HideDialogueUI();
    }
    
    // VS auto generates these method signatures for event handling
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        // Check if the cooldown period has passed
        if (Time.time < _nextInteractTime)
        {
            return; // Exit the method if still in cooldown period
        }
        // Set the next allowed interact time
        
        _nextInteractTime = Time.time + INTERACT_COOLDOWN;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!(distanceToPlayer <= detectionRadius)) return;
        StartCoroutine(HandlePlayerInteraction());

    }
    private IEnumerator HandlePlayerInteraction()
    {
        // Do talk action, show UI
        UImanager.HideInteractElement();
        dialogueManager.dialogueProgressSignal = true;
        dialogueManager.ShowDialogueUI();
        playerObject.IsSpeaking();

        // Wait until the exit condition is met
        yield return new WaitUntil(() => dialogueManager.ExitCondition());
        yield return new WaitForSeconds(3f);
        dialogueManager.dialogueProgressSignal = false;
        dialogueManager.SetExitCondition(false);
        playerObject.StoppedSpeaking();
        dialogueManager.HideDialogueUI();
    }
    
    void Update()
    {
        // Cheat used for debugging
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    CreateInvisibleBishopOwner();
        //}
        // Calculate the distance between the NPC and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            FacePlayer();
            UImanager.ShowInteractElement();
        }
    }

    public void CreateInvisibleBishopOwner()
    {
        GameObject invisibleOwner = Instantiate(invisibleOwnerPrefab, new Vector3(transform.position.x,transform.position.y,(transform.position.z-2)), Quaternion.identity);
        GameObject bishop  =Instantiate(bishopPrefab, new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
        if (bishop.TryGetComponent<GameObjectPickable>(out GameObjectPickable pickableComponent) &&
            invisibleOwner.TryGetComponent<IObjectOwner>(out IObjectOwner objectOwnerComponent))
        {
            pickableComponent.SetObjectParent(objectOwnerComponent);
        }

        invisibleOwner.transform.position = new Vector3(bishopOwnerSpawnPoint.position.x,0,bishopOwnerSpawnPoint.position.z);
        invisibleOwner.transform.rotation = Quaternion.Euler(270, invisibleOwner.transform.rotation.eulerAngles.y, invisibleOwner.transform.rotation.eulerAngles.z);
        
    }
    private void FacePlayer()
    {
        // Calculate the rotation needed to face the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

}

