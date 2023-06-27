using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeg : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Surface"))
        {
            player.legOnGround = true;      
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Surface"))
        {
            player.legOnGround = false;
        }
    }

}
