using UnityEngine;
using UnityEngine.UI;

public class UIRaceResults : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>, IDependency<RaceTimeTracker>
{
    [SerializeField] private GameObject raceResults;
    [SerializeField] private Text bestLapText;
    [SerializeField] private Text bestRaceText;
    [SerializeField] private Text currentRaceText;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private void Start()
    {   
        raceStateTracker.Completed += OnRaceCompleted;
        raceResultTime.ResultUpdated += OnResultUpdated;

        enabled = false;
        raceResults.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
        raceResultTime.ResultUpdated -= OnResultUpdated;
    }

    private void OnResultUpdated()
    {
        bestLapText.text = StringTime.SecondToTimeString(raceResultTime.BestLapTime);
        bestRaceText.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
    }

    private void OnRaceCompleted()
    {
        raceResults.SetActive(true);
        enabled = true;

        currentRaceText.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentTime);
    }
}
