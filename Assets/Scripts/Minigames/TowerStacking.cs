using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TowerStacking : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject slot1;
    public GameObject slot2;
    private List<GameObject> cubes; // List to hold references to the cubes
    private List<Color> cubeSequence; // Sequence of cube colors
    private List<Color> slotSequence; // Sequence of slot colors
    private int currentCubeIndex; // Index of the cube to be spawned next
    private int cubesMatchedSlots; // Number of cubes matched with slots
    private bool towerStacked; // Flag to know when the tower is stacked fully

    private void Start()
    {

    }


    public void CollisionDetected(TowerCollision collision, GameObject droppedCube)
    {
        GameObject slot = collision.gameObject;

    }
}