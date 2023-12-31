using UnityEngine;

public class StartTrafficLight : SingletonBase<StartTrafficLight>, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject m_RedLights;
    [SerializeField] private GameObject m_YellowLights;
    [SerializeField] private GameObject m_GreenLights;

    [SerializeField] private float m_RedLightsDelay;
    [SerializeField] private float m_YellowLightsDelay;
    [SerializeField] private float m_GreenLightsDelay;

    private float timer;
    private bool IsRaceReady = false;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        raceStateTracker.PreparationStarted += OnRacePreparationStarted;

        m_RedLights.SetActive(false);
        m_YellowLights.SetActive(false);
        m_GreenLights.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTracker.PreparationStarted -= OnRacePreparationStarted;
    }

    private void OnRacePreparationStarted()
    {
        IsRaceReady = true;
    }

    private void Update()
    {
        if (IsRaceReady == true)
        {
            timer += Time.deltaTime;
        }

        if (timer > m_RedLightsDelay)
        {
            m_RedLights.SetActive(true);
        }

        if (timer > m_YellowLightsDelay)
        {
            m_YellowLights.SetActive(true);
        }

        if (timer > m_GreenLightsDelay)
        {
            m_GreenLights.SetActive(true);

            enabled = false;
        }
    }
}
