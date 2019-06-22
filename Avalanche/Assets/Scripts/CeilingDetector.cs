using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDetector : MonoBehaviour {

    public PlayerController playerController;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            playerController.Squish(other.GetComponent<BoxMover>().fallSpeed);
        }
    }
}
