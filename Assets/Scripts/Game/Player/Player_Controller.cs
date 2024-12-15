using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player_Controller : MonoBehaviour
{
    // Variables liées aux stats du joueur
    [SerializeField] public float speed;   // Vitesse de déplacement
    [SerializeField] private float jump_force;  // Puissance du saut
    [SerializeField] private float jump_length; // Durée du saut
    [SerializeField] private float jbuffer_length;  // Durée avant d'avoir touché le sol, où le saut est toujours considéré valide
    [SerializeField] private float coyote_lenght;   // Durée du coyote time
    [SerializeField] private LayerMask layermask;

    // Variables qui récupèrent des éléments externes
    private Rigidbody2D player_rb;
    private BoxCollider2D player_coll;
    private ItemController last_touched;
    private Animator player_anim;
    private SpriteRenderer player_sprite;

    // Variables qui servent à gérer le saut
    private bool is_jumping = false;
    private float jump_timer = 0;
    private float coyote_timer = 0;
    private float jbuffer_timer = 0;
    private float speed_multi;   // Multiplicateur de vitesse pour quand le personnage saute

    // Variables liées aux contrôles
    private int direction = 1;
    private bool press_jump = false;
    private bool pressing_jump = false;

    // booléens permettant de contrôller le déplacement, le saut, et la mort
    public bool canJump = true;
    public bool canMove = true;
    public bool isDead = false;

    // Audio
    private AudioSource sfx;
    [SerializeField] private AudioClip audioJump;

    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        player_coll = GetComponent<BoxCollider2D>();
        player_anim = GetComponent<Animator>();
        player_sprite = GetComponent<SpriteRenderer>();

        sfx = gameObject.GetComponent<AudioSource>();
        GameManager gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
        sfx.volume = gameMan.sfxVolume / 100 / gameMan.sfxOffset;
    }

    void Update()
    {
        pressing_jump = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Space)) press_jump = true;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            handle_movement();
            handle_jump();
        }
        else
        {
            player_rb.velocity = Vector2.zero;
        }
    }

    private void handle_movement()
    {
        // Test la direction du joueur
        if (is_Touching(Vector2.right))
        {
            direction = -1;
            player_sprite.flipX = true;
        }

        if (is_Touching(Vector2.left))
        {
            direction = 1;
            player_sprite.flipX = false;
        }

        if (canMove)
        {
            player_rb.velocity = new Vector2(direction * (speed * speed_multi), player_rb.velocity.y);
            player_anim.SetBool("isRunning", true);
        }
        else
        {
            player_rb.velocity = Vector2.zero;
            player_anim.SetBool("isRunning", false);
        }
    }

    private void handle_jump()
    {
        // Si le joueur touche le sol
        if (is_Touching(Vector2.down))
        {
            player_anim.SetBool("isGrounded", true);

            speed_multi = 1;

            if (canJump)
            {
                coyote_timer = coyote_lenght;
            }
            else
            {
                coyote_timer = 0;
            }
        }
        else
        {
            player_anim.SetBool("isGrounded", false);

            if (player_rb.velocity.y > 0)
            {
                player_anim.SetTrigger("Jump");
            }
            else
            {
                player_anim.SetTrigger("Falling");
            }

            coyote_timer -= Time.deltaTime;
        }

        // Si le joueur vient juste d'appuyer sur la touche saut
        if (press_jump)
        {
            press_jump = false;
            jbuffer_timer = jbuffer_length;
        }

        // Vérifie si le joueur peut sauter
        if(jbuffer_timer > 0)
        {
            jbuffer_timer -= Time.deltaTime;

            if (coyote_timer > 0)
            {
                is_jumping = true;
                sfx.clip = audioJump;
                sfx.Play();
                jump_timer = jump_length;
                speed_multi = 1.65f;
                coyote_timer = 0;
            }
        }

        if (is_jumping)
        {
            // Applique la physique du saut lors de la durée du 'jump_timer'
            if (jump_timer > 0)
            {
                player_rb.velocity = new Vector2(player_rb.velocity.x, jump_force);
            }

            jump_timer -= Time.deltaTime;

            // Si le joueur lache la touche saut ou touche le plafond
            if (!pressing_jump || is_Touching(Vector2.up))
            {
                is_jumping = false;
                jump_timer = 0;
                speed_multi = 1;
            }
        }
    }

    public void Death()
    {
        isDead = true;
        GameObject.Find("CanvasUI").GetComponent<GameOver>().Death();
    }

    private bool is_Touching(Vector2 direction)
    {
        RaycastHit2D raycast = Physics2D.BoxCast(player_coll.bounds.center, player_coll.bounds.size, 0f, direction, 0.05f, layermask);

        if (raycast)
        {
            ItemController itemController;
            if (raycast.transform.TryGetComponent<ItemController>(out itemController))
            {
                if (last_touched != itemController)
                {
                    itemController.OnCollision();
                    last_touched = itemController;
                }
            }
            else
            {
                last_touched = null;
            }
        }

        return raycast.collider != null;
    }
}
