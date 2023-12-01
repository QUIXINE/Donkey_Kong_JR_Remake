using System.Collections;
using ScoreManagement;
using UnityEngine;

public class Locker_Collision : MonoBehaviour
{
    [SerializeField] private GameObject goldenKey;
    [SerializeField] private GameObject cageChain;
    [SerializeField] private GameObject cagePart;
    [SerializeField] private GameObject popUpScore01, popUpScore02;
    [SerializeField] private Transform popUpScore01OldTransform, popUpScore02OldTransform;

    private void Start() 
    {
        popUpScore01OldTransform = popUpScore01.transform;
        popUpScore02OldTransform = popUpScore02.transform;
        popUpScore01.gameObject.SetActive(false);
        popUpScore02.gameObject.SetActive(false);
    }
    private void PopUpScore()
    {
        if(popUpScore01.gameObject.activeSelf)
        {
            popUpScore02.gameObject.transform.position = transform.position;
            popUpScore02.gameObject.SetActive(true);
        }
        else
        {
            popUpScore01.gameObject.transform.position = transform.position;
            popUpScore01.gameObject.SetActive(true);
        }
        Invoke("InactivateUI", 0.45f);
       
    }

    private void InactivateUI()
    {
        print("Inactivate");
        popUpScore01.gameObject.SetActive(false);
        popUpScore02.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.CompareTag("Key"))
        {
            AudioPlayerTest.PlayAudio(AudioReferences.UnlockSound);
            Destroy(col.gameObject);
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                Score_Variables.ScorePlayer01 += 200;
                PopUpScore();

            }
            else
            {
                Score_Variables.ScorePlayer02 += 200;
                PopUpScore();
            }
            //change sprite
            goldenKey.SetActive(true);
            if(cageChain != null)
            {
                cageChain.SetActive(false);
            }
            cagePart.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
