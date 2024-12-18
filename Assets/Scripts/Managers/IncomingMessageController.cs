using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

public class IncomingMessageController : MonoBehaviour
{


    private ROSConnection ROSConnection;
    [SerializeField] private string velocityTopic = "/car/velocity";
    [SerializeField] private string cameraTopic = "/tag_visualization";
    private VelocityController VelocityController;
    private CameraStreamController CameraStreamController;

    public void SetConnectionController(ConnectionController controller)
    {
        ROSConnection = controller.GetROSConnection();
    }

    public void SetVelocityController(VelocityController controller)
    {
        VelocityController = controller;
    }

    public void SetCameraStreamController(CameraStreamController controller)
    {
        CameraStreamController = controller;
    }


    public void InitilaizeSubscribers()
    {
        // Get Velocity Data
        ROSConnection.Subscribe<Float32Msg>(velocityTopic, msg => {
            VelocityController.OnVelocityReceived(msg);
        });

        // Get Camera Data
        ROSConnection.Subscribe<ImageMsg>(cameraTopic, msg => {
            CameraStreamController.RenderImageStream(msg);
        });
    }
}
