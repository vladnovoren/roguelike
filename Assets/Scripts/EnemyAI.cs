using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AI;
using AI.Base;
using AI.Chasing;
using AI.Roaming;
using Utils.Time;

public class EnemyAI : MonoBehaviour
{
    public void Awake()
    {
        Init();
        _stateMachine = BuildStateMachine();
    }

    private void Start()
    {
        _stateMachine.OnEnter();
    }

    private void Update()
    {
        _stateMachine.Execute();
    }

    private StateMachine BuildStateMachine()
    {
        var roamFragment = BuildRoamFragment();
        var chaseFragment = BuildChaseFragment();

        var roamToChaseDecision = new RoamToChaseDecision(gameObject, enemy);
        var roamToChaseTransition = new Transition(roamToChaseDecision,
                                            chaseFragment.Entry);
        roamFragment.AddTransitionToAllStates(roamToChaseTransition);

        var attackFragment = BuildAttackFragment();

        var chaseToAttackDecision = new ChaseToCatchDecision(gameObject, enemy);
        var chaseToAttackTransition = new Transition(chaseToAttackDecision,
                                                    attackFragment.Entry);

        var attackToChaseDecision = new OppositeDecision(
                                        new ChaseToCatchDecision(gameObject,
                                                                    enemy));
        var attackToChaseTransition = new Transition(attackToChaseDecision,
                                                    chaseFragment.Entry);

        chaseFragment.AddTransitionToAllStates(chaseToAttackTransition);
        attackFragment.AddTransitionToAllStates(attackToChaseTransition);

        return new StateMachine(roamFragment.Entry);
    }

    private void Init() {
        var fov = gameObject.GetComponent<FieldOfView>();
        fov.Radius = 6.0f;

        var catchComp = gameObject.GetComponent<Catch>();
        catchComp.Radius = 2.0f;

        var sword = gameObject.GetComponent<Sword>();
        sword.Damage = 50f;
    }

    private StateMachineFragment BuildRoamFragment()
    {
        var stayState = new State();
        var followState = new State();

        var timer = new CountdownTimer();
        var stayAction = new StayAction(gameObject, timer, 1.0f, 2.0f);

        var stayToFollowDecision = new StayToFollowDecision(timer);
        var stayToFollowTransition = new Transition(stayToFollowDecision,
                                            followState);

        var followAction = new FollowAction(gameObject, 1.0f, 2.0f);

        var followToStayDecision = new FollowToStayDecision(gameObject, 0.01f);
        var followToStayTransition = new Transition(followToStayDecision,
                                            stayState);

        stayState.AddAction(stayAction);
        stayState.AddTransition(stayToFollowTransition);

        followState.AddAction(followAction);
        followState.AddTransition(followToStayTransition);

        _roamingFragment = new StateMachineFragment(stayState);
        _roamingFragment.AddState(followState);

        return _roamingFragment;
    }

    private StateMachineFragment BuildChaseFragment()
    {
        var chaseAction = new ChaseAction(gameObject, enemy, 0.01f);
        var chaseState = new State();
        chaseState.AddAction(chaseAction);

        return new StateMachineFragment(chaseState);
    }

    private StateMachineFragment BuildAttackFragment()
    {
        var fighter = new AI.Swordsman.Fighter(gameObject, enemy, 0.5f);

        var attackState = new State();
        attackState.AddAction(new AI.Swordsman.AttackAction(fighter));

        return new StateMachineFragment(attackState);
    }

    [SerializeField] private GameObject enemy;

    private StateMachine _stateMachine;
    private StateMachineFragment _roamingFragment;
}
