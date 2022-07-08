using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralArrowSequence : MonoBehaviour
{
    [Header("==== DATA ====")]
    [SerializeField] int difficulty = 2;
    // difficulty determines the number of arrow keys needed to heal the zombie.
    // difficulty = 1: 1-2 arrow keys needed to heal
    //              2: 3-4 arrow keys needed to heal
    //              3: 5-6 arrow keys needed to heal
    List<string> sequence = new List<string>();
    List<string> currentState;
    
    [Header("=== DISPLAY ===")]
    [SerializeField] Color zombieColor;
    // The prefab we use for arrow icons. The sprite gets changed!
    [SerializeField] GameObject arrowIcon;
    [SerializeField] Sprite UP_sprite;
    [SerializeField] Sprite DOWN_sprite;
    [SerializeField] Sprite LEFT_sprite;
    [SerializeField] Sprite RIGHT_sprite;
    List<GameObject> arrowIcons = new List<GameObject>();
    // The offset for each arrow icon. For now, we set it to 20f/32f. (1f/32f is 1 pixel).
    [SerializeField] int iconbarPixelOffset = 20;
    Vector3 iconTransformOffset;

    // For destruction:
    EnemyChase enemyChase;
    Animator animator;
    DamagePlayerOnCollision damagePlayerOnCollision;
    bool dying = false;

    // Start is called before the first frame update
    void Start()
    {
        iconTransformOffset = new Vector3(0f, (float)iconbarPixelOffset/32f, 0f);
        enemyChase = gameObject.GetComponent<EnemyChase>();
        animator = gameObject.GetComponent<Animator>();
        damagePlayerOnCollision = gameObject.GetComponentInChildren<DamagePlayerOnCollision>();

        SupplyArrowSequence();
        InitializeArrowIcons();
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
        if (!dying && key == currentState[0])
        {
            // If it's a match, remove the item from the current list.
            currentState.RemoveAt(0);
            
            // If this made the list empty, destroy ourselves.
            if (currentState.Count <= 0){ StartCoroutine(DestroySelf(1.5f)); }

                //print($"Healing progress! The remaining sequence is: {string.Join(", ", currentState)}!");

            // Recolor the icon to signal that we've completed it.
            // We obtain the index through comparing the lengths of currentState and sequence.
            UpdateArrowIcon(sequence.Count - currentState.Count - 1, true);

        }
        else if (!dying)
        {
            // Otherwise, it was the wrong keypress. We restart from the top.
            currentState = new List<string>(sequence);
            // Recolor all icons back to our zombie color.
            for (int i = 0; i < sequence.Count; i++){ UpdateArrowIcon(i, false); }

                //print($"Healing failure! The sequence has reverted to: {string.Join(", ", currentState)}!");
        }
    }

    void InitializeArrowIcons()
    {
        // Initializes the set of arrow icons.

        // Formula for calculating the sprite position based on the index.
        // Here, we move the first one over to make room for the rest.
        iconTransformOffset.x -= 4*(sequence.Count-1)/32f;
        // Here, we give each set of arrows a slightly different depth, so there's no z-fighting.
        iconTransformOffset.z += Random.Range(0f, 0.99999f);

        // For each arrow in our sequence:
        for (int i = 0; i < sequence.Count; i++)
        {   
            // Instantiate...
            GameObject icon = Instantiate(  arrowIcon,                                      // an icon,
                                            this.transform.position + iconTransformOffset,  // at our position + offset,
                                            Quaternion.identity,                            // with no rotation,
                                            this.transform);                                // as a child of ourselves.

            // Change the icon's sprite depending on what arrow we represent.
            SpriteRenderer iconRenderer = icon.GetComponent<SpriteRenderer>();
            switch (sequence[i])    
            {
                case "up":      iconRenderer.sprite = UP_sprite;       break;
                case "down":    iconRenderer.sprite = DOWN_sprite;     break;
                case "left":    iconRenderer.sprite = LEFT_sprite;     break;
                case "right":   iconRenderer.sprite = RIGHT_sprite;    break;
            }

            // Change the icon's color to the zombieColor.
            iconRenderer.color = zombieColor;

            // Add it to the list! We use this list to change color later on.
            arrowIcons.Add(icon);
            // Update the offset for the next loop.
            iconTransformOffset.x += 8f/32f;
        }   
    }

    void UpdateArrowIcon(int index, bool done, bool doCustomColor=false, Vector4 custom=default(Vector4))
    {
        // Updates an arrow icon with a new color.
        // params:
        // int index: The position in the list arrowIcons of the arrow we're updating.
        // bool done: Whether this icon represents an arrow that has already been pressed.
        //      This generally corresponds to color: if done is true, we change the color to
        //                                           (253f/255f, 151f/255f, 31f/255f, 1), orange.
        //                                           if done is false, we change the color to
        //                                           the zombie color defined in the inspector.                                           
        // bool doCustomColor: If this is true, we overwrite the color with the one supplied by...
        // Vector4 customColor: The customColor used if doCustomColor is true.

        // Get the SpriteRenderer for the given icon.
        SpriteRenderer iconRenderer = arrowIcons[index].GetComponent<SpriteRenderer>();

        // If we're not doing a custom color:
        if (!doCustomColor){
            // If the arrow is complete, make it orange.
            if (done){ iconRenderer.color = new Color(253f/255f, 151f/255f, 31f/255f, 1); }
            // Otherwise, reset it to the zombieColor.
            else { iconRenderer.color = zombieColor; }
        }
        // If we are using a custom color, just set it to that.
        else { iconRenderer.color = new Color(custom.x, custom.y, custom.z, custom.w); }
    }

    IEnumerator DestroySelf(float delay)
    {
        dying = true;
        // Stop moving, animating, and doing damage...
        enemyChase.stopped = true;
        enemyChase.dying = true;
        damagePlayerOnCollision.doDamage = false;
        // Wait 1.5 seconds...
        yield return new WaitForSeconds(delay);
        // And fucking die.
        Destroy(gameObject.transform.parent.gameObject);
    }

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
        //print(string.Join(", ", sequence));
    }

    string GetRandomArrow()
    {
        // Return a random element from the below list.
        return new string[]{"up", "down", "left", "right"}[Random.Range(0, 4)];
    }
}
