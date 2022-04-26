using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDictionary : MonoBehaviour
{
    // Weapon Sounds
    public AudioClip explosion1, explosion2, explosion3;
    public AudioClip blaster6, blaster8;

    // Enemy/Creature Sounds
    public AudioClip entAttack1, entAttack2, entAttack3;
    public AudioClip monsterGrowl1, monsterGrowl2, monsterGrowl3, monsterGrowl4, monsterGrowl5;
    public AudioClip insectScreech1, insectScreech2;
    public AudioClip monsterGrowlLong1, monsterGrowlLong2, monsterGrowlLong3, monsterGrowlLong4,
                        monsterGrowlLong5, monsterGrowlLong6, monsterGrowlLong7;
    public AudioClip monsterGrowlShort1, monsterGrowlShort2, monsterGrowlShort3, monsterGrowlShort4,
                        monsterGrowlShort5, monsterGrowlShort6, monsterGrowlShort7, monsterGrowlShort8;
    public AudioClip golemAttack1, golemAttack2, golemAttack3, golemAttack4;

    // Player & UI Sounds
    public AudioClip takeDamage1, takeDamage2, takeDamage3, takeDamage4, takeDamage5;
    public AudioClip playerLevelUp, playerDeath, pickUpItem;
    public AudioClip menuHover, menuOpen, menuClose, menuSelect;

    // Game Music
    public AudioClip musicLevel1, musicLevel2, musicLevel3;
    public AudioClip musicBoss1, musicBoss2, musicBoss3;
    public AudioClip musicMainMenu, musicCutscene;

    private List<AudioClip> explosions;
    private List<AudioClip> entAttacks;
    private List<AudioClip> growls;
    private List<AudioClip> longGrowls;
    private List<AudioClip> shortGrowls;
    private List<AudioClip> insectScreeches;
    private List<AudioClip> golemAttacks;
    private List<AudioClip> playerTakeDamage;

    public void LoadSounds()
    {
        explosion1 = (AudioClip)Resources.Load("Audio/Sound Effects/Explosion1");
        explosion2 = (AudioClip)Resources.Load("Audio/Sound Effects/Explosion2");
        explosion3 = (AudioClip)Resources.Load("Audio/Sound Effects/Explosion3");

        blaster6 = (AudioClip)Resources.Load("Audio/Sound Effects/Shoot-Blaster6");
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

        insectScreech1 = (AudioClip)Resources.Load("Audio/Sound Effects/InsectScreech");
        insectScreech2 = (AudioClip)Resources.Load("Audio/Sound Effects/InsectScreech2");

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

        /* ====== THESE ARE APPLIED IN LEVEL SCENE/ SCRIPTABLE OBJECTS ====== NOT WORTH MOVING RIGHT NOW
        musicLevel1 = (AudioClip)Resources.Load("Audio/Music/Level 1 Combat Theme");
        musicLevel2 = (AudioClip)Resources.Load("Audio/Music/Level 2 Combat Theme");
        musicLevel3 = (AudioClip)Resources.Load("Audio/Music/Level 3 Combat Theme");
        musicBoss1 = (AudioClip)Resources.Load("Audio/Music/Boss 1 Theme LOOP");
        musicBoss2 = (AudioClip)Resources.Load("Audio/Music/Boss 2 Theme LOOP");
        musicBoss3 = (AudioClip)Resources.Load("Audio/Music/Boss 3 Theme LOOP");
        musicMainMenu = (AudioClip)Resources.Load("Audio/Music/MainMenuMusicV3");
        musicCutscene = (AudioClip)Resources.Load("Audio/Music/Cutscene Music");
        */

        MakeLists();
    }

    public int RandomValue(int numSounds)
    {
        return (int) (Random.value * numSounds);
    }

    public void MakeLists()
    {
        explosions = new List<AudioClip>();
        entAttacks = new List<AudioClip>();
        growls = new List<AudioClip>();
        longGrowls = new List<AudioClip>();
        shortGrowls = new List<AudioClip>();
        insectScreeches = new List<AudioClip>();
        golemAttacks = new List<AudioClip>();
        playerTakeDamage = new List<AudioClip>();

        explosions.Add(explosion1);
        explosions.Add(explosion2);
        explosions.Add(explosion3);

        entAttacks.Add(entAttack1);
        entAttacks.Add(entAttack2);
        entAttacks.Add(entAttack3);

        growls.Add(monsterGrowl1);
        growls.Add(monsterGrowl2);
        growls.Add(monsterGrowl3);
        growls.Add(monsterGrowl4);
        growls.Add(monsterGrowl5);

        longGrowls.Add(monsterGrowlLong1);
        longGrowls.Add(monsterGrowlLong2);
        longGrowls.Add(monsterGrowlLong3);
        longGrowls.Add(monsterGrowlLong4);
        longGrowls.Add(monsterGrowlLong5);
        longGrowls.Add(monsterGrowlLong6);
        longGrowls.Add(monsterGrowlLong7);

        shortGrowls.Add(monsterGrowlShort1);
        shortGrowls.Add(monsterGrowlShort2);
        shortGrowls.Add(monsterGrowlShort3);
        shortGrowls.Add(monsterGrowlShort4);
        shortGrowls.Add(monsterGrowlShort5);
        shortGrowls.Add(monsterGrowlShort6);
        shortGrowls.Add(monsterGrowlShort7);
        shortGrowls.Add(monsterGrowlShort8);

        insectScreeches.Add(insectScreech1);
        insectScreeches.Add(insectScreech2);

        golemAttacks.Add(golemAttack1);
        golemAttacks.Add(golemAttack2);
        golemAttacks.Add(golemAttack3);
        golemAttacks.Add(golemAttack4);

        playerTakeDamage.Add(takeDamage1);
        playerTakeDamage.Add(takeDamage2);
        playerTakeDamage.Add(takeDamage3);
        playerTakeDamage.Add(takeDamage4);
        playerTakeDamage.Add(takeDamage5);
    }

    public AudioClip RandomExplosion()
    {
        return explosions[RandomValue(explosions.Count)];
    }

    public AudioClip RandomEntAttack()
    {
        return entAttacks[RandomValue(entAttacks.Count)];
    }

    public AudioClip RandomGrowl()
    {
        return growls[RandomValue(growls.Count)];
    }

    public AudioClip RandomLongGrowl()
    {
        return longGrowls[RandomValue(longGrowls.Count)];
    }

    public AudioClip RandomShortGrowl()
    {
        return shortGrowls[RandomValue(shortGrowls.Count)];
    }

    public AudioClip RandomInsectScreech()
    {
        return insectScreeches[RandomValue(insectScreeches.Count)];
    }

    public AudioClip RandomGolemAttack()
    {
        return golemAttacks[RandomValue(golemAttacks.Count)];
    }

    public AudioClip RandomPlayerTakeDamage()
    {
        return playerTakeDamage[RandomValue(playerTakeDamage.Count)];
    }
}
