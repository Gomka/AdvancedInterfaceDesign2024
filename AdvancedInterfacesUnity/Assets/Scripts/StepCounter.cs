using System;
using System.Collections;
using UnityEngine;

public class StepCounter : MonoBehaviour
{
    // Singleton setup
    private static StepCounter _instance;
    public static StepCounter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<StepCounter>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("StepCounter");
                    _instance = container.AddComponent<StepCounter>();
                }
            }
            return _instance;
        }
    }

    [Header("Configuration")]
    public StepCounterConfig config;
    [Header("Runtime Variables")]
    [SerializeField] public float distanceWalked = 0f;
    [SerializeField] public int stepCount = 0, stepsMinute = 0;
    private Vector3 acceleration;
    private Vector3 prevAcceleration;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError("Oops! StepCounterConfig is missing!");
            return;
        }
        prevAcceleration = Input.acceleration;
        StepDataHandler.Instance.CheckForNewDay();
    }

    private void Update()
    {
        if (config == null) return;
        DetectSteps();
        CalculateDistance();
        StepDataHandler.Instance.SaveDailySteps(stepCount);
    }


    private void DetectSteps()
    {
        acceleration = Input.acceleration;
        float delta = (acceleration - prevAcceleration).magnitude;
        if (delta > config.threshold)
        {
            stepCount++;
            Debug.Log($"Step detected! Count: {stepCount}");

            StartCoroutine(BPMCount());
        }
        prevAcceleration = acceleration;
    }

    IEnumerator BPMCount()
    {
        stepsMinute++;
        yield return new WaitForSeconds(60);
        stepsMinute--;
    }

    private void CalculateDistance()
    {
        distanceWalked = stepCount * config.stepLength;
    }

    public void CalibrateStepLength(float newStepLength)
    {
        if (newStepLength > 0)
        {
            config.stepLength = newStepLength;
            Debug.Log($"Step length calibrated to: {config.stepLength} meters");
        }
        else
        {
            Debug.LogWarning("Whoops! That's not a valid step length.");
        }
    }

    // Getter methods and data management
    public float GetDistanceWalked() => distanceWalked;
    public int GetStepCount() => stepCount;

    public void ResetStepData()
    {
        stepCount = 0;
        distanceWalked = 0f;
    }

    public void LoadStepData(int loadedStepCount)
    {
        stepCount = loadedStepCount;
        CalculateDistance();
    }
}