using UnityEngine;
using RosMessageTypes.Std;

public class ModeController : MonoBehaviour
{
    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;
    }


    public void OnStateDataReceived(StringMsg msg)
    {
        if (robotModel != null)
        {
            string stateData = msg.data;
            robotModel.UpdateStateData(stateData);
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update velocity.");
        }
    }
}
