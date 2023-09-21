using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerTakeDamage : MonoBehaviour, ITakeDamage
{
    private Animator animator;

    private void Start() 
    {
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        //Test
        /* if(Input.GetKeyDown(KeyCode.E) && PlayerHealth.lifeSpriteAmount <= 2)
        {
            animator.SetBool("Die", true);
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            Fruit[] fruits = FindObjectsOfType<Fruit>();
            print(enemies);
            print(fruits);
            foreach(Enemy enemy in enemies)
            {
                enemy.enabled = false;
            }
            foreach(Fruit fruit in fruits)
            {
                fruit.enabled = false;
            }
            PlayerHealth.health--; 
            print(PlayerHealth.health);              
        } */
    }

    public void TakeDamage()
    {
        //Every time health--, other objs will stop  
        //life at 3 to 2 and 2 to 1
        if(PlayerHealth.health > 0 && PlayerHealth.health > 1)
        {
            animator.SetBool("Die", true);
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            Fruit[] fruits = FindObjectsOfType<Fruit>();
            foreach(Enemy enemy in enemies)
            {
                enemy.enabled = false;
            }
            foreach(Fruit fruit in fruits)
            {
                fruit.enabled = false;
            }
            PlayerHealth.health--;
            PlayerHealth.lifeSpriteAmount--;
            StartCoroutine(LoadSceneAfterDead());
        }
        else if(PlayerHealth.health == 1) //life at 1 to 0
        {
            animator.SetBool("Die", true);
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            Fruit[] fruits = FindObjectsOfType<Fruit>();
            Player player = (Player)FindObjectOfType(typeof(Player));
            foreach(Enemy enemy in enemies)
            {
                enemy.enabled = false;
            }
            foreach(Fruit fruit in fruits)
            {
                fruit.enabled = false;
            }
            player.enabled = false;
            PlayerHealth.health--;
            GameState.GameOverUI.SetActive(true);
        }
    }

    private IEnumerator LoadSceneAfterDead()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}