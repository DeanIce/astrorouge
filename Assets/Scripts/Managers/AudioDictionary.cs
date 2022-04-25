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
        explosion1 = (AudioClip)Resources.Load("Audio/Sound Effects/Explosion1");
        explosion2 = (AudioClip)Resources.Load("Audio/Sound Effects/Explosion2");
        explosion3 = (AudioClip)Resources.Load("Audio/Sound Effects/Explosion3");

        //blaster1 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster1");
        //blaster2 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster2");
        //blaster3 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster3");
        //blaster4 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster4");
        //blaster5 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster5");
        blaster6 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster6");
        //blaster7 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster7");
        blaster8 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster8");

        entAttack1 = (AudioClip)Resources.Load("Audio/Sound Effects/EntAttack1");
        entAttack2 = (AudioClip)Resources.Load("Audio/Sound Effects/EntAttack2");
        entAttack3 = (AudioClip)Resources.Load("Audio/Sound Effects/EntAttack3");

        monsterGrowl1 = (AudioClip)Resources.Load("Audio/Sound Effects/Growl1");
        monsterGrowl2 = (AudioClip)Resources.Load("Audio/Sound Effects/Growl2");
        monsterGrowl3 = (AudioClip)Resources.Load("Audio/Sound Effects/Growl3");
        monsterGrowl4 = (AudioClip)Resources.Load("Audio/Sound Effects/Growl4");
        monsterGrowl5 = (AudioClip)Resources.Load("Audio/Sound Effects/Growl5");

        monsterGrowlLong1 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong1");
        monsterGrowlLong2 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong2");
        monsterGrowlLong3 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong3");
        monsterGrowlLong4 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong4");
        monsterGrowlLong5 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong5");
        monsterGrowlLong6 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong6");
        monsterGrowlLong7 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlLong7");

        monsterGrowlShort1 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort1");
        monsterGrowlShort2 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort2");
        monsterGrowlShort3 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort3");
        monsterGrowlShort4 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort4");
        monsterGrowlShort5 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort5");
        monsterGrowlShort6 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort6");
        monsterGrowlShort7 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort7");
        monsterGrowlShort8 = (AudioClip)Resources.Load("Audio/Sound Effects/GrowlShort8");

        golemAttack1 = (AudioClip)Resources.Load("Audio/Sound Effects/RockGolemAttack1");
        golemAttack2 = (AudioClip)Resources.Load("Audio/Sound Effects/RockGolemAttack2");
        golemAttack3 = (AudioClip)Resources.Load("Audio/Sound Effects/RockGolemAttack3");
        golemAttack4 = (AudioClip)Resources.Load("Audio/Sound Effects/RockImpact");

        takeDamage1 = (AudioClip)Resources.Load("Audio/Sound Effects/Hit1");
        takeDamage2 = (AudioClip)Resources.Load("Audio/Sound Effects/Hit2");
        takeDamage3 = (AudioClip)Resources.Load("Audio/Sound Effects/Hit3");
        takeDamage4 = (AudioClip)Resources.Load("Audio/Sound Effects/Hit4");
        takeDamage5 = (AudioClip)Resources.Load("Audio/Sound Effects/Hit5");

        playerLevelUp = (AudioClip)Resources.Load("Audio/Sound Effects/LevelUp");
        playerDeath = (AudioClip)Resources.Load("Audio/Sound Effects/Death Sound");
        pickUpItem = (AudioClip)Resources.Load("Audio/Sound Effects/Pickup-Item");

        menuHover = (AudioClip)Resources.Load("Audio/Sound Effects/Hover-MenuOption");
        menuClose = (AudioClip)Resources.Load("Audio/Sound Effects/Menu-Close");
        menuOpen = (AudioClip)Resources.Load("Audio/Sound Effects/Menu-Open");
        menuSelect = (AudioClip)Resources.Load("Audio/Sound Effects/Select-MenuOption");

        musicLevel1 = (AudioClip)Resources.Load("Audio/Music/Level 1 Combat Theme");
        musicLevel2 = (AudioClip)Resources.Load("Audio/Music/Level 2 Combat Theme");
        musicLevel3 = (AudioClip)Resources.Load("Audio/Music/Level 3 Combat Theme");
        musicBoss1 = (AudioClip)Resources.Load("Audio/Music/Boss 1 Theme LOOP");
        musicBoss2 = (AudioClip)Resources.Load("Audio/Music/Boss 2 Theme LOOP");
        musicBoss3 = (AudioClip)Resources.Load("Audio/Music/Boss 3 Theme LOOP");
        musicMainMenu = (AudioClip)Resources.Load("Audio/Music/MainMenuMusicV3");
        musicCutscene = (AudioClip)Resources.Load("Audio/Music/Cutscene Music");
    }
}
