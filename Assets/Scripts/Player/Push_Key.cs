using PlayerSpace;
using UnityEngine;

public class Push_Key : MonoBehaviour
{
    [Tooltip("used to define how much the key will move on y-axis after collide with player")]
    [SerializeField] private float distanceToGo;
    [SerializeField] private Transform posDualHand01, posDualHand02, posOnHead;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask keyLayerMask;
    private Player player;


    private void Start() 
    {
        player = GetComponent<Player>();
    }
    private void Update() 
    {
        if(player.CurrentState == Player.PlayerState.DualHanded)
        {
            if(Physics2D.Raycast(posDualHand01.position, Vector2.up, rayDistance, keyLayerMask))
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(posDualHand01.position, Vector2.up, rayDistance, keyLayerMask);
                if(hitInfo.collider.CompareTag("Key"))
                {
                    hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo), Space.World);
                }
            }

            if(Physics2D.Raycast(posDualHand02.position, Vector2.up, rayDistance, keyLayerMask))
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(posDualHand02.position, Vector2.up, rayDistance, keyLayerMask);
                if(hitInfo.collider.CompareTag("Key"))
                {
                    hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo), Space.World);
                }
            }
        }

        if(Physics2D.Raycast(posOnHead.position, Vector2.up, rayDistance, keyLayerMask))
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(posOnHead.position, Vector2.up, rayDistance, keyLayerMask);
            if(hitInfo.collider.CompareTag("Key"))
            {
                hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo), Space.World);
            }
        }
    }
}
