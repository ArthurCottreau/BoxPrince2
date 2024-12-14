using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interactions : MonoBehaviour
{
    [SerializeField] private bool newMove;
    [SerializeField] private bool newJump;
    [SerializeField] private float slideSpeed = 3;
    [SerializeField] private bool willKill = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur entre dans la zone d'activation d'un block special, active les effets sur le joueur
        if (collision.name == "Player")
        {
            Player_Controller control = collision.gameObject.GetComponent<Player_Controller>();
            if (!willKill)
            {
                control.canMove = newMove;
                control.canJump = newJump;
                control.speed = slideSpeed;
            }
            else
            {
                control.Death();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Lorsque le joueur sort de la zone d'activation du block special, désactive les effets sur le joueur
        if (collision.name == "Player")
        {
            Player_Controller control = collision.gameObject.GetComponent<Player_Controller>();

            control.canMove = true;
            control.canJump = true;
            control.speed = 3;
        }
    }
}
