using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public GameObject[] waypoints1;
    
    public GameObject[] waypoints2;

    static GameMaster instance;
    public static GameMaster Instance
    {
        get 
        { if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameMaster)) as GameMaster;
            }
            return instance;
        }
        
        set { instance = value; }
    }

    private void Awake()
    {
        instance = this;
    }

}
