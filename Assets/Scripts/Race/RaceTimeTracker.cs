using UnityEngine;
using UnityEngine.Events;

public class RaceTimeTracker : MonoBehaviour, IDependency<RaceStateTracker>
{
    public event UnityAction LapTimeUpdated;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private float currentTime;
    public float CurrentTime => currentTime;

    private float previousLapTime;
    public float PreviousLapTime => previousLapTime;
    public float CurrentLapTime => currentTime - timeWithoutPrevious;

    private float timeWithoutPrevious;

    private void Start()
    {
        raceStateTracker.Started += OnRaceStarted;
        raceStateTracker.Completed += OnRaceCompleted;
        raceStateTracker.LapCompleted += OnLapCompleted;

        enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTracker.Started -= OnRaceStarted;
        raceStateTracker.Completed -= OnRaceCompleted;
        raceStateTracker.LapCompleted -= OnLapCompleted;
    }

    private void OnRaceStarted()
    {
        enabled = true;
        currentTime = 0;
        timeWithoutPrevious = 0;
    }

    private void OnRaceCompleted()
    {
        enabled = false;
    }

    private void OnLapCompleted(int _)
    {
        previousLapTime = currentTime - timeWithoutPrevious;

        timeWithoutPrevious += previousLapTime;

        LapTimeUpdated?.Invoke();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
    }
}
