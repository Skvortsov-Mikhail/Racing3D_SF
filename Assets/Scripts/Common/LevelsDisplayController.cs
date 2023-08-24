using UnityEngine;

public class LevelsDisplayController : MonoBehaviour
{
    private int completedRaces = 0;

    private UIRaceButton[] races;

    private void Start()
    {
        races = GetComponentsInChildren<UIRaceButton>();

        UpdateRacesAvailability();

        RaceCompletion.Instance.DataUpdated += UpdateRacesAvailability;

        print(races.Length);
    }

    private void OnDestroy()
    {
        RaceCompletion.Instance.DataUpdated -= UpdateRacesAvailability;
    }

    public void UpdateRacesAvailability()
    {
        completedRaces = RaceCompletion.GetCompletedRacesCount();

        ShowAvailableRaces();
        HideInaccessibleRaces();
    }

    private void ShowAvailableRaces()
    {
        for (int i = 0; i < completedRaces + 1; i++)
        {
            races[i].UnlockRace();

            if (i + 1 >= races.Length) break;
        }
    }

    private void HideInaccessibleRaces()
    {
        for (int i = completedRaces + 1; i < races.Length; i++)
        {
            races[i].LockRace();
        }
    }
}
