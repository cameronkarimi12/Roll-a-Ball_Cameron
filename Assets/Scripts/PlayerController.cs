/************************************************************
* COMPONENT OF: Player
* REQUIRED DEPENDENCIES: Rigidbody Component
* DESCRIPTION: It listens for WASD and Arrow key presses to set 
*              vertical and horizontal directions. It uses those
*              to push the player's Rigidbody in that direction
*              at a preset force.
* AUTHOR: CKarimi
* VERSION: 1.0
*************************************************************/
using UnityEngine;

public class PlayerController : MonoBehaviour
{// Movement fields
    private float horizontalMovement;
    private float verticalMovement;
    private float force;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {horizontalMovement = 0.707f;
verticalMovement = 0.707f;
force = 4.75f;

        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(horizontalMovement, 0, verticalMovement);
        GetComponent<Rigidbody>().AddForce(direction * force);
    }
}
