using UnityEngine;
using System.Collections;


	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
	public class GameManager : MonoBehaviour
	{
        private const float aggroRange = 10f;
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.5f;							//Delay between each Player turn.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		public Sprite[] items = new Sprite[10];

		public SmoothCamera2D camera;
		public bool firtTimeLoad = true;
	    public GameObject player;
        public Text levelText;									//Text to display current level number.
        public GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		public GameObject levelBackGround;
		public GameObject menuprincipal;	
		public GameObject infoplayer;	
		public GameObject createplayer;
        public Text TextName;
        public Text TextStats;
        public Text TextArmor;
        public Text TextLevel;
        public Text TextWeapon;
        public GameObject[,] weaponRange;
        public Image ImageWeapon;
        private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		public int level = 1;									//Current level number, expressed in game as "Day 1".
		public List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
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
            level = player != null ? player.GetComponent<Player>().getLevel() : 1;
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
			
			//Get a reference to our image LevelImage by finding it by name.
			levelImage = (GameObject)Instantiate(Resources.Load("Prefabs/Canvas"));
			levelImage.name = "Canvas";
			infoplayer = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasInfoPlayer")); 

			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();


            //Set the text of levelText to the string "Day" and append the current level number.
            levelText.text = "Day " + level;
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);
			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);
			infoplayer.SetActive (true);


			TextName = GameObject.Find("TextName").GetComponent<Text>();
			TextStats = GameObject.Find("TextStats").GetComponent<Text>();
			TextArmor = GameObject.Find("TextArmor").GetComponent<Text>();
			TextLevel = GameObject.Find("TextLevel").GetComponent<Text>();
			TextWeapon = GameObject.Find("TextWeapon").GetComponent<Text>();
			ImageWeapon = GameObject.Find("ImageWeapon").GetComponent<Image>();

            weaponRange = new GameObject[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    weaponRange[i, j] = GameObject.Find("Range" + ((i * 5) + j));
                }
            }

		    if (playerInstanciate == true) {
			    player = (GameObject)Instantiate (Resources.Load ("Prefabs/Player"));
			    player.name = "Player";
			    playerInstanciate = false;
		    }

			GameObject.Find ("Player").GetComponent<Player> ().enabled = true;
			GameObject.Find ("Player").GetComponent<Player>().transform.position = new Vector3(0,0,0);

			camera = GameObject.Find ("Main Camera").GetComponent<SmoothCamera2D>();
			camera.target = player.transform;
			
			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
			
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			boardScript.SetupScene(level);
		}
		
		
		//Hides black image used between levels
		void HideLevelImage()
		{
			levelBackGround = GameObject.Find ("LevelImage");
			//Disable the levelImage gameObject.
			levelBackGround.SetActive (false);
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}
		
		//Update is called every frame.
		void Update()
		{
			//Start moving enemies.
            if(!enemiesMoving && !doingSetup)
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
			//Set levelText to display number of levels passed and game over message
			levelText.text = "After " + level + " days, you starved.";
			
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			
			//Disable this GameManager.
			enabled = false;
		}
		
		//Coroutine to move enemies in sequence.
		IEnumerator MoveEnemies()
		{
			//While enemiesMoving is true player is unable to move.
			enemiesMoving = true;
			
			//Wait for turnDelay seconds, defaults to .1 (100 ms).
			yield return new WaitForSeconds(turnDelay);
			
			//If there are no enemies spawned (IE in first level):
			if (enemies.Count == 0) 
			{
				//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
				yield return new WaitForSeconds(turnDelay);
			}
			
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

