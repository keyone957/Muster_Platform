using Unity.VisualScripting;
using UnityEngine;

// [RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
    ParticleSystem particleSystem;
    ParticleSystem.Particle[] m_Particles;
    Vector3[] thirdPoints;
    Vector3[] startPoints;

    // AttractorMove
    [SerializeField] float speed;
    Transform target;
    private int numParticlesAlive;
    private bool isAttractorMove = false;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        thirdPoints = new Vector3[particleSystem.main.maxParticles];
    }

    void Update()
    {
        if (isAttractorMove)
        {
            numParticlesAlive = particleSystem.GetParticles(m_Particles);
            for (int i = 0; i < numParticlesAlive; i++)
            {
                if (m_Particles[i].velocity.magnitude > 0)
                {
                    thirdPoints[i] = m_Particles[i].velocity.normalized * speed;
                    Debug.Log(thirdPoints[i]);
                    m_Particles[i].velocity = Vector3.zero;
                }
                var ratio = 1 - (m_Particles[i].remainingLifetime / m_Particles[i].startLifetime);
                var p1 = transform.position;
                var p2 = thirdPoints[i];
                var p3 = target.position;
                var p4 = Vector3.Lerp(p1, p2, ratio);
                var p5 = Vector3.Lerp(p2, p3, ratio);
                m_Particles[i].position = Vector3.Lerp(p4, p5, ratio);
            }
            particleSystem.SetParticles(m_Particles, numParticlesAlive);
        }
    }
    public void InitAttractorMove(Transform _target)
    {
        target = _target;
        isAttractorMove = true;
        particleSystem.Play();
    }
    public void EndAttractorMove()
    {
        target = null;
        isAttractorMove = false;
        particleSystem.Stop(true);
    }
    public void EmitParticle()
    {
        particleSystem.Emit(1);
    }
}
