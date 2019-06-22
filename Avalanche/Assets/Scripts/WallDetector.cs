using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {

    public PlayerController playerController;
    public int side;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            playerController.OnWall(side, other.GetComponent<BoxMover>().modifiedSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            playerController.OffWall();
        }
    }
}
