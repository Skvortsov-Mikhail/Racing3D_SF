using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum RaceState
{
    Preparation,
    Countdown,
    Race,
    Passed
}

public class RaceStateTracker : MonoBehaviour, IDependency<TrackPointCircuit>
{
    public event UnityAction PreparationStarted;
    public event UnityAction Started;
    public event UnityAction Completed;
    public event UnityAction<TrackPoint> TrackPointPassed;
    public event UnityAction<int> LapCompleted;

    private TrackPointCircuit trackPointCircuit;
    public void Construct(TrackPointCircuit obj) => trackPointCircuit = obj;


    [SerializeField] private Timer countdownTimer;
    public Timer CountdownTimer => countdownTimer;

    [SerializeField] private int lapsToComplete;
    public int LapsToComplete => lapsToComplete;

    private RaceState state;
    public RaceState State => state;

    private void StartState(RaceState state)
    {
        this.state = state;
    }

    private void Start()
    {
        StartState(RaceState.Preparation);

        countdownTimer.enabled = false;
        countdownTimer.Finished += OnCountdownTimerFinished;

        trackPointCircuit.TrackPointTriggered += OnTrackPointTriggered;
        trackPointCircuit.LapCompleted += OnLapCompleted;
    }

    private void OnDestroy()
    {
        countdownTimer.Finished -= OnCountdownTimerFinished;

        trackPointCircuit.TrackPointTriggered -= OnTrackPointTriggered;
        trackPointCircuit.LapCompleted -= OnLapCompleted;
    }

    private void OnTrackPointTriggered(TrackPoint trackPoint)
    {
        TrackPointPassed?.Invoke(trackPoint);
    }

    private void OnCountdownTimerFinished()
    {
        StartRace();
    }

    private void OnLapCompleted(int lapAmount)
    {
        if (trackPointCircuit.Type == TrackType.Sprint)
        {
            CompleteRace();
        }

        if (trackPointCircuit.Type == TrackType.Circular)
        {
            CompleteLap(lapAmount);

            if (lapAmount == lapsToComplete)
            {
                CompleteRace();
            }
        }
    }

    public void LaunchPreparationStart()
    {
        if (state != RaceState.Preparation) return;

        StartState(RaceState.Countdown);

        countdownTimer.enabled = true;

        PreparationStarted?.Invoke();
    }

    private void StartRace()
    {
        if (state != RaceState.Countdown) return;

        StartState(RaceState.Race);

        Started?.Invoke();
    }

    private void CompleteRace()
    {
        if (state != RaceState.Race) return;

        StartState(RaceState.Passed);

        Completed?.Invoke();

        RaceCompletion.SaveRaceResult(SceneManager.GetActiveScene().name);
    }

    private void CompleteLap(int lapAmount)
    {
        LapCompleted?.Invoke(lapAmount);
    }
}
