using UnityEngine;

public class CarInputControl : MonoBehaviour, IDependency<Car>
{
    [SerializeField] private AnimationCurve m_BrakeCurve;
    [SerializeField] private AnimationCurve m_SteerCurve;

    [SerializeField] [Range(0.0f, 1.0f)] private float autoBrakeStrength = 0.5f;

    [SerializeField] private float wheelSpeed;

    private Car car;
    public void Construct(Car obj) => car = obj;

    private float verticalAxis;
    private float horizontalAxis;
    private float handbrakeAxis;

    private bool m_IsHandBrakeUse;

    private void Update()
    {
        wheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateThrottleAndBrake();
        UpdateSteer();
        UpdateHandbrake();

        UpdateAutoBrake();


        if(Input.GetKeyDown(KeyCode.E))
        {
            car.UpGear();
        }  
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            car.DownGear();
        }
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        handbrakeAxis = Input.GetAxis("Jump");
    }

    private void UpdateThrottleAndBrake()
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
        {
            car.ThrottleControl = Mathf.Abs(verticalAxis);
            car.BrakeControl = 0;
        }

        else
        {
            car.ThrottleControl = 0;
            car.BrakeControl = m_BrakeCurve.Evaluate(wheelSpeed / car.MaxSpeed);
        }

        if(verticalAxis < 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f && !m_IsHandBrakeUse)
        {
            car.ShiftToReverseGear();
        }
        
        if (verticalAxis > 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f && !m_IsHandBrakeUse)
        {
            car.ShiftToFirstGear();
        }
    }

    private void UpdateSteer()
    {
        car.SteerControl = m_SteerCurve.Evaluate(wheelSpeed / car.MaxSpeed) * horizontalAxis;
    }
    
    private void UpdateHandbrake()
    {
        if (handbrakeAxis != 0)
            m_IsHandBrakeUse = true;
        else
            m_IsHandBrakeUse = false;

        car.HandBrakeControl = handbrakeAxis;
    }

    private void UpdateAutoBrake()
    {
        if(verticalAxis == 0)
        {
            car.BrakeControl = m_BrakeCurve.Evaluate(wheelSpeed / car.MaxSpeed) * autoBrakeStrength;
        }
    }

    public void Reset()
    {
        verticalAxis = 0;
        horizontalAxis = 0;
        handbrakeAxis = 0;

        car.ThrottleControl = 0;
        car.SteerControl = 0;
        car.BrakeControl = 0;
        car.HandBrakeControl = 0;
    }

    public void Stop()
    {
        Reset();

        car.BrakeControl = 1;
    }
}
