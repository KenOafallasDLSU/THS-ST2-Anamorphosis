using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Canvas modal;
    [SerializeField] private Text timerText;
    [SerializeField] private Text finalTime;
    private float startTime;
    private bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
        modal.gameObject.SetActive(false);
        startTime = Time.time;

        EventBroadcaster.Instance.AddObserver(EventNames.Anamorphosis_Events.ON_WIN, this.Completed);
    }

    // Update is called once per frame
    void Update()
    {
        if (completed)
            return;

        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("00");

        timerText.text = minutes + ":" + seconds;
    }

    public void Completed()
    {
        completed = true;
        timerText.color = Color.yellow;

        modal.gameObject.SetActive(true);
        string time = timerText.text;
        finalTime.text = "You completed the puzzle in " + time;

        EventBroadcaster.Instance.RemoveObserver(EventNames.Anamorphosis_Events.ON_WIN);
    }
}
