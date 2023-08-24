using System;
using UnityEngine;
using UnityEngine.Events;

public class RaceCompletion : SingletonBase<RaceCompletion>
{
    public event UnityAction DataUpdated;

    public const string racesFilename = "RaceCompletion.dat";

    [Serializable]
    private class RaceScore
    {
        public RaceInfo race;
        public bool isCompleted;
    }

    [SerializeField] private RaceScore[] completionData;

    private new void Awake()
    {
        base.Awake();

        Saver<RaceScore[]>.TryLoad(racesFilename, ref completionData);
    }

    public static void SaveRaceResult(string currentSceneName)
    {
        if (Instance != null)
        {
            foreach (var item in Instance.completionData)
            {
                if (item.race.SceneName == currentSceneName)
                {
                    if (item.isCompleted == false)
                    {
                        item.isCompleted = true;

                        Saver<RaceScore[]>.Save(racesFilename, Instance.completionData);
                    }
                }
            }
        }

        else
        {
            Debug.LogError("RaceCompletion not found");
        }
    }

    public static int GetCompletedRacesCount()
    {
        int score = 0;

        foreach (var item in Instance.completionData)
        {
            if (item.isCompleted == true)
            {
                score++;
            }
        }

        return score;
    }

    public static void ResetCompletionData()
    {
        foreach (var item in Instance.completionData)
        {
            item.isCompleted = false;
        }

        Instance.DataUpdated?.Invoke();
    }
}
