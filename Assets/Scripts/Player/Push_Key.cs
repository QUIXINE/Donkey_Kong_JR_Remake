using PlayerSpace;
using UnityEngine;


namespace PlayerSpace
{

    sealed public class Push_Key : MonoBehaviour
    {
        [Tooltip("used to define how much the key will move on y-axis after collide with player")]
        [SerializeField] private float distanceToGo;
        [SerializeField] private Transform posDualHand01, posDualHand02, posOnHead, groundCheckPos01, groundCheckPos02, keyCheckPosOnFoot;
        [SerializeField] private float rayDistance;
        [SerializeField] private float rayDistanceFirstValue;
        [SerializeField] private LayerMask itemLayerMask;
        private Player player;
        private float vertical;

        private void Start() 
        {
            player = GetComponent<Player>();
            rayDistanceFirstValue = rayDistance;
        }
        private void Update()
        {
            Pushing_Key();
            vertical = Input.GetAxis("Vertical");

            //used this to check if player move up after collides w/ the key which makes the ray distance 0
            //why -2.5f --> it's the distance to make sure the ray won't detect the key and makes the key move after it's at the lowest position of the chain  
            if(transform.position.y > -2.5f)
            {
                rayDistance = rayDistanceFirstValue;
            }

           
            
        }

        private void Pushing_Key()
        {
            if (player.CurrentState == Player.PlayerState.DualHanded && rayDistance != 0)
            {

                //push on hand
                if (Physics2D.Raycast(posDualHand01.position, Vector2.up, rayDistance, itemLayerMask))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(posDualHand01.position, Vector2.up, rayDistance, itemLayerMask);
                    if (hitInfo.collider.CompareTag("Key"))
                    {
                        hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo), Space.World);
                    }
                }
                else if (Physics2D.Raycast(groundCheckPos02.position, -Vector2.up, rayDistance, itemLayerMask))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckPos02.position, -Vector2.up, rayDistance, itemLayerMask);
                    if (hitInfo.collider.CompareTag("Key"))
                    {
                        hitInfo.collider.gameObject.transform.Translate(new Vector2(0, -distanceToGo), Space.World);
                    }
                }

                //push on foot
                if (Physics2D.Raycast(posDualHand02.position, Vector2.up, rayDistance, itemLayerMask))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(posDualHand02.position, Vector2.up, rayDistance, itemLayerMask);
                    if (hitInfo.collider.CompareTag("Key"))
                    {
                        hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo), Space.World);
                    }
                }
                else if (Physics2D.Raycast(keyCheckPosOnFoot.position, -Vector2.up, rayDistance, itemLayerMask))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(keyCheckPosOnFoot.position, -Vector2.up, rayDistance, itemLayerMask);
                    if (hitInfo.collider.CompareTag("Key"))
                    {
                        hitInfo.collider.gameObject.transform.Translate(new Vector2(0, -distanceToGo), Space.World);
                    }
                }
            }

            //TwoHanded push
            else if(player.CurrentState == Player.PlayerState.TwoHanded && rayDistance != 0)
            {
                //push on haed
                if (Physics2D.Raycast(posOnHead.position, Vector2.up, rayDistance, itemLayerMask))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(posOnHead.position, Vector2.up, rayDistance, itemLayerMask);
                    if (hitInfo.collider.CompareTag("Key"))
                    {
                        hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo), Space.World);
                    }
                }
                //push under the bottom
                else if(Physics2D.Raycast(groundCheckPos01.position, -Vector2.up, rayDistance, itemLayerMask))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckPos01.position, -Vector2.up, rayDistance, itemLayerMask);
                    if (hitInfo.collider.CompareTag("Key"))
                    {
                        hitInfo.collider.gameObject.transform.Translate(new Vector2(0, -distanceToGo), Space.World);
                        if(transform.position.y <= -2.9f)
                        {
                            rayDistance = 0;
                        }
                    }
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D col) 
        {
            //Collide w/ item, in this case is key (index 10)
            //used this so that the ray won't work when player's head detect the key
            if(col.gameObject.layer == 10 && vertical < 0 && col.transform.position.y <= -3.512f)
            {
                print("collide 0");
                rayDistance = 0;
            }
        
        }

        private void OnCollisionEnter2D(Collision2D col) 
        {
            //Collide w/ Ground (index 6)
            //used this so that after back to the groun the ray will work again
            if(col.gameObject.layer == 6)
            {
                rayDistance = rayDistanceFirstValue;
            }  
        }
    }


        
}

