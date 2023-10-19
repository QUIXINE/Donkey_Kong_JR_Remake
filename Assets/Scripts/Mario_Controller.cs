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
    public GameObject Prefab2;
    public GameObject Prefab3;
    //Variable
    public int Pattern = 1;
    public float timer = 0.0f;
    public float Timeremain = 2f;
    public bool Spawnyet = false;
    private int RndNum;
    public bool ActiveBbird;
    public bool ActiveSpark;
    public bool ActiveSpawnOnePattern;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (ActiveBbird)
        {
            Wait();
        }
        if (ActiveSpark)
        {
            WaitSpark();
        }
        if (ActiveSpawnOnePattern)
        {
            OneWait();
        }
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

    void OneWait()
    {
        timer += Time.deltaTime;

        if (timer >= Timeremain)
        {
            SpawnEnemy();
            timer = 0.0f;
        }
        if (timer >= Timeremain)
        {
            Spawnyet = false;
        }

    }

    void WaitSpark()
    {
        timer += Time.deltaTime;

        RndNum = Random.Range(1, 4);

        if (timer >= Timeremain && RndNum == 1)
        {
            SpawnEnemy();
            timer = 0.0f;
        }

        if (timer >= Timeremain && RndNum == 2)
        {
            SpawnEnemy1();
            timer = 0.0f;
        }
        if (timer >= Timeremain && RndNum == 3)
        {
            SpawnEnemy2();
            timer = 0.0f;
        }
        if (timer >= Timeremain && RndNum == 4)
        {
            SpawnEnemy3();
            timer = 0.0f;
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

    void SpawnEnemy2()
    {
        Instantiate(Prefab2, Spawner.transform.position, Quaternion.identity);
        Spawnyet = true;
    }
    void SpawnEnemy3()
    {
        Instantiate(Prefab3, Spawner.transform.position, Quaternion.identity);
        Spawnyet = true;
    }


}


