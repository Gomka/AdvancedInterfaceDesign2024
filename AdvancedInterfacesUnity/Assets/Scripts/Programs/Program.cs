using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Program", menuName = "Scriptable Objects/Program")]
public class Program : ScriptableObject
{
    public string programName = "Program name";
    public Section[] sections;
    public int totalDuration;

    public Program(string programName, Section[] sections, int totalDuration)
    {
        this.programName = programName;
        this.sections = sections;

        totalDuration = 0;

        foreach (Section sec in sections)
        {
            totalDuration += sec.duration;
        }
    }
}