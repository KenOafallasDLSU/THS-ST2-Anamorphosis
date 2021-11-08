using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image title;
    [SerializeField] private Button start;
    [SerializeField] private Button exit;
    [SerializeField] private Button instructions;
    [SerializeField] private Image inst_msg;
    [SerializeField] private Button x;
    [SerializeField] private Button one;
    [SerializeField] private Button two;
    [SerializeField] private Button three;
    [SerializeField] private Button four;
    [SerializeField] private Button five;
    [SerializeField] private Button six;
    [SerializeField] private Button seven;
    [SerializeField] private Button eight;
    [SerializeField] private Button nine;
    [SerializeField] private Button ten;


    void Start()
    {
        one.gameObject.SetActive(false);
        two.gameObject.SetActive(false);
        three.gameObject.SetActive(false);
        four.gameObject.SetActive(false);
        five.gameObject.SetActive(false);
        six.gameObject.SetActive(false);
        seven.gameObject.SetActive(false);
        eight.gameObject.SetActive(false);
        nine.gameObject.SetActive(false);
        ten.gameObject.SetActive(false);
        inst_msg.gameObject.SetActive(false);
        x.gameObject.SetActive(false);
    }

    public void showSelection()
    {
        start.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);

        one.gameObject.SetActive(true);
        two.gameObject.SetActive(true);
        three.gameObject.SetActive(true);
        four.gameObject.SetActive(true);
        five.gameObject.SetActive(true);
        six.gameObject.SetActive(true);
        seven.gameObject.SetActive(true);
        eight.gameObject.SetActive(true);
        nine.gameObject.SetActive(true);
        ten.gameObject.SetActive(true);
    }

    public void showInst()
    {
        start.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);
        inst_msg.gameObject.SetActive(true);
        x.gameObject.SetActive(true);
    }

    public void backToMain()
    {
        start.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        instructions.gameObject.SetActive(true);
        inst_msg.gameObject.SetActive(false);
        x.gameObject.SetActive(false);
    }

}
