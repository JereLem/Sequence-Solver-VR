using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject[] slots; // Array to hold references to the slots
    private List<GameObject> cubes; // List to hold references to the cubes
    private int cubeMatchedSlots; // Number of slots matched with cubes
    private GameStats gameStats; // Access Gamestats
    private GameObject vrgm;

    void Start()
    {
        // Get references to the slots
        slots = GameObject.FindGameObjectsWithTag("Slot");

        // Get gamestats script
        vrgm = GameObject.FindGameObjectWithTag("VrGameManager");
        gameStats = vrgm.GetComponent<GameStats>();

        // Assign matched cube-slots to zero
        cubeMatchedSlots = 0;

        // Initialize cubes list
        cubes = new List<GameObject>();

        // Assign random colors to the cubes and slots
        AssignRandomColors();
    }

    void AssignRandomColors()
    {
        // Define positions for the cubes in a line
        Vector3 lineStart = new Vector3(-8f, 0f, -8f);
        float spacing = 2f;

        // Define colors for the cubes and slots
        Color[] colors = new Color[]
        {
            Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.black, Color.gray
        };

        // Shuffle colors array
        ShuffleArray(colors);

        // Assign colors to cubes and corresponding slots
        for (int i = 0; i < slots.Length; i++)
        {
            // Assign a color to the cube
            GameObject cube = Instantiate(cubePrefab, transform.position + lineStart + Vector3.right * spacing * i, Quaternion.identity);
            cube.GetComponent<Renderer>().material.color = colors[i];
            cubes.Add(cube);

            // Assign a color to the slot
            slots[i].GetComponent<Renderer>().material.color = colors[i];
        }
    }


    void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public void CollisionDetected(SlotCollision collision, GameObject droppedCube)
    {
        GameObject slot = collision.gameObject;

        // Check if the color of the dropped cube matches the color of the slot
        Color droppedCubeColor = droppedCube.GetComponent<Renderer>().material.color;
        Color slotColor = slot.GetComponent<Renderer>().material.color;

        if (droppedCubeColor == slotColor)
        {
            Debug.Log("Colors match!");
            cubeMatchedSlots++;
            if (cubeMatchedSlots == slots.Length)
            {
                // All slots matched, delete the whole puzzle and each cube
                DeletePuzzle();
                gameStats.solvedMinigames++;
            }
        }
    }

    void DeletePuzzle()
    {
        // Destroy each cube
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }

        // Destroy the puzzle
        Destroy(gameObject);
    }
}
