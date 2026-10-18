using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Most of this class was generated using Vibe AI
    public AudioClip explosionClip;
    public AudioClip missileShotClip;
    public AudioClip laserClip;
    public AudioClip engineThrustClip;

    AudioSource oneShotSource;
    AudioSource laserSource;
    AudioSource thrustSource;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        oneShotSource = gameObject.AddComponent<AudioSource>();
        laserSource  = gameObject.AddComponent<AudioSource>();
        thrustSource = gameObject.AddComponent<AudioSource>();

        oneShotSource.playOnAwake = false;
        laserSource.playOnAwake  = false;
        thrustSource.playOnAwake = false;

        laserSource.clip  = laserClip;
        laserSource.loop  = true;

        thrustSource.clip = engineThrustClip;
        thrustSource.loop = true;
    }

    public void PlayExplosion()  => oneShotSource.PlayOneShot(explosionClip);
    public void PlayMissileShot() => oneShotSource.PlayOneShot(missileShotClip);

    public void SetLaserActive(bool active)
    {
        if (active && !laserSource.isPlaying) laserSource.Play();
        else if (!active && laserSource.isPlaying) laserSource.Stop();
    }

    public void SetThrust(float throttle)
    {
        if (throttle > 0.05f && !thrustSource.isPlaying) thrustSource.Play();
        else if (throttle <= 0.05f && thrustSource.isPlaying) thrustSource.Stop();

        thrustSource.volume = 0.3f + 0.7f * throttle;
    }
}