using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float walkingSpeed, runningSpeed, jumpForce, gravity, lookSpeed, lookXLimit;

    private CharacterController characterController;
    private Camera playerCamera;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    public bool CanMove { get; private set; }

    public Room CurrentRoom { get; private set; }
    public void SetRoom(Room room) { CurrentRoom = room; }

    private bool isJumping;
    private bool isRunning;

    [Header("Sounds")]
    [SerializeField]
    [FMODUnity.EventRef]
    private string walkEvent;
    private FMOD.Studio.EventInstance walkSound;

    [SerializeField]
    [FMODUnity.EventRef]
    private string runEvent;
    private FMOD.Studio.EventInstance runSound;

    [SerializeField]
    [FMODUnity.EventRef]
    private string jumpEvent;
    private FMOD.Studio.EventInstance jumpSound;

    [SerializeField]
    [FMODUnity.EventRef]
    private string landEvent;
    private FMOD.Studio.EventInstance landSound;

    [SerializeField]
    private float walkHearingDeistance;
    [SerializeField]
    private float runHearingDeistance;
    [SerializeField]
    private float jumpAndLandHearingDeistance;

    [SerializeField]
    private float timeBetweenStep;
    private float timeSinceLastFootstep;

    private Enemy enemy;

    private float curSpeedX, curSpeedY;

    private GameManager gameManager;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CanMove = true;

        walkSound = FMODUnity.RuntimeManager.CreateInstance(walkEvent);
        runSound = FMODUnity.RuntimeManager.CreateInstance(runEvent);
        jumpSound = FMODUnity.RuntimeManager.CreateInstance(jumpEvent);
        landSound = FMODUnity.RuntimeManager.CreateInstance(landEvent);

        enemy = FindObjectOfType<Enemy>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public Vector3 GetEyesPosition()
    {
        return new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y - .5f, playerCamera.transform.position.z);
    }

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        curSpeedX = CanMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        curSpeedY = CanMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && CanMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
            jumpSound.start();
            MakeNoise(jumpAndLandHearingDeistance);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (characterController.isGrounded && isJumping)
        {
            landSound.start();
            MakeNoise(jumpAndLandHearingDeistance);
            isJumping = false;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            isJumping = true;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (CanMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if (curSpeedX != 0 || curSpeedY != 0) FootstepSound();
        }
    }

    private void FootstepSound()
    {
        float tbsMod = 1;
        if (isRunning)
        {
            tbsMod = 2;
        }

        if (Time.time - timeSinceLastFootstep >= timeBetweenStep / tbsMod)
        {
            if (isRunning)
            {
                runSound.start();
                MakeNoise(runHearingDeistance);
            }
            else
            {
                walkSound.start();
                MakeNoise(walkHearingDeistance);
            }

            timeSinceLastFootstep = Time.time;
        }
    }

    private void MakeNoise(float distance)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

        if (distanceToEnemy <= distance) enemy.HearingSound(transform.position);
    }

    public void Death()
    {
        CanMove = false;
        gameManager.DeathSequence();
    }
}
