using UnityEngine;
using System.Collections;


	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
/**
 * Class who instantiate all component of the game and do the move for the ennemies, check when the player died
 * */
	public class GameManager : MonoBehaviour
	{
        private const float aggroRange = 10f;
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		public Sprite[] items = new Sprite[4];					//Sprite of all image weapon

		public SmoothCamera2D camera;							//Instance of the camera
		public bool firtTimeLoad = true;						//Indicate if its the first time loading the scene
		public GameObject player;								//Instance of player
		public Text levelText;									//Text to display current level number.
        public GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		public GameObject menuprincipal;						//Canva menuPrincipal
		public GameObject infoplayer;							//Canva about the info of the player (level, weapon, etc)
		public GameObject createplayer;							//Canva of creation for the player
		public Text TextName;									//Text in the canva infoplayer
		public Text TextHp;										//Text in the canva infoplayer
		public Text TextStats;									//Text in the canva infoplayer
		public Text TextLevel;									//Text in the canva infoplayer
		public Text TextWeapon;									//Text in the canva infoplayer
        public GameObject[,] weaponRange;						//Image who show the range of the current weapon
        public Image ImageWeapon;								//Image of the current weapon
        private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		public int level = 1;									//Level compteur	
		public List<Enemy> enemies;								//List of all Enemy units, used to issue them move commands.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool playerInstanciate = true;	
		
		
		
		//Awake is always called before any Start functions
		public void Awake()
		{
			//Check if instance already exists
			if (instance == null)
				
				//if not, set instance to this
				instance = this;
			
			//If instance already exists and it's not this:
			else if (instance != this)
				
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
			
			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
			
			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();
	
			//Call the InitGame function to initialize the first level 
		    if (firtTimeLoad == false) {
			    InitGame();
		    }

			//Call the canva menuPrincipale for the first time loading the scene
		    if (firtTimeLoad == true) {
			    menuprincipal = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasMenuPrincipal")); 
			    menuprincipal.SetActive (true);
			    firtTimeLoad = false;
		    }
	    }
		//This is called each time a scene is loaded.
		void OnLevelWasLoaded(int index)
		{
			//Add one to our level number.
			level++;
			//Call InitGame to initialize our level.
			InitGame();
		}
		
		//Initializes the game for each level.
		public void InitGame()
		{
			//get level of the player if player exist
            level = player != null ? player.GetComponent<Player>().getLevel() : 1;

			//Get a reference to our image LevelImage by finding it by name.
			levelImage = (GameObject)Instantiate(Resources.Load("Prefabs/Canvas"));
			levelImage.name = "Canvas";
			infoplayer = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasInfoPlayer")); 

			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
            Invoke("HideLevelImage", 0.1f);
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);
			infoplayer.SetActive (true);


			//get all info in the canva infoplayer
			TextName = GameObject.Find("TextName").GetComponent<Text>();
			TextHp = GameObject.Find("TextHp").GetComponent<Text>();
			TextStats = GameObject.Find("TextStats").GetComponent<Text>();
			TextLevel = GameObject.Find("TextLevel").GetComponent<Text>();
			TextWeapon = GameObject.Find("TextWeapon").GetComponent<Text>();
			ImageWeapon = GameObject.Find("ImageWeapon").GetComponent<Image>();

			//make the array of the range of the current weapon
            weaponRange = new GameObject[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    weaponRange[i, j] = GameObject.Find("Range" + ((i * 5) + j));
                }
            }

			//if first time load, instantiate the player
		    if (playerInstanciate == true) {
			    player = (GameObject)Instantiate (Resources.Load ("Prefabs/Warrior"));
			    player.name = "Player";
			    playerInstanciate = false;
		    }

			//put the player in the beginning position
			GameObject.Find ("Player").GetComponent<Player> ().enabled = true;
			GameObject.Find ("Player").GetComponent<Player>().transform.position = new Vector3(0,0,0);

			camera = GameObject.Find ("Main Camera").GetComponent<SmoothCamera2D>();
			camera.target = player.transform;
			
			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
			
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			boardScript.SetupScene(level);
		}

		//hide the blackscreen when the loading is finish
        public void HideLevelImage()
        {
            GameObject.Find("LevelImage").SetActive(false);
        }
		
		//Update is called every frame.
		void Update()
		{
			//Start moving enemies.
            if(!enemiesMoving)
			    StartCoroutine (MoveEnemies ());
		}
		
		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}
		
		
		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//Disable this GameManager.
			enabled = false;

			GameObject canvaGameOver = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasGameOver")); 
			canvaGameOver.SetActive (true);
		}
		
		//Coroutine to move enemies in sequence.
		IEnumerator MoveEnemies()
		{
			enemiesMoving = true;
			yield return new WaitForSeconds(0.1f);
			
			//Loop through List of Enemy objects.
			for (int i = 0; i < enemies.Count; i++)
			{
                if(enemies[i] != null && enemies[i].isActiveAndEnabled && Vector2.Distance(enemies[i].transform.position, player.transform.position) < aggroRange){
				    //Call the MoveEnemy function of Enemy at index i in the enemies List.
				    enemies[i].MoveEnemy ();
                }
			}
			
			//Enemies are done moving, set enemiesMoving to false.
			enemiesMoving = false;
		}
	}

