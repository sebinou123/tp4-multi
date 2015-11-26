using UnityEngine;
using System.Collections;

public class RadioButton : MonoBehaviour
{
    static string[] options = new string[] { "Warrior", "Wizard"};
    static Rect position = new Rect(448, 290, 400, 100);
    int selected = 2;

    void OnGUI()
    {
        selected = GUI.SelectionGrid(position, selected, options, options.Length, GUI.skin.toggle);

    }

}
