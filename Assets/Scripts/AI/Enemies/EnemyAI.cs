using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehavior> steeringBehaviors;
    [SerializeField]
    private List<Detector> detectors;
    [SerializeField]
    private AIData aiData;
    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;
    [SerializeField]
    private Vector2 movementInput;
    [SerializeField]
    private ContextSolver movementDirectionSolver;
    [SerializeField]
    private float attackDistance = 0.5f;

    private bool following;
    private bool playerDetected;
    private bool attackPlayer;

    [FormerlySerializedAs("OnAttackPressed")] [Header("Events")]
    //Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent<bool> onAttackPressed;
    public UnityEvent<Vector2> onMovementInput;
    public UnityEvent<bool> onPlayerDetected;
    
    private void Start()
    {
        InvokeRepeating(nameof(PerformDetection), 0, detectionDelay);
    }
    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }
    private void Update()
    {
        //Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        onPlayerDetected?.Invoke(following);
        //Moving the Agent
        onMovementInput?.Invoke(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        
        float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

        if (distance < attackDistance)
        {
            //Attack logic
            attackPlayer = true;
            movementInput = Vector2.zero;
            yield return new WaitForSeconds(attackDelay);
            StartCoroutine(ChaseAndAttack());
        }
        else
        {
            //Chase logic
            attackPlayer = false;
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviors, aiData);
            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(ChaseAndAttack());
        }
        onAttackPressed?.Invoke(attackPlayer);
    }
}

