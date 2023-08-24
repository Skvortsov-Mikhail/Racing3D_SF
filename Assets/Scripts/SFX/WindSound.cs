using UnityEngine;

public class WindSound : MonoBehaviour, IDependency<Car>
{
    [SerializeField] private AudioSource windAudioSource;

    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float baseVolume = 0.4f;

    [SerializeField] private float pitchModifier;
    [SerializeField] private float volumeModifier;

    private Car car;
    public void Construct(Car obj) => car = obj;

    private void Update()
    {
        windAudioSource.pitch = Mathf.Clamp(basePitch + pitchModifier * car.NormalizedLinearVelocity, -3, 3);
        windAudioSource.volume = Mathf.Clamp(baseVolume + volumeModifier * car.NormalizedLinearVelocity, 0, 1);
    }
}
