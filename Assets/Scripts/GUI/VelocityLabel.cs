using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VelocityLabel : MonoBehaviour
{
    [SerializeField] UIDocument UIDocument;
    #region UIElements
    private Label velocityLabel;
    private Label wheelFRLabel;  // Front Right Wheel Velocity
    private Label wheelFLLabel;  // Front Left Wheel Velocity
    private Label wheelRRLabel;  // Rear Right Wheel Velocity
    private Label wheelRLLabel;  // Rear Left Wheel Velocity
    private Label maxVelocityLabel;
    private Label maxSteeringLabel;
    #endregion

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;


        robotModel.OnVelocityChanged += UpdateVelocityLabel;
        robotModel.OnWheelVelocitiesChanged += UpdateWheelVelocities;
        robotModel.OnMaxSteeringChanged += UpdateSteeringLabel;
        robotModel.OnMaxVelocityChanged += UpdateMaxVelocityLabel;
    }

    void Awake()
    {
        var root = UIDocument.rootVisualElement;
        velocityLabel = root.Q<Label>("VelocityValue");
        wheelFRLabel = root.Q<Label>("LabelFR");
        wheelFLLabel = root.Q<Label>("LabelFL");
        wheelRRLabel = root.Q<Label>("LabelRR");
        wheelRLLabel = root.Q<Label>("LabelRL");
        maxSteeringLabel = root.Q<Label>("MaxSteeringLabel");
        maxVelocityLabel = root.Q<Label>("MaxVelocityLabel");

        // Check that all labels were found
        if (velocityLabel == null) Debug.LogError("VelocityValue label not found in the UI.");
        if (wheelFRLabel == null) Debug.LogError("FR label not found in the UI.");
        if (wheelFLLabel == null) Debug.LogError("FL label not found in the UI.");
        if (wheelRRLabel == null) Debug.LogError("RR label not found in the UI.");
        if (wheelRLLabel == null) Debug.LogError("RL label not found in the UI.");
        if (maxSteeringLabel == null) Debug.LogError("MaxSteeringLabel not found in the UI.");
        if (maxVelocityLabel == null) Debug.LogError("MaxVelocityLabel not found in the UI.");
    
    }


    private void UpdateSteeringLabel(){
        if (robotModel != null)
        {
            maxSteeringLabel.text = robotModel.maxSteering.ToString("F2");
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update steering label.");
        }
    }

    private void UpdateMaxVelocityLabel(){
        if (robotModel != null)
        {
            maxVelocityLabel.text = robotModel.maxVelocity.ToString("F2");
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update max velocity label.");
        }
    }

    private void UpdateVelocityLabel(float newVelocity)
    {
        if (robotModel != null)
        {
            velocityLabel.text = newVelocity.ToString("F2");
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update velocity label.");
        }
    }

     private void UpdateWheelVelocities()
    {
        //Debug.Log("Updating wheel velocities");
        if (robotModel != null)
        {
            if (wheelFRLabel != null)
                wheelFRLabel.text = robotModel.velocityFR.ToString("F2");
            if (wheelFLLabel != null)
                wheelFLLabel.text = robotModel.velocityFL.ToString("F2");
            if (wheelRRLabel != null)
                wheelRRLabel.text = robotModel.velocityRR.ToString("F2");
            if (wheelRLLabel != null)
                wheelRLLabel.text = robotModel.velocityRL.ToString("F2");
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update wheel velocity labels.");
        }
    }

}
