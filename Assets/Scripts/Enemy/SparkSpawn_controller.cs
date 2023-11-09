using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkSpawn_controller : MonoBehaviour
{
    private int count = 0;
    public GameObject spawnPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            count++;
            if(count == 8)
            {
                Instantiate(spawnPrefab);
            }

            if (count == 9)
            {
                Instantiate(spawnPrefab);
            }
            if (count == 10)
            {
                Instantiate(spawnPrefab);
                count = 0;
            }

        }
    }
}
