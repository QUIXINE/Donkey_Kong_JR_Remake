using UnityEngine;
using System.Collections;
using PlayerSpace;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using TakeDamage;

public class Final_Vine : MonoBehaviour 
{
    [SerializeField] private PlayableDirector playableDirector;
    private void OnTriggerEnter2D(Collider2D col) 
    {
        print("Reach final vine");
        Player player = col.GetComponent<Player>();
        Player_TakeDamage player_TakeDamage = col.GetComponent<Player_TakeDamage>();
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            player.transform.position = new Vector2(-4.21f, 3f);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            player.transform.position = new Vector2(-0.685f, 3f);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            player.transform.position = new Vector2(-0.685f, 3f);
        }
            
        DisablePlayer(player, player_TakeDamage);
        playableDirector.Play();
    }    

    private void DisablePlayer(Player player, Player_TakeDamage player_TakeDamage)
    {
        player_TakeDamage.enabled = false;
        player.enabled = false;
        LoadAfterWin();
    }

    private void LoadAfterWin()
    {
        StartCoroutine(GameState.LoadSceneAfterWin());
    }

}