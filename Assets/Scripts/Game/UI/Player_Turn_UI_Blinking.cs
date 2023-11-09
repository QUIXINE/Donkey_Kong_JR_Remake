using UnityEngine;

public class Player_Turn_UI_Blinking : MonoBehaviour 
{
    [SerializeField] private Animator animator1UP;    
    [SerializeField] private Animator animator2UP;

    private void Update() 
    {
        if(PlayerPrefs.GetInt("Current_Player") == 1)
        {
            animator2UP.enabled = false;
            animator1UP.enabled = true;
        }
        else 
        {
            animator1UP.enabled = false;
            animator2UP.enabled = true;
        }
    }    
}