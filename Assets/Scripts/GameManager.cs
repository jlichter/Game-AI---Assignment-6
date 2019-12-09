using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Panda;

public class GameManager : MonoBehaviour
{
    public Harley harley;

    private Text digitalClock; // text for displaying the digital clock
    private Text gameText; // text for what is happening in-game 


    // for keeping track of the time
    private const int TIMESCALE = 58; // how fast we want our minutes to be 
    private double hour, minute, second, days;
    private string periodOfDay; // am or pm  

    // for displaying the time 
    private double displayHour;
    private string displayTime;
    private string displayH;
    private string displayM;

    // for when the owner goes to work 
    private double workHours, workMinutes, workSeconds;
    public bool isAtWork;

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

    [Task]
    bool IsFillingBowl() {
        string inputstring = Input.inputString;
        if(inputstring.ToUpper() == "F") {
            return true;
        }
        return false;
    }
    [Task]
    public void OutputEating() {
        Debug.Log("Harley dives into the food bowl face first.");
    }
    [Task]
    public void OutputDoneEating() {
        Debug.Log("Harley finishes eating.");
    }

    [Task]
    void UpdateTime() {

        /* if user pressed H  (Hour)
         * time.hour += 1 (advance the hour a full hour)
         *
         * if the user pressed I  (Idle)
         * time.minute += 15 (advance 15 mins)
         * 
         * if the User preses D  (starting a new day)
         * time.hour = 7
         * time.minute = 0
         * time.second = 0 
         */


        // get the time, in seconds 
        second += Time.deltaTime * TIMESCALE; 
        // increment the minutes if we've reached 60 seconds
        if (second >= 60) {
            minute++;
            UpdateHarley();
            second = 0;

        } // if we've reached 60 minutes, increment the hour 
        else if(minute >= 60) { 
            hour++;
            minute = 0;
        } // if we reached 24 hours, increment the day 
        else if(hour >= 24) { 
            hour = 0; // for formatting time purposes
            minute = 0;
            second = 0;
            days++;
        }
        // switching to early morning  
        if(hour == 0) {
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
        else if(hour == 0){ 
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
        digitalClock.text =  displayTime;



    }
    // to get the minutes in correct format 
    string LeadingZero(int n) {
        return n.ToString().PadLeft(2, '0');
    }
    void UpdateHarley() {
        harley.hunger -= 0.05;
    }
    void feed() {

        Debug.Log("harley has been fed!");
        harley.hunger += 20;


    }


}
