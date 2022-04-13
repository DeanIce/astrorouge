using Managers;
using UnityEngine;

public class StatusEffectAudioDictionary
{
    private AudioClip burnSound1, burnSound2;
    private AudioClip freezeSound1, freezeSound2;
    private AudioClip lightningSound1, lightningSound2;
    private AudioClip poisonSound1, poisonSound2;

    public void LoadSounds()
    {
        poisonSound1 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Poison1");
        poisonSound2 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Poison2");
        burnSound1 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Fire1");
        burnSound2 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Fire2");
        freezeSound1 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Ice1");
        freezeSound2 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Ice2");
        lightningSound1 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Lightning1");
        lightningSound2 = (AudioClip) Resources.Load("Audio/Sound Effects/PowerUp-Lightning2");
    }

    public void ApplyBurnSFX()
    {
        AudioManager.Instance.PlaySFX(randomBool() ? burnSound1 : burnSound2);
    }

    public void ApplyPoisonSFX()
    {
        AudioManager.Instance.PlaySFX(randomBool() ? poisonSound1 : poisonSound2);
    }

    public void ApplyLightningSFX()
    {
        AudioManager.Instance.PlaySFX(randomBool() ? lightningSound1 : lightningSound2);
    }

    public void ApplySlowSFX()
    {
        AudioManager.Instance.PlaySFX(randomBool() ? freezeSound1 : freezeSound2);
    }

    private bool randomBool()
    {
        if (Random.value >= 0.5)
            return true;
        return false;
    }
}