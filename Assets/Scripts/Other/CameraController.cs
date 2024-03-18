using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonMonoBehavior<CameraController>
{
    [SerializeField]
    private float cameraSpeed = 1000;
    [SerializeField]
    private Transform target;
    

    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                                                    target.position.x - 0.95f, 
                                                    target.position.y - 0.2f, 
                                                    transform.position.z),
                                                    cameraSpeed * Time.deltaTime);
        }
    }
    
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
