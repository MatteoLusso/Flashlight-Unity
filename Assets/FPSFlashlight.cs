using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSFlashlight : MonoBehaviour
{
    public Camera player;
    public float lightCelerity;
    public float lightSmoothFactor;
    public bool manualLightOffset;
    public Vector3 lightOffset;
    public bool flickering;
    private float flickeringTimer;
    public float flickeringMaxWait;
    public float flickeringMinWait;
    private bool isFlickering;
    public int flickeringMaxDuration;
    public float lightMaxRange;
    private bool lightOn;

    void Start() {

        if(!manualLightOffset)
        {
            lightOffset = this.transform.position - player.transform.position;
        }

        isFlickering = false;
        lightOn = true;

        this.GetComponent<Light>().range = lightMaxRange;
    }
    
    void LateUpdate() {

        // Change light rotation to follow the direction between mouse cursor and camera
        
        Ray mouseRay = player.ScreenPointToRay(Input.mousePosition);

        this.transform.forward = Vector3.Lerp(this.transform.forward, mouseRay.direction.normalized, lightCelerity * Time.deltaTime);

        Quaternion lightAngle = Quaternion.AngleAxis(Vector3.SignedAngle(this.transform.forward, player.transform.forward, Vector3.up), Vector3.up);

        // Calculate new light position

        Vector3 lightNewPosition;
        lightNewPosition = player.transform.TransformPoint(lightOffset);
        this.transform.position = Vector3.Lerp(this.transform.position, lightNewPosition, lightSmoothFactor * Time.deltaTime);

        // Light On/Off

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(lightOn)
            {
                this.GetComponent<Light>().range = 0;
                flickering = false;
                lightOn = false;

                StopCoroutine("Flickering");
            }
            else
            {
                this.GetComponent<Light>().range = lightMaxRange;
                flickering = true;
                lightOn = true;
            }
        }

        // Flickering Effect 

        if(flickering)
        {
            if(flickeringTimer >= Random.Range(flickeringMinWait, flickeringMaxWait) && !isFlickering)
            {
                StartCoroutine("Flickering");
            }

            if(isFlickering)
            {
                flickeringTimer = 0.0f;
            }
            else
            {
                flickeringTimer += Time.deltaTime;
            }
        }
    }

    IEnumerator Flickering()
    {
        isFlickering = true;

        int flickeringDuration = (int)Random.Range(4, flickeringMaxDuration);

        if(flickeringDuration % 2 != 0)
        {
            flickeringDuration += 1;
        }

        Debug.Log(flickeringDuration);

        for(int i = 0; i <= flickeringDuration; i++)
        {
            flickeringTimer = 0.0f;
            if(i % 2 != 0)
            {
                this.GetComponent<Light>().range = Random.Range(0.0f, lightMaxRange);
                yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
            }
            else
            {
                this.GetComponent<Light>().range = lightMaxRange;
                yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
            }
        }
        isFlickering = false;
    }
}
