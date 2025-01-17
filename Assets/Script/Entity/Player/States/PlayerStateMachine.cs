using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
//这个类用于以一定的逻辑操控一个人物的所有动作（PlayerState）之间的相互转化；此处不需要继承MonoBehaviour
{
    //存储这个状态机当下展示的动作状态是什么；{ get; private set; }表示这个变量对外部是只可读，不可更改的
    public PlayerState currentState { get; private set; }

    //存储状态机上一个状态是什么
    public PlayerState formerState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        //设定这个状态机的初始状态，并进入该状态
        this.currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        //退出上一个状态（即把其关联的参数设置为false）
        currentState.Exit();
        //在转换状态之前，记录下是从什么状态转换到了下一个状态
        formerState = currentState;
        //设置当前状态为输入的状态，然后进入该状态（即把其关联的参数设置为true）
        currentState = _newState;
        currentState.Enter();
    }
}
