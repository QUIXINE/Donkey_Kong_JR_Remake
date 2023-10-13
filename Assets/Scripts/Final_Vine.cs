using UnityEngine;
using System.Collections;
using PlayerSpace;

public class Final_Vine : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        print("Reach final vine");
        Player player = col.GetComponent<Player>();
        StartCoroutine(DisablePlayer(player));
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