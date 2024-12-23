using System;
using System.Collections.Generic;
using System.Linq;
using RosMessageTypes.Sensor;
using Unity.Robotics.Visualizations;
using Model;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PointCloud2CustomVisualizerSettings",
    menuName = "Robotics/Sensor Visualizers/MyPointCloud2", order = 1)]
public class PointCloud2CustomVisualizerSettings : VisualizerSettingsGeneric<PointCloud2Msg>
{
    public enum ColorMode
    {
        HSV,
        SeparateRGB,
        CombinedRGB,
    }

    [HideInInspector, SerializeField] ColorMode m_ColorModeSetting;

    public ColorMode ColorModeSetting
    {
        get => m_ColorModeSetting;
        set => m_ColorModeSetting = value;
    }

    public string[] Channels
    {
        get => m_Channels;
        set => m_Channels = value;
    }

    string[] m_Channels;

    public string XChannel
    {
        get => m_XChannel;
        set => m_XChannel = value;
    }

    string m_XChannel = "x";

    public string YChannel
    {
        get => m_YChannel;
        set => m_YChannel = value;
    }

    string m_YChannel = "y";

    public string ZChannel
    {
        get => m_ZChannel;
        set => m_ZChannel = value;
    }

    string m_ZChannel = "z";

    public string HueChannel
    {
        get => m_HueChannel;
        set => m_HueChannel = value;
    }

    string m_HueChannel = "intensity";

    public string RgbChannel
    {
        get => m_RgbChannel;
        set => m_RgbChannel = value;
    }

    string m_RgbChannel = "rgb";

    public string RChannel
    {
        get => m_RChannel;
        set => m_RChannel = value;
    }

    string m_RChannel = "";

    public string GChannel
    {
        get => m_GChannel;
        set => m_GChannel = value;
    }

    string m_GChannel = "";

    public string BChannel
    {
        get => m_BChannel;
        set => m_BChannel = value;
    }

    string m_BChannel = "";

    public string SizeChannel
    {
        get => m_SizeChannel;
        set => m_SizeChannel = value;
    }

    string m_SizeChannel = "";

    public float[] HueRange
    {
        get => m_HueRange;
        set => m_HueRange = value;
    }

    float[] m_HueRange = { 0, 30 };

    public float[] RRange
    {
        get => m_RRange;
        set => m_RRange = value;
    }

    float[] m_RRange = { 0, 100 };

    public float[] GRange
    {
        get => m_GRange;
        set => m_GRange = value;
    }

    float[] m_GRange = { 0, 100 };

    public float[] BRange
    {
        get => m_BRange;
        set => m_BRange = value;
    }

    float[] m_BRange = { 0, 100 };

    public float[] SizeRange
    {
        get => m_SizeRange;
        set => m_SizeRange = value;
    }

    float[] m_SizeRange = { 0, 100 };

    public float Size
    {
        get => m_Size;
        set => m_Size = value;
    }

    float m_Size = 0.007f;

    public bool UseRgbChannel
    {
        get => m_UseRgbChannel;
        set => m_UseRgbChannel = value;
    }

    bool m_UseRgbChannel = true;

    public bool UseSizeChannel
    {
        get => m_UseSizeChannel;
        set => m_UseSizeChannel = value;
    }

    bool m_UseSizeChannel = true;

    /*
     * My Class Variables
     */
    private List<MyPointCloudDrawing> pointCloudList = new List<MyPointCloudDrawing>();

    [SerializeField] public float decayTime = 5.0f;

    // How many points to skip when drawing the point cloud (1 = show all, 40 = 40 times less points)
    [FormerlySerializedAs("denseFactor")] public int skipPoints = 1;
    Drawing3d drawing;

