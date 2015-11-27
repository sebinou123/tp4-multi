using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioButton : MonoBehaviour
{
    static string[] options = new string[] { "Warrior", "Wizard"};
    static Rect position = new Rect(448, 290, 200, 20);
    public int selected = 2;
	private Image levelImage;	

    void OnGUI()
    {
        selected = GUI.SelectionGrid(position, selected, options, options.Length, GUI.skin.toggle);

    }


	// Update is called once per frame
	void Update () {
		GameObject imageObject = GameObject.FindGameObjectWithTag("img");
		levelImage = imageObject.GetComponent<Image>();
		
		if (selected == 1) {
			levelImage.sprite = Resources.Load <Sprite> ("Sprites/wiz");
		} else {
			levelImage.sprite = Resources.Load <Sprite> ("Sprites/warrior");
		}
		
		
	}

}
