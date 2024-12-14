using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrownController : MonoBehaviour
{
    public Transform player;
    public float hoverDistance = 0.6f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Met en place une variable qui ne modifie pas la position de la couronne
        // et laisse la gravit� du rigidbody
        float targetyPosition = transform.position.y;

        // Qui est modifi�e si le joueur est en train de tomber
        if (transform.position.y <= player.position.y + hoverDistance)
        {
            rb.velocity = Vector2.zero; // Annule tout d�placement li� au rigidbody de la courrone
            targetyPosition = player.position.y + hoverDistance; // Place la couronne sur la t�te
        }

        transform.position = new Vector2(player.position.x, targetyPosition);
    }
}