    public override void Draw(Drawing3d drawing, PointCloud2Msg message, MessageMetadata meta)
    {
        //if (Robot.Instance.ActiveRobot != Robot.ACTIVEROBOT.Charlie) return;
        this.drawing = drawing;
        float currentTime = Time.time;

        TFFrame currentTFFrame = TFSystem.instance.GetTransform(message.header);

        drawing.SetTFTrackingSettings(m_TFTrackingSettings, message.header);

        PointCloudDrawing pointCloud = PointCloudDrawing.Create(drawing.gameObject, (int)(message.data.Length / message.point_step));
        // Custom PointCloudDrawing object to store timestamp
        MyPointCloudDrawing myPointCloudDrawing = new MyPointCloudDrawing(pointCloud, currentTime);
        pointCloudList.Add(myPointCloudDrawing);

        Channels = message.fields.Select(field => field.name).ToArray();
        Dictionary<string, int> channelToIdx = new Dictionary<string, int>();
        for (int i = 0; i < message.fields.Length; i++)
        {
            channelToIdx.Add(message.fields[i].name, i);
        }

        Func<int, Color> colorGenerator = (int iPointStep) => Color.white;

        if (m_UseRgbChannel)
        {
            switch (ColorModeSetting)
            {
                case ColorMode.HSV:
                    if (m_HueChannel.Length > 0)
                    {
                        int hueChannelOffset = (int)message.fields[channelToIdx[m_HueChannel]].offset;
                        colorGenerator = (int iPointStep) =>
                        {
                            //int colC = BitConverter.ToInt16(message.data, (iPointStep + hueChannelOffset));
                            var colC = BitConverter.ToSingle(message.data, (iPointStep + hueChannelOffset));
                            // 0.5f to get half of the color wheel (because otherwise red is at the beginning and end of the color wheel)
                            return Color.HSVToRGB(0.5f * Mathf.InverseLerp(m_HueRange[0], m_HueRange[1], colC), 1, 1);
                        };
                    }

                    break;
                case ColorMode.SeparateRGB:
                    if (m_RChannel.Length > 0 && m_GChannel.Length > 0 && m_BChannel.Length > 0)
                    {
                        int rChannelOffset = (int)message.fields[channelToIdx[m_RChannel]].offset;
                        int gChannelOffset = (int)message.fields[channelToIdx[m_GChannel]].offset;
                        int bChannelOffset = (int)message.fields[channelToIdx[m_BChannel]].offset;
                        colorGenerator = (int iPointStep) =>
                        {
                            var colR = Mathf.InverseLerp(m_RRange[0], m_RRange[1],
                                BitConverter.ToSingle(message.data, iPointStep + rChannelOffset));
                            var colG = Mathf.InverseLerp(m_GRange[0], m_GRange[1],
                                BitConverter.ToSingle(message.data, iPointStep + gChannelOffset));
                            var colB = Mathf.InverseLerp(m_BRange[0], m_BRange[1],
                                BitConverter.ToSingle(message.data, iPointStep + bChannelOffset));
                            return new Color(colR, colG, colB, 1);
                        };
                    }

                    break;
                case ColorMode.CombinedRGB:
                    if (m_RgbChannel.Length > 0)
                    {
                        int rgbChannelOffset = (int)message.fields[channelToIdx[m_RgbChannel]].offset;
                        colorGenerator = (int iPointStep) => new Color32
                        (
                            message.data[iPointStep + rgbChannelOffset + 2],
                            message.data[iPointStep + rgbChannelOffset + 1],
                            message.data[iPointStep + rgbChannelOffset],
                            255
                        );
                    }

                    break;
            }
        }

        int xChannelOffset = (int)message.fields[channelToIdx[m_XChannel]].offset;
        int yChannelOffset = (int)message.fields[channelToIdx[m_YChannel]].offset;
        int zChannelOffset = (int)message.fields[channelToIdx[m_ZChannel]].offset;
        int sizeChannelOffset = 0;
        bool useSizeChannel = m_UseSizeChannel && m_SizeChannel != "";
        if (useSizeChannel)
            sizeChannelOffset = (int)message.fields[channelToIdx[m_SizeChannel]].offset;
        int maxI = message.data.Length / (int)message.point_step;
        
        // Iterate through all points in the point cloud, may skip some points depending on skipPoints
        skipPoints = Math.Max(1, skipPoints);

        for (int i = 0; i < maxI; i += skipPoints)
        {
            i = Math.Min(i, maxI - 1);
            int iPointStep = i * (int)message.point_step;
            var x = BitConverter.ToSingle(message.data, iPointStep + xChannelOffset);
            var y = BitConverter.ToSingle(message.data, iPointStep + yChannelOffset);
            var z = BitConverter.ToSingle(message.data, iPointStep + zChannelOffset);
            
            // Check for NaN and Infinity values and skip
            if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z) || float.IsInfinity(x) || float.IsInfinity(y) || float.IsInfinity(z))
            {
                continue;
            }
            
            //x = (float) Math.Cos(theta) * x - (float) Math.Sin(theta) * z;
            //z = (float) Math.Sin(theta) * x + (float) Math.Cos(theta) * z;
            //x += (float) Charlie.Instance.CurrentX;
            //z += (float)Charlie.Instance.CurrentZ;
            Vector3<FLU> rosPoint = new Vector3<FLU>(x, y, z);


