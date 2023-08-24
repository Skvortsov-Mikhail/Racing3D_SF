using UnityEngine;

[RequireComponent(typeof(Light))]
public class RearHeadLight : MonoBehaviour, IDependency<Car>
{
    [SerializeField] private float m_BrakeFactor;

    private Car car;
    public void Construct(Car obj) => car = obj;

    private Light m_Light;

    private void Start()
    {
        m_Light = GetComponent<Light>();
    }

    private void Update()
    {
        m_Light.enabled = car.BrakeControl > m_BrakeFactor;
    }
}
