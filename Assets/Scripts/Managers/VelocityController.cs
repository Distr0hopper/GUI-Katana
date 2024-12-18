using UnityEngine;
using RosMessageTypes.Std;

public class VelocityController : MonoBehaviour
{
    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;
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
}
