using UnityEngine;
using ScoreManagement;
using TakeDamage;

namespace PlayerSpace{
public partial class Player 
{
    #region Check enemy to earn point
    private EnemyCollidePlayer enemy_Collision;
    void CallEnemyStack()
    {
        if(canGetPointFromEnemy)
        {
            if(enemy_Collision != null)
            {
                if(/* hit == 0 && */enemyList.Count != 0 && enemy_Collision.IsAbleToGetPoint)
                {
                    AudioPlayerTest.PlayAudio(AudioReferences.EnemyClearSound);
                    ScorePopUpDisplay.PopUpScore(enemyList[0].gameObject.transform, scoreOfEnemy);    //Pop up score
                    if(PlayerPrefs.GetInt("Current_Player") == 1)
                    {
                        ScoreVariables.ScorePlayer01 = ScoreVariables.ScorePlayer01 + scoreOfEnemy;
                    }
                    else
                    {
                        ScoreVariables.ScorePlayer02 = ScoreVariables.ScorePlayer02 + scoreOfEnemy;;
                    }
                    canGetPointFromEnemy = false;
                }
            }
        }
    }

    private void EnemyStack()
    {
        
        //Why  check after jump straight --> because ray will be created after press jump btn (I put the Call EnemyStack() inside Jump()), if 
        //Raycast hits enemy
        //RaycastHit2D[] Hits = new RaycastHit2D[2];
        //int hit = Physics2D.CircleCastNonAlloc(groundCheckPos01.position, CircleRadius, -groundCheckPos01.up, Hits, circleRayDis, enemyLayerMask); //0.7, 0.1
        Collider2D[] collider2Ds = new Collider2D[2];
        int hit = Physics2D.OverlapBoxNonAlloc(groundCheckPos01.position, new Vector2(sizeX, sizeY), 0, collider2Ds, enemyLayerMask); //0.7, 0.1

        EnemyScore enemy01;
        EnemyScore enemy02;
        
        //RaycastHit2D[] Hits2
        if(enemyList != null)
        {
            if(hit == 1 && enemyList.Count == 0)
            {
                enemy01 = collider2Ds[0].gameObject.GetComponent<EnemyScore>();     //out of index --> learn how to use RaycastHit2D[]
                enemy_Collision = collider2Ds[0].gameObject.GetComponent<EnemyCollidePlayer>();
                enemyList.Add(enemy01);
                
            }
            else if(hit == 2 && enemyList.Count == 0)
            {
                //Try to check use this method for 2 objs at once(hits 2 enemies at the same time)
                enemy01 = collider2Ds[0].gameObject.GetComponent<EnemyScore>(); 
                enemy_Collision = collider2Ds[0].gameObject.GetComponent<EnemyCollidePlayer>();
                enemyList.Add(enemy01);
                enemy02 = collider2Ds[0].gameObject.gameObject.GetComponent<EnemyScore>();
                enemyList.Add(enemy02);
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
    }
    #endregion
    
}
}