/*
using UnityEngine;
using UnityEngine.UI;

public class UITrafficLights : MonoBehaviour
{
    [SerializeField] private Text m_Text;

    [SerializeField] private float GoTextDuration;

    private StartTrafficLight trafficLight;

    private float timer;

    private void Start()
    {
        m_Text.enabled = true;

        trafficLight = StartTrafficLight.Instance;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < trafficLight.YellowTimer)
        {
            m_Text.color = new Color(0.75f, 0.08f, 0.08f);
            m_Text.text = (trafficLight.GreenTimer - timer + 0.5f).ToString("F0") + "...";
        }

        else if(timer < trafficLight.GreenTimer + 0.5f)
        {
            m_Text.color = new Color(0.75f, 0.75f, 0.08f);
            m_Text.text = (trafficLight.GreenTimer - timer + 0.5f).ToString("F0") + "...";
        }

        if(trafficLight.IsTrafficLightReady)
        {
            m_Text.color = new Color(0.08f, 0.75f, 0.08f);
            m_Text.text = "Go!!!";
        }

        if (timer > GoTextDuration + trafficLight.GreenTimer)
        {
            gameObject.SetActive(false);
        }
    }
}
*/
