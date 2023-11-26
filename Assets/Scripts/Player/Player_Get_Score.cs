using UnityEngine;
using ScoreManagement;
using TakeDamage;

namespace PlayerSpace{
public partial class Player 
{
    #region Check enemy to earn point
    private Enemy_Collision enemy_Collision;
    void CallEnemyStack()
    {
        if(canGetPointFromEnemy)
        {
            /* RaycastHit2D[] Hits = new RaycastHit2D[2];
            int hit = Physics2D.CircleCastNonAlloc(groundCheckPos01.position, CircleRadius, -groundCheckPos01.up, Hits, circleRayDis, boundaryLayerMask); //0.7, 0.1 */
            if(enemy_Collision != null)
            {
                if(/* hit == 0 && */enemyList.Count != 0 && enemy_Collision.IsAbleToGetPoint)
                {
                    AudioPlayerTest.PlayAudio(AudioReferences.EnemyClearSound);
                    Score_PopUp_Display.PopUpScore(enemyList[0].gameObject.transform, scoreOfEnemy);    //Pop up score
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
        }
        /* if(enemyList.Count != 0)
        enemyList.Clear(); */

    }

    private void EnemyStack()
    {
        
        //Why  check after jump straight --> because ray will be created after press jump btn (I put the Call EnemyStack() inside Jump()), if 
        //Raycast hits enemy
        //RaycastHit2D[] Hits = new RaycastHit2D[2];
        //int hit = Physics2D.CircleCastNonAlloc(groundCheckPos01.position, CircleRadius, -groundCheckPos01.up, Hits, circleRayDis, enemyLayerMask); //0.7, 0.1
        Collider2D[] collider2Ds = new Collider2D[2];
        int hit = Physics2D.OverlapBoxNonAlloc(groundCheckPos01.position, new Vector2(sizeX, sizeY), 0, collider2Ds, enemyLayerMask); //0.7, 0.1

        Enemy_Score enemy01;
        Enemy_Score enemy02;
        
        //RaycastHit2D[] Hits2
        if(enemyList != null)
        {
            if(hit == 1 && enemyList.Count == 0)
            {
                enemy01 = collider2Ds[0].gameObject.GetComponent<Enemy_Score>();     //out of index --> learn how to use RaycastHit2D[]
                enemy_Collision = collider2Ds[0].gameObject.GetComponent<Enemy_Collision>();
                enemyList.Add(enemy01);
                print(enemyList);
                
            }
            else if(hit == 2 && enemyList.Count == 0)
            {
                //Try to check use this method for 2 objs at once(hits 2 enemies at the same time)
                enemy01 = collider2Ds[0].gameObject.GetComponent<Enemy_Score>(); 
                enemy_Collision = collider2Ds[0].gameObject.GetComponent<Enemy_Collision>();
                enemyList.Add(enemy01);
                enemy02 = collider2Ds[0].gameObject.gameObject.GetComponent<Enemy_Score>();
                enemyList.Add(enemy02);
                print(enemyList);
            } 
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