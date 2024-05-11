using UnityEngine;

public class SlotCollision : MonoBehaviour 
{
    public GameObject droppedCube;
     void OnCollisionEnter(Collision collision)
     {
        transform.parent.GetComponent<Puzzle>().CollisionDetected(this, collision.gameObject);
        droppedCube = collision.gameObject;
        Debug.Log("" + droppedCube.name);
     }
 }