using System.Linq;
using UnityEngine;

public class BottleNote : NoteController
{
    private bool orderShown = false;

    protected override void ShowNote()
    {
        BottleRoom br = (BottleRoom)room;
        if (br && !orderShown)
        {
            noteText += "\n\n";
            for (int i = 0; i < br.GetBottleOrderString().Count; i++)
            {
                noteText += br.GetBottleOrderString()[i]+ " ";
            }
            //Prevent the order from being added again
            orderShown = true;
        }
        noteCodeAreaUI.enabled = false;
        base.ShowNote();
    }

    protected override void OnCodeGenerated()
    {
        //Do nothing
    }
}
