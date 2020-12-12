using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] private AudioSource movementAudioSource;
    [SerializeField] private AudioSource soundAudioSource;

    [Header("Audio clips")]
    [SerializeField] private AudioClip engineIdlingClip;
    [SerializeField] private AudioClip engineDrivingClip;
    [SerializeField] private AudioClip engineDeathClip;
    [SerializeField] private AudioClip engineShotClip;
    [SerializeField] private AudioClip engineChargingClip;

    private float audioPitchRange = 0.2f;
    private float audioOriginalPitch;

    public void Init()
    {
        audioOriginalPitch = movementAudioSource.pitch;
    }

    public void PlaySound(PlayerAudioType audioType)
    {
        switch (audioType)
        {
            case PlayerAudioType.TankDeath:
                {
                    soundAudioSource.clip = engineDeathClip;
                }
                break;
            case PlayerAudioType.TankShot:
                {
                    soundAudioSource.clip = engineShotClip;
                }
                break;
            case PlayerAudioType.ShotCharging:
                {
                    soundAudioSource.clip = engineChargingClip;
                }
                break;
            default:
                break;
        }
        soundAudioSource.Play();
    }

    public void PlayEngineAudio(float movementValue, float turnValue)
    {
        if (Mathf.Abs(movementValue) < 0.1f && Mathf.Abs(turnValue) < 0.1f)
        {
            if (movementAudioSource.clip == engineDrivingClip)
            {
                movementAudioSource.clip = engineIdlingClip;
                movementAudioSource.pitch = Random.Range(audioOriginalPitch - audioPitchRange, audioOriginalPitch + audioPitchRange);
                movementAudioSource.Play();
            }
        }
        else
        {
            if (movementAudioSource.clip == engineIdlingClip)
            {
                movementAudioSource.clip = engineDrivingClip;
                movementAudioSource.pitch = Random.Range(audioOriginalPitch - audioPitchRange, audioOriginalPitch + audioPitchRange);
                movementAudioSource.Play();
            }
        }
    }
}

public enum PlayerAudioType
{
    TankDeath,
    TankShot,
    ShotCharging,
    EngineIdle,
    EngineDriving
}