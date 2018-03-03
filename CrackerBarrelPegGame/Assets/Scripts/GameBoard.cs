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

    void NextLevel()
    {
        
        CreateLevelText();
        CreateButton();
    }

    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private GameObject Message;

    void CreateButton()
    {
        GameObject init = (GameObject)Instantiate(Button, new Vector3(0,-0.5f,0), Quaternion.identity);
    }

    void CreateLevelText()
    {
        GameObject init = (GameObject)Instantiate(Message, new Vector3(0,0.5f,0), Quaternion.identity);
    }
}
