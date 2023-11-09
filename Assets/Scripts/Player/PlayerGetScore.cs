using UnityEngine;
using ScoreManagement;

namespace PlayerSpace{
public partial class Player 
{
    #region Check enemy to earn point
    void CallEnemyStack()
    {
        if(canGetPointFromEnemy)
        {
            if(enemyList.Count != 0)
            {
                if(PlayerPrefs.GetInt("Current_Player") == 1)
                {
                    Score_Variables.ScorePlayer01 = Score_Variables.ScorePlayer01 + scoreOfEnemy;
                }
                else
                {
                    Score_Variables.ScorePlayer02 = Score_Variables.ScorePlayer02 + scoreOfEnemy;;
                }
                canGetPointFromEnemy = false;
            }
        }
        /* if(enemyList.Count != 0)
        enemyList.Clear(); */

    }

    private void EnemyStack()
    {
        
        //Why  check after jump straight --> because ray will be created after press jump btn (I put the Call EnemyStack() inside Jump()), if 
        //Raycast hits enemy
        RaycastHit2D[] Hits = new RaycastHit2D[2];
        int hit = Physics2D.CircleCastNonAlloc(groundCheckPos01.position, 0.7f, -groundCheckPos01.up, Hits, 1f, enemyLayerMask);

        EnemyScore enemy01;
        EnemyScore enemy02;
        
        //RaycastHit2D[] Hits2
        if(hit == 1 && enemyList.Count == 0)
        {
            enemy01 = Hits[0].collider.gameObject.GetComponent<EnemyScore>();     //out of index --> learn how to use RaycastHit2D[]
            enemyList.Add(enemy01);
            print(enemyList);
            
        }
        else if(hit == 2 && enemyList.Count == 0)
        {
            //Try to check use this method for 2 objs at once(hits 2 enemies at the same time)
            enemy01 = Hits[0].collider.gameObject.GetComponent<EnemyScore>(); 
            enemyList.Add(enemy01);
            enemy02 = Hits[1].collider.gameObject.GetComponent<EnemyScore>();
            enemyList.Add(enemy02);
            print(enemyList);
        } 
     
        if(hit != 0)
        {
            if (hit == 1)
            {
                scoreOfEnemy = 100;
            }
            else if (hit == 2)
            {
                scoreOfEnemy = 300;
            }
            CallEnemyStack();
        }
        /* if(hit != 0)    //Just checking in console
        { 
            for(int i = 0; i < 2; i++)
            {
                if(Hits[i] != false)
                //print(colliders[i].gameObject.name); //result: if 2 at the time, print 2 enemies, if 1 print 1
                AccuratePoint();
            }
        } */
    }
    #endregion
    
}
}