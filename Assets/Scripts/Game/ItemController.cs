using UnityEngine;

public class ItemController : MonoBehaviour
{
    public PlatformScript Item;
    private Sprite ItemIcon;
    private float decaytime;
    private float decayMultiplier;
    private SpriteRenderer[] sprites;
    private float progress = 255;

    private void Start()
    {
        ItemIcon = Item.icon;
        decaytime = Item.decayTime;
        decayMultiplier = Item.decayMultiplier;
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        handle_TimeLeft();
        handle_Decay();
    }

    private void handle_TimeLeft()
    {
        // Détruit la platform le compte a rebourd decaytime atteint 0
        decaytime -= Time.deltaTime * decayMultiplier;

        if (decaytime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void handle_Decay()
    {
        // Gère l'effet de decay du block
        progress = decaytime * (255 / Item.decayTime);
        float prog_a = progress / 255;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = new Color(prog_a, prog_a, prog_a, 1);
        }
    }

    public void OnCollision()
    {
        decaytime -= Item.decayCollision;
    }

}
