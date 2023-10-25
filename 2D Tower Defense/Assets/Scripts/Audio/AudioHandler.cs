using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;


    [Header("Sound Clips")]
    [SerializeField] SoundClip cardClicked;
    [SerializeField] SoundClip cardHovered;

    void OnEnable()
    {
        CardEvents.OnCardClicked += CardClickedSound;
        CardEvents.OnCardHovered += CardHovererdSound;
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= CardClickedSound;
        CardEvents.OnCardHovered -= CardHovererdSound;
    }

    void PlaySFX(SoundClip sfx) => sfxSource.PlayOneShot(sfx.Clip, sfx.Volume);

    void CardClickedSound(Card _) => PlaySFX(cardClicked);
    void CardHovererdSound(Card _) => PlaySFX(cardHovered);
}
