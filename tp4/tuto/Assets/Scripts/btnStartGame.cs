using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class btnStartGame : MonoBehaviour {

	public static GameManager game;
	public static string name;

	public void startGame(){
		InputField i = GameObject.Find ("CanvasCreateHero(Clone)").GetComponentInChildren<InputField>();
	
		if (i.text.ToString ().Length != 0) {
			name = i.text.ToString ();
			Destroy (GameObject.Find ("CanvasCreateHero(Clone)"));
			Destroy (GameObject.Find ("scriptCreateHero"));
			game = GameObject.Find ("GameManager(Clone)").GetComponent<GameManager> ();
			game.InitGame ();
		}
	}
}
