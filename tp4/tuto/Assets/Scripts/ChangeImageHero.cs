using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeImageHero : MonoBehaviour {
    private Image levelImage;	
	// Update is called once per frame
	void Update () {
        GameObject imageObject = GameObject.FindGameObjectWithTag("imgHero");
         levelImage = imageObject.GetComponent<Image>();
         levelImage.sprite = Resources.Load("witches-wizards-4611", typeof(Sprite)) as Sprite;
	}
}
