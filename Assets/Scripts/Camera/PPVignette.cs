using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPVignette : MonoBehaviour, IDependency<Car>
{
    [SerializeField] [Range(0.0f, 1.0f)] private float m_MinVignetteFactor;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_MaxVignetteFactor;

    private Car car;
    public void Construct(Car obj) => car = obj;

    private PostProcessVolume PPVol;
    private Vignette vignette;

    private void Start()
    {
        PPVol = GetComponent<PostProcessVolume>();
        PPVol.profile.TryGetSettings(out vignette);
    }

    private void Update()
    {
        vignette.intensity.value = Mathf.Lerp(m_MinVignetteFactor, m_MaxVignetteFactor, car.NormalizedLinearVelocity);
    }
}
