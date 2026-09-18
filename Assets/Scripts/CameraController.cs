using UnityEngine;
/************************************************************
* COMPONENT OF: main camera
* REQUIRED DEPENDENCIES: 
* DESCRIPTION: Controls the movement and behavior of the main camera
* AUTHOR: CKarimi
* DATE WRITTEN: 2026-09-18
* VERSION: 1.0
*************************************************************/
public class CameraController : MonoBehaviour
{[SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    // Update is called once per frame
    void LateUpdate()
    {transform.position = playerTransform.position + offset; 

        
    }
}
