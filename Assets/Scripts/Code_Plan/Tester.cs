/* using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Tester : MonoBehaviour {



    public Transform obstacleCheckPos;
    public SpriteRenderer spriteRenderer;
    public LayerMask itemLayerMask;
    public float xL;
    public float yL;
    public float xPos;
    public float yPos;

    public Collider2D collider2D;

    private void FixedUpdate() {
        
        
        collider2D = Physics2D.OverlapBox(obstacleCheckPos.position , new Vector3(xL, yL, 0), 0, itemLayerMask);
        if (collider2D)
        {
            print("Change Color");
            spriteRenderer.color = Color.green;
        }
    }

    private void OnDrawGizmos() {



          //Ground check
  
       

        //Obstacle Check
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(obstacleCheckPos.position, new Vector3(xL, yL, 0));
    }
} */