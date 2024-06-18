using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine.UIElements;
using System.ComponentModel;
using TMPro.EditorUtilities;

public class Undermain : MonoBehaviour
{
    //All Main Variables
    [Header ("Character Stats")]
    #region Variables
    public int playerHealth;
    public int baseHealth;
    public int playerMaxHealth;
    public int heal;
    public string playerWepon = "Arm";
    public int playerXP;
    public int baseLevel;
    public int playerLevel;
    public int gainedXP;
    public int expThreshold;
    public int playerMaxDamage;
    public int baseMaxDamage;
    public int playerAttackRoll;
    public int playerDamage;
    public int food;
    public int baseFood;
    public int monsterHealth;
    public int monsterDMG;
    public int monsterDefence;
    public int dialougIntro = 0;
    public int roomCheck;
    public int rooms;
    public int floor;
    public int flirt;
    public bool menu;
    public bool turn;
    public bool fight;
    public bool act;
    public bool item;
    public bool spare;
    public bool spared;
    public bool roomBuild;
    public bool choice;
    public bool dialouge;
    public bool gameOver;
    public bool floorChange;
    public bool playerFree;
    public bool playerLock;
    public bool introCMP;
    public bool gameStart;
    public bool monster;
    public bool chest;
    public bool shop;
    #endregion
    //Audio Sources
    #region Audio Sources
    public AudioSource background;
    public AudioSource SFX;
    private AudioBehaviour manager;
    #endregion
    //Main Music For Game
    #region Music
    public AudioClip intro;
    public AudioClip introTense;
    public AudioClip introWoken;
    public AudioClip mainroom;
    public AudioClip firstFloorRuins;
    public AudioClip secondFloorSnow;
    public AudioClip thirdFloorWaterfall;
    public AudioClip thirdFloorLore;
    public AudioClip forthFloorPremonition;
    public AudioClip forthFloorLab;
    public AudioClip forthFloorLore;
    public AudioClip shopSong;
    public AudioClip battle;
    public AudioClip battle2;
    public AudioClip battle3;
    public AudioClip battle4;
    public AudioClip death;
    #endregion
    // Sound Effect Files For The Game
    #region SoundFX
    public AudioClip start;
    public AudioClip select;
    public AudioClip attack;
    public AudioClip hit;
    public AudioClip move;
    public AudioClip shopEnter;
    public AudioClip kill;
    public AudioClip crit;
    public AudioClip levelUp;
    public AudioClip healing;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("press A to start: ");
        manager = FindObjectOfType<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //this is the system that calculates the dialouge part of the game and updates it so when you press space you can continue through the dialouge
        #region dialouge system
        if (introCMP == false)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                dialougIntro = 1;
                Intro();
                SoundEffect(start);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearLog();
                dialougIntro = dialougIntro + 1;
                Intro();
                SoundEffect(select);
            }
        }
        if (dialouge == true && choice != true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearLog();
                dialougIntro = dialougIntro + 1;
                Dialouge();
            }
        }
        if (choice == true)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ClearLog();
                dialougIntro = dialougIntro + 1;
                Dialouge();
                choice = false;
                SoundEffect(select);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                ClearLog();                
                choice = false;
                dialouge = false;
                floorChange = true;
                Debug.Log("You Enter A Place Flowing With WaterFalls And Shimering Blue Bugs");
                SoundEffect(select);
            }
        }
        #endregion
        //this system check if the player has completed the intro and if they have then allow the player to press a and enter the game.
        //it also determins how many rooms there shal be on the first floor in the game
        if (introCMP != false && playerLock == true && gameStart == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ClearLog();
                Debug.Log("you enter the ruins");                
                SoundEffect(move);
                playerFree = true;
                playerLock = false;
                gameStart = true;
                Room();
                roomBuild = false;
                Debug.Log("you are now free to move around the underground");
                Debug.Log("Your Movement Controls Are F: To Move, X: To Open Your Inventory Menu, Use (A, S, D) Keys To Interact With The Menu, D: To Cancel Menu Action");
                floorChange = true;
                dialougIntro = 6;
            }
        }
        //this is for building new rooms for the player to move through
        if (roomBuild == true)
        {
            Room();
            roomBuild = false;
        }
        //this is for when the player starts the game. it allows the player to move through pracedualy generated rooms
        if (playerFree != false && monster != true && dialouge != true)
        {            
            if (Input.GetKeyDown(KeyCode.F))
            {
                ClearLog();
                Debug.Log("you have entered a new room");
                RoomCheck();
                SoundEffect(move);
                rooms = rooms - 1;
            }
            if (Input.GetKeyDown(KeyCode.X) && menu == false)
            {
                ClearLog();
                Debug.Log("You Have Opened The Menu");
                Debug.Log("What Would You Like To Do");
                Debug.Log("A: Use Item, S: Check Rooms Left, D: Cancel");
                menu = true;                
            }            
            //this handles the floor changes and their songs
             switch (floorChange)
             {
                case true:
                    switch (floor)
                    {
                        case 1:
                            backgroundAudioChange(firstFloorRuins);
                            floorChange = false;
                            break;
                        case 2:
                            backgroundAudioChange(secondFloorSnow);
                            floorChange = false;
                            monster = false;
                            break;
                        case 3:
                            backgroundAudioChange(thirdFloorWaterfall);
                            floorChange = false;
                            monster = false;
                            break;
                        case 4:
                            backgroundAudioChange(forthFloorLab);
                            floorChange = false;
                            monster = false;
                            break;
                    }
                    break;
                case false:
                    // CODE HERE
                    break;
             }
            //this checks the amount of rooms there are left and if they are 0 then change floors
            if (rooms < 0)
            {
                ClearLog();
                monster = false;
                floor = floor + 1;                
                dialougIntro = dialougIntro + 1;
                dialouge = true;
                roomBuild = true;
                Dialouge();
                if (dialouge == false)
                {
                    floorChange = true;
                }
            }                
            //this is the EXP system
            if (playerXP >= expThreshold)
            {
                playerXP = playerXP - expThreshold;
                SoundEffect(levelUp);
                playerMaxDamage = (int)(playerMaxDamage * 1.25);
                expThreshold = (int)(expThreshold * 1.5);
                playerLevel = playerLevel + 1;
                playerMaxHealth = (int)(playerMaxHealth * 1.25);
                playerHealth = playerMaxHealth;
                Debug.Log("You Leveled Up!");
                Debug.Log("You Are Now Level " + playerLevel + "!");
            }
        }
        //This Activates Menu System
        if (menu == true)
        {
            Menu();
            playerFree = false;
            playerLock = true;
        }
        if (menu != true && playerFree == false && gameStart == true && shop == false)
        {
            playerLock = false;
        }
        //this is a small bug fix for the player free system not seaming to turn true again for some odd reason        
        if (menu != true && playerLock == false)
        {
            playerFree = true;            
        }
        //this is the game over system
        if (gameOver != false)
        {
            Debug.Log("Game Over");
            Debug.Log("Y O U  H A V E  T O  S T A Y  D E T E R M I N E D  Y O U N G  O N E");
            Debug.Log("You Can Restart By Pressing A");
            playerHealth = baseHealth;
            playerMaxHealth = baseHealth;
            playerLevel = baseLevel;
            playerMaxDamage = baseMaxDamage;
            playerXP = 0;
            expThreshold = 20;
            gameOver = false;
            floor = 1;
            dialougIntro = 6;
        }
        //this is the battle system it uses the values of the players max damage and a attack roll to determin if the attack hits
        //it also does the same for the enemy
        if (monster != false)
        {   
            if (turn == false && spared != true)
            {
                Debug.Log("What Will You Deside To Do?");
                Debug.Log("Z: Fight");
                Debug.Log("X: Act");
                Debug.Log("C: Item");
                Debug.Log("V: Spare");
                turn = true;
            }
            if (Input.GetKeyDown(KeyCode.Z) && turn == true)
            {
                ClearLog();
                fight = true;
                SoundEffect(select);
                Debug.Log("You Choose To Fight This Turn");
            }
            if (Input.GetKeyDown(KeyCode.X) && turn == true)
            {
                ClearLog();
                act = true;
                SoundEffect(select);
                Debug.Log("You Choose To Act");
                Debug.Log("A: Check");
                Debug.Log("S: Flirt");
            }
            if (Input.GetKeyDown(KeyCode.C) && turn == true)
            {
                ClearLog();
                Debug.Log("You Choose To Use A Item");                
                if (playerHealth == playerMaxHealth)
                {
                    Debug.Log("You Are Already At Full Health");
                    turn = false;
                    SoundEffect(select);
                }
                if (food >= 1 && playerHealth != playerMaxHealth)
                {
                    SoundEffect(healing);
                    heal = Random.Range(5, 30);
                    Debug.Log("You Healed " + heal + "HP!");
                    turn = false;
                    item = false;
                    food = food - 1;
                    playerHealth = playerHealth + heal; 
                    if (playerHealth >= playerMaxHealth)
                    {
                        playerHealth = playerMaxHealth;
                    }
                }
                if (food <=0)
                {
                    Debug.Log("You Have No Healing Items On You");
                    turn = false;
                    item = false;
                    SoundEffect(select);
                }
            }
            if (Input.GetKeyDown(KeyCode.V) && turn == true)
            {
                ClearLog();
                Debug.Log("You Choose To Spare The Monster");
                if (flirt >= 2)
                {
                    gainedXP = Random.Range(5, 15);
                    Debug.Log("You Spared The Monster You Gained " + gainedXP + "EXP!");
                    monster = false;
                    turn = false;
                    spare = false;
                    SoundEffect(kill);
                    floorChange = true;
                    playerXP = playerXP + gainedXP;
                    flirt = 0;
                }
                else
                {
                    Debug.Log("The Monster Dosent Like You Enough To Be Spared. Try Acting First");
                    turn = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) && act == true)
            {
                ClearLog();
                Debug.Log("This Monster Has Currently " + monsterHealth + "HP And " + monsterDefence + "DF");
                turn = false;
                act = false;
            }
            if (Input.GetKeyDown(KeyCode.S) && act == true)
            {
                ClearLog();
                Debug.Log("You Give a Flirty Comment");
                flirt = Random.Range(1, 11);
                if (flirt >= 5)
                {
                    Debug.Log("The Monster Is Flattered By Your Comment");
                    turn = false;
                }
                else
                {
                    Debug.Log("The Monster Didnt Seem To Care");
                    turn = false;
                }
                turn = false;
                act = false;
            }
            //this is the demeaner for the attack system
            if (Input.GetKeyDown(KeyCode.Space) && fight == true)
            {
                //this clears the log and makes the rolls for the player and monster
                ClearLog(); 
                fight = false;
                turn = false;
                Debug.Log("You Swing Your " + playerWepon);
                playerAttackRoll = Random.Range(1, 21);                 
                if (playerAttackRoll >= 19)
                {
                    //this is for when the player is lucky enough to crit the monster
                    SoundEffect(crit);
                    monsterHealth = monsterHealth - playerMaxDamage;
                    playerDamage = playerMaxDamage;
                    Debug.Log("YOU CRIT THE MONSTER YOU DID " + playerMaxDamage + " DAMAGE!");
                    Debug.Log("The Monsters Health Is Now: " + monsterHealth);
                }   
                //this is a check to see if the player hits higher then the monster defence stat and how much damage they do
                if (playerAttackRoll !< 19 && playerAttackRoll > monsterDefence)
                {   
                    playerDamage = Random.Range(1, playerMaxDamage);
                    monsterHealth = monsterHealth - playerDamage;
                    Debug.Log("You Hit The Monster For: " + playerDamage + " Damage");
                    Debug.Log("The Monsters Health Is Now: " + monsterHealth);
                    SoundEffect(hit);
                }
                //this is when the player misses
                if (playerAttackRoll < monsterDefence)
                {
                    playerDamage = 0;
                    Debug.Log("You Missed");
                    SoundEffect(attack);
                }
                
                //this is for when the monster dies. it gives the player exp from a random range and then resets its health for the next monster (how kind of them)
                if (monsterHealth <= 0)
                {
                    floorChange = true;
                    SoundEffect(kill);
                    ClearLog();
                    monster = false;
                    monsterHealth = 20;
                    Debug.Log("You Hit The Monster For: " + playerDamage + " Damage");
                    Debug.Log("The Monsters Health Is Now: 0");
                    Debug.Log("you killed it");
                    gainedXP = Random.Range(1, 10);
                    playerXP = playerXP + gainedXP;
                    Debug.Log("You gained: " + gainedXP + "XP");                    
                }                
            }
            if (turn == false && monster == true)
            {
                monsterDMG = Random.Range(1, 15);
                //this is the monster damage calculations
                if (monsterDMG >= 6 && monsterHealth > 0)
                {                                        
                    playerHealth = playerHealth - monsterDMG;
                    Debug.Log("the monster hit you for: " + monsterDMG);
                    Debug.Log("your health is now: " + playerHealth);                    
                }                                 
            }
            //this is for when the player is unfortunate enough to die
            if (playerHealth <= 0)
            {
                playerFree = false;
                gameOver = true;
                monster = false;
                backgroundAudioChange(death);
            }
        }
        else
        {
            turn = false;
            monsterHealth = 20;
            flirt = 0;
        }
        if (shop == true)
        {
            playerLock = true;
            Shop();
        }
    }
    // this holds all the dialouge for the intro it is quite big but thats because it was to tell a story before the game starts
    void Intro()
    {
        if (dialougIntro == 1)
        {
            Debug.Log("your at home sitting in a arm chair swaying back and forth happily");
            Debug.Log("you feel as if everything was finaly peaceful even though you just got home from work");
            Debug.Log("everything was perfect for you in that moment");
            Debug.Log("untill...");
            Debug.Log("Press Space To Continue: ");
            backgroundAudioChange(intro);

        }
        if (dialougIntro == 2)
        {
            Debug.Log("you start to feel hungery 'ugg i could really go for some food right about now'");
            Debug.Log("you could head up to the kitchen and go get some food but the chair is too comfortable 'uhhgg but i really dont wanna have to get up'");
            Debug.Log("you get out the chair anyway because you are very hungery after all 'ok just a little snack wont hurt then i can sit down before the chair gets cold again'");
            Debug.Log("once you arive at the kitchen you see the counter top your fridge and some other standerd kitchen stuff");
            Debug.Log("as you open the fridge you suddenly feel a sharp pain in your chest 'AAHh what the heck!' you say out loud");
            Debug.Log("Press Space To Continue: ");

        }
        if (dialougIntro == 3)
        {

            backgroundAudioChange(introTense);
            Debug.Log("the feeling gets more and more intense as you fall to the floor in pain 'AHHH WHAT IS HAPPENING'");
            Debug.Log("as the feeling continues to get worse you start to see hear a voice in the distance (hey... hey wake up... HEY!.. I DIDNT BRING YOU ALL THIS WAY TO NAP!)");
            Debug.Log("this voice seams familiar but you dont know where you hurd it from 'WHO ARE YOU! *coughing* WHERE ARE YOU! *cough*'");
            Debug.Log("you feel a sharp pain rush to your head 'OH NOT MORE PAIN!' (W A K E  U P !)");
            Debug.Log("Press Space To Continue: ");


        }
        if (dialougIntro == 4)
        {
            Debug.Log("suddenly you awake in this Forest like place youre memory starts to return to you. you were asleep all along");
            Debug.Log("another person is with you but you remember who they are now. their name is jake he was taking you on a journy to a mountain for a job");
            Debug.Log("JAKE: dude finally you wake up. shesh i thought i would have to be sitting here all day. anyway the place of the job should be just up here");
            Debug.Log("as you both head up the mountain you slip and fall into a hole 'AAAHH JAKE' aaannd fall on you back 'Ooof'");
            Debug.Log("after a couple of secconds you stand back up. you can hear jake yelling but you have fallen down too far to hear him");
            Debug.Log("Press Space To Continue");
            backgroundAudioChange(introWoken);
        }
        if (dialougIntro == 5)
        {
            Debug.Log("you shout at the top of your lungs 'JAKE DONT WORRY IM OK GO SEE IF YOU CAN GET HELP FOR ME WHILE I TRY AND FIND A WAY OUT!' jake goes silent");
            Debug.Log("as you look around you dont see anything familiar everything looks like ruins");
            Debug.Log("you end up walking forward and enter into a room... there is a flower... its smileing at you...");
            Debug.Log("as you approach the flower it suddenly jerks into the ground and dissapiers saying (Y O U  A R E  N O T  T H E  O N E)");
            Debug.Log("as confused as you are you dont mind it and you head to the this big ruined tower looking thing");
            Debug.Log("Press Space To Continue");
        }
        if (dialougIntro == 6)
        {
            Debug.Log("as you walk up to this tower there is a sign on the front saying [welcome to the underground a place full of monsters and wonder]");
            Debug.Log("[this place has been built a long time ago howe*%@$#& get out is from reaching the fith floor of this tower");
            Debug.Log("[this is no ordanary place however it was built by humans back during the #@7@$&^*)( times]");
            Debug.Log("[there will be trials there so be careful as you progress the floors things may get more difficult but we believe in you]");
            Debug.Log("[i sadly can no longer be there to help guide you through these ruins fallen one but i hope this sign finds you well]");
            Debug.Log("Toriel");
            Debug.Log("You Now Have A Choice To Make You Can Either Wait For Jake, Continue Through The Ruins Or Read The Sign Again What Shall You Do");
            Debug.Log("Tab: Wait");
            Debug.Log("Enter: Continue");
            Debug.Log("Space: Read Sign");
            introCMP = true;
        }
    }
    void Dialouge()
    {
        if (dialougIntro == 7)
        {
            Debug.Log("You Enter A Snowy Floor 'how did... but how does... you know what this place is weird enough im just not gonna ask.'");
            dialouge = false;
        }
        if (dialougIntro == 8)
        {
            backgroundAudioChange(thirdFloorLore);
            Debug.Log("You Enter The Third Floor But Something Seams Off Here");
            Debug.Log("There Are Signs On The Walls EveryWhere");
            Debug.Log("Some Even Have Projecters On Them However They Are Too Old And Glitchy To See Whats On Them");
            Debug.Log("you can read the signs if you so choos to however you can also continue with the game");
            Debug.Log("What Shal It Be");
            Debug.Log("A: continue with game");
            Debug.Log("S: continue with dialouge and story");
            choice = true;
        }        
    }
    //this is the room building system
    void Room()
    {
        rooms = Random.Range(5, 11);                       
    }
    //this is the room check system
    void RoomCheck()
    {   
        //this checks the room for ay monsters and chests
        roomCheck = Random.Range(1, 21);
        if (roomCheck >= 5)
        {
            ClearLog();
            shop = true;
            chest = false;
            monster = false;
            backgroundAudioChange(shopSong);
            SoundEffect(shopEnter);
            Debug.Log("You Walk Into A Shop. There Is Someone Standing There.");
            Debug.Log("OH hello there young man how may i help you today human");
            Debug.Log("Z: Buy Items, X: Chat, C: Leave");
        }
        if (roomCheck >= 10)
        {
            monster = true;
            shop = false;
            turn = true;
            Debug.Log("a monster proached you");
            Debug.Log("What Will You Deside To Do?");
            Debug.Log("Z: Fight");
            Debug.Log("X: Act");
            Debug.Log("C: Item");
            Debug.Log("V: Spare");
            monsterDefence = Random.Range(1, 17);
            if (floor == 1 && monster == true)
            {
                backgroundAudioChange(battle);
            }
            if (floor == 2 && monster == true)
            {
                backgroundAudioChange(battle2);
            }
            if (floor == 3 && monster == true)
            {
                backgroundAudioChange(battle3);
            }
            if (floor == 4 && monster == true)
            {
                backgroundAudioChange(battle4);
            }
        }
        //this checks for chests in the room
        if (roomCheck >= 18)
        {
            chest = true;
            if (chest == true && monster == false)
            {
                Debug.Log("There Is A Chest In This Room 'huh i wonder if it has anything useful");
                food = food + Random.Range(1, 10);
                Debug.Log("You grabbed " + food + "Healing Items From The Chest");
                chest = false;
            }
            if (chest == true && monster == true)
            {
                food = food + Random.Range(1, 10);
                Debug.Log("You Notice The Chest And The Monster In The Same Room 'Crap i gotta grab that quick'");
                Debug.Log("You Leap Towards The Chest In A Hurry And Grab Everything You Thought Was Useful");
                Debug.Log("You Now Have " + food + " Healing Items!");
                chest = false;
            }                
        }        
    }
    //this is the menu system
    void Menu()
    {
        //honestly this was just a sneaky lil figure out from testing
        //basicaly if i press x to open the menu well and tried to use x to use menu thingos and it would just use the menu thingo it was attached to why not make it play a sound effect instead and change all the other buttons in the game to better match this stupid thing i cant figure out... i am ok :3... how is your day btw... honestly didnt think you would head all the way over here... ... ... sooo... u having fun understanding my brain?.. yes.. no?.. who am i kidding you may never even read this far lmao. even if you did its not as if i will know the answer untill you get to the feedback lol. well if you have gotten this far though hope you have a fun time reading the rest of the code :3.. cya.
        if (Input.GetKeyDown(KeyCode.X))
        {
            SoundEffect(select);
        }
        //this heals the player 
        if (Input.GetKeyDown(KeyCode.A))
        {
            ClearLog();
            heal = Random.Range(5, 30);
            
            if (playerHealth == playerMaxHealth)
            {
                Debug.Log("You Are Already At Max Health");
                SoundEffect(select);
                playerLock = false;
            }
            if (food == 0)
            {
                Debug.Log("You Dont Have Any Healing Items");
                SoundEffect(select);
                playerLock = false;               
            }
            if (playerHealth < playerMaxHealth && food > 0)
            {         
                playerHealth = playerHealth + heal;
                if (playerHealth >= playerMaxHealth)
                {
                    playerHealth = playerMaxHealth;
                }
                food = food - 1;
                Debug.Log("You Chose To Use Item U Healed Yourself You Have Healed " + heal + "HP!");
                SoundEffect(healing);
                playerLock = false;
            }                                 
            menu = false;            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ClearLog();
            Debug.Log("You Have Chosen To Check Rooms Left");
            Debug.Log("There Are " + rooms + "Rooms Left Before The Next Floor");
            menu = false;
            playerLock = false;
            SoundEffect(select);            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ClearLog();
            Debug.Log("You Closed The Menu");
            playerLock = false;            
            menu = false;            
            SoundEffect(select);            
        }              
    }
    //this is the shop system
    void Shop()
    {     
       if (Input.GetKeyDown(KeyCode.Z))
        {
            ClearLog();
            SoundEffect(select);
            Debug.Log("What would you like to buy human");
            Debug.Log("We have plenty of items to choose from");
        }
    }
    // this clears the logs when required e.g when the dialouge is playing (got from the net because i couldnt figure out how to do it)
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
       method.Invoke(new object(), null);
    }
    // this is used to change the background audio
    void backgroundAudioChange(AudioClip music)
    {
        background.Stop();
        background.clip = music;
        background.Play();
    }
    //this is used for all the sound effects in the game
    void SoundEffect(AudioClip SoundFX)
    {
        SFX.Stop();
        SFX.clip = SoundFX;
        SFX.Play();
    }
}