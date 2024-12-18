using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SliderFilled : MonoBehaviour
{

    private VisualElement root;
    private VisualElement slider;
    private VisualElement dragger;
    private VisualElement bar;

    private VisualElement newDragger;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slider = root.Q<VisualElement>("SpeedSlider");
        dragger = slider.Q<VisualElement>("unity-dragger");
        

        // Add bar to the dragger
        AddElements();

        // When the slider value changes, update the bar
        slider.RegisterCallback<ChangeEvent<int>>(SliderValueChanged);

        // When the sliders layout changes, update the bar
        slider.RegisterCallback<GeometryChangedEvent>(SliderInit);
    }

    void AddElements(){
        bar = new VisualElement();
        dragger.Add(bar);
        bar.name = "FilledPortion";
        bar.AddToClassList("bar");

        newDragger = new VisualElement();
        slider.Add(newDragger);
        newDragger.name = "NewDragger";
        newDragger.AddToClassList("new-dragger");
        newDragger.pickingMode = PickingMode.Ignore;
    }

    void SliderInit(GeometryChangedEvent evt){
        Vector2 dist = new Vector2((newDragger.layout.width - dragger.layout.width) / 2,(newDragger.layout.height - dragger.layout.height) /2);
        Vector2 pos = dragger.parent.LocalToWorld(dragger.transform.position);
        newDragger.transform.position = newDragger.parent.WorldToLocal(pos-dist);
    }

    void SliderValueChanged(ChangeEvent<int> evt){
        Vector2 dist = new Vector2((newDragger.layout.width - dragger.layout.width) / 2,(newDragger.layout.height - dragger.layout.height) /2);
        Vector2 pos = dragger.parent.LocalToWorld(dragger.transform.position);
        newDragger.transform.position = newDragger.parent.WorldToLocal(pos-dist);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
