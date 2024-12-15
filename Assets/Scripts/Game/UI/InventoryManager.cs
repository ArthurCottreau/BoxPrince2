using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{                                                           //Déclarations pour la barre d'inventaire
    private int maxTime;                                         //durée de la barre d'inventaire
    public float speedMultiplier;                               //multiplicateur quand main vide
    public Image fill;                                          //référence à l'image de chargement

    [SerializeField] private Image[] inv_slots;

    private float currentTime;
    private float timeSpeed = 1;

                                                            //Déclarations pour le système d'inventaire
    public List<PlatformScript> platformsList;                  // array pioche des différentes plateformes
    public List<PlatformScript> inventory;                      // array inventaire, 0 étant la main du joueur, limité à 4 slots plus tard
    private PlatformScript temp;                                // variable temp pour switch

    // Audio
    private AudioSource sfx;
    [SerializeField] private AudioClip audioBlockNew;
    [SerializeField] private AudioClip audioBlockSwap;
    [SerializeField] private AudioClip audioBlockDeny;

    public void NewBlock()
    {
        int rnd = Random.Range(0, platformsList.Count);         // tire un nombre aléatoire entre 0 et la taille de la liste des plateformes
        inventory.Add(platformsList[rnd]);                      // ajoute à l'inventaire la plateforme correspondante
        InvDisplay();
        sfx.clip = audioBlockNew;
        sfx.Play();
    }
    
    // pour une raison ou une autre, les bouttons refusent d'appeler des fonctions demandants une entrée.
    // Je voulais faire qu'une seule fonction InvSwap à la base mais bon...
    public void InvSwap(int inv_slot)
    {
        if (inventory.Count >= inv_slot + 1)
        {
            temp = inventory[inv_slot];
            inventory[inv_slot] = inventory[0];
            inventory[0] = temp;
            InvDisplay();
            sfx.clip = audioBlockSwap;
            sfx.Play();
        }
        else
        {
            Debug.LogWarning("Attention : Le slot d\'inventaire " + inv_slot + " est vide.");
            sfx.clip = audioBlockDeny;
            sfx.Play();
        }
    }

    private void multiply_speed()
    {
        fill.color = Color.gray;
        timeSpeed = speedMultiplier;
    }

    private void BarUpdate()
    {
        if (currentTime >= maxTime) // check si barre inventaire est remplie
        {
            if (inventory.Count < 4) // l'inventaire ne peut pas dépasser 4
            {
                NewBlock();
                timeSpeed = 1;
                fill.color = Color.black;
                currentTime = 0;
            }
        }
        else // remplissage barre si pas remplie
        {
            currentTime += (1 * Time.deltaTime * timeSpeed);
        }
        fill.fillAmount = currentTime / maxTime; // display barre
    }

    private void InvDisplay()
    {
        int size = inventory.Count;

        for (int i = 0; i < size; i++)
        {
            if (size > i)
            {
                inv_slots[i].sprite = inventory[i].icon;
                inv_slots[i].color = Color.white;
            }
        }
    }

    public PlatformScript GetActiveSlot()
    {
        int size = inventory.Count - 1;

        if (size >= 0)
        {
            PlatformScript platform = inventory[0];

            inv_slots[size].color = Color.clear;
            inv_slots[size].sprite = null;
            ClearActiveSlot();
            InvDisplay();

            if (size == 0) multiply_speed(); // Si vide augmente la vitesse

            return platform;
        }

        return null;
    }

    public PlatformScript GetInvSlot(byte a)
    {
        return inventory[a];
    }

    public void ClearActiveSlot()
    {
        inventory.RemoveAt(0);
    }

    private void Start()
    {
        currentTime = 0;
        fill.fillAmount = 0;
        InvDisplay();

        // Configure le volume
        sfx = gameObject.GetComponent<AudioSource>();
        GameManager gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
        sfx.volume = gameMan.sfxVolume / 100 / gameMan.sfxOffset;

        // Configure le temp d'attente pour un nouveau bloque
        switch (gameMan.difficulty)
        {
            case 0:
                maxTime = 3;
                break;
            case 1:
                maxTime = 4;
                break;
            case 2:
                maxTime = 5;
                break;
            default:
                maxTime = 100;
                Debug.Log("Erreur, la valeur lié à la difficulté est mal configurer !");
                break;
        }
    }

    private void Update()
    {
        BarUpdate();
    }
}
