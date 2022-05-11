using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BaseThrowables : PoolableObject
{
    /* time it takes before the throwable expodes */
    [SerializeField] [Tooltip("time it takes before the throwable expodes")] float throwableExplodeTimer = 1.0f;

    /* particle system to be played when the throwable explodes */
    [SerializeField] [Tooltip("particle system to be played when the throwable explodes")] ParticleSystem explodeParticleSystem;

    /* the time in seconds it should wait before pooling this throwable back to the object pool */
    [SerializeField] [Tooltip("the time in seconds it should wait before pooling this throwable back to the object pool")] float poolTimeAfterExplosion = 1.0f;

    /* the audio source used to play audio for this throwable */
    AudioSource m_AudioSource;

    /* rigidbody for this throwable */
    Rigidbody m_RigidBody;

    public BaseThrowables() { }

    /*
    * sets both audio source and rigidbody of this throwable
    */
    public override void OnStart()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    /*
    * called when the throwable explodes
    */
    public virtual IEnumerator OnThrowableExplode()
    {
        yield return new WaitForSeconds(throwableExplodeTimer);

        PlayExplodeAudio();
        PlayExplodeSFX();

        Debug.Log(name + " just exploded!");

        Invoke("Pool", poolTimeAfterExplosion);
    }

    /*
    * Called when the player throws this throwable
    */
    public virtual void OnThrowThrowable(Vector3 forceDirection, float forceMultiplier = 1f)
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

    /*
    * plays the explosion particle effect
    */
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

    /*
    * returns the pool time after explosion
    */
    public float GetPoolTimeAfterExplosion()
    {
        return poolTimeAfterExplosion;
    }

    /*
    * returns the rigidbody
    */
    protected Rigidbody GetRigidbody()
    {
        return m_RigidBody;
    }
}
