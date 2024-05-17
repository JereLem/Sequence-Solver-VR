using UnityEngine;

public class TowerCollision : MonoBehaviour 
{
    public GameObject droppedCube;
     void OnCollisionEnter(Collision collision)
     {
        transform.parent.GetComponent<TowerStacking>().CollisionDetected(this, collision.gameObject);
        droppedCube = collision.gameObject;
        Debug.Log("" + droppedCube.name);
     }
 }