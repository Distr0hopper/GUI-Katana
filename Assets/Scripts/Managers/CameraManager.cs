using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    * CameraFollow.cs
    * This script is used to make the camera follow the Pointcloud.
    * It is attached to the camera object.    
*/
public class CameraFollow : MonoBehaviour
{

    // Find the Pointcloud object

    bool PointcloudFound = false;
    GameObject Pointcloud;

    void Start()
    {
        Pointcloud = GameObject.Find("Drawing");
        if (Pointcloud != null)
        {
            PointcloudFound = true;
        }
    }

    void Update()
    {
        if (PointcloudFound)
        {
            // Get the position of the Pointcloud
            Vector3 PointcloudPosition = Pointcloud.transform.position;

            // Set the camera position to the Pointcloud position
            transform.position = new Vector3(PointcloudPosition.x, PointcloudPosition.y, PointcloudPosition.z - 10);
        }
    }
}
