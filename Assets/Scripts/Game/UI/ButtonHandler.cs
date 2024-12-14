using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private KeyCode butt_key;

    private Button inst_butt;

    void Start()
    {
        inst_butt = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(butt_key))
        {
            inst_butt.onClick.Invoke();
        }
    }
}
