using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour
{
    [SerializeField][Tooltip("Left-Side Peg")] GameObject L;
    [SerializeField][Tooltip("Right-Side Peg")] GameObject R;
    [SerializeField][Tooltip("Top-Side Peg")] GameObject T;
    [SerializeField][Tooltip("Bottom-Side Peg")] GameObject B;
    [SerializeField][Tooltip("Top-Left Peg")] GameObject TL;
    [SerializeField][Tooltip("Top-Right Peg")] GameObject TR;
    [SerializeField][Tooltip("Bottom-Left Peg")] GameObject BL;
    [SerializeField][Tooltip("Bottom-Right Peg")] GameObject BR; 

    // Checks to see if this piece can jump any of its neighboring pieces
    public bool CanJump()
    {      
        if (gameObject.GetComponent<Renderer>().enabled)
        {
            if (CanJump(L)) return true;
            if (CanJump(R)) return true;
            if (CanJump(T)) return true;
            if (CanJump(B)) return true;
            if (CanJump(TL)) return true;
            if (CanJump(TR)) return true;
            if (CanJump(BL)) return true;
            if (CanJump(BR)) return true;
        }
        return false;
    }

    // Checks to see if this piece can jump the supplied piece
    bool CanJump(GameObject j)
    {
        if (VerifyActivePiece(j))
            if(j.GetComponent<GamePiece>().CanBeJumped(gameObject)) 
                return true;

        return false;
    }

    // Returns true if this piece can be jumped by the given piece
    // Requires valid neighboring piece and an empty space opposite it to return true
    public bool CanBeJumped(GameObject p)
    {
        GameObject pegToCheck = GetOppositePiece(p);

        if (pegToCheck != null && pegToCheck.tag != "nullObj")
            return !VerifyActivePiece(pegToCheck);

        return false;
    }

    public bool VerifyActivePiece(GameObject p)
    {
        if (p != null) return p.GetComponent<Renderer>().enabled;
        return false;
    }

    public void ToggleVisible()
    {
        Renderer visible = gameObject.GetComponent<Renderer>();
        visible.enabled = !visible.enabled;

        Collider2D clickable = gameObject.GetComponent<Collider2D>();
        clickable.enabled = !clickable.enabled;
    }

    void ToggleSelect()
    {
        if (GameBoard.instance.SelectedPiece == null)
            GameBoard.instance.SelectedPiece = gameObject;
        else GameBoard.instance.SelectedPiece = null;

        ToggleGlow();
    }

    //I've set things up so that instead of creating and deleting objects on the fly,
    //  we can simply toggle visibility. Jumping a piece requires an "empty" board space
    //  on the opposite end of the piece to jump. So we toggle off the jumping piece and
    //  the piece to jump, and toggle on the empty space. Then set the empty space color
    //  to match the jumping piece's color to make it appear that the piece has moved.
    //If we wanted to use sprites instead of color-shaded pieces, we can change from
    //  a color swap to a sprite swap just as easily. 
    public void Jump(GameObject j)
    {
        if (CanJump(j))
        {
            ToggleVisible();
            j.GetComponent<GamePiece>().ToggleVisible();
            GamePiece landingSpace = j.GetComponent<GamePiece>().GetOppositePiece(gameObject).GetComponent<GamePiece>();
            landingSpace.ToggleVisible();
            landingSpace.ChangeColor(gameObject.GetComponent<SpriteRenderer>().color);
            GameBoard.instance.MovedPiece();
            ToggleSelect();       
        }
        else StartCoroutine(ShakeAnimation());
    }

    // Returns the piece opposite the given piece "p"
    public GameObject GetOppositePiece(GameObject p)
    {
        if (p == L) return R;
        else if (p == R) return L;
        else if (p == T) return B;
        else if (p == B) return T;
        else if (p == TL) return BR;
        else if (p == TR) return BL;
        else if (p == BL) return TR;
        else if (p == BR) return TL;

        return null;
    }

    // If no jumping piece selected, select this piece if it could jump another
    // If this piece is already selected, deselect it
    // If another piece is selected to jump, call Jump() on this piece
    void OnMouseDown()
    {
        if (!GameBoard.instance.ActiveAnimation) 
        {
            if (GameBoard.instance.SelectedPiece == null) 
            {
                if (CanJump()) ToggleSelect();
                else StartCoroutine(ShakeAnimation());
            }
            else if (GameBoard.instance.SelectedPiece == gameObject) ToggleSelect();
            else GameBoard.instance.SelectedPiece.GetComponent<GamePiece>().Jump(gameObject);
        }
    }

    IEnumerator ShakeAnimation()
    {
        float offset = 0.01f;
        Vector2 start = gameObject.transform.position;
        Vector2 left = start; left.x -= offset;
        Vector2 right = start; right.x += offset;
        GameBoard.instance.ActiveAnimation = true;

        for(byte s = 0; s < 6; ++s)
        {
            if (gameObject.transform.position.x != right.x)
                gameObject.transform.position = right;
            else gameObject.transform.position = left;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.transform.position = start;
        GameBoard.instance.ActiveAnimation = false;
    }

    public void ChangeColor(Color c)
    {
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }

    public void ToggleGlow()
    {
        SpriteRenderer glow = transform.Find("PegSelected").GetComponent<SpriteRenderer>();
        if (GameBoard.instance.SelectedPiece == this.gameObject) glow.enabled = true;
        else glow.enabled = false;
    }
}

