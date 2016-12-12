using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CommandData : ScriptableObject  {
    public string commandName = "New Command"; 
    public Color thisColor = Color.white;

    [Space(10)]
    [TextArea]
    public string code = "";
}

//public enum CommandInputType { Integer, Dropdown }
//public enum CommandType { moveForward, rotateLeft, rotateRight }
