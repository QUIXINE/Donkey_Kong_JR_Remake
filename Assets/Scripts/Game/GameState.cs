using UnityEngine;

public class GameState : MonoBehaviour 
{
    public static GameObject GameOverUI;
    [SerializeField] private GameObject gameOverUI;
    private void Start() 
    {
        GameOverUI = gameOverUI;
    }
}