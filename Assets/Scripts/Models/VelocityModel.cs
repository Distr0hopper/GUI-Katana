using System;
using UnityEngine;

[Serializable]
public class VelocityModel
{
    public Vector3 LinearVelocity { get; private set; }
    public Vector3 AngularVelocity { get; private set; }

    public delegate void VelocityUpdatedHandler();
    public event VelocityUpdatedHandler OnVelocityUpdated;

    public void UpdateVelocity(Vector3 linear, Vector3 angular)
    {
        LinearVelocity = linear;
        AngularVelocity = angular;

        // Notify listeners
        OnVelocityUpdated?.Invoke();
    }
}
