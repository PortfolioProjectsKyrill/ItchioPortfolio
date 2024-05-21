using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    [Header("Speed Vars")]
    [SerializeField] private float realSpeed;
    public float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float backwardsMaxSpeed;
    [SerializeField] private float speedMulti = 1f;

    [Header("Accelerating & Decelerating")]
    [SerializeField] private float decelerateTime;
    [SerializeField] private float decelerateMaxTime;
    [SerializeField] private float accelerateTime;
    [SerializeField] private float accelerateMaxTime;

    [Header("Lerping Vars (DO NOT TOUCH)")]
    [SerializeField] private float lerpTime;
    [SerializeField] private float lerpTimeReverseMulti;

    [Header("Turning")]
    [SerializeField] private float turnSpeed;

    [Header("Controlling booleans")]
    [SerializeField] private bool canMove;
    public bool canLand;
    public bool hasLanded;

    [Header("Landing")]
    public LandingPlatform currentPlatform;
    [SerializeField] private Vector3 currentLandingPos;
    [SerializeField] private Quaternion currentLandingRot;
    [SerializeField] private Coroutine landingCoroutine;
    [SerializeField] private float landingLerpSpeed;
    [SerializeField] private float totalLandingLerpTime;
    [SerializeField] private Vector3 posBeforeLanding;

    [Header("Fuel Variables")]
    [SerializeField] private float fuelConsumption;
    public float totalFuel;
    [SerializeField] private float minFuelConsmptSpeed;
    [SerializeField] private float maxFuel;
    private readonly float minFuel = 0;

    [Header("Timers")]
    [SerializeField] private float t = 0;
    private readonly float time = 1;

    [Header("Text")]
    [SerializeField] private Image Fuel;
    [SerializeField] private bool hasInput;
    public bool canUseBoat;
    public bool goingBackwards;
    private Rigidbody rb;

    private void Awake()
    {
        totalFuel = 100;//because of timing needs to be in awake
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LandBoatStart();
        hasLanded = true;
        canMove = false;
    }

    public void LandBoatStart()
    {
        landingCoroutine = StartCoroutine(LerpToPlatform());
    }

    private void FixedUpdate()
    {
        realSpeed = (speed * speedMulti);

        if (realSpeed >= 0)
            goingBackwards = false;
        else
            goingBackwards = true;

        if (totalFuel > minFuel)
        {
            rb.velocity = transform.forward * realSpeed;

            if (speed > minFuelConsmptSpeed || speed < -minFuelConsmptSpeed)
            {
                UseFuel();
            }
        }

        if (!hasInput && canUseBoat)
        {
            if (realSpeed > 0)
            {
                Decelerate(0);
            }
            else
            {
                Accelerate(0);
            }
        }

        if (!canUseBoat || !canMove)
            return;//break out of function

        //all the seperate functions
        Turn();
        Moving();
    }

    private void Update()
    {
        if (Fuel != null)
        {
            Fuel.fillAmount = Mathf.Lerp(Fuel.fillAmount, totalFuel / maxFuel, Time.deltaTime * 5);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            totalFuel = maxFuel;
        }

        if (!canUseBoat)
            return;//break out of function

        Landing();
    }

    private void UseFuel()
    {
        if (t < time)
        {
            t += Time.deltaTime;
        }
        else
        {
            if (t > time) { t = time; }

            if (!goingBackwards)
            {
                totalFuel -= (fuelConsumption * speed);
            }
            else if (goingBackwards && speed < 0)
            { totalFuel += (fuelConsumption * speed); }

            if (totalFuel < 0)
            {
                totalFuel = 0;
            }

            t = 0;
        }
    }

    public void AddFuel(float fuelAmount)
    {
        if (totalFuel + fuelAmount > maxFuel)
        {
            totalFuel = maxFuel;
        }
        else
        {
            totalFuel += fuelAmount;
        }
    }

    public void AddFuelCapacity(float value)
    {
        maxFuel += value;
    }

    public void RemoveFuelCapacity(float value)
    {
        maxFuel -= value;
    }

    public void AddSpeedMultiplier()
    {
        speedMulti += 0.33f;
    }

    public void RemoveSpeedMultiplier()
    {
        speedMulti -= 0.33f;
    }

    private void Moving()
    {
        //speed *= speedMulti;
        //rb.MovePosition(transform.position + transform.forward * (speed * speedMulti) / 10);
        //transform.position += transform.forward * (speed * speedMulti) / 10;

        if (Input.GetKey(KeyCode.W))
        {
            hasInput = true;
            Accelerate(maxSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            hasInput = true;
            Decelerate(backwardsMaxSpeed);
        }
        else { hasInput = false; }
    }
    
    /// <summary>
    /// Accelerates vehicle by increasing speed variable
    /// </summary>
    private void Accelerate(float endSpeed)
    {
        if (accelerateTime < accelerateMaxTime)
        {
            if (realSpeed < endSpeed)
            {
                speed = Mathf.Lerp(speed, endSpeed, (accelerateTime / accelerateMaxTime) / 25);
            }
        }

        if (accelerateTime > accelerateMaxTime)
        {
            accelerateTime = 0;
        }
        else
        {
            accelerateTime += Time.deltaTime;
        }

    }

    /// <summary>
    /// Delerates vehicle by decreasing speed variable
    /// </summary>
    /// <param name="endSpeed"></param>
    private void Decelerate(float endSpeed)
    {
        if (decelerateTime < decelerateMaxTime)
        {
            speed = Mathf.Lerp(speed, endSpeed, (decelerateTime / decelerateMaxTime) / 25);
        }

        if (decelerateTime > decelerateMaxTime)
            decelerateTime = 0;
        else
            decelerateTime += Time.deltaTime;
    }

    /// <summary>
    /// Handles the rotation of the vehicle
    /// </summary>
    private void Turn()
    {
        Vector3 steerDirVect;//direction vector(rotation)
        float steerAmount;//amount that gets added to rotation
        float steerDirection;

        //determine rotation amount
        if (speed < 0)
            steerDirection = -Input.GetAxis("Horizontal");
        else
            steerDirection = Input.GetAxis("Horizontal");

        steerAmount = (turnSpeed * realSpeed) * steerDirection;

        //apply rotation amount to rotation vector
        steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        //actually rotate
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, 2 * Time.deltaTime);
    }

    /// <summary>
    /// Checking in different function instead of update directly
    /// </summary>
    private void Landing()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasLanded == false)
        {
            landingCoroutine ??= StartCoroutine(LerpToPlatform());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && hasLanded == true)
        {
            landingCoroutine ??= StartCoroutine(LerpToOGPos());
        }
    }

    #region Lerping Coroutines

    /// <summary>
    /// WIP, Lerps vehicle to landing position
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpToPlatform()
    {
        if (canLand)
        {
            print("yes");

            //confirm vehicle has landed
            hasLanded = true;

            speed = 0;

            var t = 0f;
            var start = transform.position;
            var rotation = transform.rotation;
            //secure pos before lerping
            posBeforeLanding = start;

            canMove = false;
            //Locate Platform (get child landing location)
            currentLandingPos = currentPlatform.landPos;
            currentLandingRot = currentPlatform.landRot;
            //Disable user input
            //Lerp (set doneMoving to true at the end)

            while (t < totalLandingLerpTime)
            {
                t += Time.deltaTime;

                transform.position = Vector3.Slerp(start, currentLandingPos, t);
                transform.rotation = Quaternion.Slerp(rotation, currentLandingRot, t);

                yield return new WaitForSeconds(0.01f);
            }

            //set to equivelant of null
            currentLandingPos = Vector3.zero;

            landingCoroutine = null;

            //canUseBoat = true;
        }
    }

    private IEnumerator LerpToOGPos()
    {

        //canUseBoat = true;
        hasLanded = false;

        var t = 0f;
        var start = transform.position;
        Vector3 target = start;
        //increase vertical
        target.y = posBeforeLanding.y;

        while (t < totalLandingLerpTime)
        {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(start, target, t);

            yield return new WaitForSeconds(0.01f);
        }

        canMove = true;

        posBeforeLanding = Vector3.zero;

        landingCoroutine = null;

    }
    #endregion

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("maxfuel", maxFuel);
        PlayerPrefs.SetFloat("totalfuel", totalFuel);
        PlayerPrefs.SetFloat("speedmulti", speedMulti);
    }
}
