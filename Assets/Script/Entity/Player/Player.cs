using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Player : Entity
{
    #region States
    public PlayerStateMachine stateMachine {  get; private set; }
    //��������ܵ���������״̬������
    public PlayerIdleState idleState { get; private set; }
    //���������վ��״̬
    public PlayerMoveState moveState { get; private set; }
    //����������ƶ�״̬
    public PlayerJumpState jumpState { get; private set; }
    //�����������Ծ״̬
    public PlayerAirState airState { get; private set; }
    //���������׹��״̬
    public PlayerDashState dashState { get; private set; }
    //��������ĳ��״̬
    public PlayerWallSlideState wallSlideState { get; private set; }
    //��������Ļ�ǽ״̬
    public PlayerWallJumpState wallJumpState { get; private set; }
    //���������ǽ��״̬
    public PlayerPrimaryAttack primaryAttackState { get; private set; }
    //��������Ĺ���״̬
    public PlayerCounterAttackState counterAttackState { get; private set; }
    //��������ĵ���״̬�����״̬������������ص�parameters
    public PlayerAimSwordState aimSwordState { get; private set; }
    //�����������׼״̬
    public PlayerThrowSwordState throwSwordState { get; private set;}
    //���������Ͷ��״̬
    public PlayerDeadState deadState { get; private set; }
    //��������״̬
    public PlayerBlackholeState blackholeState { get; private set; }
    //����ڶ�״̬
    #endregion

    #region Components
    //��¼���������ͳ�ƽű�
    public PlayerStats sts {  get; private set; }
    public UI_MainScene ui { get; private set; }
    public PlayerManager manager { get; private set; }
    public PlayerSkillManager skill { get; private set; }
    #endregion

    #region Default
    //˽�еģ����ڴ洢ԭ�еķ��Ǻ�λ�ƣ�moveSpeed�ڻ����������ˣ���ص�ԭ���ٶ�
    private float defaultJumpForce;
    private float defaultDashSpeed;
    #endregion

    #region Movement
    [Header("Player Movement Info")]
    //�����ڿ��е��ƶ��ٶ���moveSpeed��С��һ��
    public float airMoveSpeedRate = 0.9f;
    #endregion

    #region Jump
    [Header("Jump Info")]
    //��ʼ��Ծ��
    public float jumpForce = 15;
    //����ʣ��Ŀ���Ծ���������������Զ�����
    public int jumpNum = 2;
    #endregion

    #region Dash
    [Header("Dash Info")]
    //���ʱ��Ĭ��0.2�룬���ƶ��ٶȳ���dashSpeed���ʵĳ���ʱ��
    public float dashDuration = 0.2f;
    //����ٶ�Ҫ��moveSpeed�󣬲�Ȼ���г���ˣ�Ĭ��Ϊ26
    public float dashSpeed = 26;
    //�ڿ��г�̹�һ�κ󣬼�ʹ��ȴʱ�䵽��Ҳ���ܵڶ��γ��
    public bool canDash {  get; private set; } = true;

    //����PlayerSkillManager�������Skill�����DashSkill������������ؿɵ������ݣ����������б���
    //��ȴʱ�䳤��
    //public float dashCooldown = 0.6f;
    //��ȴʱ���ʱ��
    //private float dashCooldownTimer;
    #endregion

    #region AttackDetails
    [Header("Attack Details")]
    //�ܶ����빥��״̬ʱά��ԭ���ٶȣ����ԸУ���ʱ��
    public float runIntoAttackInertiaDuration = 0.1f;
    //���治ͬ�ι����Ĳ�ͬλ�Ƶ���������
    public Vector2[] attackMovement;

    //���������ܴ�������Чʱ��
    public float counterAttackDuration = 0.3f;
    #endregion

    #region WallSlide
    [Header("WallSlide Info")]
    //��ǽ���ٶȱ���
    public float slideSpeed = 0.9f;
    //��ǽ�ļ��������ٶȱ��ʣ�������ͨ��ǽ�ٶȱ��ʣ�
    public float biggerSlideSpeed = 0.99f;
    //ǽ������ˮƽ�ٶ�ʩ��ʱ��
    public float wallJumpReverseSpeedDuration = 0.1f;
    #endregion

    #region CDPlayer
    [Header("CDPlayer")]
    //�������������õ�������Ԥ����
    public GameObject cdPlayerPrefab;
    //���ڴ洢���ɺ���������������ڷ�ֹ���ɶ��������Ԥ����
    public GameObject assignedCDPlayer {  get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        #region States
        //�½�һ��״̬��
        stateMachine = new PlayerStateMachine();
        //����Player����ʹ�õ�״̬���������������Unity��Animator����Ӧ�ж�Parameter����"Idle"��������PlayerState��"idleState"�Ĺ���
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        //ȷ��PlayerState��������Ӱ���player������ű����Player�����������״̬�Ŀ�����Animator��Move������
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        //��ʼ����Ծ״̬
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        //ע������󶨵�Ҳ��Jump��������Ϊûʲô����
        airState = new PlayerAirState(this, stateMachine, "Jump");
        //��ʼ�����״̬
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        //��ʼ����ǽ״̬
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        //��ʼ��ǽ��״̬
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
        //��ʼ����һ�ι���
        primaryAttackState = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        //��ʼ����������
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        //��ʼ����׼����
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        //��ʼ��Ͷ��״̬
        throwSwordState = new PlayerThrowSwordState(this, stateMachine, "ThrowSword");
        //��ʼ������״̬
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
        //�ͷźڶ���ԭ�����
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        #endregion
    }

    protected override void Start()
    {
        base.Start();

        #region Components
        //����ͳ�ƽű�
        sts = GetComponent<PlayerStats>();
        ui = UI_MainScene.instance;
        manager = PlayerManager.instance;
        skill = PlayerSkillManager.instance;
        #endregion

        #region Default
        //�����ʼ�ٶ�ֵ
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
        #endregion

        //��վ��״̬��ʼ����ҵ�״̬��
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        #region GamePause
        //��Ϸ������ͣ״̬��ʱ�򣬲�ִ��������κ����
        if (Time.timeScale == 0)
            return;
        #endregion

        base.Update();

        //�˴���ͨ��MonoBehavior��Update���������ϵ���PlayerState���е�Update����������ˢ������״̬
        stateMachine.currentState.Update();
        //��������ĳ��״̬
        DashController();
        //����������������ٻ�
        CDPlayerController();
    }

    #region Skill
    private void DashController()
    {
        //��������
        if (manager.ability_CanDash == false)
            return;

        //���ܴӹ�������׼��Ͷ��״̬���
        if (stateMachine.currentState != primaryAttackState && stateMachine.currentState != aimSwordState && stateMachine.currentState != throwSwordState)
        {
            //��̿��Դ����������״̬���뿪ʼ���ʶ����ڴ˴�Update�︳��������ȼ���ֻҪ������shift������ȴʱ��������������״̬��
            //ע������ʹ����PlayerSkillManager
            if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dashSkill.CanUseSkill() && canDash)
            {
                stateMachine.ChangeState(dashState);
                //�ָ���ȴ
                skill.dashSkill.RefreshCooldown();
            }
        }
    }
    public void ExitBlackholeAbility()
    {
        //ֹͣ�ͷźڶ�����
        stateMachine.ChangeState(airState);
    }
    #endregion

    #region SlowEntityOverride
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        //���Եļ��٣�д��ǰ�棬��ΪInvoke�ָ��ٶȵĺ����ڵ��µ�base����
        jumpForce *= (1 - _slowPercentage);
        dashSpeed *= (1 - _slowPercentage);

        base.SlowEntityBy(_slowPercentage, _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        //�ָ��ٶ�
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    #endregion

    #region FlipControllerOverride
    public override void FlipController()
    {
        if (isKnocked)
            return;
        else
        {
            //����ģ���Щ״ֱ̬�Ӹ������е����ҵ�����tm�����������⣡
            if(stateMachine.currentState != wallSlideState && stateMachine.currentState != aimSwordState && stateMachine.currentState != primaryAttackState && stateMachine.currentState != throwSwordState)
            {
                //��������һ��xInput���ƣ���ֹĪ�����ٶ�����������ת������
                if ((stateMachine.currentState.xInput > 0) && (rb.velocity.x > 0) && !facingRight)
                {
                    Flip();
                }
                if ((stateMachine.currentState.xInput < 0) && (rb.velocity.x < 0) && facingRight)
                {
                    Flip();
                }
            }
        }
    }
    #endregion

    #region CDPlayer
    private void CDPlayerController()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //�������ٻ�����֮ǰ���ɹ�����������������ڵ�������������ifһ��Ҫ����һ��if��ǰ�棬��Ȼ�����ɵ�prefab�����̱������
            if (assignedCDPlayer != null)
            {
                //��Ч
                AudioManager.instance.PlaySFX(7, null);

                //��ֹ���ɶ��������
                Destroy(assignedCDPlayer);
            }
            //�������ٻ�����֮ǰû���ɹ����������򴴽�һ���µ�
            if(assignedCDPlayer == null)
            {
                //��Ч
                AudioManager.instance.PlaySFX(7, null);

                //��ʼ��������һ��������
                GameObject _newCDPlayer = Instantiate(cdPlayerPrefab, transform.position, transform.rotation);

                //��¼һ�£�������һ���µ�����������ֹ�����ٻ�
                assignedCDPlayer = _newCDPlayer;
            }
        }
    }
    #endregion

    #region DieOverride
    protected override void DieDetect()
    {
        //������Ѫ��С�ڵ����㣬����PlayerStats������������
        if (sts.currentHealth <= 0)
        {
            //��������״̬
            stateMachine.ChangeState(deadState);
        }
    }
    #endregion

    #region Accessibility
    public void CanDashSetting(bool _bool)
    {
        //�Ҳ��������������Unity�п��Բ��ݣ�������Ҫ��������������ű��б����ݸ�ֵ�����õ���һ����������
        canDash = _bool;
    }
    #endregion

    #region AttackAnimationRelatedScripts
    //���˺���������ʱ������������������ʱ�򣩣����ص��õ�ǰ״̬��TriggerWhenAnimationFinished()�����Ľ���������ȼ����������
    //public void AnimationTrigger(){stateMachine.currentState.TriggerWhenAnimationFinished();}
    public void AnimationTrigger() => stateMachine.currentState.TriggerWhenAnimationFinished();
    #endregion
}
