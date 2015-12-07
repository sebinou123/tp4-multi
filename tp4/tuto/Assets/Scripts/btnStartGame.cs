using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Class associate with the button startgame, when the button is pressed, we check if the player set his name and after, we can begin to instantiate alle the compent of the game
public class btnStartGame : MonoBehaviour {

	public static GameManager game;
	//name of player
	public static string name;


	public void startGame(){
		InputField i = GameObject.Find ("CanvasCreateHero(Clone)").GetComponentInChildren<InputField>();
	
		if (i.text.ToString ().Length != 0) {
			SoundManager.instance.musicSource.clip = (AudioClip)Resources.Load("Audio/GameTheme");
			SoundManager.instance.musicSource.Play();
			name = i.text.ToString ();
			Destroy (GameObject.Find ("CanvasCreateHero(Clone)"));
			Destroy (GameObject.Find ("scriptCreateHero"));
			game = GameObject.Find ("GameManager(Clone)").GetComponent<GameManager> ();
			game.InitGame ();
		}
	}
}
