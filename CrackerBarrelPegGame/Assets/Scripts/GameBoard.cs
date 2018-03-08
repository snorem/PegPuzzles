using UnityEngine;
using System.Collections;

// Solutions for levels 1 & 2 can be found at http://www.gibell.net/pegsolitaire/CBSolutions.html
// SOlution for level 3 can be found at http://www.chessandpoker.com/peg-solitaire-solution.html
public class GameBoard : MonoBehaviour
{
    public static GameBoard instance;

    [SerializeField] GameObject[] PegHoles;
    public GameObject SelectedPiece;

    byte TotalMoves;
    public bool ActiveAnimation;
    public bool LevelComplete;
    
    void Awake()
    {
        // Making the board globally accessable, without keeping it through scenes
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        TotalMoves = 0;
        SelectedPiece = null;
        ActiveAnimation = false;
        LevelComplete = false;

        // To complete a level n-1 moves are made, where n is the peg count
        foreach (GameObject peg in PegHoles)
            if (peg.GetComponent<Renderer>().enabled)
                TotalMoves += 1;
    }

    bool AvailableMoves()
    {
        foreach (GameObject peg in PegHoles)
        {
            if (peg.GetComponent<GamePiece>().CanJump()) return true;
        }
        return false;
    }

    public void MovedPiece()
    {
        TotalMoves -= 1;
        if (TotalMoves == 1)
        {
            LevelComplete = true;
            NextLevel();
        }
        else if (!AvailableMoves()) NextLevel();
    }

    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private GameObject Message;

    void NextLevel()
    {
        CreateObject(Button, -0.5f);
        CreateObject(Message, 0.5f);
    }

    void CreateObject(GameObject img, float deltaY)
    {
        GameObject init = (GameObject)Instantiate(img, new Vector3(0, deltaY, 0), Quaternion.identity);
    }
}
