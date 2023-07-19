using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMachine : MonoBehaviour
{
    public Unit receiver;
    Queue<ICommand> commandQueue = new Queue<ICommand>();

    private void Start()
    {
        receiver = GetComponent<Unit>();
    }

    private void Update()
    {
        if (receiver != null)
        {
            CommandMachineOn();
        }
        else
        {
            if (commandQueue.Count > 0) commandQueue.Clear();
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
            if (receiver.cur_state == receiver.state_skill)
            {
                if (commandQueue.Count > 0)
                {
                    if (commandQueue.Peek() is StopCommand)
                    {
                        (commandQueue.Dequeue() as StopCommand)?.Execute();
                    }
                    else
                    {
                        if (commandQueue.Count > 1) commandQueue.Dequeue();
                    }
                }
            }
            else
            {
                if (commandQueue.Count > 0)
                {
                    commandQueue.Dequeue().Execute();
                }
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
