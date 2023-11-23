using PlayerSpace;
using ScoreManagement;
using UnityEngine;
using UnityEngine.Playables;

public class Finish_Level04 : MonoBehaviour 
{
    [SerializeField] private GameObject cagePart01, cagePart02, cagePart03, cagePart04, cagePart05, cagePart06;
    private bool isWin;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Player player;
    private void Start() {
    }
    private void Update() 
    {
        if(cagePart01.activeSelf == false && cagePart02.activeSelf == false && cagePart03.activeSelf == false && cagePart04.activeSelf == false
         && cagePart05.activeSelf == false && cagePart06.activeSelf == false && !isWin)
        {
            player.gameObject.transform.position = new Vector2(-3.59f, -4.612f);
            player.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            //Trigger PlayerableDirector
            playableDirector.Play();
            StartCoroutine(GameState.LoadSceneAfterWin());
            isWin = true;
        }

        //for testing
        /* if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(GameState.LoadSceneAfterWin());
        } */
    }  
}