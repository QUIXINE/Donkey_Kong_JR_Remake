using UnityEngine;
using System.Collections;
using PlayerSpace;

namespace TakeDamage
{
    sealed public class Player_TakeDamage : MonoBehaviour
    {
        public  Animator animator;
        private Player player;
        private Rigidbody2D rb;
        private CapsuleCollider2D capsuleCollider;
        [SerializeField] private float dieHeight = -3.95f;
        public bool IsCollidedWithEnemy {get; set;}

        private void Start() 
        {
            animator = GetComponentInChildren<Animator>();
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
                animator.SetBool("DieOtherCondition", true);
                TakeDamage();
            }
            
        }

        public void TakeDamage()
        {
            Mario_Controller mario_Controller = FindObjectOfType<Mario_Controller>();
            if(mario_Controller != null)
            {
                print("disable mariocontroller");
                mario_Controller.enabled = false;
            }
            //Every time health--, other objs will stop  
            //life at 3 to 2 and 2 to 1
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                if(Player01Health.Instance.Health > 0 && Player01Health.Instance.Health > 1)
                {
                    animator.SetBool("Die", true);
                    Enemy_Collide_Player[] enemies = FindObjectsOfType<Enemy_Collide_Player>();   //Script that makes enemy moves
                    Fruit[] fruits = FindObjectsOfType<Fruit>();
                    foreach(Enemy_Collide_Player enemy in enemies)
                    {
                        enemy.enabled = false;
                    }
                    foreach(Fruit fruit in fruits)
                    {
                        fruit.enabled = false;
                    }
                    player.enabled = false;
                    Player01Health.Instance.Health--;
                    Player01Health.Instance.LifeSpriteAmount--;
                    gameObject.layer = 0;   //Do this so player will only collide with only one enemy when collides with 2 enemies at the same time
                    StartCoroutine(AddGravity());
                    StartCoroutine(GameState.LoadSceneAfterDead());
                }
                else if(Player01Health.Instance.Health == 1) //life at 1 to 0
                {
                    animator.SetBool("Die", true);
                    Enemy_Collide_Player[] enemies = FindObjectsOfType<Enemy_Collide_Player>();   //Script that makes enemy moves
                    Fruit[] fruits = FindObjectsOfType<Fruit>();
                    Player player = (Player)FindObjectOfType(typeof(Player));
                    foreach(Enemy_Collide_Player enemy in enemies)
                    {
                        enemy.enabled = false;
                    }
                    foreach(Fruit fruit in fruits)
                    {
                        fruit.enabled = false;
                    }
                    player.enabled = false;
                    Player01Health.Instance.Health--;
                    StartCoroutine(AddGravity());
                    StartCoroutine(GameState.GameOver());
                }
            }
            else
            {
                if(Player02Health.Instance.Health > 0 && Player02Health.Instance.Health > 1)
                {
                    animator.SetBool("Die", true);
                    Enemy_Collide_Player[] enemies = FindObjectsOfType<Enemy_Collide_Player>();   //Script that makes enemy moves
                    Fruit[] fruits = FindObjectsOfType<Fruit>();
                    foreach(Enemy_Collide_Player enemy in enemies)
                    {
                        enemy.enabled = false;
                    }
                    foreach(Fruit fruit in fruits)
                    {
                        fruit.enabled = false;
                    }
                    player.enabled = false;
                    Player02Health.Instance.Health--;
                    Player02Health.Instance.LifeSpriteAmount--;
                    gameObject.layer = 0;   //Do this so player will only collide with only one enemy when collides with 2 enemies at the same time
                    StartCoroutine(AddGravity());
                    StartCoroutine(GameState.LoadSceneAfterDead());
                }
                else if(Player02Health.Instance.Health == 1) //life at 1 to 0
                {
                    animator.SetBool("Die", true);
                    Enemy_Collide_Player[] enemies = FindObjectsOfType<Enemy_Collide_Player>();   //Script that makes enemy moves
                    Fruit[] fruits = FindObjectsOfType<Fruit>();
                    Player player = (Player)FindObjectOfType(typeof(Player));
                    foreach(Enemy_Collide_Player enemy in enemies)
                    {
                        enemy.enabled = false;
                    }
                    foreach(Fruit fruit in fruits)
                    {
                        fruit.enabled = false;
                    }
                    player.enabled = false;
                    Player02Health.Instance.Health--;
                    StartCoroutine(AddGravity());
                    StartCoroutine(GameState.GameOver());
                }
            }
        }

        //Fall from height
        private void FallFromHeight()
        {
            if (rb.velocity.y < dieHeight && player.IsGroundedChecker() && !IsCollidedWithEnemy)
            {
                print(rb.velocity.y);
                animator.SetBool("IsGrounded", true);   //in PlayerCheckers script, IsGrounded() doesn't let animator bool IsGrounded = true 
                                                        //as a condition of fall from height dying, so I set it here 
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

            //if on ground will turn collider off
            if(!player.IsGroundedChecker() && !player.collideWithWater)
            {
                // capsuleCollider.enabled = false;
                rb.gravityScale = 2;
            }
        }
        
    }
}