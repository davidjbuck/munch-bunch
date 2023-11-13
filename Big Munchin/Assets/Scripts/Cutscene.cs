using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene
{
    public Operation[] csOps;
    public bool[] operationsFinished;
    public int currentOperation;
    public struct Operation
    {
        public int numOpsPerformed;
        public OperationType opType;
        public TerminationType termType;
    }

    public enum OperationType {CutTo = 0, LerpTo = 1, BezierCurve = 2, PanTo = 3};
    public enum TerminationType {Dialogue = 0, Timed = 1, Destination = 2};

    public Cutscene(Operation[] cutsceneOperations) 
    {
        currentOperation = 0;
        csOps = (Operation[])cutsceneOperations.Clone();
        operationsFinished = new bool[csOps.Length];
        for(int i = 0; i < operationsFinished.Length; i++)
        {
            operationsFinished[i] = false;
        }
    }
    //in order to allow for multiple camera operations to happen simultaneously, such as panning, lerping, or bezier curving,
    //TerminateOperations checks how many camera operations should be happening based on the currentOperation,
    //moving on only when each of the operations that should be going on has concluded. If numOpsPerformed is 1, then only
    //the current operation must have concluded, and if it is 2 or more than the CutsceneCamera Director will perform each of those
    //operations in parrallel, calling TerminateOperation when each of those individual operations has concluded.
    public void TerminateOperation(int opNum)
    {
        operationsFinished[opNum] = true;
        if (csOps[currentOperation].numOpsPerformed == 1)
        {
            currentOperation++;
        }
        else
        {
            bool opsDone = true;
            for(int i = currentOperation; i<currentOperation+ csOps[currentOperation].numOpsPerformed; i++)
            {
                if (!operationsFinished[i])
                {
                    opsDone = false;
                }
            }
            if(opsDone)
            {
                currentOperation += csOps[currentOperation].numOpsPerformed;
            }
        }
    }

}
