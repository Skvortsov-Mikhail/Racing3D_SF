using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] m_WheelAxles;

    [SerializeField] private float m_WheelBaseLenght;

    [SerializeField] private Transform centerOfMass;

    [Header("DownForce")]
    [SerializeField] private float downForceMin;
    [SerializeField] private float downForceMax;
    [SerializeField] private float downForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float angularDragMin;
    [SerializeField] private float angularDragMax;
    [SerializeField] private float angularDragFactor;

    public float MotorTorque;
    public float SteerAngle;
    public float BrakeTorque;
    public float HandBrakeTorque;

    public float LinearVelocity => rigidbody.velocity.magnitude * 3.6f;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody == null? GetComponent<Rigidbody>() : rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (centerOfMass != null)
        {
            rigidbody.centerOfMass = centerOfMass.localPosition;
        }

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            m_WheelAxles[i].ConfigureVehicleSubsteps(50, 50, 50);
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();

        UpdateDownForce();

        UpdateWheelAxles();
    }

    public float GetAverageRpm()
    {
        float sum = 0;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            sum += m_WheelAxles[i].GetAvarageRpm();
        }

        return sum / m_WheelAxles.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAverageRpm() * m_WheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    public void Reset()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void UpdateAngularDrag()
    {
        rigidbody.angularDrag = Mathf.Clamp(angularDragFactor * LinearVelocity, angularDragMin, angularDragMax);
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);

        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            if (m_WheelAxles[i].IsMotor == true)
            {
                amountMotorWheel += 2;
            }
        }

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            m_WheelAxles[i].Update();

            m_WheelAxles[i].ApplyMotorTorque(MotorTorque / amountMotorWheel);
            m_WheelAxles[i].ApplySteerAngle(SteerAngle, m_WheelBaseLenght);
            m_WheelAxles[i].ApplyBrakeTorque(BrakeTorque);

            if (m_WheelAxles[i].IsHandbraken == true)
            {
                m_WheelAxles[i].ApplyBrakeTorque(HandBrakeTorque);
            }
        }
    }
}
