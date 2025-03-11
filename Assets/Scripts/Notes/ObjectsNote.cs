using UnityEngine;

public class ObjectsNote : NoteController
{
    protected override void ShowNote()
    {
        noteCodeAreaUI.enabled = true;
        noteCodeAreaUI.text = " ";
        room.GetComponent<ObjectsRoom>().GetTranslatedObjectSequence().ForEach(x => noteCodeAreaUI.text += x + ' ');
        base.ShowNote();
    }     
}
