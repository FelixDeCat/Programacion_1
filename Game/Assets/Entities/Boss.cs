using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA2;

public class Boss : Entity
{

    protected EnemyShooterPreset preset;
    public EnemyShooterPreset Preset { set { preset = value; } }
    bool isDeath;
    public LayerMask pared;
    protected int life;
    public int Life
    {
        get { return life; }
        set
        {
            if (value < 0) { life = 0; isDeath = true; }
            else life = value;
            try { lifebar.UpdateLife(life); }
            catch (System.NullReferenceException ex) { Debug.LogWarning("OJO! lo estas llamando antes de crear el GenericLifeBar"); }
        }
    }
    protected GenericLifeBar lifebar;
    protected TriggerFilter<Bullet> bulletTrigger;
    protected EnemyAnimation anim;
    protected StateGraphicFeedback feedbackstate;

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
    private void FeedbackFound(StateGraphicFeedback obj) { feedbackstate = obj; }

    Vector3 dirbullet;
    bool receiveABullet;
    void BulletReceived(Bullet b)
    {
        b.Desactivar();
        var damage = b.Damage;
        Life -= damage;

        var aux = b.transform.position - transform.position;
        aux = aux.normalized;
        dirbullet = aux ;
        receiveABullet = true;

        if (isDeath)
        {
            if (!anim.CanDieMore())
            {
                bulletTrigger.Enable(false);
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public override void Awake()
    {
        myRb = gameObject.GetComponent<Rigidbody2D>();

        Life = 5000;

        gameObject.FindAndLink<UI_Generic_LifeBar>(LifeBarFound);
        gameObject.FindAndLink<Sensor>(SensorFound);
        gameObject.FindAndLink<Animator>(AnimatorFound);
        gameObject.FindAndLink<StateGraphicFeedback>(FeedbackFound);

        shoot = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, pointToFire, 4f, bulletModel, 10);

        shoot1 = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, pointToFire1, 3f, bulletModel, 5);
        shoot2 = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, pointToFire2, 3f, bulletModel, 5);

        shoot3 = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, pointToFire3, 1f, bulletModelLento, 50);


    }

    public override void Init()
    {
        StateMachine();

        GetThePlayer();
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
        Menu.instancia.Mostrar(true, "Nivel Finalizado");

        SendInputToFSM(Inputs.DIE);
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
        DIE
    }
    private EventFSM<Inputs> _myFsm;
    protected virtual void StateMachine()
    {
        #region STATE CONFIGS
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        var idle = new State<Inputs>(CommonState.IDLE);
        var onSigth = new State<Inputs>(CommonState.ONSIGHT);
        var pursuit = new State<Inputs>(CommonState.PURSUIRT);
        var searching = new State<Inputs>(CommonState.SEARCHING);
        var attack = new State<Inputs>(CommonState.ATTACKING);
        var die = new State<Inputs>(CommonState.DIE);

        StateConfigurer.Create(idle)
            .SetTransition(Inputs.ON_LINE_OF_SIGHT, onSigth)
            .SetTransition(Inputs.DIE, die)
            .Done();

        StateConfigurer.Create(onSigth)
            .SetTransition(Inputs.PROBOCATED, pursuit)
            .SetTransition(Inputs.OUT_LINE_OF_SIGHT, searching)
            .SetTransition(Inputs.DIE, die)
            .Done();

        StateConfigurer.Create(pursuit)
            .SetTransition(Inputs.OUT_LINE_OF_SIGHT, searching)
            .SetTransition(Inputs.IN_RANGE_TO_ATTACK, attack)
            .SetTransition(Inputs.DIE, die)
            .Done();

        StateConfigurer.Create(searching)
            .SetTransition(Inputs.TIME_OUT, idle)
            .SetTransition(Inputs.ON_LINE_OF_SIGHT, pursuit)
            .SetTransition(Inputs.DIE, die)
            .Done();

        StateConfigurer.Create(attack)
            .SetTransition(Inputs.OUT_RANGE_TO_ATTACK, pursuit)
            .SetTransition(Inputs.OUT_LINE_OF_SIGHT, searching)
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

            CheckBullet();
        };

        //******************
        //*** ON SIGHT
        //******************
        onSigth.OnEnter += x =>
        {
            //feedbackstate.SetStateGraphic(preset.sprite_OnSight);
        };
        onSigth.OnUpdate += () =>
        {
            Deb_Estado = "ON SIGTH";
            if (LineOfSight()) {
                LookSmooth();
                OnSight_CountDownForProbocate();
            }
            else { timer_to_probocate = 0; SendInputToFSM(Inputs.OUT_LINE_OF_SIGHT); }
        };
        onSigth.OnExit += x =>
        {
           // feedbackstate.SetStateGraphic();
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
            //feedbackstate.SetStateGraphic(preset.sprite_Search);
        };

        searching.OnUpdate += () =>
        {
            CheckBullet();

            Deb_Estado = "SEARCH";
            if (LineOfSight()) SendInputToFSM(Inputs.ON_LINE_OF_SIGHT);
            else
            {
                transform.Rotate(0,0,2);
                OutSight_CountDownForIgnore();
            }
        };

        searching.OnExit += x =>
        {
           // feedbackstate.SetStateGraphic();
        };

        //******************
        //*** ATTACK
        //******************
        attack.OnUpdate += () =>
        {
            Deb_Estado = "ATTACK";
            if (LineOfSight())
            {
                LookSmooth();

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
        //*** DEATH
        //******************
        die.OnEnter += x =>
        {
            Deb_Estado = "DEATH";
            canMove = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            anim.Die();
            lifebar.Off();
        };

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        #endregion

        _myFsm = new EventFSM<Inputs>(idle);
    }
    protected void SendInputToFSM(Inputs inp) { _myFsm.SendInput(inp); }
    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////


    [Header("For Probocate")]
    public float Time_to_Probocate = 5f;
    protected float timer_to_probocate;
    protected void OnSight_CountDownForProbocate()
    {
        if (timer_to_probocate < Time_to_Probocate) timer_to_probocate = timer_to_probocate + 1 * Time.deltaTime;
        else { timer_to_probocate = 0; SendInputToFSM(Inputs.PROBOCATED); }
    }
    [Header("For Distance To Attack")]
    public float minDisToAttack = 5f;
    protected bool IsInDistanceToAttack()
    {
        return Vector3.Distance(transform.position, target.transform.position) < minDisToAttack;
    }
    [Header("For Ignore in Time Out")]
    public float TimeToIgnore = 5f;
    protected float _timer_to_ignore;
    protected void OutSight_CountDownForIgnore()
    {
        if (_timer_to_ignore < TimeToIgnore) _timer_to_ignore = _timer_to_ignore + 1 * Time.deltaTime;
        else { _timer_to_ignore = 0; SendInputToFSM(Inputs.TIME_OUT); }
    }
    [Header("For Line of Sight")]
    public float viewDistance;
    public float viewAngle;
    protected GameObject target;

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
    protected bool canMove = true;
    protected Vector2 _directionToTarget;
    protected float _angleToTarget;
    protected float _distanceToTarget;
    protected bool _playerInSight;

    public void CheckBullet()
    {
        if (receiveABullet)
        {
            transform.up = dirbullet;
        }
    }

    protected virtual void FollowPlayer()
    {
        if (!canMove) return;

        myRb.velocity = new Vector2(_directionToTarget.x * speed, _directionToTarget.y * speed);

        LookSmooth();
    }

    protected virtual void LookSmooth()
    {
        transform.up = Vector2.Lerp(transform.up, _directionToTarget, rotationSpeed * Time.deltaTime);
    }

    [Header("For Attack")]
    public float cooldownAttack;
    protected float timer;
    public int damage;
    public Shoot<Bullet> shoot;
    public Shoot<Bullet> shoot1;
    public Shoot<Bullet> shoot2;
    public Shoot<Bullet> shoot3;

    public Transform pointToFire;
    public Transform pointToFire1;
    public Transform pointToFire2;
    public Transform pointToFire3;
    public GameObject bulletModel;
    public GameObject bulletModelLento;
    public int bulletDamage;

    protected void Attack()
    {
        if (timer == 0) Shoot();
        if (timer < cooldownAttack) timer = timer + 1 * Time.deltaTime;
        else timer = 0;
    }

    //shoot super hardcodeado :)

    public virtual void Shoot() {
        if (life > 4000) shoot.Shot();
        else {
            if (life > 3000) {
                shoot.Shot();
                shoot1.Shot();
            }
            else {
                if (life > 2000) {
                    shoot.Shot();
                    shoot1.Shot();
                    shoot2.Shot();
                }
                else {
                    shoot.Shot();
                    shoot1.Shot();
                    shoot2.Shot();
                    shoot3.Shot();
                }
            }
        }
    }

    public virtual void GetThePlayer()
    {
        target = Main.instancia.player.gameObject;
    }

    //////////////////////////////////////////////////////
    //////////////////////////////////////////////////////

    [Header("Gizmos & Feedback")]
    public bool DrawAndDebug;
    [SerializeField]
    TextMesh debug_estado; public object Deb_Estado { set { debug_estado.text = DrawAndDebug ? value.ToString() : ""; } }

   
    protected virtual void OnDrawGizmos()
    {
        if (!DrawAndDebug) return;

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