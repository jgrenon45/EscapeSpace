using UnityEngine;

public class CodeNode : NoteController
{
    protected override void ShowNote()
    {
        noteCodeAreaUI.enabled = true;
        noteCodeAreaUI.text = "";
        noteCodeAreaUI.text = room.GetComponent<CodeRoom>().GetCodeToString();
        base.ShowNote();
    }        
}
