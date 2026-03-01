using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    // Reference to waypoints
    public List<Transform> points;
    // The int value for the next point index
    public int nextID;
    // The value that applies to ID for changing
    private int idChangeValue = 1;
    // Speed of movement
    public float speed = 2;

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        // Make BoxCollider trigger
        GetComponent<Collider2D>().isTrigger = true;
        // Create root object
        GameObject root = new GameObject(name + "_Root");
        // Reset position of root to this enemy object
        root.transform.position = transform.position;
        // Set enemy objects as child of root
        transform.SetParent(root.transform);
        // Create Waypoints object
        GameObject waypoints = new GameObject("Waypoints");
        // Reset Waypoints position to root
        // Set Waypoints as child of root
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        // Create two points (gameobject) and reset their position to waypoints object
        // Set the points as children of waypoint object
        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;

        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;

        // Init points list then add the points to it
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        // Get the next Point transform
        Transform goalPoint = points[nextID];
        // Flip the enemy transform to look into the point's direction
        if (goalPoint.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
        // Move the enemy towards the goal point
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        // Check the distance between enemy and goal point to trigger next point
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.2f)
        {
            // Check if we are at the end of the line (make the change -1)
            if (nextID == points.Count - 1) 
                idChangeValue = -1;
            // Check if we are at the start of the line (make the change +11)
            if (nextID == 0)
                idChangeValue = 1;
            // Apply the change to nextID
            nextID += idChangeValue;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{name} Triggered");

            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(25, transform.position);
            }
        }
    }
}
