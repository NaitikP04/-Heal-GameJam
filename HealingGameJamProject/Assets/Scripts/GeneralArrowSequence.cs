using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralArrowSequence : MonoBehaviour
{
   
    [SerializeField] int difficulty = 2;
    // difficulty determines the number of arrow keys needed to heal the zombie.
    // difficulty = 1: 1-2 arrow keys needed to heal
    //              2: 3-4 arrow keys needed to heal
    //              3: 5-6 arrow keys needed to heal
    List<string> sequence = new List<string>();
    List<string> currentState;
    //Random rnd = new Random();

    // Start is called before the first frame update
    void Start()
    {
        SupplyArrowSequence();
        currentState = new List<string>(sequence);
    }

    // Update is called once per frame
    void Update()
    {
        // Tracking keypresses:
        if (Input.GetKeyDown("up"))     { LogKeypress("up");    }
        if (Input.GetKeyDown("down"))   { LogKeypress("down");  }
        if (Input.GetKeyDown("left"))   { LogKeypress("left");  }
        if (Input.GetKeyDown("right"))  { LogKeypress("right"); }
    }

    void LogKeypress(string key)
    {
        if (key == currentState[0])
        {
            currentState.RemoveAt(0);
            if (currentState.Count <= 0){ DestroySelf(); }
            print($"Healing progress! The remaining sequence is: {string.Join(", ", currentState)}!");
        }
        else
        {
            currentState = new List<string>(sequence);
            print($"Healing failure! The sequence has reverted to: {string.Join(", ", currentState)}!");
        }
    }

    void DestroySelf() { Destroy(this.gameObject); }

    void SupplyArrowSequence()
    {
        int length;
        // Take the difficulty,
        //                 multiply by 2,
        //                      and subtract 1 or 0.
        length = difficulty*2 - Random.Range(0,2);
        
        // For length # of times, add a random arrow.
        for (int i = 0; i < length; i++){ sequence.Add(GetRandomArrow()); }

        // DEBUG CODE UNTIL PROPER DISPLAY CAN BE MADE
        print(string.Join(", ", sequence));
    }

    string GetRandomArrow()
    {
        // Return a random element from the below list.
        return new string[]{"up", "down", "left", "right"}[Random.Range(0, 4)];
    }
}
