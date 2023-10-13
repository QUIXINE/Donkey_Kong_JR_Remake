using UnityEngine;

public class Push_Key : MonoBehaviour
{
    [Tooltip("used to define how much the key will move on y-axis after collide with player")]
    [SerializeField] private float distanceToGo;
    [SerializeField] private Transform pos01, pos02, posOnHead;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask keyLayerMask;
   
    private void Update() 
    {
        if(Physics2D.Raycast(pos01.position, Vector2.up, rayDistance, keyLayerMask))
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(pos01.position, Vector2.up, rayDistance, keyLayerMask);
            if(hitInfo.collider.CompareTag("Key"))
            {
                hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo));
            }
        }

        if(Physics2D.Raycast(pos02.position, Vector2.up, rayDistance, keyLayerMask))
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(pos02.position, Vector2.up, rayDistance, keyLayerMask);
            if(hitInfo.collider.CompareTag("Key"))
            {
                hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo));
            }
        }

        if(Physics2D.Raycast(posOnHead.position, Vector2.up, rayDistance, keyLayerMask))
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(posOnHead.position, Vector2.up, rayDistance, keyLayerMask);
            if(hitInfo.collider.CompareTag("Key"))
            {
                hitInfo.collider.gameObject.transform.Translate(new Vector2(0, distanceToGo));
            }
        }
    }
}
