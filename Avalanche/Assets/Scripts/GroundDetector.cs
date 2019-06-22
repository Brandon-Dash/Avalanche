using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour {

    public PlayerController playerController;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            playerController.OnGround(true);
        }
        else if (other.tag == "Water")
        {
            playerController.hitWater();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            playerController.OnGround(false);
        }
    }
}
