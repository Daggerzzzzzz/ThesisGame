using UnityEngine;

public class CameraController : SingletonMonoBehavior<CameraController>
{
    [SerializeField]
    private float cameraSpeed = 1000;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private UIInGame inGameUI;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera bigMapCamera;

    [HideInInspector]
    public bool bigMapActive = false;
    
    

    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x - 0.95f, target.position.y - 0.4f, transform.position.z), cameraSpeed * Time.deltaTime);
        }
    }
    
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        bigMapActive = true;
        bigMapCamera.enabled = true;
        mainCamera.enabled = false;
        
        inGameUI.miniMap.SetActive(false);
        inGameUI.bigMapText.SetActive(true);
    }

    public void DeactivateBigMap()
    {
        bigMapActive = false;
        bigMapCamera.enabled = false;
        mainCamera.enabled = true;
        
        inGameUI.miniMap.SetActive(true);
        inGameUI.bigMapText.SetActive(false);
    }
}