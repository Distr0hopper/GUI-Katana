using UnityEngine;
using RosMessageTypes.Std;

public class VelocityController : MonoBehaviour
{
    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;
    }

    public void OnWheelVelocitiesReceived(Float32MultiArrayMsg msg)
    {
        if (robotModel != null)
        {
            float[] wheelVelocities = msg.data; // Extract the wheel velocities
            //Debug.Log($"Received wheel velocities: {string.Join(", ", wheelVelocities)}");
            robotModel.UpdateWheelVelocities(wheelVelocities);
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update wheel velocities.");
        }
    }

    public void OnVelocityReceived(Float32Msg msg)
    {
        if (robotModel != null)
        {
            float scalarVelocity = msg.data; // Extract the scalar velocity
            robotModel.UpdateScalarVelocity(scalarVelocity);
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update velocity.");
        }
    }

    public void OnMaxVelocityReceived(Float32Msg msg)
    {
        if (robotModel != null)
        {
            float maxVelocity = msg.data; // Extract the max velocity
            robotModel.UpdateMaxVelocity(maxVelocity);
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update max velocity.");
        }
    }

    public void OnMaxSteeringReceived(Float32Msg msg)
    {
        if (robotModel != null)
        {
            float maxSteering = msg.data; // Extract the max steering
            robotModel.UpdateMaxSteering(maxSteering);
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update max steering.");
        }
    }
}
