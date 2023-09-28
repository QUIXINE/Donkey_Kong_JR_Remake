using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mario_Controller : MonoBehaviour
{
    //GameObjct
    public GameObject Spawner;
    public GameObject Prefab;
    public GameObject Prefab1;
    //Variable
    public int Pattern = 1;
    public float timer = 0.0f;
    public float Timeremain = 2f;
    public bool Spawnyet = false;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Wait();
    }

    void Wait()
    {
        timer += Time.deltaTime;

        if (timer >= Timeremain && Pattern == 1)
        {
            SpawnEnemy();
            timer = 0.0f;
            Pattern = 2;
            
        }

        if (timer >= Timeremain && Pattern == 2)
        {
            SpawnEnemy1();
            timer = 0.0f;
            Pattern = 1;
        }
        if (timer >= Timeremain)
        {
            Spawnyet = false;
        }

    }

    void SpawnEnemy()
    {
        Instantiate(Prefab, Spawner.transform.position, Quaternion.identity);
        Spawnyet=true;
    }

    void SpawnEnemy1()
    {
        Instantiate(Prefab1, Spawner.transform.position, Quaternion.identity);
        Spawnyet = true;
    }



}


