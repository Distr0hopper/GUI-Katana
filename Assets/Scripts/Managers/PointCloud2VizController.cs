using UnityEngine;
using Unity.Robotics.Visualizations;
using RosMessageTypes.Sensor;

public class PointCloud2VizController : MonoBehaviour 
{
    private PointCloud2CustomVisualizer visualizer;
    private Drawing3d drawing;
    private PointCloud2CustomVisualizerSettings settings;
    
    void Start()
    {
        // Get reference to the visualizer component
        visualizer = FindObjectOfType<PointCloud2CustomVisualizer>();
        
        // Create a new Drawing3d instance
        GameObject drawingObject = new GameObject("PointCloudDrawing");
        drawing = drawingObject.AddComponent<Drawing3d>();
        
        // Create settings instance
        settings = ScriptableObject.CreateInstance<PointCloud2CustomVisualizerSettings>();
        
        // Configure settings as needed
        settings.Size = 0.005f;
        settings.UseRgbChannel = true;
        settings.ColorModeSetting = PointCloud2CustomVisualizerSettings.ColorMode.HSV;
        settings.skipPoints = 0;
        settings.decayTime = 0;
    }

    public void OnPointCloudReceived(PointCloud2Msg message)
    {
        if (settings != null)
        {
            // Draw the pointcloud using the settings
            settings.Draw(drawing, message, new MessageMetadata());
        }
    }
    
    void OnDestroy()
    {
        // Clean up
        if (settings != null)
        {
            settings.DestroyDrawing();
            Destroy(settings);
        }
    }
}