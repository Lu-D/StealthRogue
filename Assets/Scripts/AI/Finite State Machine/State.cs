using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State{   
    public abstract void Enter(BEnemy owner);
    public abstract void Execute(BEnemy owner);
    public abstract void Exit(BEnemy owner);
}
