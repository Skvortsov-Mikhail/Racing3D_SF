using UnityEngine;
using UnityEngine.UI;

public class UILapRecordTime : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>, IDependency<RaceTimeTracker>
{
    [SerializeField] private GameObject bestLapObject;
    [SerializeField] private GameObject lastLapObject;
    [SerializeField] private GameObject currentLapObject;
    [SerializeField] private Text bestLapText;
    [SerializeField] private Text lastLapText;
    [SerializeField] private Text currentLapTimeText;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private void Start()
    {
        raceStateTracker.Started += OnRaceStarted;
        raceStateTracker.Completed += OnRaceCompleted;
        raceResultTime.ResultUpdated += OnResultUpdated;

        bestLapObject.SetActive(false);
        lastLapObject.SetActive(false);
        currentLapObject.SetActive(false);
        currentLapTimeText.enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTracker.Started -= OnRaceStarted;
        raceStateTracker.Completed -= OnRaceCompleted;
        raceResultTime.ResultUpdated -= OnResultUpdated;
    }

    private void OnRaceStarted()
    {
        bestLapObject.SetActive(true);
        lastLapObject.SetActive(true);
        currentLapObject.SetActive(true);

        currentLapTimeText.enabled = true;

        bestLapText.text = StringTime.SecondToTimeString(raceResultTime.BestLapTime);
        lastLapText.text = StringTime.SecondToTimeString(raceResultTime.LastLapTime);
    }

    private void OnResultUpdated()
    {
        bestLapText.text = StringTime.SecondToTimeString(raceResultTime.BestLapTime);
        lastLapText.text = StringTime.SecondToTimeString(raceResultTime.LastLapTime);
    }

    private void OnRaceCompleted()
    {
        bestLapObject.SetActive(false);
        lastLapObject.SetActive(false);
        currentLapObject.SetActive(false);

        currentLapTimeText.enabled = false;
    }

    private void Update()
    {
        currentLapTimeText.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentLapTime);
    }
}
