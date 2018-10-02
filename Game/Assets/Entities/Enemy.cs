using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA2;

public class Enemy : Entity, IRemovable<Enemy>
{
    [Header("For Eject")]
    public float feedbackHit;

    bool isDeath;

    Action<Enemy> del_to_remove;

    public LayerMask pared;

    int life; public int Life { get { return life; }
        set
        {
            if (value < 0) { life = 0; isDeath = true; }
            else life = value;
            try { lifebar.UpdateLife(life); }
            catch (System.NullReferenceException ex) { Debug.LogWarning("OJO! lo estas llamando antes de crear el GenericLifeBar"); }
        }
    }
    bool red, green;
    public bool IsRed { get { return red; } }
    public bool IsGreen { get { return green; } }
    Renderer myRender;
    public Renderer Render { get { return myRender; } }
    public Color Color { set { myRender.material.color = value; if (value == Color.red) red = true; if (value == Color.green) green = true; } }

    GenericLifeBar lifebar;
    TriggerFilter<Bullet> bulletTrigger;
    EnemyAnimation anim;

    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////

    void LifeBarFound(UI_Generic_LifeBar uifounded) { lifebar = new GenericLifeBar(uifounded, life, life); }
    void SensorFound(Sensor sensorfounded)
    {
        bulletTrigger = new TriggerFilter<Bullet>(
            sensorfounded, 
            BulletReceived, 
            Layers.PLAYER_BULLET, 
            TriggerFilter<Bullet>.TriggerType._2D);
    }
    void AnimatorFound(Animator _animatorFounded)
    {
        anim = new EnemyAnimation(_animatorFounded);
    }

