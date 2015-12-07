using UnityEngine;
using System.Collections;

//Class associate with the create hero button, when pressed, we instantiate the canva for the creation of hero
public class btnCreateHero : MonoBehaviour {

	public void create(){

		Destroy (GameObject.Find ("CanvasMenuPrincipal(Clone)"));

		GameObject createHero = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasCreateHero")); 
		createHero.SetActive (true);
	}
}
