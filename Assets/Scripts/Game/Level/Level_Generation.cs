using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Generation : MonoBehaviour
{
    [SerializeField] LevelChunks ch_l;

    private List<GameObject> ch_list = new List<GameObject>();
    private Transform cam_pos;
    private Vector3 create_pos = new Vector3(0, 2.0f, 0);
    private int rand_range;

    private int chunk_size = 20;

    void Start()
    {
        cam_pos = Camera.main.transform;
        ch_list.Add(transform.GetChild(0).gameObject);
        rand_range = ch_l.Chunks.Length;
    }

    private void Update()
    {
        createChunk();
        deleteChunk();
    }

    private void createChunk()
    {
        // Lorsque la caméra s'élève assez haut, un chunk sera génerer
        if (cam_pos.position.y >= create_pos.y + 5)
        {
            create_pos = new Vector3(0, create_pos.y + chunk_size, 0);

            // Prend un numéro aléatoire entre 0 et le nombre de chunk existant dans 'ch_l', puis le créais en tant que child de la grille
            int rand_num = Random.Range(0, rand_range);
            GameObject newchunk = Instantiate(ch_l.Chunks[rand_num], create_pos, Quaternion.identity, gameObject.transform);
            ch_list.Add(newchunk);  // Puis l'ajoute à une liste (pour que le chunk se fasse supprimer plus tard)
        }
    }

    private void deleteChunk()
    {
        // Supprime le chunk si il descend trop loin
        if (ch_list[0].transform.position.y < create_pos.y - chunk_size)
        {
            Destroy(ch_list[0]);
            ch_list.RemoveAt(0);
        }
    }
}
