using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMachine : MonoBehaviour
{
    public Unit receiver;
    Queue<ICommand> commandQueue = new Queue<ICommand>();
    public void Init(Unit myUnit)
    {
        receiver = myUnit;
    }

    private void Update()
    {
        if (receiver != null)
        {
            CommandMachineOn();
        }
    }
    public void AddCommand(ICommand c)
    {
        commandQueue.Enqueue(c);
    }
    public void ClearCommand()
    {
        commandQueue.Clear();
    }




    void CommandMachineOn()
    {
        if (receiver.canReceive())
        {
            if (commandQueue.Count > 0)
            {
                commandQueue.Dequeue().Execute();
            }
        }
        else if (!receiver.canReceive())
        {
            if (commandQueue.Count > 1)
            {
                commandQueue.Dequeue();
            }
        }
    }
}
