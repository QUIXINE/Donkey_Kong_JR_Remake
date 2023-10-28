using ScoreManagement;
using UnityEngine;

public class Locker_Collision : MonoBehaviour
{
    private SpriteRenderer spR;
    [SerializeField] private GameObject goldenKey;
    [SerializeField] private GameObject cageChain;
    [SerializeField] private GameObject cagePart;


    private void Start() 
    {
        spR = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.CompareTag("Key"))
        {
            print("collide");
            Destroy(col.gameObject);
            Score.ScorePlayer01 += 200;
            //increase 200 point per each
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
