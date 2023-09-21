List<Enemy> list = new List<Enemy>();


# First try EnemyStack()
void CallEnemyStack()
{
    if(isJumping)
    {
        Score.ScoreText.text = TotalScore + EnemyStack();
    }
    list.RemoveAll();

}

int score;

int EnemyStack()
{
    
    //Raycast hits enemy
    RaycastHit2D hit...
    Enemy enemy01;
    Enemy enemy02;
    

    if(hit && list == null)
    {
        if(enemy01 == null)
        {
            enemy01 = hit.collider.gameObject.GetComponent<Enemy>();
            list.Add(enemy01);
        }
    }
    else if(hit && list != null)
    {
        //Try to check use this method for 2 objs at once(hits 2 enemies at the same time)
        if(enemy01 != null) 
        {
            enemy02 = hit.collider.gameObject.GetComponent<Enemy>();
            list.Add(enemy02);
        }
    }

    if (enemy01 != null && enemy02 == null)
    {
        score = 100;
    }
    else if (enemy01 != null && enemy02 != null)
    {
        score = 300;
    }
    
    return 0;

}


# Second Try
void CalculatePoint()
{
    if(canGetPointFromEnemy)
    {
        if(enemyList != 0)
        Score.ScoreText.text = TotalScore + score;
    }

}

int score;

int EnemyStack()
{
    
    //Raycast hits enemy
    RaycastHit2D hit...
    Enemy enemy01;
    Enemy enemy02;
    

    if(hit && list == null)
    {
        if(enemy01 == null)
        {
            enemy01 = hit.collider.gameObject.GetComponent<Enemy>();
            list.Add(enemy01);
        }
    }
    else if(hit && list != null)
    {
        //Try to check use this method for 2 objs at once(hits 2 enemies at the same time)
        if(enemy01 != null) 
        {
            enemy02 = hit.collider.gameObject.GetComponent<Enemy>();
            list.Add(enemy02);
        }
    }

    if(enemylist.Count != 0)
    {
        if (enemy01 != null && enemy02 == null)
        {
            score = 100;
        }
        else if (enemy01 != null && enemy02 != null)
        {
            score = 300;
        }
        waitToAccuratePoint()
    }
    
    return 0;

}
if- IsGrounded()
{
    enmyList.Clear();
}


IEnumerator waitToAccuratePoint()
{
    yield return new Wait...(1f);
    CalculatePoint();
}
