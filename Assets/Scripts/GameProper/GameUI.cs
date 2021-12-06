using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button instructions;
    [SerializeField] private Image inst_msg;
    [SerializeField] private Button x;
    [SerializeField] private Button box;
    [SerializeField] private Button nobox;

    [SerializeField] private Canvas modal;
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject boundingBox;

    void Start()
    {
        inst_msg.gameObject.SetActive(false);
        x.gameObject.SetActive(false);
    }

    public void showSelection()
    {
        instructions.gameObject.SetActive(false);
        nobox.gameObject.SetActive(false);
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

    public void showBox()
    {
        box.gameObject.SetActive(true);
        nobox.gameObject.SetActive(false);
        boundingBox.gameObject.SetActive(true);
    }

    public void hideBox()
    {
        box.gameObject.SetActive(false);
        nobox.gameObject.SetActive(true);
        boundingBox.gameObject.SetActive(false);
    }
}

