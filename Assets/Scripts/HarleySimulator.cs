using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Panda;

public class HarleySimulator : MonoBehaviour
{
    /* for accessing harley (our ai agent) */
    public Harley harley;
 
    /* text used in game */
    private Text digitalClock; // text for displaying the digital clock
    private Text dayText; // text for displaying number of days passed 
    // for the ui scrolling text and display
    public ScrollRect gameScrollRect;
    public int scrollNum = 1; // what line we are in in the scroll
    public Text gameText; // text for what is happening in-game 
    public Image harleyImage;
    // image for when harley dies ( :( )
    public Sprite gameOverSprite;

    /* for keeping track of the time */
    public const int TIMESCALE = 58; // how fast we want our minutes to be 
    public double hour, minute, second, days;
    public string periodOfDay; // am or pm  

    /* for displaying the time */ 
    private double displayHour;
    private string displayTime, displayDay, displayH, displayM, displayD;
   
    /* for when the user loses the game (harley dies) */
    [Task]
    public bool gameOver = false;

    /* for when the owner goes to work */ 
    public double workHours, workMinutes, workSeconds;
    [Task]
    public bool isAtWork;
    /* NOTE: for adjusting the rate at which harley's levels decrease.
    this is here for you to tune with */
    [Header("Rates for harley's levels decreasing")]
    public float hungerRate = 0.05f;
    public float lonelyRate = 0.3f;
    public float boredomRate = 0.4f;
    public float tiredRate = 0.2f;
    public float pottyRate = 0.2f;
    // for further adjustment and not just adjusting one value, the tinker rate
    // will increase/decrease all harley's rates at input value 
    public float tinkerRate = 0.36f;
    // Start is called (initialize game values)
    void Start()
    {
        digitalClock = GameObject.Find("Digital Clock").GetComponent<Text>();
        dayText = GameObject.Find("Day Text").GetComponent<Text>();
        // we start at 7:00 a.m., with 0 days passed 
        days = 0;
        hour = 7;
        minute = 0;
        second = 0;
        periodOfDay = "a.m.";
        gameText.text = "Here's Harley, what a cutie. Take care of her now!";

    }
    /* 
     * for advancing and displaying game time, always active 
     */

