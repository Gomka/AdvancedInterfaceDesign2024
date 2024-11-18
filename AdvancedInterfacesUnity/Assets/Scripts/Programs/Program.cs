using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Program", menuName = "Scriptable Objects/Program")]
public class Program : ScriptableObject
{
    public string programName = "Program name";
    public Section[] classSections;
    public int totalDuration;

    public Program(string programName, Section[] classSections, int totalDuration)
    {
        this.programName = programName;
        this.classSections = classSections;

        totalDuration = 0;

        foreach (Section sec in classSections)
        {
            totalDuration += sec.duration;
        }
    }
}