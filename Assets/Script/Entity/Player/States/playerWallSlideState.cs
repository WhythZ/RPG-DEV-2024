using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //ÿ�������������ǽ�ں�����Լ�������ȴ��������г��
        player.CanDashSetting(true);
    }

    public override void Exit()
    {
        base.Exit();

        //ÿ������ǽ�����Ծ����ˢ��Ϊ2����������wallJumpState�����һ����Ծ����ˢ��Ϊ1
        player.jumpNum = 1;
    }

    public override void Update()
    {
        base.Update();

        //�����ǽ�����а�����Ծ���������ǽ��״̬
        //�����¿ո��ʱ��Update�������������ͬʱ��ִ�У����·��������ٶȻᴥ������airState��Flip������
        //�����ᵼ���������죬�ʶ��Ѵ��������Update�������ʼ������֮�󼴽���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.ChangeState(player.wallJumpState);
            //��return���������ֹ�������ִ��
            return;
        }

        //�����ٶ�������ͨ׹���ٶ�
        if (yInput < 0)
        {
            //����ͨ����S�����������ٶ�
            player.SetVelocity(0, rb.velocity.y * player.biggerSlideSpeed);
        }
        else
        {
            //�����ٶ��������Ի����ٶȱ���
            player.SetVelocity(0, rb.velocity.y * player.slideSpeed);
        }

        #region OtherCasesToQuitWallSlideState
        //�����½�ˣ������Idle״̬
        if (player.isGround)
        {
            player.stateMachine.ChangeState(player.idleState);
        }

        //���ǻ�����ǽ�ڷ�Χ�������airState
        if (!player.isWall && !player.isGround)
        {
            player.stateMachine.ChangeState(player.airState);
        }

        /*�ݲ��ṩ������Ծ���ѳ�ǽ������;�뿪��ǽ�ķ����������ṩ��������
        //�����ǽ������xInputΪ�����泯������û�а���Ծ��������ת������airState
        if(xInput != 0 && (player.facingDir != xInput) && !Input.GetKeyDown(KeyCode.Space))
        {
            //���ת��������Ҫ����ΪFlip�����ﲢû�п��ǵ���ǽ�������ֻ������ˮƽ���ٶ�ʱ���ת��
            player.Flip();
            //Debug.Log("WallSlide�е�Flip������");

            player.stateMachine.ChangeState(player.airState);
        }
        */
        #endregion
    }
}
