using UnityEngine;
using Yarn.Unity;

public class playerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float floorY = -1f; 
    public float stopDistance = 0.5f; 

    private Vector3 targetPosition;
    private bool isMoving = false;
    private System.Action tillarrive;

    private SpriteRenderer sr;
    private Animator anim;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource walkingsoundsource;
    [SerializeField] private AudioClip walkingsound;

    public DialogueRunner lineshow;
    public static bool isShowingobj = false;

    void Awake()
    {
        if (lineshow != null)
            lineshow.AddCommandHandler("lookatu", Lookatu);
    }

    void Start()
    {
        targetPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        floorY = transform.position.y;
        Debug.Log("Default flipX = " + sr.flipX);
        
        if (walkingsoundsource == null)
        {
            walkingsoundsource = GetComponent<AudioSource>();
        } 
        
        if (walkingsoundsource != null)
        {
            walkingsoundsource.loop = true;
            walkingsoundsource.playOnAwake = false;
            if (walkingsound != null)
            {
                walkingsoundsource.clip = walkingsound;
            }
        } 
    }



    void Update()
    {

        if (inventoryManager.inventoryOpen)
        {
            isMoving = false;
            tillarrive = null;
        }
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            float diff = targetPosition.x - transform.position.x;
            if (diff > 0.1f)
                
                sr.flipX = true;
            else if (diff < -0.1f)
                sr.flipX = false;

            if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
            {
                isMoving = false;
                if (tillarrive != null)
                {
                    tillarrive.Invoke();
                    tillarrive = null;
                }
            }
        }

        if (anim!=null)
        {
            anim.SetBool("isMoving", isMoving);
        }

        if (walkingsoundsource != null && walkingsound != null)
        {
            if (isMoving && !walkingsoundsource.isPlaying)
            {
                walkingsoundsource.Play();
            }
            else if (!isMoving && walkingsoundsource.isPlaying)
            {
                walkingsoundsource.Stop();
            }
        }

        // lock Y always
        transform.position = new Vector3(
            transform.position.x,
            floorY,
            transform.position.z
        );
    }

    public void Lookatu()
    {
        isMoving = false;
        tillarrive = null;
        if (anim != null)
           anim.SetTrigger("lookatu");
    }

    public void MoveandInteract(Vector3 destination, System.Action arrivalact)
    {
        if (lineshow != null && lineshow.IsDialogueRunning) return;
        isMoving = false;
        tillarrive = null;

        float xDistance = Mathf.Abs(transform.position.x - destination.x);
        if (xDistance < stopDistance)
        {
            if (anim != null) anim.SetBool("isMoving", false);
            if (arrivalact != null)
               arrivalact.Invoke();
            return;
        }
        targetPosition = new Vector3(
            destination.x,
            floorY,              
            transform.position.z
        );
        tillarrive = arrivalact;
        isMoving = true;
    }
}