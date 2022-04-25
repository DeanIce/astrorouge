using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDictionary : MonoBehaviour
{
    // Weapon Sounds
    private AudioClip explosion1, explosion2, explosion3;
    private AudioClip blaster1, blaster2, blaster3, blaster4, blaster5, blaster6, blaster7, blaster8;

    // Enemy/Creature Sounds
    private AudioClip entAttack1, entAttack2, entAttack3;
    private AudioClip monsterGrowl1, monsterGrowl2, monsterGrowl3, monsterGrowl4, monsterGrowl5;
    private AudioClip insectScreech1, insectScreech2;
    private AudioClip monsterGrowlLong1, monsterGrowlLong2, monsterGrowlLong3, monsterGrowlLong4,
                        monsterGrowlLong5, monsterGrowlLong6, monsterGrowlLong7;
    private AudioClip monsterGrowlShort1, monsterGrowlShort2, monsterGrowlShort3, monsterGrowlShort4,
                        monsterGrowlShort5, monsterGrowlShort6, monsterGrowlShort7, monsterGrowlShort8;
    private AudioClip golemAttack1, golemAttack2, golemAttack3, golemAttack4;

    // Player & UI Sounds
    private AudioClip takeDamage1, takeDamage2, takeDamage3, takeDamage4, takeDamage5;
    private AudioClip playerLevelUp, playerDeath, pickUpItem;
    private AudioClip menuHover, menuOpen, menuClose, menuSelect;

    // Game Music
    private AudioClip musicLevel1, musicLevel2, musicLevel3;
    private AudioClip musicBoss1, musicBoss2, musicBoss3;
    private AudioClip musicMainMenu, musicCutscene;

    public void LoadSounds()
    {
        poisonSound1 = (AudioClip)Resources.Load("Audio/Sound Effects/PowerUp-Poison1");
    }
}