    void BulletReceived(Bullet b)
    {
        b.Desactivar();
        var damage = b.Damage;
        Life -= damage;
        if (isDeath)
        {
            if (!anim.CanDieMore())
            {
                bulletTrigger.Off();
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public override void Awake()
    {
        myRb = gameObject.GetComponent<Rigidbody2D>();

        myRender = GetComponent<Renderer>();
        GetThePlayer();
        myRender.material.color = red ? Color.red : Color.blue;

        Life = UnityEngine.Random.Range(60, 100);

        gameObject.FindAndLink<UI_Generic_LifeBar>(LifeBarFound);
        gameObject.FindAndLink<Sensor>(SensorFound);
        gameObject.FindAndLink<Animator>(AnimatorFound);
    }

    public override void Init()
    {
        StateMachine();
    }
    public override void Refresh()
    {
        _myFsm.Update();
        if (life <= 0) Dead();
    }
    public override void FixedRefresh()
    {
        _myFsm.FixedUpdate();
    }
    public override void Dead()
    {
        SendInputToFSM(Inputs.DIE);
    }

    public void ConfigureToRemove(Action<Enemy> deltoremove)
    {
        del_to_remove = deltoremove;
    }

    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////

    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////
    public enum Inputs
    {
        ON_LINE_OF_SIGHT,
        PROBOCATED,
        OUT_LINE_OF_SIGHT,
        TIME_OUT,
        IN_RANGE_TO_ATTACK,
        OUT_RANGE_TO_ATTACK,
        FREEZE,
        DIE
    }
    private EventFSM<Inputs> _myFsm;
    void StateMachine()
    {
        #region STATE CONFIGS
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        var idle = new State<Inputs>(CommonState.IDLE);
        var onSigth = new State<Inputs>(CommonState.ONSIGHT);
        var pursuit = new State<Inputs>(CommonState.PURSUIRT);
        var searching = new State<Inputs>(CommonState.SEARCHING);
        var attack = new State<Inputs>(CommonState.ATTACKING);
        var freeze = new State<Inputs>(CommonState.FREEZE);
        var die = new State<Inputs>(CommonState.DIE);

        StateConfigurer.Create(idle)
            .SetTransition(Inputs.ON_LINE_OF_SIGHT, onSigth)
            .SetTransition(Inputs.DIE, die)
            .SetTransition(Inputs.FREEZE, freeze)
            .Done();

        StateConfigurer.Create(onSigth)
            .SetTransition(Inputs.PROBOCATED, pursuit)
            .SetTransition(Inputs.OUT_LINE_OF_SIGHT, searching)
            .SetTransition(Inputs.DIE, die)
            .SetTransition(Inputs.FREEZE, freeze)
            .Done();

        StateConfigurer.Create(pursuit)
            .SetTransition(Inputs.OUT_LINE_OF_SIGHT, searching)
            .SetTransition(Inputs.IN_RANGE_TO_ATTACK, attack)
            .SetTransition(Inputs.DIE, die)
            .SetTransition(Inputs.FREEZE, freeze)
            .Done();

        StateConfigurer.Create(searching)
            .SetTransition(Inputs.TIME_OUT, idle)
            .SetTransition(Inputs.ON_LINE_OF_SIGHT, pursuit)
            .SetTransition(Inputs.DIE, die)
            .SetTransition(Inputs.FREEZE, freeze)
            .Done();

        StateConfigurer.Create(attack)
            .SetTransition(Inputs.OUT_RANGE_TO_ATTACK, pursuit)
            .SetTransition(Inputs.OUT_LINE_OF_SIGHT, searching)
            .SetTransition(Inputs.DIE, die)
            .SetTransition(Inputs.FREEZE, freeze)
            .Done();

        StateConfigurer.Create(freeze)
            .SetTransition(Inputs.DIE, die)
            .Done();

        StateConfigurer.Create(die).Done();


        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        #endregion
        #region STATES
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        //******************
        //*** IDLE
        //******************
        idle.OnEnter += x =>
        {
            myRb.velocity = Vector2.zero;
            anim.Idle();
        };
        idle.OnUpdate += () =>
        {
            Deb_Estado = "IDLE";
            if (LineOfSight())
            {
                Debug.Log("Line of sigth");
                SendInputToFSM(Inputs.ON_LINE_OF_SIGHT);
            }
        };

        //******************
        //*** ON SIGHT
        //******************
        onSigth.OnUpdate += () =>
        {
            Deb_Estado = "ON SIGTH";
            if (LineOfSight()) OnSight_CountDownForProbocate();
            else { timer_to_probocate = 0; SendInputToFSM(Inputs.OUT_LINE_OF_SIGHT); }
        };

        //******************
        //*** PURSUIT
        //******************
        pursuit.OnEnter += x =>
        {
            anim.Run();
        };

        pursuit.OnUpdate += () =>
        {
            Deb_Estado = "PURSUIT";
            if (LineOfSight())
            {
                FollowPlayer();

                if (IsInDistanceToAttack())
                {

                    SendInputToFSM(Inputs.IN_RANGE_TO_ATTACK);
                }
            }
            else { timer_to_probocate = 0; SendInputToFSM(Inputs.OUT_LINE_OF_SIGHT); }
        };

        pursuit.OnExit += x =>
        {
            myRb.velocity = Vector2.zero;
        };

        //******************
        //*** SEARCH
        //******************
        searching.OnEnter += x =>
        {
            anim.Walk();
        };

        searching.OnUpdate += () =>
        {
            Deb_Estado = "SEARCH";
            if (LineOfSight()) SendInputToFSM(Inputs.ON_LINE_OF_SIGHT);
            else
            {
                OutSight_CountDownForIgnore();
            }
        };

        //******************
        //*** ATTACK
        //******************
        attack.OnUpdate += () =>
        {
            Deb_Estado = "ATTACK";
            if (LineOfSight())
            {
                if (IsInDistanceToAttack())
                {
                    anim.Attack();
                    Attack();
                }
                else SendInputToFSM(Inputs.OUT_RANGE_TO_ATTACK);
            }
            else SendInputToFSM(Inputs.OUT_LINE_OF_SIGHT);
        };

        attack.OnExit += x =>
        {
            timer = 0;
        };

        //******************
        //*** FREEZE
        //******************
        freeze.OnEnter += x =>
        {
            Deb_Estado = "FREEZE";
            myRender.material.color = Color.grey;
            canMove = false;
        };

        //******************
        //*** DEATH
        //******************
        die.OnEnter += x =>
        {
            Deb_Estado = "DEATH";
            canMove = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            anim.Die();
            lifebar.Off();
            del_to_remove(this);
        };

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        #endregion

        _myFsm = new EventFSM<Inputs>(idle);
    }
    private void SendInputToFSM(Inputs inp) { _myFsm.SendInput(inp); }
    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////


    [Header("For Probocate")]
    public float Time_to_Probocate = 5f;
    float timer_to_probocate;
    void OnSight_CountDownForProbocate()
    {
        if (timer_to_probocate < Time_to_Probocate) timer_to_probocate = timer_to_probocate + 1 * Time.deltaTime;
        else { timer_to_probocate = 0; SendInputToFSM(Inputs.PROBOCATED); }
    }
    [Header("For Distance To Attack")]
    public float minDisToAttack = 5f;
    bool IsInDistanceToAttack()
    {
        return Vector3.Distance(transform.position, target.transform.position) < minDisToAttack;
    }
    [Header("For Ignore in Time Out")]
    public float TimeToIgnore = 5f;
    float _timer_to_ignore;
    void OutSight_CountDownForIgnore()
    {
        if (_timer_to_ignore < TimeToIgnore) _timer_to_ignore = _timer_to_ignore + 1 * Time.deltaTime;
        else { _timer_to_ignore = 0; SendInputToFSM(Inputs.TIME_OUT); }
    }
    [Header("For Line of Sight")]
    public float viewDistance;
    public float viewAngle;
    GameObject target;

    protected virtual bool LineOfSight()
    {
        //si no me puedo mover... al pedo calcular lo demas
        if (!canMove) return false;

        _distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (_distanceToTarget > viewDistance)
        {
            return false;
        }
        _directionToTarget = (target.transform.position - transform.position).normalized;

        _angleToTarget = Vector2.Angle(transform.up, _directionToTarget);

        if (_angleToTarget <= viewAngle)
        {
            bool obstaclesBetween = false;

            var ray1 = Physics2D.RaycastAll(transform.position, _directionToTarget, _distanceToTarget, pared);
            //var ray = Physics2D.Raycast(transform.position, _directionToTarget, _distanceToTarget);

            foreach (var v in ray1)
            {
                var val = v.collider.gameObject.layer;
                if (v.collider.gameObject.layer == Layers.WORLD)
                {
                    obstaclesBetween = true;
                }
            }
            return !obstaclesBetween ? true : false;
        }
        else return false;
    }
    [Header("For Pursuit")]
    public float rotationSpeed;
    bool canMove = true;
    Vector2 _directionToTarget;
    float _angleToTarget;
    float _distanceToTarget;
    bool _playerInSight;

    protected virtual void FollowPlayer()
    {
        if (!canMove) return;

        Vector2 v2 = new Vector2(
            _directionToTarget.x + transform.position.x * speed,
            _directionToTarget.y + transform.position.y * speed);


        myRb.velocity = new Vector2(_directionToTarget.x, _directionToTarget.y);
        transform.up = Vector2.Lerp(transform.up, _directionToTarget, rotationSpeed * Time.deltaTime);

    }

    [Header("For Attack")]
    public float cooldownAttack;
    float timer;
    public int damage;

    void Attack()
    {
        if(timer == 0) Main.instancia.player.ReceiveDamage(damage);

        if (timer < 1)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
    public void Scare()
    {
        SendInputToFSM(Inputs.FREEZE);
    }
    public void TakeDamage(int damage, Vector3 dir)
    {
        //life -= damage;
        //myRb.AddForce(dir * feedbackHit, ForceMode2D.Impulse);
        //lifebar.UpdateLife(life);
    }
    public void GetThePlayer()
    {
        target = FindObjectOfType<Player>().gameObject;
    }

    public void RemoveMe()
    {

    }

    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////

    [Header("Gizmos & Feedback")]
    public bool DrawGizmos;
    [SerializeField]
    TextMesh debug_estado; public object Deb_Estado { set { debug_estado.text = value.ToString(); } }
    protected virtual void OnDrawGizmos()
    {
        if (!DrawGizmos) return;

        target = FindObjectOfType<Player>().gameObject;

        if (_playerInSight)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * viewDistance));


        Gizmos.color = Color.black;
        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.forward) * transform.up;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.forward) * transform.up;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * viewDistance));

        //claaaro... al angle axis lo que le pasamos en realidad es el angulo, y el eje de rotacion... por eso si es 2D como en este caso
        //no le paso up como siempre, sino que le paso el Z o sea el forward
    }

    
}