using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private const float FINAL_CHEST_PROXIMITY_RADIUS = 1.3f;  
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private UImanager UImanager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private TreasureChestOpen finalChest;
    [SerializeField] private Player player;
    [SerializeField] private NPCInteract npc;
    [SerializeField] private ObjectOwnerPillar pillarL;
    [SerializeField] private ObjectOwnerPillar pillarM;
    [SerializeField] private ObjectOwnerPillar pillarR;
    [SerializeField] private ObjectOwnerPillar chessboard;
    [SerializeField] private Button mainMenuButton;
    private const string _correctObjectNameL = "Cheese";
    private const string _correctObjectNameM = "Fruit";
    private const string _correctObjectNameR = "Wine";
    private const string _correctChessboardObjectName = "Chess Bishop White(Clone)";
    private GameObjectPickable _objectL;
    private GameObjectPickable _objectM;
    private GameObjectPickable _objectR;
    private GameObjectPickable _objectChessboard;
    private bool _correctObjectL;
    private bool _correctObjectM;
    private bool _correctObjectR;
    private bool _correctObjectChessboard;
    private bool _pillarsCondition = false;
    private bool _chessCondition = false;
    public bool isPaused;

    private bool CheckPillarObject(string correctName, GameObjectPickable pickableObject){
        return pickableObject.gameObject.name.Equals(correctName);
    }

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        //mainMenuButton.onClick.AddListener(MainMenu);
        //mainMenuButton.interactable = true;
        //UImanager.ShowTimer();
    }
    // Update is called once per frame
    void Update() {
        CheckPause();
        // Complete the pillars section to continue
        if (!_pillarsCondition)
        {
            _objectL = pillarL.GetGameObject();
            _objectM = pillarM.GetGameObject();
            _objectR = pillarR.GetGameObject();
            // Inverted if to reduce nesting 
            if (_objectL is null || _objectM is null || _objectR is null) return;
            _correctObjectL = CheckPillarObject(_correctObjectNameL, _objectL);
            _correctObjectM = CheckPillarObject(_correctObjectNameM, _objectM);
            _correctObjectR = CheckPillarObject(_correctObjectNameR, _objectR);
            // CHeck if all 3 pillars have the correct objects
            if (_correctObjectL && _correctObjectM && _correctObjectR)
            {
                _pillarsCondition = true;
                // Play success sound
                audioManager.PlaySuccessSound();
                // Move the dialogue further
                dialogueManager.PillarsComplete();
                // Make the NPC give the pawn
                npc.CreateInvisibleBishopOwner();
            }
        }
        // Complete the chess section to continue
        else if (!_chessCondition)
        {
            //Debug.Log("Here i am");
            _objectChessboard = chessboard.GetGameObject();
            // Inverted if to reduce nesting 
            if (_objectChessboard is null) return;
            _correctObjectChessboard = CheckPillarObject(_correctChessboardObjectName, _objectChessboard);
            // CHeck if all 3 pillars have the correct objects
            if (_correctObjectChessboard)
            {
                _chessCondition = true;
                // Play success sound
                audioManager.PlaySuccessSound();
                // Move the dialogue further
                dialogueManager.ChessComplete();
                //Open final chest
                finalChest.OpenChest();
            }
        }
        // Everything unlocked, go to the chest to finish the game
        else
        {
            float distanceToPlayer = Vector3.Distance(finalChest.gameObject.transform.position, player.gameObject.transform.position);
            if (distanceToPlayer <= FINAL_CHEST_PROXIMITY_RADIUS)
            {
                UImanager.ShowInteractElement();
                if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Space))
                {
                    UImanager.HideInteractElement();
                    GameWin();
                }
            }
            else
            {
                UImanager.HideInteractElement();
            }
        }
    }

    private void GameWin()
    {
        GameEnd();
        UImanager.FadeToWhite("You win !");
    }

    public void GameOver()
    {
        GameEnd();
        UImanager.FadeToBlack("Game Over...");
    }

    private void GameEnd()
    {
        UImanager.PauseTimer();
        UImanager.HideInteractElement();
        UImanager.KillInteractElement();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CheckPause() {
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = true;
            UImanager.PauseTimer();
            UImanager.ShowPauseMenu();
        } else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = false;
            UImanager.ContinueTimer();
            UImanager.HidePauseMenu();
        }
    }

    public void Resume() {
        if (isPaused) {
            isPaused = false;
            UImanager.ContinueTimer();
            UImanager.HidePauseMenu();
        }
    }
}
