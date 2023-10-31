using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    [Header("Spatial Audio Source Prefab")]
    [SerializeField] AudioSource spatialSourcePrefab;

    [Header("Sound Clips")]
    [SerializeField] SoundClip cardClicked;
    [SerializeField] SoundClip cardHovered;

    void OnEnable()
    {
        CardEvents.OnCardClicked += CardClickedSound;
        CardEvents.OnCardEntered += CardHovererdSound;

        CombatEvents.OnTurretShot += TurretShotSound;
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= CardClickedSound;
        CardEvents.OnCardEntered -= CardHovererdSound;

        CombatEvents.OnTurretShot -= TurretShotSound;
    }

    void PlaySound(SoundClip sfx) => sfxSource.PlayOneShot(sfx.Clip, sfx.Volume);

    void PlaySoundAtPoint(SoundClip sfx, Vector2 position)
    {
        var source = Instantiate(spatialSourcePrefab, position, Quaternion.identity);

        // play the sound louder the more zoomed in the camera is
        source.PlayOneShot(sfx.Clip, sfx.Volume * (1 - MainCamera.Instance.ZoomLerp));

        Destroy(source.gameObject, sfx.Clip.length);
    }

    

    void CardClickedSound(Card _) => PlaySound(cardClicked);
    void CardHovererdSound(Card _) => PlaySound(cardHovered);

    void TurretShotSound(Turret turret) => PlaySoundAtPoint(turret.shootSound, turret.transform.position);
}
