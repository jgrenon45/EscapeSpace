using System.Linq;
using UnityEngine;

public class BottleNote : NoteController
{
    protected override void ShowNote()
    {
        BottleRoom br = (BottleRoom)room;
        if (br)
        {
            noteText += "\n\n";
            for (int i = 0; i < br.GetBottleOrderString().Count; i++)
            {
                noteText += br.GetBottleOrderString()[i]+ " ";
            }
        }
        noteCodeAreaUI.enabled = false;
        base.ShowNote();
    }
}
