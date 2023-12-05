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
        
        if(col.TryGetComponent<Player>(out Player player))
        {
            Player_TakeDamage player_TakeDamage = col.GetComponent<Player_TakeDamage>();
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            print("Change transform");
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                player.gameObject.transform.position = new Vector2(-4.1f, 2.7f);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                player.gameObject.transform.position = new Vector2(-0.5f, 2.8f);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 5)
            {
                player.gameObject.transform.position = new Vector2(-0.5f, 2.8f);
            }
            DisablePlayer(player, player_TakeDamage);
            playableDirector.Play();
        }
            
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