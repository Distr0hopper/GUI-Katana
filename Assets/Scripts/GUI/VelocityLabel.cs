using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VelocityLabel : MonoBehaviour
{
    [SerializeField] UIDocument UIDocument;
    #region UIElements
    private Label velocityLabel;
    #endregion

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;

        robotModel.OnVelocityChanged += UpdateVelocityLabel;
    }

    void Awake()
    {
        var root = UIDocument.rootVisualElement;
        velocityLabel = root.Q<Label>("VelocityValue");
    }

    private void UpdateVelocityLabel(float newVelocity)
    {
        Debug.Log($"VelocityLabel: Updated velocity to {newVelocity}");
        if (robotModel != null)
        {
            velocityLabel.text = newVelocity.ToString("F2");
        }
        else
        {
            Debug.LogWarning("RobotModel is null. Cannot update velocity label.");
        }
    }


}
