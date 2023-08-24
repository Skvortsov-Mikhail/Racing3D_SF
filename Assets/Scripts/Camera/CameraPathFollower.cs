using UnityEngine;

public class CameraPathFollower : CarCameraComponent, IDependency<RaceStateTracker>
{
    [SerializeField] private Transform preparationPath;
    [SerializeField] private Transform comletedPath;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float preparationMovementSpeed;
    [SerializeField] private float completedMovementSpeed;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private Vector3[] preparationPoints;
    private int preparationPointIndex;

    private Vector3[] completedPoints;
    private int completedPointIndex;

    private void Start()
    {
        preparationPoints = new Vector3[preparationPath.childCount];

        for (int i = 0; i < preparationPoints.Length; i++)
        {
            preparationPoints[i] = preparationPath.GetChild(i).position;
        }

        completedPoints = new Vector3[comletedPath.childCount];

        for (int i = 0; i < completedPoints.Length; i++)
        {
            completedPoints[i] = comletedPath.GetChild(i).position;
        }
    }

    private void Update()
    {
        if(raceStateTracker.State == RaceState.Preparation)
        {
            transform.position = Vector3.MoveTowards(transform.position, preparationPoints[preparationPointIndex], preparationMovementSpeed * Time.deltaTime);

            if (transform.position == preparationPoints[preparationPointIndex])
            {
                if (preparationPointIndex == preparationPoints.Length - 1)
                {
                    preparationPointIndex = 0;
                }

                else
                {
                    preparationPointIndex++;
                }
            }
        }

        if(raceStateTracker.State == RaceState.Passed)
        {
            transform.position = Vector3.MoveTowards(transform.position, completedPoints[completedPointIndex], completedMovementSpeed * Time.deltaTime);

            if (transform.position == completedPoints[completedPointIndex])
            {
                if (completedPointIndex == completedPoints.Length - 1)
                {
                    completedPointIndex = 0;
                }

                else
                {
                    completedPointIndex++;
                }
            }
        }
        

        transform.LookAt(lookTarget);
    }

    public void StartMoveToNearestPoint()
    {
        float minDistance = float.MaxValue;

        for (int i = 0; i < preparationPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, preparationPoints[i]);

            if (distance < minDistance)
            {
                minDistance = distance;
                preparationPointIndex = i;
            }
        }
    }

    public void SetLookTarget(Transform target)
    {
        lookTarget = target;
    }
}
