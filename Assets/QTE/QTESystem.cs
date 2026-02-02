using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QTESystem : MonoBehaviour
{
    [System.Serializable]
    public class QTEStep
    {
        public InputActionReference inputAction;
        public float timeToPress = 1.5f;
    }

    [Header("Sequence")]
    public List<InputActionReference> randomActionPool;
    public int stepCount = 5;
    public float timePerStep = 1.5f;

    [Header("Timing")]
    public float delayBeforeNextKey = 0.5f;

    [Header("UI")]
    public GameObject qtePanel;
    public TextMeshProUGUI keyText;

    [Header("Gauge")]
    public GaugeSystem gaugeSystem;

    [Header("Events")]
    public UnityEvent onQTESuccess;
    public UnityEvent onQTELose;

    [Header("Sound Events")]
    public UnityEvent onGoodInput;
    public UnityEvent onBadInput;

    private List<QTEStep> sequence = new();
    private int index;

    public void StartQTE()
    {
        BuildSequence();
        index = 0;
        StartCoroutine(QTECoroutine());
    }

    private void BuildSequence()
    {
        sequence.Clear();
        for (int i = 0; i < stepCount; i++)
        {
            sequence.Add(new QTEStep
            {
                inputAction = randomActionPool[Random.Range(0, randomActionPool.Count)],
                timeToPress = timePerStep
            });
        }
    }

    private IEnumerator QTECoroutine()
    {
        qtePanel.SetActive(false);

        while (index < sequence.Count)
        {
            yield return new WaitForSeconds(delayBeforeNextKey);

            var step = sequence[index];
            qtePanel.SetActive(true);
            keyText.text = step.inputAction.action.GetBindingDisplayString();

            bool success = false;
            bool failed = false;

            step.inputAction.action.Enable();

            void OnCorrect(InputAction.CallbackContext ctx)
            {
                success = true;
                onGoodInput.Invoke();
            }

            step.inputAction.action.performed += OnCorrect;

            float timer = 0f;

            while (timer < step.timeToPress)
            {
                if (success || failed)
                    break;

                if (Keyboard.current.anyKey.wasPressedThisFrame && !success)
                {
                    failed = true;
                    onBadInput.Invoke();
                }

                timer += Time.deltaTime;
                yield return null;
            }

            step.inputAction.action.performed -= OnCorrect;
            step.inputAction.action.Disable();

            if (!success || failed)
            {
                LoseQTE();
                yield break;
            }

            qtePanel.SetActive(false);
            index++;
        }

        WinQTE();
    }

    private void WinQTE()
    {
        qtePanel.SetActive(false);
        onQTESuccess.Invoke();
        gaugeSystem.StartGauge();
    }

    private void LoseQTE()
    {
        qtePanel.SetActive(false);
        onQTELose.Invoke();
        StartQTE(); // 🔁 relance automatique
    }
}
