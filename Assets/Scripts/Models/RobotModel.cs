using System;
using UnityEngine;

public class RobotModel
{
    #region ROS_Properties
    public Vector3 Velocity { get; private set; }
    public float velocityFR { get; private set; }
    public float velocityFL { get; private set; }
    public float velocityRR { get; private set; }
    public float velocityRL { get; private set; }
    public float ScalarVelocity { get; private set; }
    public Vector3 Pose { get; private set; }
    public float DistanceToWall { get; private set; }

    public float maxVelocity { get; private set; }
    public float maxSteering { get; private set; }

    public float wallDistance { get; private set; }

    private bool manualModeActive;
    public bool ManualModeActive
    {
        get => manualModeActive;
        private set
        {
            if (manualModeActive != value)
            {
                manualModeActive = value;
                Debug.Log($"Manual mode changed to {manualModeActive}");
                OnManualModeChanged?.Invoke(manualModeActive);
            }
        }
    }

    private AutoModes currentAutoMode;
    public AutoModes CurrentAutoMode
    {
        get => currentAutoMode;
        private set
        {
            if (currentAutoMode != value)
            {
                currentAutoMode = value;
                Debug.Log($"Auto mode changed to {currentAutoMode}");
                OnAutoModeChanged?.Invoke(currentAutoMode);
            }
        }
    }

    public enum AutoModes
    {
        None,
        Explore,
        Docking,
        Return
    }
    #endregion

    #region UI Properties
    private float speed;
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            OnSpeedChanged?.Invoke(speed);
        }
    }

    private bool emergencyStopActive;
    public bool EmergencyStopActive
    {
        get => emergencyStopActive;
        set
        {
            if (emergencyStopActive != value)
            {
                emergencyStopActive = value;
                Debug.Log($"Emergency stop changed to {emergencyStopActive}");
                OnEmergencyStopChanged?.Invoke(value);
            }
        }
    }
    #endregion

    #region Events
    public event Action<float> OnSpeedChanged;
    public event Action<float> OnVelocityChanged;
    public event Action<bool> OnEmergencyStopChanged;
    public event Action<bool> OnManualModeChanged;
    public event Action<AutoModes> OnAutoModeChanged;
    public event Action OnWheelVelocitiesChanged;
    public event Action OnMaxSteeringChanged;
    public event Action OnMaxVelocityChanged;
    public event Action OnWallDistanceChanged;
    #endregion

    public RobotModel()
    {
        ScalarVelocity = 0.0f;
        Pose = Vector3.zero;
        DistanceToWall = 0.0f;
        Speed = 100;
        EmergencyStopActive = false;
        ManualModeActive = false;
        CurrentAutoMode = AutoModes.None;
    }

    public void UpdateScalarVelocity(float newVelocity)
    {
        ScalarVelocity = newVelocity;
        OnVelocityChanged?.Invoke(newVelocity);
    }

    public void UpdateStateData(string stateData)
    {
        try
        {
            var state = JsonUtility.FromJson<RobotState>(stateData);

            // Handle emergency stop
            EmergencyStopActive = state.ebrake_active;

            // Handle manual mode independently
            ManualModeActive = state.manual_mode;

            // Handle auto modes
            if (state.explore_mode)
            {
                CurrentAutoMode = AutoModes.Explore;
            }
            else if (state.docking_mode)
            {
                CurrentAutoMode = AutoModes.Docking;
            }
            else if (state.return_mode)
            {
                CurrentAutoMode = AutoModes.Return;
            }
            else
            {
                CurrentAutoMode = currentAutoMode;
            }

            // Debug output for state updates
            Debug.Log($"Updated state: EmergencyStop={EmergencyStopActive}, ManualMode={ManualModeActive}, CurrentAutoMode={CurrentAutoMode}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to parse state data: {ex.Message}");
        }
    }

    public void UpdateWheelVelocities(float[] wheelVelocities)
    {
        if (wheelVelocities.Length == 4)
        {
            velocityFR = wheelVelocities[0];
            velocityFL = wheelVelocities[1];
            velocityRR = wheelVelocities[2];
            velocityRL = wheelVelocities[3];

            OnWheelVelocitiesChanged?.Invoke();
            //Debug.Log($"Updated wheel velocities: FR={velocityFR}, FL={velocityFL}, RR={velocityRR}, RL={velocityRL}");
        }
        else
        {
            Debug.LogError("Invalid wheel velocity data received.");
        }

    }

    public void UpdateMaxVelocity(float newMaxVelocity)
    {
        maxVelocity = newMaxVelocity;
        //Debug.Log($"Updated max velocity: {maxVelocity}");
        OnMaxVelocityChanged?.Invoke();
    }

    public void UpdateWallDistance(float newWallDistance)
    {
        wallDistance = newWallDistance;
        //Debug.Log($"Updated wall distance: {wallDistance}");
        OnWallDistanceChanged?.Invoke();
    }

    public void UpdateMaxSteering(float newMaxSteering)
    {
        maxSteering = newMaxSteering;
        //Debug.Log($"Updated max steering: {maxSteering}");
        OnMaxSteeringChanged?.Invoke();
    }

    [Serializable]
    private class RobotState
    {
        public bool ebrake_active;
        public bool manual_mode;
        public bool explore_mode;
        public bool docking_mode;
        public bool return_mode;
    }
}