            Vector3 unityPoint = rosPoint.toUnity;
            // Transform Point within current TF frame
            // unityPoint = currentTFFrame.TransformPoint(unityPoint);

            // Vector3 rotatedPoint = new Vector3();
            // rotatedPoint.x = Mathf.Cos((float) (0 * Mathf.Deg2Rad)) * unityPoint.x - Mathf.Sin((float) (0 * Mathf.Deg2Rad)) * unityPoint.z;
            // rotatedPoint.z = Mathf.Sin((float) (0 * Mathf.Deg2Rad)) * unityPoint.x + Mathf.Cos((float) (0 * Mathf.Deg2Rad)) * unityPoint.z;
            
            // unityPoint.x = rotatedPoint.x;
            // unityPoint.z = rotatedPoint.z;
            
            
            // // Transform Point with the Robot Position so that it changes position with the robot
            // unityPoint.x += 0;
            // unityPoint.z += 0;

            Color color = colorGenerator(iPointStep);

            float radius;
            if (useSizeChannel)
            {
                var size = BitConverter.ToSingle(message.data, iPointStep + sizeChannelOffset);
                radius = Mathf.InverseLerp(m_SizeRange[0], m_SizeRange[1], size) * m_Size;
            }
            else

            {
                radius = m_Size;
            }

            myPointCloudDrawing.pointCloudDrawing.AddPoint(unityPoint, color, radius);
        }

        // Go through List of MyPointCloudDrawing objects and destroy the ones that are older than decayTime
        for (int i = 0; i < pointCloudList.Count; i++)
        {
            var current = pointCloudList[i];
            if (currentTime - current.timestamp > decayTime)
            {
                current.pointCloudDrawing.gameObject.SetActive(false);
                if (!current.pointCloudDrawing.gameObject.activeSelf)
                {
                    DestroyCurrentPointCloud(current);
                    //Remove from List
                    pointCloudList.RemoveAt(i);
                }
            }
        }
    }

    public void DestroyAllPointclouds()
    {
        foreach (var pointCloud in pointCloudList)
        {
            //Destroy(pointCloud.pointCloudDrawing.gameObject.GetComponent<MeshRenderer>());
            Destroy(pointCloud.pointCloudDrawing.gameObject);
        }
        pointCloudList.Clear();
    }

    public void DestroyDrawing()
    {
        if (drawing == null) return;
        Destroy(drawing.gameObject);
        Destroy(drawing);
        pointCloudList.Clear();
    }

    public void ClearPointcloudList()
    {
        pointCloudList.Clear();
    }
    private void DestroyCurrentPointCloud(MyPointCloudDrawing current)
    {
        if(drawing == null) return;
        //Destroy game Object
        Destroy(current.pointCloudDrawing.gameObject);
        //Clear Mesh Filter
        current.pointCloudDrawing.m_Mesh.Clear();
        //current.pointCloudDrawing.Bake();
        //Destroy Mesh and Mesh Filter
        Destroy(current.pointCloudDrawing.gameObject.GetComponent<MeshFilter>());
        Destroy(current.pointCloudDrawing.gameObject.GetComponent<MeshFilter>().mesh);
        //Destroy PointCloudDrawing object
        Destroy(current.pointCloudDrawing);
    }
    


    /*
    public override Action CreateGUI(PointCloud2Msg message, MessageMetadata meta)
    {
        var formatDict = new Dictionary<PointField_Format_Constants, List<string>>();

        foreach (var field in message.fields)
            if (formatDict.ContainsKey((PointField_Format_Constants)field.datatype))
                formatDict[(PointField_Format_Constants)field.datatype].Add(field.name);
            else
                formatDict.Add((PointField_Format_Constants)field.datatype, new List<string> { field.name });

        var formats = "";
        foreach (var f in formatDict)
            if (f.Value.Count > 0)
                formats += $"{f.Key}: {string.Join(", ", f.Value)}\n";

        return () =>
        {
            message.header.GUI();
            GUILayout.Label($"Height x Width: {message.height}x{message.width}\nData length: {message.data.Length}\nPoint step: {message.point_step}\nRow step: {message.row_step}\nIs dense: {message.is_dense}");
            GUILayout.Label($"Channels:\n{formats}");
        };
    }
    */
}