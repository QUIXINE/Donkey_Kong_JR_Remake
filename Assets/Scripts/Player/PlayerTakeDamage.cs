using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using PlayerSpace;
using Unity.VisualScripting;

namespace TakeDamage
{
    public class PlayerTakeDamage : MonoBehaviour, ITakeDamage
    {
        private Animator animator;
        private Player player;
        private Rigidbody2D rb;
        private CapsuleCollider2D capsuleCollider;

        private void Start() 
        {
            animator = GetComponent<Animator>();
            player = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        private void Update() 
        {
            FallFromHeight();
            //for testing
            if(Input.GetKeyDown(KeyCode.E))
            {
                TakeDamage();
            }
            
        }

        public void TakeDamage()
        {
            //Every time health--, other objs will stop  
            //life at 3 to 2 and 2 to 1
            if(PlayerHealth.Instance.health > 0 && PlayerHealth.Instance.health > 1)
            {
                animator.SetBool("Die", true);
                Enemy[] enemies = FindObjectsOfType<Enemy>();   //Script that makes enemy moves
                Fruit[] fruits = FindObjectsOfType<Fruit>();
                foreach(Enemy enemy in enemies)
                {
                    enemy.enabled = false;
                }
                foreach(Fruit fruit in fruits)
                {
                    fruit.enabled = false;
                }
                player.enabled = false;
                PlayerHealth.Instance.health--;
                PlayerHealth.Instance.lifeSpriteAmount--;
                gameObject.layer = 0;   //Do this so player will only collide with only one enemy when collides with 2 enemies at the same time
                StartCoroutine(AddGravity());
                StartCoroutine(GameState.LoadSceneAfterDead());
            }
            else if(PlayerHealth.Instance.health == 1) //life at 1 to 0
            {
                animator.SetBool("Die", true);
                Enemy[] enemies = FindObjectsOfType<Enemy>();   //Script that makes enemy moves
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
                PlayerHealth.Instance.health--;
                StartCoroutine(AddGravity());
                StartCoroutine(GameState.GameOver());
            }
        }

        //Fall from height
        private void FallFromHeight()
        {
            if (rb.velocity.y < -3.95f && player.IsGroundedChecker())
            {
                TakeDamage();
                rb.velocity = Vector2.zero;
            }
        }
        
        //Add gravity after player is dead and Disable collider
        private IEnumerator AddGravity()
        {
            rb.velocity = Vector2.zero;
            //Wait until dying animation finish then add gravity
            yield return new WaitForSeconds(2f);
            if(!player.IsGroundedChecker() && !player.collideWithWater)
            {
                capsuleCollider.enabled = false;
                rb.gravityScale = 2;
            }
            StopCoroutine(AddGravity());
        }
        
    }
}