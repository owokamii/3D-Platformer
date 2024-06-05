using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Player_FSM : MonoBehaviour
{
    #region For All Variables
    private StateMachine stateMachine;
    public SpriteRenderer spriteRenderer;

    [Header("Targets")]
    [SerializeField] private Transform predator;

    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [Header("Health Parameter")]
    [SerializeField] private float health = 100.0f;

    [Header("Vehicle Parameter")]
    //[SerializeField] private float wanderRadius = 2f;
    //[SerializeField] private float distanceAhead = 4f;
    //[SerializeField] private float nextTargetTimer = 10.0f;
    public float slowingRadius = 1.0f;
    //[SerializeField] private float stoppingDistance = 0.05f;

    [Header("Detection Range")]
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float detectionRadius = 2.0f;
    [SerializeField] private float detectionCooldown = 5.0f;
    private float currentTimer;

    [Header("Wander Parameter")]
    private Vector3 randomTarget;
    private Vector3 circleLocation;
    private float currentTargetTimer;

    [Header("Booleans")]
    private bool reachedPredator;
    private bool isFleeing;
    private bool isShot;

    [Header("Corpse")]
    [SerializeField] private GameObject meatPrefab;

    [Header("Sprites")]
    public Sprite wander;
    public Sprite flee;
    #endregion For All Variables

    private void Awake()
    {
        stateMachine = new StateMachine();

        // 1. Create all possible states
        #region For All States
        var playerMoveState = new PlayerState_Move(this);         // Move
        #endregion For All States

        // 2. Set all transitions
        #region For All Transitions
        /*bunnyWanderState.AddTransition(bunnyFleeState, (timeInState) =>
        {
            return PredatorInRange() || ProjectileInRange();
        });

        bunnyFleeState.AddTransition(bunnyWanderState, (timeInState) =>
        {
            return !PredatorAvailable() && !PredatorLocked() && !ProjectileInRange();
        });*/
        #endregion For All Transitions

        // 3. Set the starting state
        stateMachine.SetInitialState(playerMoveState);
    }

    private void Update()
    {
        UpdateKnowledge();
        stateMachine.Tick(Time.deltaTime);
    }

    public void UpdateKnowledge()
    {
        UpdateBars();
        UpdateDetection();
        StartCooldown();
    }

    private void UpdateBars()
    {
        float normalizedHealth = health / 100;

        healthText.text = $"{normalizedHealth:P0}";
    }

    private void UpdateDetection()
    {
        if (!predator)
        {
            predator = null;
            isShot = false;
            isFleeing = false;
        }

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.right * transform.localScale.x, 0, detectionLayer);

        if (hit.collider != null)
        {

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Human") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Predator"))
            {
                predator = hit.collider.gameObject.transform;
                currentTimer = 0;
                reachedPredator = true;
                isFleeing = true;
            }
        }
        else
        {
            reachedPredator = false;
        }
    }

    private void StartCooldown()
    {
        if (predator)
        {
            if (PredatorLocked() && !PredatorInRange() || PredatorLocked() && ProjectileInRange() && !PredatorInRange())
            {
                currentTimer += Time.deltaTime;
                if (currentTimer >= detectionCooldown)
                {
                    currentTimer = 0f;
                    predator = null;
                    isShot = false;
                    isFleeing = false;
                }
            }
        }
    }

    #region For State Actions

    public void DepleteHealth()
    {
        healthSlider.value -= 25;
        health = healthSlider.value;
        health = Mathf.Max(health, 0.0f);

        if (health <= 0)
        {
            Instantiate(meatPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion For State Actions

    #region For Transition Checks
    private bool PredatorInRange()
    {
        return reachedPredator;
    }

    private bool ProjectileInRange()
    {
        return isShot;
    }

    private bool PredatorLocked()
    {
        return isFleeing;
    }

    private bool PredatorAvailable()
    {
        return predator;
    }
    #endregion For Transition Checks

    #region For Visuals

    #endregion For Visuals

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}