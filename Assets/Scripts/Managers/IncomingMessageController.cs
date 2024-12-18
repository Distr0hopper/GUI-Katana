using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;

public class IncomingMessageController : MonoBehaviour
{


    private ROSConnection ROSConnection;
    [SerializeField] private string velocityTopic = "/robot/velocity";
    [SerializeField] private string cameraTopic = "/camera/color/image_raw/compressed";
    private VelocityController VelocityController;
    private CameraStreamController CameraStreamController;


    void Awake()
    {
        ROSConnection = ROSConnection.GetOrCreateInstance();
        
        // Find the controllers
        VelocityController = FindObjectOfType<VelocityController>();
        CameraStreamController = FindObjectOfType<CameraStreamController>();
    }
    void Start()
    {
        // Get Velocity Data
        ROSConnection.RegisterPublisher<TwistMsg>(velocityTopic);
        ROSConnection.Subscribe<TwistMsg>(velocityTopic, msg => {
            VelocityController.OnVelocityReceived(msg);
        });

        // Get Camera Data
        ROSConnection.RegisterPublisher<CompressedImageMsg>(cameraTopic);
        ROSConnection.Subscribe<CompressedImageMsg>(cameraTopic, msg => {
            CameraStreamController.RenderImageStream(msg);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
