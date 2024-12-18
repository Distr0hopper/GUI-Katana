using System;
using UnityEngine;

public class RobotModel
{
    // Velocity (later updated from ROS topic)
    public Vector3 Velocity { get; private set; }

    // Pose (later updated from ROS topic)
    public Vector3 Pose { get; private set; }

    // Distance to Wall (later updated from ROS topic)
    public float DistanceToWall { get; private set; }

    // Speed (controlled by the slider in the UI)
    private int speed;
    public int Speed
    {
        get => speed;
        set
        {
            speed = value;
            OnSpeedChanged?.Invoke(speed); // Notify listeners (controller that sends speed via ROS Topic)
        }
    }

    // Event to notify listeners when the speed changes
    public event Action<int> OnSpeedChanged;

    // Constructor to initialize default values
    public RobotModel()
    {
        Velocity = Vector3.zero;
        Pose = Vector3.zero;
        DistanceToWall = 0.0f;
        Speed = 100;
    }

    // Methods to update properties from ROS topics
    public void UpdateVelocity(Vector3 newVelocity)
    {
        Velocity = newVelocity;
    }

    public void UpdatePose(Vector3 newPose)
    {
        Pose = newPose;
    }

    public void UpdateDistanceToWall(float distance)
    {
        DistanceToWall = distance;
    }
}
