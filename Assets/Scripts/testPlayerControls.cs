using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayerControls : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotation = 90.0f;
        
    void Update()
    {        
        //Looks for A key or Left Arrow input
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {  
            //Sets new position to be 1 unit to the left
            Vector3 newPos = new Vector3(transform.position.x - speed, transform.position.y, 0);

                transform.position = newPos;
                transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        //Looks for D key or Right Arrow input
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        { 
            //Sets new position to be 1 unit to the right
            Vector3 newPos = new Vector3(transform.position.x + speed, transform.position.y, 0);
                //Sets the player's new position and rotation
                transform.position = newPos;
                transform.rotation = Quaternion.Euler(0, 0, 3 * rotation);
                    
        }

        //Looks for S key or Down Arrow input
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        { 
            //Sets new position to be 1 unit down
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y - speed, 0);

                transform.position = newPos;
                transform.rotation = Quaternion.Euler(0, 0, 2 * rotation);
        }

        //Looks for W key or Up Arrow input
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        { 
            //Sets new position to be 1 unit up
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + speed, 0);

                transform.position = newPos;
                transform.rotation = Quaternion.identity;          
        }
    }
}
