using UnityEngine;

public class EngineSound : MonoBehaviour, IDependency<Car>
{
    [Header("EngineAudio")]
    [SerializeField] private AudioSource engineAudioSource;

    [SerializeField] private float pitchModifier;
    [SerializeField] private float volumeModifier;
    [SerializeField] private float rpmModifier;
    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float baseVolume = 0.4f;

    [Space(10)]
    [SerializeField] private AudioSource NeutralAudioSource;
    [SerializeField] private AudioSource UpGearAudioSource;

    private Car car;
    public void Construct(Car obj) => car = obj;

    private void Start()
    {
        car.GearUpped += OnGearUpped;
    }

    private void Update()
    {
        if (car.GetGearName().Equals("N"))
        {
            if (NeutralAudioSource.isPlaying == false)
            {
                NeutralAudioSource.Play();
            }

            engineAudioSource.Stop();
        }
        else
        {
            if (engineAudioSource.isPlaying == false)
            {
                engineAudioSource.Play();
            }

            NeutralAudioSource.Stop();
        }

        if (engineAudioSource.isPlaying == true)
        {
            engineAudioSource.pitch = basePitch + pitchModifier * (car.NormalizedEngineRpmInCurrentGear * rpmModifier);
            engineAudioSource.volume = baseVolume + volumeModifier * car.NormalizedEngineRpmInCurrentGear;
        }
    }

    public void OnGearUpped()
    {
        UpGearAudioSource.Play();
    }

    private void OnDestroy()
    {
        car.GearUpped -= OnGearUpped;
    }
}
