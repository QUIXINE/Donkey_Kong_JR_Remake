using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public bool onepattern;
    public bool twopattern;
    public bool fourpattern;
    public GameObject[] waypoints1;
    public GameObject W1;
    public GameObject[] waypoints2;
    public GameObject W2;
    public GameObject[] waypoints3;
    public GameObject W3;
    public GameObject[] waypoints4;
    public GameObject W4;

    int totalway1 =0;
    int totalway2 =0;
    int totalway3 =0;
    int totalway4 =0;

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

    private void Start()
    {
        if(onepattern)
        {
            totalway1 = W1.transform.childCount;
            waypoints1 = new GameObject[totalway1];

            for (int i = 0; i < totalway1; i++)
            {
                waypoints1[i] = W1.transform.GetChild(i).gameObject;
            }
        }
        if (twopattern)
        {
            totalway1 = W1.transform.childCount;
            totalway2 = W2.transform.childCount;
            waypoints1 = new GameObject[totalway1];
            waypoints2 = new GameObject[totalway2];
            for (int i = 0; i < totalway1; i++)
            {
                waypoints1[i] = W1.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < totalway2; i++)
            {
                waypoints2[i] = W2.transform.GetChild(i).gameObject;
            }
        }
        if(fourpattern)
        {
            totalway1 = W1.transform.childCount;
            totalway2 = W2.transform.childCount;
            totalway3 = W3.transform.childCount;
            totalway4 = W4.transform.childCount;

            waypoints1 = new GameObject[totalway1];
            waypoints2 = new GameObject[totalway2];
            waypoints3 = new GameObject[totalway3];
            waypoints4 = new GameObject[totalway4];

            for (int i = 0; i < totalway1; i++)
            {
                waypoints1[i] = W1.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < totalway2; i++)
            {
                waypoints2[i] = W2.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < totalway3; i++)
            {
                waypoints3[i] = W3.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < totalway4; i++)
            {
                waypoints4[i] = W4.transform.GetChild(i).gameObject;
            }
        }

    }
    private void Awake()
    {
        instance = this;
    }

}
