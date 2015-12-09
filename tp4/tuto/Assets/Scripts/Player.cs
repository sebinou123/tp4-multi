using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;
using UnityEditor.VersionControl;	//Allows us to use UI.


	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
        public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
        public const float attackDelay = 0.5f;		//Delay time in seconds to restart level.
		public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
		
		private Animator animator;					//Used to store a reference to the Player's animator component.

        public int maxHp = 300;
        public float experience;
        public float maxExperience;
        public WeaponManager weaponManager;

        private bool weaponBeingSwapped = false;
        private FacingDirection facing = FacingDirection.Up;     
		private Player instance;


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
	
	}

		
		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{
            this.experience = 0;
            this.maxExperience = level*50 + 50;
            this.maxHp = 100 + (int)Mathf.Ceil(0.2f * Mathf.Exp(0.2f * level));
            this.hp = this.maxHp;
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();
			
			//Call the Start function of the MovingObject base class.
			base.Start ();

            weaponManager = new WeaponManager();

    
		}
		
		//call when an event is started
		private void Update ()
		{
			//if q or e, change the current weapon
            if (!weaponBeingSwapped && Input.GetKey(KeyCode.Q))
            {
                weaponBeingSwapped = true;
                StartCoroutine(swapWeapon(false));
            }
            else if (!weaponBeingSwapped && Input.GetKey(KeyCode.E))
            {
                weaponBeingSwapped = true;
                StartCoroutine(swapWeapon(true));
			//if its the arrows, change the facing of the player
            }else if(Input.GetKey(KeyCode.UpArrow)){
				animator.SetInteger("Direction", 0);
                facing = FacingDirection.Up;
                updateInfosPlayer();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
				animator.SetInteger("Direction", 3);
                facing = FacingDirection.Right;
                updateInfosPlayer();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
			animator.SetInteger("Direction", 1);
                facing = FacingDirection.Down;
                updateInfosPlayer();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
			animator.SetInteger("Direction", 2);
                facing = FacingDirection.Left;
                updateInfosPlayer();
            }
			//if its spacebar, begin attack routine
            else if (Input.GetKey(KeyCode.Space) && !attacking)
            {
				if(facing == FacingDirection.Up) {
					animator.SetTrigger ("WarriorAttackUp");
				}
				else if(facing == FacingDirection.Down) {
					animator.SetTrigger ("WarriorAttackDown");
				}
				else if(facing == FacingDirection.Right) {
					animator.SetTrigger ("WarriorAttackRight");
				}
				else {
					animator.SetTrigger ("WarriorAttackLeft");
				}
                attacking = true;
                StartCoroutine(attackRoutine());
            }
			//if its moving (a,s,d,w), try to move if its not a blocking layer
            if (!base.moving)
            {
                int horizontal = 0;  	//Used to store the horizontal move direction.
                int vertical = 0;		//Used to store the vertical move direction.

                //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
                horizontal = (int)(Input.GetAxisRaw("Horizontal"));

                //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
                vertical = (int)(Input.GetAxisRaw("Vertical"));

                //Check if moving horizontally, if so set vertical to zero.
                if (horizontal != 0)
                {
                    vertical = 0;
                }

                //Check if we have a non-zero value for horizontal or vertical
                if (horizontal != 0 || vertical != 0)
                {
                    //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
                    //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                    AttemptMove<Wall>(horizontal, vertical);
                }
            }
            updateInfosPlayer();
		}

		//update all stats about the player
        private void updateInfosPlayer()
        {
            GameManager.instance.TextName.text = btnStartGame.name;
            GameManager.instance.TextWeapon.text = weaponManager.getCurrentWeapon().getWeaponName();
            GameManager.instance.TextStats.text = weaponManager.getCurrentWeapon().ToString();
            GameManager.instance.TextLevel.text = "" + this.level;
            GameManager.instance.TextHp.text = "" + this.hp + " / " + this.maxHp;
            GameManager.instance.ImageWeapon.sprite = GameManager.instance.items[weaponManager.getCurrentWeaponIndex()];
            updateWeaponRange();
        }

		//when the player is attacked by an ennemy, he lose hp and we check if he died
        public override float onHit(float damageDealt)
        {
            base.onHit(damageDealt);

			//Check to see if game has ended.
			CheckIfGameOver ();
            return 0;
        }
		
		//call when the player attack, check if he did attack an ennemy and collect experience if he kill the ennemy
        IEnumerator attackRoutine()
        {
            int maxInt = 5;
            Vector2 playerPos = transform.position;
			//get the range of the weapon with the good facing
            int[,] attackedBlocks = weaponManager.getCurrentWeapon().getWeaponRange(facing);
            for (int i = 0; i < maxInt; i++)
            {
                for (int j = 0; j < maxInt; j++)
                {
                    if (attackedBlocks[i, j] == 1)
                    {
                        Vector2 attackedXY = new Vector2(playerPos.x + j - 2, playerPos.y - i + 2);
                        foreach(Enemy go in GameManager.instance.enemies){
						//if he hit the ennemy, the enemy lose hp and gain experience if he's dead
                            if (go.transform.position.x == attackedXY.x && go.transform.position.y == attackedXY.y)
                            {
                                this.gainExperience(go.onHit(weaponManager.getCurrentWeapon().getWeaponDamage()));
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(attackDelay);
            attacking = false;
        }

		//set what will be the previous and the next weapon of our current weapon
        IEnumerator swapWeapon(bool positiveSwap)
        {
            if (positiveSwap) { 
                weaponManager.nextAvailable(true);
            }
            else
            {
                weaponManager.previousAvailable(true);
            }
            updateInfosPlayer();
            yield return new WaitForSeconds(0.1f);
            weaponBeingSwapped = false;
        }

		//update the image representation of the weapon range with the good facing
        private void updateWeaponRange()
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    GameManager.instance.weaponRange[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>(weaponManager.getCurrentWeapon().getWeaponRange(facing)[i, j] == 1 ? "Sprites/attackRange" : "Sprites/noAttackRange");
                }
            }
        }

		//player get experience and check if he gain a level, so he get his hp back and get more hp at heach level. Show for 2 second a message to the user to know that his
		//hero gain a level
        public void gainExperience(float amount)
        {
            this.experience += amount;
            if (experience >= maxExperience)
            {	
				StartCoroutine(ShowMessage(2));
                this.level++;
                this.maxExperience = maxExperience + 50;
                this.maxHp = 100 + (int)Mathf.Ceil(0.2f * Mathf.Exp(0.2f * level));
                this.hp = this.maxHp;
            }
        }


		//show the message level UP for a specific time (delay)
		IEnumerator ShowMessage (float delay) {
			GameObject levelup = (GameObject)Instantiate(Resources.Load("Prefabs/CanvasLevelUP")); 
			levelup.SetActive (true);
			yield return new WaitForSeconds(delay);
			levelup.SetActive (false);
		}
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{			
			//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
			base.AttemptMove <T> (xDir, yDir);
			//Hit allows us to reference the result of the Linecast done in Move.
			RaycastHit2D hit;

			//do the right animation of moving with the right facing
			if(facing == FacingDirection.Up) {
				animator.SetTrigger ("MovingUp");
			}
			else if(facing == FacingDirection.Down) {
				animator.SetTrigger ("MovingDown");
			}
			else if(facing == FacingDirection.Right) {
				animator.SetTrigger ("MovingRight");
			}
			else {
				animator.SetTrigger ("MovingLeft");
			}

			SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			
			//If Move returns true, meaning Player was able to move into an empty space.
			if (Move (xDir, yDir, out hit)) 
			{
				//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
				SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}
			
			//Since the player has moved and lost food points, check if the game has ended.
			CheckIfGameOver ();
		}		
		
		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		private void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{
				//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Restart", restartLevelDelay);
				
				//Disable the player object since level is over.
				enabled = false;
			}
		}
		
		
		//Restart reloads the scene when called.
        private void Restart()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game.
            Application.LoadLevel(Application.loadedLevel);
        }

        public int getLevel()
        {
            return this.level;
        }

        protected override void OnCantMove<T>(T component)
        {
        }
		
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (this.hp <= 0) 
			{
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			}
		}

		//enum of each possible facing direction
        public enum FacingDirection
        {
            Up, Right, Down, Left
        }
	}


