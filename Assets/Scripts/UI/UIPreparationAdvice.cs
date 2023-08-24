using UnityEngine;

public class UIPreparationAdvice : MonoBehaviour, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject preparationAdvice;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;


    private void Start()
    {
        preparationAdvice.SetActive(true);

        raceStateTracker.PreparationStarted += OnPreparationStart;
    }

    private void OnDestroy()
    {
        raceStateTracker.PreparationStarted -= OnPreparationStart;
    }

    private void OnPreparationStart()
    {
        preparationAdvice.SetActive(false);
    }
}
