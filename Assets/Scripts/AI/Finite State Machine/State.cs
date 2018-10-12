using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State{   
    public abstract void Enter(EnemyControl owner);
    public abstract void Execute(EnemyControl owner);
    public abstract void Exit(EnemyControl owner);
}
