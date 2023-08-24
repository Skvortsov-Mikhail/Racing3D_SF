using UnityEngine;

[System.Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider m_LeftWheelCollider;
    [SerializeField] private WheelCollider m_RightWheelCollider;

    [SerializeField] private Transform m_LeftWheelMesh;
    [SerializeField] private Transform m_RightWheelMesh;

    [SerializeField] private bool m_IsMotor;
    [SerializeField] private bool m_IsSteer;
    [SerializeField] private bool m_IsHandbraken;

    [Header("Physics Parameters")]
    [SerializeField] private float m_WheelAxleWidth;

    [SerializeField] private float m_AntiRollForce;

    [SerializeField] private float m_AdditionalWheelDownForce;

    [SerializeField] private float baseForwardStiffnes = 1.5f;
    [SerializeField] private float stabilityForwardFactor = 1.0f;

    [SerializeField] private float baseSidewaysStiffness = 2.0f;
    [SerializeField] private float stabilitySidewaysFactor = 1.0f;

    private WheelHit leftWheelHit;
    private WheelHit rightWheelHit;

    public bool IsMotor => m_IsMotor;
    public bool IsSteer => m_IsSteer;
    public bool IsHandbraken => m_IsHandbraken;

    public void Update()
    {
        UpdateWheelHit();

        ApplyAntiRoll();
        ApplyDownForce();
        CorrectStiffness();

        SyncMeshTransform();
    }

    #region public API

    public void ConfigureVehicleSubsteps(float speedThreshold, int speedBelowThreshold, int stepsAboveThreshold)
    {
        m_LeftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
        m_RightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
    }

    public void ApplyMotorTorque(float motorTorque)
    {
        if (m_IsMotor == false) return;

        m_LeftWheelCollider.motorTorque = motorTorque;
        m_RightWheelCollider.motorTorque = motorTorque;
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLenght)
    {
        if (m_IsSteer == false) return;

        float radius = Mathf.Abs(wheelBaseLenght * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steerAngle))));
        float angleSign = Mathf.Sign(steerAngle);

        if (steerAngle > 0)
        {
            m_LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (m_WheelAxleWidth * 0.5f))) * angleSign;
            m_RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (m_WheelAxleWidth * 0.5f))) * angleSign;
        }

        else if (steerAngle < 0)
        {
            m_LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (m_WheelAxleWidth * 0.5f))) * angleSign;
            m_RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (m_WheelAxleWidth * 0.5f))) * angleSign;
        }

        else
        {
            m_LeftWheelCollider.steerAngle = 0;
            m_RightWheelCollider.steerAngle = 0;
        }

        m_LeftWheelCollider.steerAngle = steerAngle;
        m_RightWheelCollider.steerAngle = steerAngle;
    }

    public void ApplyBrakeTorque(float brakeTorque)
    {
        m_LeftWheelCollider.brakeTorque = brakeTorque;
        m_RightWheelCollider.brakeTorque = brakeTorque;
    }

    public float GetAvarageRpm()
    {
        return (m_LeftWheelCollider.rpm + m_RightWheelCollider.rpm) * 0.5f;
    }

    public float GetRadius()
    {
        return m_LeftWheelCollider.radius;
    }

    #endregion

    private void UpdateWheelHit()
    {
        m_LeftWheelCollider.GetGroundHit(out leftWheelHit);
        m_LeftWheelCollider.GetGroundHit(out rightWheelHit);
    }

    private void ApplyAntiRoll()
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (m_LeftWheelCollider.isGrounded == true)
        {
            travelL = (-m_LeftWheelCollider.transform.InverseTransformPoint(leftWheelHit.point).y - m_LeftWheelCollider.radius) / m_LeftWheelCollider.suspensionDistance;
        }

        if (m_RightWheelCollider.isGrounded == true)
        {
            travelR = (-m_RightWheelCollider.transform.InverseTransformPoint(rightWheelHit.point).y - m_RightWheelCollider.radius) / m_RightWheelCollider.suspensionDistance;
        }

        float forceDir = (travelL - travelR);

        if (m_LeftWheelCollider.isGrounded == true)
        {
            m_LeftWheelCollider.attachedRigidbody.AddForceAtPosition(m_LeftWheelCollider.transform.up * -forceDir * m_AntiRollForce, m_LeftWheelCollider.transform.position);
        }

        if (m_RightWheelCollider.isGrounded == true)
        {
            m_RightWheelCollider.attachedRigidbody.AddForceAtPosition(m_RightWheelCollider.transform.up * forceDir * m_AntiRollForce, m_RightWheelCollider.transform.position);
        }
    }

    private void ApplyDownForce()
    {
        if (m_LeftWheelCollider.isGrounded == true)
        {
            m_LeftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelHit.normal * -m_AdditionalWheelDownForce *
                m_LeftWheelCollider.attachedRigidbody.velocity.magnitude, m_LeftWheelCollider.transform.position);
        }

        if (m_RightWheelCollider.isGrounded == true)
        {
            m_RightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelHit.normal * -m_AdditionalWheelDownForce *
                m_RightWheelCollider.attachedRigidbody.velocity.magnitude, m_RightWheelCollider.transform.position);
        }
    }

    private void CorrectStiffness()
    {
        WheelFrictionCurve leftForward = m_LeftWheelCollider.forwardFriction;
        WheelFrictionCurve rightForward = m_RightWheelCollider.forwardFriction;

        leftForward.stiffness = baseForwardStiffnes + Mathf.Abs(leftWheelHit.forwardSlip) * stabilityForwardFactor;
        rightForward.stiffness = baseForwardStiffnes + Mathf.Abs(rightWheelHit.forwardSlip) * stabilityForwardFactor;

        m_LeftWheelCollider.forwardFriction = leftForward;
        m_RightWheelCollider.forwardFriction = rightForward;


        WheelFrictionCurve leftSideways = m_LeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = m_RightWheelCollider.sidewaysFriction;

        leftSideways.stiffness = baseSidewaysStiffness + Mathf.Abs(leftWheelHit.sidewaysSlip) * stabilitySidewaysFactor;
        rightSideways.stiffness = baseSidewaysStiffness + Mathf.Abs(rightWheelHit.sidewaysSlip) * stabilitySidewaysFactor;

        m_LeftWheelCollider.sidewaysFriction = leftSideways;
        m_RightWheelCollider.sidewaysFriction = rightSideways;
    }

    private void SyncMeshTransform()
    {
        UpdateWheelTransform(m_LeftWheelCollider, m_LeftWheelMesh);
        UpdateWheelTransform(m_RightWheelCollider, m_RightWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
