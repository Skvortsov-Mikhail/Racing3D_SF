using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RaceResultTime : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>
{
    public const string SaveMark = "_player_best_time";
    public const string SaveLapMark = "_player_best__lap_time";

    public event UnityAction ResultUpdated;

    [SerializeField] private float goldTime;

    private float playerRecordTime;
    private float currentTime;

    // mine
    private float bestLapTime;
    public float BestLapTime => bestLapTime;
    private float lastLapTime;
    public float LastLapTime => lastLapTime;
    //

    public float GoldTime => goldTime;
    public float PlayerRecordTime => playerRecordTime;
    public float CurrentTime => currentTime;

    public bool IsRecordNotNull => playerRecordTime != 0;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        raceStateTracker.Completed += OnRaceCompleted;
        raceTimeTracker.LapTimeUpdated += OnLapCompleted;
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
        raceTimeTracker.LapTimeUpdated -= OnLapCompleted;
    }

    private void OnRaceCompleted()
    {
        float absoluteRecord = GetAbsoluteRecord();

        if (raceTimeTracker.CurrentTime < absoluteRecord || IsRecordNotNull == false)
        {
            playerRecordTime = raceTimeTracker.CurrentTime;

            Save();
        }

        currentTime = raceTimeTracker.CurrentTime;

        ResultUpdated?.Invoke();
    }

    private void OnLapCompleted()
    {
        lastLapTime = raceTimeTracker.PreviousLapTime;

        if (raceTimeTracker.PreviousLapTime < bestLapTime || bestLapTime == 0)
        {
            bestLapTime = lastLapTime;
        }

        ResultUpdated?.Invoke();
    }

    public float GetAbsoluteRecord()
    {
        if (playerRecordTime < goldTime && IsRecordNotNull == true)
        {
            return playerRecordTime;
        }

        else
        {
            return goldTime;
        }
    }

    private void Load()
    {
        playerRecordTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + SaveMark, 0);
        bestLapTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + SaveLapMark, 0);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + SaveMark, playerRecordTime);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + SaveLapMark, bestLapTime);
    }
}
