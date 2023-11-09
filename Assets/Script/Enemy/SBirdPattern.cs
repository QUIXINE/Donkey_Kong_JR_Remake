using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBirdPattern : MonoBehaviour
{
    public GameObject TopRope;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TopRope.SetActive(true);
        }
    }
}
