using UnityEngine;
using System.Collections;

public class btnCreateHero : MonoBehaviour {

	public void create(){

		Destroy (GameObject.Find ("CanvasMenuPrincipal(Clone)"));

		GameObject createHero = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasCreateHero")); 
		createHero.SetActive (true);
	}
}
