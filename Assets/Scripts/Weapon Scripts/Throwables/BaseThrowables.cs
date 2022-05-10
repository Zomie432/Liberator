using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BaseThrowables : PoolableObject
{
    /* time it takes before the throwable expodes */
    [SerializeField] float throwableExplodeTimer = 1.0f;

    /* particle system to be played when the throwable explodes */
    [SerializeField] ParticleSystem explodeParticleSystem;

    /*  */
    [SerializeField] float poolTimeAfterExplosion = 1.0f;

    AudioSource m_AudioSource;

    Rigidbody m_RigidBody;

    public BaseThrowables() { }

    /*
    * called once when the object is created
    */
    public override void OnStart()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    public virtual IEnumerator OnThrowableExplode()
    {
        yield return new WaitForSeconds(throwableExplodeTimer);

        PlayExplodeAudio();
        PlayExplodeSFX();

        Debug.Log(name + " just exploded!");

        Invoke("Pool", poolTimeAfterExplosion);
    }

    public virtual void OnThrowThrowable(Vector3 cameraForward, float forceMultiplier = 1f)
    {
        Debug.Log("Throw" + name);
        StartCoroutine(OnThrowableExplode());
    }

    /*
    * plays the explosion audio
    */
    protected void PlayExplodeAudio()
    {
        m_AudioSource.Play();
    }

    protected void PlayExplodeSFX()
    {
        explodeParticleSystem.Play();
    }

    /*
    * returns the explode time for this throwable
    */
    public float GetExplodeTimer()
    {
        return throwableExplodeTimer;
    }

    public float GetPoolTimeAfterExplosion()
    {
        return poolTimeAfterExplosion;
    }

    protected Rigidbody GetRigidbody()
    {
        return m_RigidBody;
    }
}
