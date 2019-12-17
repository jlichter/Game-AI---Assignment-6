using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Panda;

public class Harley : MonoBehaviour
{
    // tunable values for levels of Harley 
    public double hunger; // hunger measure
    public double tiredness; // fatigue measure  
    public double loneliness; // loneliness measure 
    public double boredom; // boredom measure 
    public double pottyLevel; // potty measure 

    // amount of food in harley's doggie bowl
    public double doggieBowl;
    // max amount of food that can be in harley's doggie bowl 
    public double maxBowlSize = 9;

    [Task] // for if harley is hungry, harley is too hungry, too full 
    public bool isHungry,isStarving, isHavingTreat,isEating,tooFull = false;
    [Task]
    public bool bowlFilled,bowlTooFull = false;
    [Task] // being tired, needing nap, nedding sleep, and/or being exhausted 
    public bool isTired, needsNap, needsSleep, isExhausted = false;
    [Task] // sleeping
    public bool isSleeping = false;
    public double goalNapHours = 2;
    public double goalNapMinutes = 30;
    public double goalSleepHours = 8;
    public double goalSleepMinutes = 2;
    public double sleepSeconds, sleepMinutes, sleepHours = 0;
    [Task] // for when harley goes out, needs to use the bathroom,   
    public bool isOutside, needsPotty, peedInside = false;
    [Task] // for different levels of loneliness 
    public bool isLonely,isMoreLonely,isVeryLonely = false;
    [Task] // for receiving attention (petting or belly rub)
    public bool isBeingPet, isBeingBellyRubbed = false;
    [Task] // for receiving too much attention 
    public bool tooMuchAttention = false; 
    [Task]     // for playing or being bored
    public bool isPlaying, isBored = false;
    // for when the owner is at work 
    [Task]
    public bool ownerAtWork = false;

    private Text levelsText;
    private string levelsString;

    public HarleySimulator simulator; // access to simulator script 

    // Start is called before the first frame update
    void Start()
    {
        // initialize all harley's levels at the start of the game 
        // NOTE: if you would like, you can adjust harley's starting values here as well.
        hunger = 20;
        tiredness = 21;
        loneliness = 30;
        boredom = 35;
        pottyLevel = 18;
        doggieBowl = 0;
        // get reference to text for levels
        levelsText = GameObject.Find("Harley Levels").GetComponent<Text>();
        // get reference to the simulator script 
        simulator = GameObject.Find("Main Camera").GetComponent<HarleySimulator>();
    }

   /* 
    * Tasks for checking Harley's current measures 
    */

    [Task] // checks how hungry Harley is 
    void CheckHunger() {
        if (hunger <= 15 && hunger > 8) { // harley is hungry but not starving
            isHungry = true;
            isStarving = false;
        } else if (hunger <= 8 && hunger > 0) { // harley is starving 
            isHungry = false;
            isStarving = true;
        } else if (hunger <= 0) { // harley has passed away 
            simulator.gameOver = true;
        } else if (hunger > 48) { // harley is too full 
            tooFull = true;
            isHungry = false;
            isStarving = false;
        } else { // nothing, harley is just fine 
            isHungry = false;
            isStarving = false;
        }
          
    }
    [Task] // checks if the bowl is filled or not 
    void CheckBowl() {

        if (doggieBowl > 0 && doggieBowl < 9) {
            bowlFilled = true;
            bowlTooFull = false;
        } else if (doggieBowl > 0 && doggieBowl >= 9) {
            bowlFilled = true;
            bowlTooFull = true;
        } else {
            bowlFilled = false;
            bowlTooFull = false;
        }

    }
    [Task] // checks how tired Harley is 
    void CheckTiredness() {
        if(tiredness < 10) {
            isTired = true;
        } else {
            isTired = false;
        }
    }
    [Task] // check if harley should nap or fall asleep for the night 
    void CheckTimeOfDay() {
        if(simulator.hour >= 7 && simulator.hour <= 22) {
            needsNap = true;
            needsSleep = false;
        } else {
            needsSleep = true;
            needsNap = false;
        }
    }
    [Task] // checks how badly Harley has to go to the bathroom 
    void CheckPotty() {
        if (pottyLevel < 12 && pottyLevel > 0) {
            needsPotty = true;
        }else if(pottyLevel <= 0) {
            peedInside = true;
            needsPotty = false;
        } 
        else {
            needsPotty = false;
            peedInside = false;
        }
    }
    [Task] // checks how bored Harley is 
    void CheckBoredom() {
        if (boredom < 20) {
            isBored = true;
        } else {
            isBored = false;
        }
    }
    [Task] // checks the level of harley's loneliness
    public void CheckLoneliness() {
        if (loneliness <= 22) {
            isLonely = true;
        }else if(loneliness > 48) { // receiving too much attention 
            isLonely = false;
            tooMuchAttention = true;
        } // else, she is not lonely and isn't receiving too much attention
        else {
            isLonely = false;
            tooMuchAttention = false;
        }
    }
    [Task] // changes from not too full to too full and vice versa
    public void ChangeFullness(int n) {

        hunger -= n; // harley puked because she was too full
        tooFull = !tooFull; // now shes not 
        Task.current.Succeed();
    }
    [Task]
    void CheckTimeAway() {

        if (simulator.workHours >= 8 && simulator.workHours < 14) {

            // harley's loneliness increases
            isMoreLonely = true;
            IncreaseLoneliness(0.1);
            
             
        } else if (simulator.workHours >= 14 && simulator.workHours < 24) {
            // increase a bit more 
            IncreaseLoneliness(0.3);
            isVeryLonely = true;
            isMoreLonely = false;

        } else {
            isVeryLonely = false;
            isMoreLonely = false;
        }

    }
    /* 
    * Tasks for adding / subtracting from harley's levels based off 
    * activities or player input 
    */

