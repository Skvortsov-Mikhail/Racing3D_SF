using UnityEngine;

public class FrontHeadLightsControl : MonoBehaviour
{
    [SerializeField] private FrontHeadLight[] m_HeadLights;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            for(int i = 0; i < m_HeadLights.Length; i++)
            {
                m_HeadLights[i].SwitchHeadLightState();
            }
        }
    }
}
