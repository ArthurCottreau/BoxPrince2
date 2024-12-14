using System.Collections.Generic;
using UnityEngine;

public class Build_Manager : MonoBehaviour
{
    [SerializeField] private Transform level_layer;
    [SerializeField] private Color col_build;
    [SerializeField] private Color col_unbuild;

    private SpriteRenderer sprite_rend;
    private Sprite default_sprite;
    private PolygonCollider2D poly_col;
    private List<GameObject> trig_list = new List<GameObject>();

    private InventoryManager inv_manag;
    private PlatformScript select_obj;
    private bool can_build = true;

    // Audio
    private AudioSource sfx;
    [SerializeField] private AudioClip audioPlace;
    [SerializeField] private AudioClip audioPlaceFail;

    private void Start()
    {
        sprite_rend = gameObject.GetComponent<SpriteRenderer>();
        default_sprite = sprite_rend.sprite;
        inv_manag = gameObject.GetComponent<InventoryManager>();
        poly_col = gameObject.GetComponent<PolygonCollider2D>();

        sfx = gameObject.GetComponent<AudioSource>();
        GameManager gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
        sfx.volume = gameMan.sfxVolume / 100 / gameMan.sfxOffset;
    }

    void Update()
    {
        // Place le 'curseur' à la position de la souris
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = pos;

        handleCursorLook();
        handleBuilding(pos);
    }

    private void handleCursorLook()
    {
        // Gère l'image que le curseur utilise et la collision du PolygonCollider2D
        if (inv_manag.inventory.Count > 0)
        {
            select_obj = inv_manag.GetInvSlot(0);
            sprite_rend.sprite = select_obj.icon;

            List<Vector2> physicsShape = new List<Vector2>();
            sprite_rend.sprite.GetPhysicsShape(0, physicsShape);
            poly_col.SetPath(0, physicsShape);
        }
        else
        {
            sprite_rend.sprite = default_sprite;
        }
    }

    private void handleBuilding(Vector2 pos)
    {
        // Si le joueur click, récupère l'objet dans l'emplacement actif de l'inventaire et le place à la position de la souris
        if (Input.GetMouseButtonDown(0))
        {
            if (can_build)
            {
                select_obj = inv_manag.GetActiveSlot();

                if (select_obj)
                {
                    sfx.clip = audioPlace;
                    sfx.Play();
                    Instantiate(select_obj.prefab, pos, Quaternion.identity, level_layer);
                }
            }
            else
            {
                sfx.clip = audioPlaceFail;
                sfx.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ajoute à la liste des triggers qui sont en contact avec le curseur
        trig_list.Add(collision.gameObject);

        can_build = false;
        sprite_rend.color = col_unbuild;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Retire le trigger que le curseur vient de quitter de la liste
        trig_list.Remove(collision.gameObject);

        // Si la liste est vide (donc le curseur est contact avec aucun trigger), autorise la contrustion
        if (trig_list.Count == 0)
        {
            can_build = true;
            sprite_rend.color = col_build;
        }
    }
}