using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button instructions;
    [SerializeField] private Image inst_msg;
    [SerializeField] private Button x;

    void Start()
    {
        inst_msg.gameObject.SetActive(false);
        x.gameObject.SetActive(false);
    }

    public void showSelection()
    {
        instructions.gameObject.SetActive(false);
    }

    public void showInst()
    {
        instructions.gameObject.SetActive(false);
        inst_msg.gameObject.SetActive(true);
        x.gameObject.SetActive(true);
    }

    public void closeInst()
    {
        instructions.gameObject.SetActive(true);
        inst_msg.gameObject.SetActive(false);
        x.gameObject.SetActive(false);
    }
}

