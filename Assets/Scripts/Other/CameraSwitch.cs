using UnityEngine;
using Cinemachine;

public class CameraSwitch : SingletonMonoBehavior<CameraSwitch>
{
    [SerializeField]
    private CinemachineVirtualCamera[] cinemachineCameras;
    [SerializeField]
    private CinemachineVirtualCamera playerCinemachine;
    [SerializeField]
    private CinemachineVirtualCamera mapCinemachine;
    [SerializeField]
    private CinemachineVirtualCamera startCamera;

    private CinemachineVirtualCamera currentCamera;

    private void Start() 
    {
        currentCamera = startCamera;

        for(int i = 0; i < cinemachineCameras.Length; i++)
        {
            if(cinemachineCameras[i] == currentCamera)
            {
                cinemachineCameras[i].Priority = 20;
            }
            else
            {
                cinemachineCameras[i].Priority = 10;
            }
        }
    }

    public void PlayerEnter()
    {
        SwitchToCamera(mapCinemachine);      
    }
    public void PlayerExit() 
    {
        SwitchToCamera(playerCinemachine);
    }
    private void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        currentCamera = targetCamera;
        currentCamera.Priority = 20;

        for(int i = 0; i < cinemachineCameras.Length; i++)
        {
            if(cinemachineCameras[i] != currentCamera)
            {
                cinemachineCameras[i].Priority = 10;
            }
        }
    }
}
