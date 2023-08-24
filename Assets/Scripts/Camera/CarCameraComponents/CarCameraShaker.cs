using UnityEngine;

public class CarCameraShaker : CarCameraComponent
{
    // [SerializeField] [Range(0.0f, 1.0f)] private float normalizeSpeedShake;

    [SerializeField] private float shakeAmount;

    private void Update()
    {
        /*
        if(car.NormalizedLinearVelocity > normalizeSpeedShake)
        {
            transform.localPosition += Random.insideUnitSphere * shakeAmount * car.NormalizedLinearVelocity * Time.deltaTime;
        }
        */

        transform.localPosition += Random.insideUnitSphere * shakeAmount * car.NormalizedLinearVelocity * Time.deltaTime;
    }
}
