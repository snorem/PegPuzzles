using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

    [SerializeField] Sprite[] Images;

    void Awake()
    {
        if (GameBoard.instance.LevelComplete)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
                gameObject.GetComponent<SpriteRenderer>().sprite = Images[0];
            else
                gameObject.GetComponent<SpriteRenderer>().sprite = Images[1];
        }
        else gameObject.GetComponent<SpriteRenderer>().sprite = Images[2];
    }

    void OnMouseDown()
    {
        if (GameBoard.instance.LevelComplete)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
                SceneManager.LoadScene(0);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
