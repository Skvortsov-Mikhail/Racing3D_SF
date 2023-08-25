using UnityEngine;

public class CarCameraShaker : CarCameraComponent
{
    [SerializeField] private float shakeAmount;

    private void Update()
    {
        transform.localPosition += Random.insideUnitSphere * shakeAmount * car.NormalizedLinearVelocity * Time.deltaTime;
    }
}
