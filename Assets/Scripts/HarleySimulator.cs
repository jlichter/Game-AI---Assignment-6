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
    private Text gameText; // text for what is happening in-game 


    /* for keeping track of the time */
    private const int TIMESCALE = 58; // how fast we want our minutes to be 
    public double hour, minute, second, days;
    private string periodOfDay; // am or pm  

    /* for displaying the time */ 
    private double displayHour;
    private string displayTime, displayH, displayM;
    
    /* for when the owner goes to work */ 
    private double workHours, workMinutes, workSeconds;
    public bool isAtWork;

    /* for user input */
    int action;

    // Start is called before the first frame update
    void Start()
    {
        digitalClock = GameObject.Find("Digital Clock").GetComponent<Text>();
        gameText = GameObject.Find("Game Text").GetComponent<Text>();
        hour = 7;
        minute = 0;
        second = 0;
        periodOfDay = "a.m.";
    }
    [Task] // takes input from the user 
    void TakeInput() {

        string inputstring = Input.inputString;
        // F loads the dog dish up with food 
        if(inputstring.ToUpper() == "F") {
            action = 1;
        } // T gives Harley a treat 
        else if(inputstring.ToUpper() == "T") {
            action = 2;
        } // K throws a stick for Harley to fetch (done repeatedly)
        else if(inputstring.ToUpper() == "K") {
            action = 3;
        } // P pets Harley 
        else if(inputstring.ToUpper() == "P") {
            action = 4;
        } // B belly rubs Harley  
        else if(inputstring.ToUpper() == "B") {
            action = 5;
        } // W goes for a walk 
        else if(inputstring.ToUpper() == "W") {
            action = 6;
        } // L leaves Harley alone  
        else if(inputstring.ToUpper() == "L") {
            action = 7;
        } // G makes owner go to work 
        else if(inputstring.ToUpper() == "G") {
            action = 8;
        } // A makes open arrive back from work  
        else if(inputstring.ToUpper() == "A") {
            action = 9;
        } // I advances time 15 minutes (action 10)
        else if(inputstring.ToUpper() == "I") {
            action = 10;
        } // H advances time by 1 hour (action 11) 
        else if(inputstring.ToUpper() == "H") {
            action = 11;
        } // D starts a new day  
        else if (inputstring.ToUpper() == "D") {
            action = 12;
        } // S makes a mysterious sound  
        else if (inputstring.ToUpper() == "S") {
            action = 13;
        }

    }
    [Task] // fill Harley's bowl
    void FillBowl() {
        if(action == 1) {
            harley.hunger += 20;
            Debug.Log("Harley eats. Yum!");
            action = 0;
        }
    }
    [Task] // playing fetch with Harley
    void PlayFetch() {
        if(action == 3) {
            if (harley.isHungry) {
                Debug.Log("Harley is too hungry to play right now!");
            } else {
             //   Debug.Log("It's time to play some fetch with Harley!");
                harley.isPlaying = true;
            }
            
            action = 0;
        }
    }
    [Task] // sets bool 'atWork' to true when owner leaves 
    void GoToWork() {
        if (action == 8) {
            isAtWork = true;
            Debug.Log("The owner left for work.");
            action = 0;
        }
    }
    [Task] // sets bool 'atWork' to false when owner comes back home 
    void ArriveHome() {
        if (action == 8) {
            isAtWork = false;
            Debug.Log("The owner came back from work.");
            action = 0;
        }
    }
    [Task] // advances the time by 15 minutes 
    void AdvanceMinute() {

        if (action == 10) {
            minute += 15;
            Debug.Log("Time advanced 15 minutes.");
            UpdateHarley(15);
            action = 0;
        }

    }
    [Task] // advances the time by an hour 
    void AdvanceHour() {

       if(action == 11) {
            hour += 1;
            Debug.Log("Time advanced 1 hour.");
            UpdateHarley(60);
            action = 0;
        }

    }

    [Task] // advances time in the game 
    void AdvanceTime() {

        // get the time, in seconds 
        second += Time.deltaTime * TIMESCALE;
        // increment the minutes if we've reached 60 seconds
        if (second >= 60) {
            minute++;
            UpdateHarley(1);
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

        // time of day formatted as a string 
        displayTime = displayH + ":" + displayM + " " + periodOfDay;
        // update digital clock 
        digitalClock.text = displayTime;
        return;
    }
    // to get the minutes in correct format 
    string LeadingZero(int n) {
        return n.ToString().PadLeft(2, '0');
    }
    // updates Harley's levels depending on the time that has passed by,
    // as well as other factors 
    void UpdateHarley(int amount) {
        
        // harley's hunger increases at a constant rate 
        harley.hunger -= 0.05 * amount; 
        // if the owner is at work, harley gets more bored and more lonely 
        if (isAtWork) {
            harley.boredom -= 0.8 * amount;
        } else {
            // if harley isn't playing, her boredom increases 
            if(!harley.isPlaying) harley.boredom -= 0.5 * amount;
        }


    }
}
