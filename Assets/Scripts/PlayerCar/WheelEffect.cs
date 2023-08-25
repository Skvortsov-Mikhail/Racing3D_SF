using UnityEngine;

public class WheelEffect : MonoBehaviour
{
    [SerializeField] private WheelCollider[] m_WheelsColliders;
    [SerializeField] private ParticleSystem[] m_WheelsSmoke;

    [SerializeField] private float m_ForwardSlipLimit;
    [SerializeField] private float m_SidewaySlipLimit;

    [SerializeField] private AudioSource m_Audio;

    [SerializeField] private GameObject m_SkidPrefab;

    private WheelHit wheelHit;
    private Transform[] skidTrail;

    private void Start()
    {
        skidTrail = new Transform[m_WheelsColliders.Length];
    }

    private void Update()
    {
        bool isSlip = false;

        for (int i = 0; i < m_WheelsColliders.Length; i++)
        {
            m_WheelsColliders[i].GetGroundHit(out wheelHit);

            if (m_WheelsColliders[i].isGrounded == true)
            {
                if (wheelHit.forwardSlip > m_ForwardSlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) > m_SidewaySlipLimit)
                {
                    if (skidTrail[i] == null)
                    {
                        skidTrail[i] = Instantiate(m_SkidPrefab).transform;
                    }

                    if (m_Audio.isPlaying == false)
                    {
                        m_Audio.Play();
                    }

                    if (skidTrail[i] != null)
                    {
                        skidTrail[i].position = m_WheelsColliders[i].transform.position - wheelHit.normal * m_WheelsColliders[i].radius;
                        skidTrail[i].forward = -wheelHit.normal;

                        m_WheelsSmoke[i].transform.position = skidTrail[i].position;
                        m_WheelsSmoke[i].Emit(10);
                    }

                    isSlip = true;
                    continue;
                }
            }

            skidTrail[i] = null;
            m_WheelsSmoke[i].Stop();
        }

        if (isSlip == false)
        {
            m_Audio.Stop();
        }
    }
}
