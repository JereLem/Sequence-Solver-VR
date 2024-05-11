using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCollision : MonoBehaviour
{
    public GameObject droppedShape;
     void OnCollisionEnter(Collision collision)
     {
        transform.parent.GetComponent<ShapeSorting>().CollisionDetected(this, collision.gameObject);
        droppedShape = collision.gameObject;
     }
}
