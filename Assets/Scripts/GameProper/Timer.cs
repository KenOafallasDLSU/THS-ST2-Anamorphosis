using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject anamorphic;
    [SerializeField] private GameObject whole;

    [SerializeField] private Canvas modal;
    [SerializeField] private Text timerText;
    [SerializeField] private Text finalTime;
    private float startTime;
    private float totalTime;
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

        totalTime = Time.time - startTime;

        string minutes = ((int)totalTime / 60).ToString();
        string seconds = (totalTime % 60).ToString("00");

        timerText.text = minutes + ":" + seconds;
    }

    public void Completed()
    {
        completed = true;
        timerText.color = Color.yellow;

        // replace anamorphic model with whole model
        anamorphic.gameObject.SetActive(false);
        whole.gameObject.SetActive(true);

        // play SFX
        SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.WinSFX);

        StartCoroutine(ShowWinPopup());

        EventBroadcaster.Instance.RemoveObserver(EventNames.Anamorphosis_Events.ON_WIN);
    }

    public IEnumerator ShowWinPopup()
    {
        // 3 second delay
        yield return new WaitForSeconds(3);

        // show Win UI popup
        modal.gameObject.SetActive(true);
        string time = timerText.text;
        finalTime.text = "You completed the puzzle in " + time;
    }
}
