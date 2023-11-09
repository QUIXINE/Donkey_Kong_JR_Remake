using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton_Caller : MonoBehaviour {
    private void Start() 
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            Singleton_Tester.Instance.Num_Teller++;
            print(Singleton_Tester.Instance.Num_Teller); //scene _Level02, Num_Teller == 2 becasue the value is Instance's
        }
    }
    private void Update() 
    {
        //Test if I can reset Single ton by create new obj then assign it as parent and destroy it
        //Yes, it can be reset
        /* if(Input.GetKeyDown(KeyCode.E))
        {
            Singleton_Tester.Instance.Num_Teller++;
            print(Singleton_Tester.Instance.Num_Teller);
        } */
        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            SceneManager.LoadScene("Menu_Scene");
        }
        else if(Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene("_Level01");
        }
        else if(Input.GetKeyDown(KeyCode.T)) 
        {
            SceneManager.LoadScene("_Level02");
        }    
    }
}