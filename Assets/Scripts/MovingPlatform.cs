using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoint = 0;
    public float moveSpeed = 2;

    private void Update()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        // move towards next goal point
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.deltaTime * moveSpeed);
        // Check if we are in very close proximity to the next point
        if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {
            // If so change the goal point to the next one
            // Check if we reached the last point, reset to first point
            if (goalPoint == points.Count - 1)
            {
                goalPoint = 0;
            }
            else
            {
                goalPoint++;
            }
            
        }

    }
}
