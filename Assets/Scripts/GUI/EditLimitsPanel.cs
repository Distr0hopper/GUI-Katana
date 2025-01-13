using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EditLimitsPanel : MonoBehaviour
{

    [SerializeField] UIDocument UIDocument;
    #region UIElements

    private Button editSteering;
    private Button editVelocity;
    private Button closeEditButton;

    private FloatField velocityInput;
    private FloatField steeringInput;

    private VisualElement editPanel;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        var root = UIDocument.rootVisualElement;
        editSteering = root.Q<Button>("EditSteering");
        editVelocity = root.Q<Button>("EditVelocity");
        closeEditButton = root.Q<Button>("CloseEditButton");
        editPanel = root.Q<VisualElement>("EditPanel");
        velocityInput = root.Q<FloatField>("VelocityInput");
        steeringInput = root.Q<FloatField>("SteeringInput");

        // On button click open panel 
        editSteering.clicked += OnEditButtonClicked;
        editVelocity.clicked += OnEditButtonClicked;

        closeEditButton.clicked += OnCloseEditButtonClicked;




    }

    private void OnEditButtonClicked(){
        editPanel.AddToClassList("bottompanel-up");
    }

    private void OnCloseEditButtonClicked(){
        editPanel.RemoveFromClassList("bottompanel-up");
    }

}
