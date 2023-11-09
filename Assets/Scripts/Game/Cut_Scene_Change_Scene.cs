using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//Used for loading next scene of cutscene
public class Cut_Scene_Change_Scene : MonoBehaviour 
{

    private void Awake() 
    {
        StartCoroutine(LoadNextScene());    
    }
        
    
    //How long animation will play then increase each player's current scene (Player.._Scene)
    private IEnumerator LoadNextScene()
    {
        int currentScene =  SceneManager.GetActiveScene().buildIndex;

        if(PlayerPrefs.GetInt("Current_Player") == 1)
        {
            switch(currentScene)
            {
                case 1:
                yield return new WaitForSeconds(13); //opening
                PlayerPrefs.SetInt("Player01_Scene", currentScene + 1);
                SceneManager.LoadScene(currentScene + 1);

                break;

                case 4:
                yield return new WaitForSeconds(9); //cut scene after _Level02
                PlayerPrefs.SetInt("Player01_Scene", currentScene + 1);
                SceneManager.LoadScene(currentScene + 1);
                break;
            }
        }
        else
        {
            switch(currentScene)
            {
                case 1:
                yield return new WaitForSeconds(13); //opening
                PlayerPrefs.SetInt("Player02_Scene", currentScene + 1);
                SceneManager.LoadScene(currentScene + 1);

                break;

                case 4:
                yield return new WaitForSeconds(9); //cut scene after _Level02
                PlayerPrefs.SetInt("Player02_Scene", currentScene + 1);
                SceneManager.LoadScene(currentScene + 1);
                break;
            }
        }
    }    

    //test 2 yeild return
  /*   private IEnumerator TestReturn()
    {
        
        switch (l)
        {
            default:
                yield return new WaitForSeconds(10f);   
                SceneManager.LoadScene(1);
                print("Wait 10 secs");                       //Wait to finish clear stage animation 10 sec
                
                break;
            
            case 2:
                yield return new WaitForSeconds(16);                            //Wait animation to finish
                SceneManager.LoadScene(1);
                print("Wait 16 secs");   
                break;
        }
    
    }
 */
}