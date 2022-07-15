using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIBuffManager : MonoBehaviour
{
    public IEnumerator powerUpForDuration(float speedMultiplier, float powerUpTime, GameObject target)
        {
            target.GetComponent<BasicPlayerMovement>().maxVelocity *= speedMultiplier;
            target.GetComponent<BasicPlayerMovement>().accelerationTime = 0.05f;
            target.GetComponent<Health>().invulnerable = true;
            target.GetComponent<Health>().SIBuffActive = true;
            
            yield return new WaitForSeconds(powerUpTime);
            
            //float elapsed = 0f;
            //while (elapsed < powerUpTime)
            //{
            //    elapsed += Time.deltaTime;
            //    Debug.Log("i exist!");
            //    yield return null;
            //}
            
            target.GetComponent<BasicPlayerMovement>().maxVelocity /= speedMultiplier;
            target.GetComponent<BasicPlayerMovement>().accelerationTime = 0.3f;
            target.GetComponent<Health>().invulnerable = false;
            target.GetComponent<Health>().SIBuffActive = false;
        }
}
