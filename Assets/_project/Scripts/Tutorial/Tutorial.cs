using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Tutorial : MonoBehaviour
{
    [Header("references")] public Transform knappeMan;
    public Transform moveTowards;
    [SerializeField] private DialogueInteraction tutorialOnStart;
    [SerializeField] private DialogueInteraction tutorialJump;
    [SerializeField] private DialogueInteraction tutorialMoveToPond;
    [SerializeField] private DialogueInteraction tutorialFishing;
    private PlayerStateMachine _playerStateMachine;
    private Transform _playerTransform;
    private BoxCollider _boxColl;

    public float distanceThreshold;
    public float angleThreshold;
    
    //moving
    private bool _isCounting;
    [SerializeField] private bool movingCompleted;

    //jumping
    private int _timesToJump = 5;
    private bool _inJumpingTutorial;
    [SerializeField] private bool jumpingCompleted;

    //MTW
    [SerializeField] private bool _moveTowardsPondCompleted;
    [SerializeField] private Pond tutorialFishingPond;
    [SerializeField] private GameObject endTutorial;

    private void Awake()
    {
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        _playerTransform = _playerStateMachine.transform;
        _boxColl = knappeMan.GetComponent<BoxCollider>();
        _boxColl.enabled = false; 
    }

    private void Start()
    {
        DialogueTrigger.TriggerDialogue(tutorialOnStart);
    }
    
    private bool IsPlayerCloseAndFacingNpc()
    {
        float distance = Vector3.Distance(_playerTransform.position, knappeMan.position);
        if (distance <= distanceThreshold)
        {
            Vector3 directionToNpc = (knappeMan.position - _playerTransform.position).normalized;
            float angle = Vector3.Angle(_playerTransform.forward, directionToNpc);

            return angle < angleThreshold;
        }

        return false;
    }
    public void EndTutorialCall()
    {
        StartCoroutine(EndTutorial());
    }
    private IEnumerator EndTutorial()
    {
        //show end tutorial screen
        endTutorial.SetActive(true);
        yield return new WaitForSeconds(5);
        MenuManager.instance.LoadScene(3);
    }

    /// <summary>
    /// checks if player has pressed each key in the array once before setting "movingCompleted" to true
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckPlayerMovementHold()
    {
        KeyCode[] wasd = { KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S };
        bool[] wasdBools = new bool[4];

        while (!wasdBools.All(x => x))
        {
            for (int i = 0; i < wasd.Length; i++)
            {
                if (Input.GetKey(wasd[i]))
                {
                    wasdBools[i] = true;
                }
            }

            yield return null;
        }

        if (wasdBools.All(x => x))
        {
            movingCompleted = true;
        }
    }
    
    private void Update()
    {
        if (!movingCompleted && !_isMoveToPondInitiated)
        {
            CheckMovementTutorial();
        }

        if (!jumpingCompleted && movingCompleted)
        {
            CheckJumpTutorial();
        }

        if (jumpingCompleted && movingCompleted && !_moveTowardsPondCompleted)
        {
            MoveTowardsPond();
        }

        if (_moveTowardsPondCompleted)
        {
            CheckFishingTutorial();
        }
        
    }

    private void CheckMovementTutorial()
    {
        if (!tutorialOnStart.inThisDialogue)
        {
            StartCoroutine(CheckPlayerMovementHold());
        }
    }

    private void CheckJumpTutorial()
    {
        if (movingCompleted && !tutorialJump.inThisDialogue && !jumpingCompleted && !_inJumpingTutorial)
        {
            _inJumpingTutorial = true;
            DialogueTrigger.TriggerDialogue(tutorialJump);
        }

        if (_playerStateMachine.IsJumpPressed && !jumpingCompleted)
        {
            _timesToJump--;
            if (_timesToJump == 0)
                jumpingCompleted = true;
        }
    }

    private IEnumerator MoveKnappeManTowards(Transform target, float duration)
    {
        Vector3 startPosition = knappeMan.position;
        Quaternion startRotation = knappeMan.rotation; // Store the initial rotation

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            knappeMan.position = Vector3.Lerp(startPosition, target.position, t);

            // Calculate the target rotation based on the direction
            Vector3 direction = (target.position - knappeMan.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Interpolate between the initial and target rotations
            knappeMan.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that knappeMan reaches the exact target position and rotation
        knappeMan.position = target.position;

        yield return new WaitForSeconds(0.25f);
        
        StartCoroutine(TurnKnappeManSmoothly());

    }
    
    private IEnumerator TurnKnappeManSmoothly()
    {
        Quaternion currentRotation = knappeMan.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f); // Yaw 180 degrees
        float rotationTime = 1.0f; // Adjust the time as needed

        float elapsedRotationTime = 0f;
        while (elapsedRotationTime < rotationTime)
        {
            float t = elapsedRotationTime / rotationTime;
            knappeMan.rotation = Quaternion.Slerp(currentRotation, targetRotation, t);

            elapsedRotationTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly 180 degrees on the Y-axis
        knappeMan.rotation = targetRotation;

        _boxColl.enabled = true;
        yield break;
    }
    
    private bool _isMoveToPondInitiated = false;
    private void MoveTowardsPond()
    {
        // If the movement and dialogue haven't been initiated yet
        if (!_isMoveToPondInitiated)
        {
            DialogueTrigger.TriggerDialogue(tutorialMoveToPond);
            
            // Check if the dialogue is not in progress
            if (!tutorialMoveToPond.inThisDialogue)
            {
                // Set the flag to true, indicating that the dialogue is done
                _isMoveToPondInitiated = true;

                // Start the movement coroutine
                StartCoroutine(MoveKnappeManTowards(moveTowards, 3f)); // 3f is the duration of the movement
                _moveTowardsPondCompleted = true;
            }
        }
    }

    private void CheckFishingTutorial()
    {
        if (IsPlayerCloseAndFacingNpc())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueTrigger.TriggerDialogue(tutorialFishing);
            }

            tutorialFishingPond.enabled = true;
        }
    }
}