using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvents : MonoBehaviour
{
    public Slider quickTimeSlider;
    public TMP_Text promptText;

    private bool isSliderFrozen;
    private QTEType currentEventType;
    private KeyCode keyToRespond;
    private readonly KeyCode[] availableKeys = { KeyCode.A, KeyCode.D, KeyCode.S };
    private const int MaxSliderValue = 10;
    private const int RapidPressStartValue = 5;
    private const string PassMessage = "Pass!";
    private const string FailMessage = "Fail!";

    private bool isEventActive = false;
    public bool isAttacking = false;
    public bool quickTimeEventCompleted= false;


    public enum QTEType { RapidPress, SinglePress, MatchPress };

    private KeyCode[] matchPressSequence;
    private int currentSequenceIndex = 0;
    public float decreaseSpeed;

    public void Attack(QTEType eventType)
    {
        isAttacking = true;
        currentEventType = eventType;

        switch (currentEventType)
        {
            case QTEType.RapidPress:
                StartRapidPressEvent();
                break;
            case QTEType.SinglePress:
                StartSinglePressEvent();
                break;
            case QTEType.MatchPress:
                StartMatchPressEvent();
                break;
        }

        isEventActive = true;
        isSliderFrozen = false;
    }

    private void StartRapidPressEvent()
    {
        keyToRespond = GetRandomKey();
        promptText.text = keyToRespond.ToString();
        quickTimeSlider.value = RapidPressStartValue;
    }

    private void StartSinglePressEvent()
    {
        keyToRespond = GetRandomKey();
        promptText.text = keyToRespond.ToString();
        quickTimeSlider.value = MaxSliderValue;
    }

    private void StartMatchPressEvent()
    {
        int sequenceLength = 3;
        matchPressSequence = new KeyCode[sequenceLength];
        quickTimeSlider.value = MaxSliderValue;
        // Generate random sequence
        for (int i = 0; i < sequenceLength; i++)
        {
            matchPressSequence[i] = GetRandomKey();
        }

        currentSequenceIndex = 0;

        // Convert KeyCode sequence to string and display
        promptText.text = KeyCodeSequenceToString(matchPressSequence);
    }

    private string KeyCodeSequenceToString(KeyCode[] sequence)
    {
        string sequenceString = "";

        for (int i = 0; i < sequence.Length; i++)
        {
            if (i > 0)
            {
                sequenceString += " -> ";  // Add an arrow between keys
            }

            sequenceString += sequence[i].ToString();
        }

        return sequenceString;
    }

    private KeyCode GetRandomKey()
    {
        int randomIndex = Random.Range(0, availableKeys.Length);
        return availableKeys[randomIndex];
    }

    void Update()
    {
        if (!isEventActive)
        {
            return;
        }

        if (!isSliderFrozen)
        {
            quickTimeSlider.value = Mathf.MoveTowards(quickTimeSlider.value, 0, decreaseSpeed * Time.deltaTime);
        }

        switch (currentEventType)
        {
            case QTEType.RapidPress:
                HandleRapidPressEvent();
                break;
            case QTEType.SinglePress:
                HandleSinglePressEvent();
                break;
            case QTEType.MatchPress:
                HandleMatchPressEvent();
                break;
        }

        if (quickTimeSlider.value == 0)
        {
            EventFailed();
        }
    }

    private void HandleRapidPressEvent()
    {
        if (Input.GetKeyDown(keyToRespond) && quickTimeSlider.value > 0)
        {
            quickTimeSlider.value += 1;

            if (quickTimeSlider.value == MaxSliderValue)
            {
                EventPassed();
            }
        }
    }

    private void HandleSinglePressEvent()
    {
        if (Input.GetKeyDown(keyToRespond) && quickTimeSlider.value > 0)
        {
            EventPassed();
        }
    }

    private void HandleMatchPressEvent()
    {
        if (Input.GetKeyDown(matchPressSequence[currentSequenceIndex]))
        {
            currentSequenceIndex++;

            if (currentSequenceIndex == matchPressSequence.Length)
            {
                EventPassed();
            }
            else
            {
                promptText.text = matchPressSequence[currentSequenceIndex].ToString();
            }
        }
    }

    private void EventPassed()
    {
        promptText.text = PassMessage;
        isSliderFrozen = true;
        isEventActive = false;
        quickTimeEventCompleted = true;
    }

    private void EventFailed()
    {
        promptText.text = FailMessage;
        isSliderFrozen = true;
        isEventActive = false;
        quickTimeEventCompleted = true;
    }
}