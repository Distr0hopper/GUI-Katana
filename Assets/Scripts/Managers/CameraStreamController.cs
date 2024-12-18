
using RosMessageTypes.Sensor;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEngine;


public class CameraStreamController : MonoBehaviour
{
    [SerializeField] public RenderTexture imageStream;
    private CameraStateModel cameraStateModel;
    private float noDataTimeout = 3.0f;
    private float lastDataTime; 


    void Start()
    {
        lastDataTime = Time.time;
    }

    public void RenderImageStream(ImageMsg msg)
    {
        Debug.Log("Received image stream.");
        lastDataTime = Time.time;
        cameraStateModel?.UpdateCameraState(true);

        Texture2D tex2D = msg.ToTexture2D();
        //tex2D = rotateTexture(tex2D, true);
        RenderTexture renderTex = RenderTexture.GetTemporary(
            tex2D.width,
            tex2D.height,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(tex2D, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        imageStream.Release();
        Graphics.Blit(tex2D, imageStream);

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        Destroy(tex2D);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastDataTime > noDataTimeout)
        {
            cameraStateModel.UpdateCameraState(false);
        }
    }

    public void SetCameraStateModel(CameraStateModel cameraStateModel)
    {
        // Set the camera state model
        this.cameraStateModel = cameraStateModel;
    }
}
