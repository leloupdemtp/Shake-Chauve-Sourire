using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GaugeSystem : MonoBehaviour
{
    [Header("State")]
    public bool isActive = false;

    [Header("Gauge Values")]
    [Range(0f, 100f)]
    public float currentValue = 50f;
    public float drainSpeed = 10f;

    [Header("Target Zone (%)")]
    public float targetMin = 80f;
    public float targetMax = 90f;

    [Header("Timer")]
    public float timeToMaintain = 3f;

    [Header("UI")]
    public Image gaugeFill;
    public GameObject endGaugeUI;

    [Header("Score")]
    public ScoreSystem scoreSystem;
    public int scorePerGauge = 100;

    [Header("Events")]
    public UnityEvent onGaugeStart;
    public UnityEvent onGaugeWin;
    public UnityEvent onGaugeLose;
    public UnityEvent onGaugeEnd;

    private float currentTimer;
    private bool finished;

    void Update()
    {
        if (!isActive || finished)
            return;

        Drain();
        UpdateUI();
        HandleTimer();
        CheckLose();
    }

    // =========================
    public void StartGauge()
    {
        ResetGauge();
        isActive = true;
        onGaugeStart.Invoke();
    }

    private void ResetGauge()
    {
        currentValue = Mathf.Clamp(currentValue, 0f, 100f);
        currentTimer = 0f;
        finished = false;
        endGaugeUI?.SetActive(false);
    }

    private void Drain()
    {
        currentValue -= drainSpeed * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0f, 100f);
    }

    private void HandleTimer()
    {
        if (currentValue >= targetMin && currentValue <= targetMax)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= timeToMaintain)
                Win();
        }
        else
        {
            currentTimer = 0f;
        }
    }

    private void CheckLose()
    {
        if (currentValue <= 0 || currentValue >= 100)
            Lose();
    }

    public void AddPoints(float amount)
    {
        if (!isActive) return;
        currentValue = Mathf.Clamp(currentValue + amount, 0f, 100f);
    }
    public void RemovePoints(float amount)
    {
        if (!isActive) return;
        currentValue = Mathf.Clamp(currentValue - amount, 0f, 100f);
    }

    // =========================
    private void Win()
    {
        finished = true;
        isActive = false;

        scoreSystem?.AddScore(scorePerGauge);

        onGaugeWin.Invoke();
        EndGauge();
    }

    private void Lose()
    {
        finished = true;
        isActive = false;

        onGaugeLose.Invoke();
        EndGauge();
    }

    private void EndGauge()
    {
        onGaugeEnd.Invoke();
        endGaugeUI?.SetActive(true);
    }

    public void CloseEndGaugeUI()
    {
        endGaugeUI?.SetActive(false);
    }

    private void UpdateUI()
    {
        if (gaugeFill != null)
            gaugeFill.fillAmount = currentValue / 100f;
    }
}
