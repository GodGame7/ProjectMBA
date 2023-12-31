// LocomotionSimpleAgent.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    float motionSmoothTime = .1f;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // Don��t update position automatically
        agent.updatePosition = false;
        anim.applyRootMotion = false;
    }

    void Update()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("vel", speed, motionSmoothTime, Time.deltaTime);

        //Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        //// Map 'worldDeltaPosition' to local space
        //float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        //float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        //Vector2 deltaPosition = new Vector2(dx, dy);

        //// Low-pass filter the deltaMove
        //float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        //smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        //// Update velocity if time advances
        //if (Time.deltaTime > 1e-5f)
        //    velocity = smoothDeltaPosition / Time.deltaTime;

        //bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        //// Update animation parameters
        //anim.SetBool("move", shouldMove);
        //anim.SetFloat("vel", velocity.magnitude);
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
}