    [Task] // advances time in the game 
    void AdvanceTime() {

        // get the time, in seconds 
        second += Time.deltaTime * TIMESCALE;
        // increment the minutes if we've reached 60 seconds
        if (second >= 60) {
            minute++;
            UpdateHarley(1f);

            second = 0;

        } // if we've reached 60 minutes, increment the hour 
        else if (minute >= 60) {
            hour++;
            minute = 0;
        } // if we reached 24 hours, increment the day 
        else if (hour >= 24) {
            hour = 0; // for formatting time purposes
            minute = 0;
            second = 0;
            days++;
        }

        DisplayTime(); // call function to display clock 

    }
    // displays time in game as a digital clock 
    void DisplayTime() {
        // switching to early morning  
        if (hour == 0) {
            periodOfDay = "a.m.";
        }
        // switching to afternoon 
        if (hour == 12) {
            periodOfDay = "p.m.";
        }
        // get the time of day for a 12 hour clock
        // after 12:00 p.m.
        if (hour >= 13) {
            displayHour = hour - 12;

        } // at midnight  
        else if (hour == 0) {
            displayHour = 12;
        } // just displaying regular time 
        else {
            displayHour = hour;

        }
        // string representation of hour and minute 
        displayH = ((int)displayHour).ToString();
        displayM = LeadingZero((int)minute);
        // string representation of day 
        displayD = days.ToString();
        // days passed formatted as a string 
        displayDay = "days: " + displayD;
        // time of day formatted as a string 
        displayTime = displayH + ":" + displayM + " " + periodOfDay;
        // update the days passed 
        dayText.text = displayDay;
        // update digital clock 
        digitalClock.text = displayTime;
        return;
    }
    [Task] // for when harley is dead, and we change picture to sad ( :( ) picture 
    void GameOverScreen() {

        harleyImage.sprite = gameOverSprite;
        gameText.text = "GAME OVER\n" + "Harley died!";
        days = 0;
        hour = 7;
        minute = 0;
        second = 0;
        harley.hunger = 20;
        harley.tiredness = 24;
        harley.loneliness = 30;
        harley.boredom = 35;
        harley.pottyLevel = 18;
        harley.doggieBowl = 0;
        Task.current.Succeed();

    }
    /* 
     * inputs 
     */
    [Task] // "F" is to fill harley's bowl with dog food 
    bool PressedF() {
        bool pressedF = false;
        string inputstring = Input.inputString;
        if(inputstring.ToUpper() == "F") {
            pressedF = true;

        }

        return pressedF;
    }
    [Task] // fills harley's dog bowl on success 
    void FillBowl() {
        
        harley.doggieBowl += 3;
        Task.current.Succeed();

    }
    [Task] // "T" is to give harley a treat 
    bool PressedT() {
        bool pressedT = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "T") {
            pressedT = true;
        }
        return pressedT;
    }

    [Task] // "K" is to play fetch
    bool PressedK() {
        bool pressedK = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "K") {
            pressedK = true;
        }
        return pressedK;
    }
    [Task] // "P" pets harley 
    bool PressedP() {
        bool pressedP = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "P") {
            pressedP = true;
        }
        return pressedP;
    }
    [Task] // "B" gives harley a belly rub 
    bool PressedB() {
        bool pressedB = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "B") {
            pressedB = true;
        }
        return pressedB;
    }
    [Task] // "W" lets harley out for a walk 
    bool PressedW() {
        bool pressedW = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "W") {
            pressedW = true;
        }
        return pressedW;
    }
    [Task] // "L" leaves harley alone to sleep 
    bool PressedL() {
        bool pressedL = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "L") {
            pressedL = true;
        }
        return pressedL;
    }
    [Task] // "G" lets owner go to work
    bool PressedG() {
        bool pressedG = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "G") {
            pressedG = true;
        }
        return pressedG;
    }
    [Task] // "A" makes owner arrive home from work 
    bool PressedA() {
        bool pressedA = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "A") {
            pressedA = true;
        }
        return pressedA;
    }
    [Task] // "I" advances time by 15 minutes 
    bool PressedI() {
        bool pressedI = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "I") {
            pressedI = true;
        }
        return pressedI;
    }
    [Task] // "K" throws a ball for harley to fetch 
    bool PressedH() {
        bool pressedH = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "H") {
            pressedH = true;
        }
        return pressedH;
    }
    [Task] // "D" advances the day 
    bool PressedD() {
        bool pressedD = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "D") {
            pressedD = true;
        }
        return pressedD;
    }
    [Task] // "S" plays a mysterious sound  
    bool PressedS() {
        bool pressedS = false;
        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "S") {
            pressedS = true;
        }
        return pressedS;
    }
    /* 
     * actions based off inputs 
     */
    [Task] // gives harley a treat todo look at 
    void GiveTreat() {

        string inputstring = Input.inputString;
        if (inputstring.ToUpper() == "T") {
            harley.isHavingTreat = true;
        }
    }
    [Task] // sets bool 'atWork' to true when owner leaves 
    void GoToWork() {
        isAtWork = true;
        Task.current.Succeed();
    }
    [Task] // sets bool 'atWork' to false when owner comes back home 
    void ArriveHome() {
        // restart time away at work, everything sets to 0
        workSeconds = 0;
        workMinutes = 0;
        workHours = 0;
        isAtWork = false;
        Task.current.Succeed();
         
    }
    [Task] // advances the time by 15 minutes 
    void AdvanceMinute() {
       
        minute += 15;
        if (isAtWork) workMinutes += 15;
        if (harley.isSleeping) harley.sleepMinutes += 15;
        UpdateHarley(15);
        Task.current.Succeed();
        
    }
    [Task] // advances the time by an hour 
    void AdvanceHour() {

        hour += 1;
        if (isAtWork) workHours += 1;
        if (harley.isSleeping) harley.sleepHours += 1;
        UpdateHarley(60);
        Task.current.Succeed();        
      
    }
    [Task] // advance the time to the next day (7:00 a.m) 
    void AdvanceDay() {

        // get the current hour and minute 
        double currentHour = hour;
        double currentMinute = minute;
        // for the difference in time 
        double hourDifference = 0;
        double minuteDifference = 0;

        // (if at work) for the current hour at work and the current minute at work
        double currentWorkHour = workHours;
        double currentWorkMinute = workMinutes;
        // ( if at work) for the difference in time  
        double workHourDifference = 0;
        double workMinuteDifference = 0;
        //if it is after 7am, we need to get the difference in times to properly update harley 
        if (hour >= 7) {
            // get the difference in hours from midnight 
            hourDifference = 24 - currentHour;
            // add 7 for after midnight 
            hourDifference += 7;
            // update the work hour difference 
            workHourDifference = hourDifference;
            // get the difference in midnight 
            minuteDifference = 60 - currentMinute;
            // update the work minute difference 
            workMinuteDifference = minuteDifference;
            // if at work, change the times 
            if (isAtWork) {
                workHours += workHourDifference;
                workMinutes += workMinuteDifference;
            }
            // update harley by the time elapsed 
            UpdateHarley((int)(60 * hourDifference));
            UpdateHarley((int)(minuteDifference));

        } else { // else, it is after mdnight, and we just want to bring the time to 7am (start of new day)
            // get the difference in hours from 7am
            hourDifference = 7 - currentHour;
            // update the work hour difference
            workHourDifference = hourDifference;
            // get the difference in minutes 
            minuteDifference = 60 - currentMinute;
            // update the work minute difference
            workMinuteDifference = minuteDifference;
            // if at work, change the times 
            if (isAtWork) {
                workHours += workHourDifference;
                workMinutes += workMinuteDifference;
            }
            // update harley by the time elapsed 
            UpdateHarley((int)(60 * hourDifference));
            UpdateHarley((int)(minuteDifference));
        }
        // increment the days, and set the time back to 7:00 a.m
        days += 1;
        hour = 7;
        minute = 0;
        second = 0;
        periodOfDay = "a.m.";
        Task.current.Succeed();

    }

    // to get the minutes in correct format (for displaying time)
    string LeadingZero(int n) {
        return n.ToString().PadLeft(2, '0');
    }
    // updates Harley's levels depending on the time that has passed by, as well as other factors
    // i.e if she is sleeping, playing, etc 
    void UpdateHarley(float amount) {
       // Debug.Log(Time.deltaTime);
        // harley's hunger increases at a constant rate 
        harley.hunger -= hungerRate * amount * tinkerRate; 
        // if the owner is at work, harley gets more bored and more lonely 
        if (harley.isSleeping) {
            // if she is sleeping, she gains restfulness
            harley.ReceiveRest(4f * amount);         
            // everything else decreases at a slower rate 
            harley.loneliness -= 0.05f * amount * tinkerRate;
            harley.pottyLevel -= 0.05f * amount * tinkerRate;
            harley.boredom -= 0.05f * amount * tinkerRate;
        } else {
            // if harley isn't sleeping, her tiredness increases 
            harley.tiredness -= tiredRate * amount * tinkerRate;
            // harley's potty level increases 
            harley.pottyLevel -= pottyRate * amount * tinkerRate;
            // her boredom always increases if she isn't being entertained (puppies always want to play)
            if (!harley.isPlaying) harley.boredom -= boredomRate * amount * tinkerRate;
            // if harley isn't receiving attention, her loneliness increases 
            if (!harley.isBeingPet && !harley.isBeingBellyRubbed && !harley.isPlaying) harley.loneliness -= lonelyRate * amount * tinkerRate;
        }
        
    }
    /* time track for when owner is at work */
    [Task] // starts the clock for how long the owner's been away
    void StartTimeAway() {

        if (isAtWork) { // while at work, increase the time at work 

            workSeconds += Time.deltaTime * TIMESCALE;

            if (workSeconds >= 60) {
                workMinutes++;
                workSeconds = 0;
            } else if (workMinutes >= 60) {
                workHours++;
                workMinutes = 0;
            }

        }

    }
    
}
