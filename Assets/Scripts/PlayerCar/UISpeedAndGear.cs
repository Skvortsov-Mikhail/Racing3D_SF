using UnityEngine;
using UnityEngine.UI;

public class UISpeedAndGear : MonoBehaviour
{
    [SerializeField] private Text m_SpeedText;
    [SerializeField] private Text m_GearText;
    [SerializeField] private Image m_EngineRpmImage;

    private Car Car;

    private void Start()
    {
        Car = GetComponent<Car>();
    }

    private void Update()
    {
        m_SpeedText.text = Car.LinearVelocity.ToString("F0");
        m_GearText.text = Car.GetGearName();
        m_EngineRpmImage.fillAmount = Car.NormalizedEngineRpmInCurrentGear;
    }
}
