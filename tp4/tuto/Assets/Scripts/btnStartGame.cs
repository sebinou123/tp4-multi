using UnityEngine;
using System.Collections;

public class btnStartGame : MonoBehaviour {

	public static GameManager game;	

	public void startGame(){
		
		Destroy (GameObject.Find ("CanvasCreateHero(Clone)"));
		Destroy (GameObject.Find ("scriptCreateHero"));
		game = GameObject.Find ("GameManager(Clone)").GetComponent<GameManager>();
		game.InitGame();
	}
}
