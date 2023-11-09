using UnityEngine;
using System.Collections;
using PlayerSpace;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Final_Vine : MonoBehaviour 
{
    [SerializeField] private PlayableDirector playableDirector;
    private void OnTriggerEnter2D(Collider2D col) 
    {
        print("Reach final vine");
        Player player = col.GetComponent<Player>();
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
            
        StartCoroutine(DisablePlayer(player));
        playableDirector.Play();
    }    

    private IEnumerator DisablePlayer(Player player)
    {
        yield return new WaitForSeconds(0.1f);
        player.enabled = false;
        LoadAfterWin();
    }

    private void LoadAfterWin()
    {
        StartCoroutine(GameState.LoadSceneAfterWin());
    }

}