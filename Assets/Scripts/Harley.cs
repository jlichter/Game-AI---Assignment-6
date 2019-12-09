using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Harley : MonoBehaviour
{
    // tunable values for levels of Harley 
    public double hunger; // hunger measure
    public double tiredness; // fatigue measure  
    public double loneliness; // loneliness measure 
    public double boredom; // boredom measure 
    public double pottyLevel; // potty measure 

    [Task]
    public bool isHungry = false;
    [Task]
    public bool isStarving = false;
    [Task]
    public bool isTired = false;
    [Task]
    public bool needsNap = false;
    [Task]
    public bool needsSleep = false;
    [Task]
    public bool isExhausted = false;
    [Task]
    public bool isSleeping = false;
    [Task]
    public bool needsPotty = false;
    [Task]
    public bool isLonely = false;
    [Task]
    public bool isPlaying = false;
    [Task]
    public bool isBored = false;

    public HarleySimulator simulator;

    // Start is called before the first frame update
    void Start()
    {
        hunger = 10;
        tiredness = 80;
        loneliness = 30;
        boredom = 50;
        pottyLevel = 18;
        simulator = GameObject.Find("Main Camera").GetComponent<HarleySimulator>();
    }

   /* 
    * Tasks for checking Harley's current measures 
    */

    [Task] // checks how hungry Harley is 
    void CheckHunger() {
        if(hunger < 15) {
            isHungry = true;
        } else {
            isHungry = false;
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
        if(simulator.hour > 7 && simulator.hour < 11) {
            needsNap = true;
        } else {
            needsSleep = true;
        }
    }
    [Task] // checks how badly Harley has to go to the bathroom 
    void CheckPotty() {
        if (pottyLevel < 6) {
            needsPotty = true;
        } else {
            needsPotty = false;
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
    [Task]
    void CheckLoneliness() {
        
    }
    [Task] // decreases boredom by a certain measure when Harley is playing 
    void IncreaseFun(int n) {
        boredom += n;
        Task.current.Succeed();
    }
    /* 
     * Tasks for altering harley's behavior depending on player actions 
     */

    [Task] // increases Harley's exhaustion by a certain measure 
    void IncreaseTiredness(int n) {
        tiredness -= n;
        Task.current.Succeed();
    }
    [Task] // signal that Harley is done playing 
    void DonePlaying() {
        isPlaying = false;
        Task.current.Succeed();
    }
     
    [Task] // outputs a message to the console 
    public void Say(string message) {
        Debug.Log(message);
        Task.current.Succeed();
    }

    
   
   
}
