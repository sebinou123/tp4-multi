using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Class who create a radio button for the GUI
 * */
public class RadioButton : MonoBehaviour
{
    static string[] options = new string[] { "Warrior", "Wizard"};
    public int selected = 2;
	private Image levelImage;	

    void OnGUI()
    {
		RectTransform [] r = GameObject.Find("CanvasCreateHero(Clone)").GetComponents<RectTransform>();
		Rect position = new Rect((r[0].rect.width/2)-100, r[0].rect.height/2, 200, 20);
        selected = GUI.SelectionGrid(position, selected, options, options.Length, GUI.skin.toggle);

    }


	// Update is called once per frame
	void Update () {
		GameObject imageObject = GameObject.FindGameObjectWithTag("img");
		levelImage = imageObject.GetComponent<Image>();

		//if he choose the second button
		if (selected == 1) {
			levelImage.sprite = Resources.Load <Sprite> ("Sprites/wiz");
			//else, he choose the first option
		} else {
			levelImage.sprite = Resources.Load <Sprite> ("Sprites/warrior");
		}
		
		
	}

}
