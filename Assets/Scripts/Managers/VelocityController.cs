using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using UnityEngine;

public class VelocityController : MonoBehaviour
{
    [SerializeField] private VelocityModel velocityModel;

    void Start()
    {
        if (velocityModel == null)
        {
            velocityModel = new VelocityModel();
        }
    }

    public void OnVelocityReceived(TwistMsg msg)
    {
        // Extract linear and angular velocities from the message
        Vector3 linear = new Vector3(
            (float)msg.linear.x,
            (float)msg.linear.y,
            (float)msg.linear.z
        );

        Vector3 angular = new Vector3(
            (float)msg.angular.x,
            (float)msg.angular.y,
            (float)msg.angular.z
        );

        // Update the model
        velocityModel.UpdateVelocity(linear, angular);
    }
}
