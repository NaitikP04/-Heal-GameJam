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
    PauseMenu pauseMenu;

    [Header("=== DISPLAY ===")]
    [SerializeField] Color zombieColor;
    // Heal color, used for correct arrows and in our zombie color lerp.
    SpriteRenderer zombieRenderer;
    // Zombie color, determined in the Inspector
    Color healColor = new Color(253f/255f, 151f/255f, 31f/255f, 1);
    [SerializeField] GameObject healParticle;
    Color humanColor = new Color(248f/255f, 248f/255f, 242f/255f, 1);

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
    Animator zombAnimator;
    DamagePlayerOnCollision damagePlayerOnCollision;
    bool fullyHealed = false;
    HealedZombieRun healedZombieRun;
    DestroyOnExitScreen destroyOnExitScreen;

    // Start is called before the first frame update
    void Start()
    {
        zombieRenderer = gameObject.GetComponent<SpriteRenderer>();
        pauseMenu = GameObject.FindWithTag("Pause Button").GetComponent<PauseMenu>();

        iconTransformOffset = new Vector3(0f, (float)iconbarPixelOffset/32f, 0f);
        enemyChase = gameObject.GetComponent<EnemyChase>();
        zombAnimator = gameObject.GetComponent<Animator>();
        damagePlayerOnCollision = gameObject.GetComponentInChildren<DamagePlayerOnCollision>();
        healedZombieRun = gameObject.GetComponent<HealedZombieRun>();
        destroyOnExitScreen = gameObject.GetComponentInChildren<DestroyOnExitScreen>();
        destroyOnExitScreen.enabled = false;

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
        if (!pauseMenu.paused && !fullyHealed && key == currentState[0])
        {
            // If it's a match, remove the item from the current list.
            currentState.RemoveAt(0);
            
            // If this made the list empty, destroy ourselves.
            if (currentState.Count <= 0){ StartCoroutine(FullyHealedSelf()); }

                //print($"Healing progress! The remaining sequence is: {string.Join(", ", currentState)}!");

            // Recolor the icon to signal that we've completed it.
            // We obtain the index through comparing the lengths of currentState and sequence.
            UpdateArrowIcon(sequence.Count - currentState.Count - 1, true);
            // Stop the enemy from moving for 0.5 seconds.
            StartCoroutine(enemyChase.StopMovingBriefly(0.3f));
            // Get a reference to the percentage of the way we are to being fully healed.
            float progress = ((float)sequence.Count - (float)currentState.Count)/(float)sequence.Count;
            // Update the zombie color with the
            UpdateZombieColor(progress);
            // And spawn a number of particles based on it too.
            SpawnParticles(progress);
        }
        else if (!fullyHealed)
        {
            // Otherwise, it was the wrong keypress. We restart from the top.
            currentState = new List<string>(sequence);
            // Recolor all icons back to our zombie color.
            for (int i = 0; i < sequence.Count; i++){ UpdateArrowIcon(i, false); }
            // And restore our zombie back to the zombie color.
            UpdateZombieColor(0f);

                //print($"Healing failure! The sequence has reverted to: {string.Join(", ", currentState)}!");
        }
    }

    public IEnumerator FullyHealedSelf()
    {
        zombAnimator.SetBool("fullyHealed", true);
        fullyHealed = true;
        // Stop moving, animating, and doing damage...
        enemyChase.fullyHealed = true;
        damagePlayerOnCollision.doDamage = false;
        // Shrink all the arrow icons...
        for (int i = 0; i < sequence.Count; i++){
            StartCoroutine(ShrinkObjectToNone(arrowIcons[i], i/10f)); }
        // Wait a second...
        yield return new WaitForSeconds(1f);
        // And change back to a human color.
        UpdateZombieColor(1f, true);
        
        // Wait another second,
        yield return new WaitForSeconds(0.75f);
        // And start getting the fuck out of here!
        gameObject.GetComponent<Collider2D>().enabled = false;
        zombAnimator.SetBool("healedRun", true);
        healedZombieRun.enabled = true;
        destroyOnExitScreen.enabled = true;
        
    }

    // =========================================================
    //                        GRAPHICS
    // =========================================================

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
            if (done){ iconRenderer.color = healColor; }
            // Otherwise, reset it to the zombieColor.
            else { iconRenderer.color = zombieColor; }
        }
        // If we are using a custom color, just set it to that.
        else { iconRenderer.color = new Color(custom.x, custom.y, custom.z, custom.w); }
    }

    void UpdateZombieColor(float progress, bool toHuman=false)
    {
        // If we're not transitioning to human color:
                                    // Start lerping between the zombie color,
                                                                    // the heal color
                                                                        // ending at the float representing
                                                                        // our progress,
                                                                            // and do so over 0.25 seconds.
        if (!toHuman){ StartCoroutine(coroutine_UpdateZombieColor(zombieColor, healColor, progress, 0.25f)); }
        // If we ARE transitioning to human color:
                                    // Start lerping between the current color,
                                                                    // the human color,
                                                                        // ending at 1 (ie 100% lerped),
                                                                            // and do so over 1 second.
        if (toHuman){ StartCoroutine(coroutine_UpdateZombieColor(zombieRenderer.color, humanColor, 1f, 1f)); }
    }
    IEnumerator coroutine_UpdateZombieColor(Color startColor, Color endColor, float targetProgress, float maxTime )
    {
        float elapsed = 0;
        Color currentColor = zombieRenderer.color;                                      // initial color for this lerp
        Color currentTargetColor = Color.Lerp(startColor, endColor, targetProgress);    // final color for this lerp

        while (elapsed < maxTime){

                zombieRenderer.color = Color.Lerp(currentColor, currentTargetColor, (elapsed/maxTime));

                elapsed += Time.deltaTime;
                yield return null;
            }
    }

    void SpawnParticles(float progress)
    {
        int particlesToSpawn = 0;
        switch (progress)
        {
            case > 0.00f    and <= 0.25f: particlesToSpawn = 1;    break;
            case > 0.25f    and <= 0.50f: particlesToSpawn = 2;    break;
            case > 0.50f    and <= 0.75f: particlesToSpawn = 3;    break;
            case > 0.75f    and <= 1.00f: particlesToSpawn = 4;    break;
        }

        for (int i = 0; i < particlesToSpawn; i++)
        {
            Instantiate(healParticle, this.transform.position, Quaternion.identity, this.transform.parent);
        }
    }

    IEnumerator ShrinkObjectToNone(GameObject thing, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0;
        while (elapsed < 0.5f){
            thing.transform.localScale = thing.transform.localScale * 0.99f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(thing.gameObject);
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
