using UnityEngine;

[RequireComponent(typeof(Light))]
public class FrontHeadLight : MonoBehaviour
{
    public enum HeadLightState
    {
        Off,
        Low,
        High
    }
    

    [SerializeField] private float m_LowStateRange;
    [SerializeField] private float m_HighStateRange;

    [SerializeField] private float m_LowStateIntensity;
    [SerializeField] private float m_HighStateIntensity;

    [SerializeField] private float m_LowStateAngle;
    [SerializeField] private float m_HighStateAngle;


    private Light m_Light;
    private HeadLightState lightState = HeadLightState.Low;

    private void Start()
    {
        m_Light = GetComponent<Light>();

        SwitchLight();
    }

    private void SwitchLight()
    {
        switch (lightState)
        {
            case HeadLightState.Off:
                {
                    m_Light.enabled = false;
                    break;
                }

            case HeadLightState.Low:
                {
                    m_Light.enabled = true;

                    m_Light.range = m_LowStateRange;
                    m_Light.intensity = m_LowStateIntensity;
                    m_Light.spotAngle = m_LowStateAngle;

                    break;
                }

            case HeadLightState.High:
                {
                    m_Light.enabled = true;

                    m_Light.range = m_HighStateRange;
                    m_Light.intensity = m_HighStateIntensity;
                    m_Light.spotAngle = m_HighStateAngle;

                    break;
                }
        }
    }

    public void SwitchHeadLightState()
    {
        if (lightState == HeadLightState.Off)
            lightState = HeadLightState.Low;

        else if (lightState == HeadLightState.Low)
            lightState = HeadLightState.High;

        else if (lightState == HeadLightState.High)
            lightState = HeadLightState.Off;

        SwitchLight();
    }
}
