using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShapeSorting : MonoBehaviour
{
    public GameObject[] shapePrefabs; // Array to hold references to shape prefabs
    public GameObject[] slots; // Array to hold references to the slots
    public List<GameObject> shapes; // List to hold references to the shapes
    private TextMeshProUGUI textComponent; // Reference to the Text component
    private Dictionary<GameObject, string> slotExpectedShapes; // Dictionary to store the type of shape each slot expects

    // Dictionary to keep track of the number of shapes dropped into each slot
    private Dictionary<GameObject, int> slotShapeCounts;

    // Dictionary to keep track of the slot where each shape is dropped
    private Dictionary<GameObject, GameObject> shapeSlotMap;

    // Access Gamestats & VrGameManager
    private GameStats gameStats;
    private GameObject vrgm;


    void Start()
    {
        // Get references to the slots
        slots = GameObject.FindGameObjectsWithTag("Slot");

        // Initialize shapes list
        shapes = new List<GameObject>();

        // Get gamestats script
        vrgm = GameObject.FindGameObjectWithTag("VrGameManager");
        gameStats = vrgm.GetComponent<GameStats>();

        // Initialize the slotExpectedShapes dictionary
        slotExpectedShapes = new Dictionary<GameObject, string>();
        foreach (GameObject slot in slots)
        {
            Canvas canvas = slot.GetComponentInChildren<Canvas>();
            textComponent = canvas.GetComponentInChildren<TextMeshProUGUI>();
            slotExpectedShapes[slot] = textComponent.text;
        }

        // Initialize the slotShapeCounts dictionary
        slotShapeCounts = new Dictionary<GameObject, int>();
        foreach (GameObject slot in slots)
        {
            slotShapeCounts[slot] = 0;
        }

        // Initialize the shapeSlotMap dictionary
        shapeSlotMap = new Dictionary<GameObject, GameObject>();

        // Spawn shapes in a randomized grid with different colors
        SpawnShapes();
    }
    
    void Update()
    {
        // Check if all slots have received four correct shapes
        bool allSlotsFilled = true;
        foreach (var slot in slots)
        {
            if (!slotShapeCounts.ContainsKey(slot) || slotShapeCounts[slot] < 4)
            {
                allSlotsFilled = false;
                break;
            }
        }

        if (allSlotsFilled)
        {
            DeleteSorting(); // Call the function to delete the game
            gameStats.solvedMinigames++;
        }
        else if (gameStats.gameover)
        {
            DeleteSorting();
        }
    }


    void SpawnShapes()
    {
        // Shuffle the shape prefabs array to ensure randomness
        ShuffleArray(shapePrefabs);

        // Define colors for the shapes
        Color[] colors = new Color[]
        {
            Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.black, Color.gray
        };

        // Spawn exactly 4 of each shape in a randomized grid
        for (int i = 0; i < shapePrefabs.Length; i++)
        {
            // Generate random positions for each shape
            List<Vector3> randomPositions = GenerateRandomPositions(4);

            for (int j = 0; j < randomPositions.Count; j++)
            {
                // Instantiate shape with a random color
                GameObject shape = Instantiate(shapePrefabs[i], randomPositions[j], Quaternion.identity);
                shape.GetComponent<Renderer>().material.color = colors[i];
                shapes.Add(shape);
                
                // Update the slotShapeCounts dictionary
                foreach (GameObject slot in slots)
                {
                    if (Vector3.Distance(randomPositions[j], slot.transform.position) < 1.0f)
                    {
                        slotShapeCounts[slot]++;
                    }
                }
            }
        }
    }

    List<Vector3> GenerateRandomPositions(int count)
    {
        List<Vector3> positions = new List<Vector3>();

        // Generate random positions within a grid
        for (int i = 0; i < count; i++)
        {
            int randomSlotIndex = Random.Range(0, slots.Length);
            Vector3 randomPosition = slots[randomSlotIndex].transform.position + new Vector3(Random.Range(-5f ,5f), 2f, Random.Range(5f ,5f));
            positions.Add(randomPosition);
        }

        return positions;
    }

    // Function to shuffle the array
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

    public void CollisionDetected(ShapeCollision collision, GameObject droppedShape)
    {
        GameObject slot = collision.gameObject;

        // Check if the dropped shape matches the type of shape the slot expects
        string droppedShapeType = droppedShape.name.Replace("(Clone)", "");
        if (droppedShapeType == slotExpectedShapes[slot])
        {
            Debug.Log("Slot and shape matched");

            // Check if the slot is already filled with 4 shapes
            if (!slotShapeCounts.ContainsKey(slot))
            {
                slotShapeCounts[slot] = 1;
            }
            else
            {
                slotShapeCounts[slot]++;
            }

            // Lock the shape in place by mapping it to the slot
            shapeSlotMap[droppedShape] = slot;

            // Disable collider or interaction script to prevent grabbing with the controller
            Collider shapeCollider = droppedShape.GetComponent<Collider>();
            if (shapeCollider != null)
            {
                shapeCollider.enabled = false;
            }
        }
        else
        {
            Debug.Log("Slot and shape do not match");
        }
    }


    void DeleteSorting()
    {
        // Destroy each cube
        foreach (GameObject shape in shapes)
        {
            Destroy(shape);
        }

        // Destroy the puzzle
        Destroy(gameObject);
    }
}
