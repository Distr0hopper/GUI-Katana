using System;
using UnityEngine;

public class RobotModel
{   
    #region ROS_Properties
    // Velocity (later updated from ROS topic)
    public Vector3 Velocity { get; private set; }

    // Scalar velocity (magnitude of the velocity vector)
    public float ScalarVelocity {get; private set; }

    // Pose (later updated from ROS topic)
    public Vector3 Pose { get; private set; }

    // Distance to Wall (later updated from ROS topic)
    public float DistanceToWall { get; private set; }

    #endregion

    #region UI Properties
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

    // Emergeny stop state (controlled by the button in the UI)
    private bool emergencyStopActive;
public bool EmergencyStopActive
    {
        get => currentDriveMode == DriveMode.EmergencyStop;
        set
        {
            if (value)
            {
                // Activate emergency stop and save the current mode
                if (currentDriveMode != DriveMode.EmergencyStop)
                {
                    previousDriveMode = currentDriveMode;
                }
                CurrentDriveMode = DriveMode.EmergencyStop;
            }
            else
            {
                // Deactivate emergency stop and revert to previous mode
                CurrentDriveMode = previousDriveMode;
            }
            OnEmergencyStopChanged?.Invoke(value);
        }
    }

        public enum DriveMode
    {
        Manual,
        Auto, 
        EmergencyStop
    }

    private DriveMode currentDriveMode;
    private DriveMode previousDriveMode;
    public DriveMode CurrentDriveMode
    {
        get => currentDriveMode;
        set
        {
            if (currentDriveMode != value)
            {
                currentDriveMode = value;
                OnDriveModeChanged?.Invoke(currentDriveMode);
            }
        }
    }

    #endregion

    #region Events

    // Event to notify listeners when the speed changes
    public event Action<int> OnSpeedChanged;

    // Event to notify listeners when scalar velocity changes
    public event Action<float> OnVelocityChanged;

    // Event to notify listeners when emergency stop state changes
    public event Action<bool> OnEmergencyStopChanged;

    // Event to notify listeners when the drive mode changes
    public event Action<DriveMode> OnDriveModeChanged;

    #endregion
    
    // Constructor to initialize default values
    public RobotModel()
    {
        //Velocity = Vector3.zero;
        ScalarVelocity = 0.0f;
        Pose = Vector3.zero;
        DistanceToWall = 0.0f;
        Speed = 100;
        EmergencyStopActive = false;
        previousDriveMode = DriveMode.Manual;
        CurrentDriveMode = DriveMode.Manual;
    }

    private void UpdateDriveModeBasedOnEmergencyStop()
    {
        if (EmergencyStopActive)
        {
            CurrentDriveMode = DriveMode.EmergencyStop;
        }
        else
        {
            CurrentDriveMode = DriveMode.Manual;
        }
        OnEmergencyStopChanged?.Invoke(EmergencyStopActive);
    }

    public void UpdateScalarVelocity(float newVelocity)
    {
        ScalarVelocity = newVelocity;
        Debug.Log($"RobotModel updated velocity to {newVelocity}");
        OnVelocityChanged?.Invoke(newVelocity);
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
