using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    public event UnityAction GearUpped;

    [SerializeField] private float m_MaxSteerAngle;
    [SerializeField] private float m_MaxBrakeTorque;
    [SerializeField] private float m_MaxHandBrakeTorque;

    [Header("Engine")]
    [SerializeField] private AnimationCurve m_EngineTorqueCurve;
    [SerializeField] private float m_EngineMaxTorque;

    //DEBUG
    [SerializeField] private float m_EngineTorque;
    [SerializeField] private float m_EngineRpm;
    //end

    [SerializeField] private float m_EngineMinRpm;
    [SerializeField] private float m_EngineMaxRpm;
    public float NormalizedEngineRpmInCurrentGear => m_EngineRpm / m_EngineMaxRpm;

    [Header("Gearbox")]
    [SerializeField] private float[] m_Gears;
    [SerializeField] private float m_FinalDriveRatio;
    [SerializeField] private int m_SelectedGearIndex;
    public int GearsAmount => m_Gears.Length;
    public int SelectedGearIndex => m_SelectedGearIndex;

    //DEBUG
    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float m_UpShiftEngineRpm;
    [SerializeField] private float m_DownShiftEngineRpm;

    [SerializeField] private float m_MaxSpeed;

    public float MaxSpeed => m_MaxSpeed;
    public float LinearVelocity => carChassis.LinearVelocity;
    public float NormalizedLinearVelocity => carChassis.LinearVelocity / m_MaxSpeed;
    public float WheelSpeed => carChassis.GetWheelSpeed();

    private CarChassis carChassis;
    public Rigidbody Rigidbody => carChassis == null ? GetComponent<CarChassis>().Rigidbody : carChassis.Rigidbody;

    // DEBUG
    [SerializeField] private float m_Speed;
    public float ThrottleControl;
    public float SteerControl;
    public float BrakeControl; // need for RearLights
    public float HandBrakeControl;
    //end

    private void Start()
    {
        carChassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        m_Speed = LinearVelocity;

        UpdateEngineTorque();

        AutoGearShift();

        if (LinearVelocity >= m_MaxSpeed)
            m_EngineTorque = 0;

        carChassis.MotorTorque = m_EngineTorque * ThrottleControl;
        carChassis.SteerAngle = m_MaxSteerAngle * SteerControl;
        carChassis.BrakeTorque = m_MaxBrakeTorque * BrakeControl;
        carChassis.HandBrakeTorque = m_MaxHandBrakeTorque * HandBrakeControl;
    }

    public void Reset()
    {
        carChassis.Reset();

        carChassis.MotorTorque = 0;
        carChassis.BrakeTorque = 0;
        carChassis.SteerAngle = 0;

        ThrottleControl = 0;
        BrakeControl = 0;
        SteerControl = 0;
        HandBrakeControl = 0;
    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Reset();

        transform.position = position;
        transform.rotation = rotation;
    }

    public void UpGear()
    {
        if (m_SelectedGearIndex == m_Gears.Length - 1) return;

        ShiftGear(m_SelectedGearIndex + 1);
        GearUpped?.Invoke();
    }

    public void DownGear()
    {
        ShiftGear(m_SelectedGearIndex - 1);
    }

    public void ShiftToReverseGear()
    {
        selectedGear = rearGear;
        m_SelectedGearIndex = -2;
    }

    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }

    public void ShiftToNeutral()
    {
        selectedGear = 0;
        m_SelectedGearIndex = -1;
    }

    public string GetGearName()
    {
        string gear = "";

        switch (m_SelectedGearIndex)
        {
            case >= 0:
                gear = (m_SelectedGearIndex + 1).ToString();
                break;
            case -1:
                gear = "N";
                break;
            case -2:
                gear = "R";
                break;
        }

        if(Mathf.Abs(LinearVelocity) < 0.5f)
        {
            gear = "N";
        }

        return gear;
    }
    private void AutoGearShift()
    {
        if (selectedGear < 0) return;

        if (m_EngineRpm >= m_UpShiftEngineRpm)
            UpGear();
        if (m_EngineRpm < m_DownShiftEngineRpm)
            DownGear();
    }

    private void ShiftGear(int gearIndex)
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, m_Gears.Length - 1);

        selectedGear = m_Gears[gearIndex];
        m_SelectedGearIndex = gearIndex;
    }

    private void UpdateEngineTorque()
    {
        m_EngineRpm = m_EngineMinRpm + Mathf.Abs(carChassis.GetAverageRpm() * selectedGear * m_FinalDriveRatio);
        m_EngineRpm = Mathf.Clamp(m_EngineRpm, m_EngineMinRpm, m_EngineMaxRpm);

        m_EngineTorque = m_EngineTorqueCurve.Evaluate(m_EngineRpm / m_EngineMaxRpm) * m_EngineMaxTorque * m_FinalDriveRatio * Mathf.Sign(selectedGear);
    }
}