    /* positive additions */

    [Task] // decreases boredom by a certain measure when Harley is playing 
    void IncreaseFun(int n) {
        boredom += n;
        Task.current.Succeed();
    }
    [Task] // decreases tiredness by a certain measure when harley is sleeping 
    public void ReceiveRest(float n) {

        tiredness += n * Time.deltaTime;
    }
    [Task] // for when harley has a treat 
    public void EatTreat(int n) {
        hunger += n;
        Task.current.Succeed();

    }
    [Task] // for when harley is being pet 
    public void ReceivePetting(int n) {

        loneliness += n;
        Task.current.Succeed();
    }
    [Task] // for when harley is receiving a belly rub 
    void ReceiveBellyRub(int n) {

        loneliness += n;
        Task.current.Succeed();
    }
    [Task] // for when harley goes to the bathroom 
    void GoToBathroom(int n) {

        pottyLevel += n;
        Task.current.Succeed();
    }
    [Task] // harley eats from bowl until it is empty
    void EatFromBowl() {

        while (doggieBowl >= 0) {
            hunger += 5;
            doggieBowl -= 2;
            isEating = true;
        }
        Task.current.Succeed();
    }

    /* negative addtions */

    [Task] // increases Harley's exhaustion by a certain measure (usually when she is playing
    void IncreaseTiredness(int n) {
        tiredness -= n;
        Task.current.Succeed();
    }

    [Task] // increases harley's loneliness by a certain amount (done when player is away) 
    void IncreaseLoneliness(double n) {

        loneliness -= n * Time.deltaTime;
    }
   
    /* 
     * functions to change boolean values 
     */

    [Task] // changes not eating to eating and vice versa
    void ChangeEatBool() {

        isEating = !isEating;
        Task.current.Succeed();
    }
    [Task] // changes not peed inside to peed inside and vice versa
     void ChangePeedBool() {

        peedInside = !peedInside;
        Task.current.Succeed();
    }
    [Task] // changes not outside to outside and vice versa 
    void ChangeIsOut() {

        isOutside = !isOutside;
        Task.current.Succeed();
    }
    [Task] // changes no petting to petting and vice versa 
    void ChangePetBool() {

        isBeingPet = !isBeingPet;
        Task.current.Succeed();
    }
    [Task] // changes no belly rub to belly rub and vice versa 
    void ChangeBellyBool() {

        isBeingBellyRubbed = !isBeingBellyRubbed;
        Task.current.Succeed();
    }
    [Task] // changes not playing to playing and vice versa 
    void ChangePlayBool() {

        isPlaying = !isPlaying;
        Task.current.Succeed();
    }
    [Task] // changes harley from sleeping to not sleeping and tired to not tired (and vice versa)
    void ChangeSleepBool() {

        isSleeping = !isSleeping;
        isTired = !isTired;
        Task.current.Succeed();
    }
    /* 
     * functions for keeping track of the time harley is sleeping 
     */
    [Task] // continously updates the length of time harley is sleeping while she is sleeping 
    void StartSleepTime() {

        if (isSleeping) {

            sleepSeconds += Time.deltaTime * 58;
            if(sleepSeconds >= 60) {
                sleepMinutes++;
                sleepSeconds = 0;
            }else if(sleepMinutes >= 60) {
                sleepHours++;
                sleepMinutes = 0;
            }

        }
      //  Task.current.Succeed();

    }
  
    [Task] // once harley reaches her goal sleep/goal nap time, she will be done sleeping 
    void CheckSleepTime() {
        if (needsSleep) {
            if (sleepHours >= goalSleepHours && sleepMinutes >= goalSleepMinutes) {
                isSleeping = false;
                sleepSeconds = 0;
                sleepHours = 0;
                sleepMinutes = 0;

            }
        }else if (needsNap) {
            if (sleepHours >= goalNapHours && sleepMinutes >= goalNapMinutes) {
                isSleeping = false;
                sleepSeconds = 0;
                sleepHours = 0;
                sleepMinutes = 0;

            }
        }
       
    //    Task.current.Succeed();
   
    }
    /*
     * Task for Harley or simulator to respond with a message 
     */
    [Task] // outputs a message to the console 
    public void Say(string message) {

        if (simulator.gameText.text != "") simulator.gameText.text += "\n";

        simulator.gameText.text += "[" + simulator.scrollNum.ToString() + "] "+ message;
        simulator.scrollNum += 1;
        Canvas.ForceUpdateCanvases();
        simulator.gameScrollRect.verticalNormalizedPosition = 0f;
        
        Task.current.Succeed();
    }
    /*
     * Task to display all of harley's current levels 
     */
    [Task] 
    void DisplayLevels() {

        levelsString = "hunger: " + RoundedValue(hunger) + "\n" +
            "tiredness: " + RoundedValue(tiredness) + "\n" +
            "affection: " + RoundedValue(loneliness) + "\n" +
            "activity: " + RoundedValue(boredom) + "\n" +
            "bladder: " + RoundedValue(pottyLevel) + "\n";

        levelsText.text = levelsString;

    }
    // rounds the level to the nearest decimal place by 2 for DisplayLevels()
    string RoundedValue(double n) {

        var rounded = Mathf.Round((float)n * 100) / 100.0;
        return rounded.ToString();
    }

}
