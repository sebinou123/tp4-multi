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
        public Text foodText;
		
		private Animator animator;					//Used to store a reference to the Player's animator component.

        public int maxHp = 300;
        public int armor;
        public float experience;
        public WeaponManager weaponManager;
        public int weaponLevel = 1;

        private bool weaponBeingSwapped = false;
        private FacingDirection facing = FacingDirection.Up;                     
		
		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();
			
			//Call the Start function of the MovingObject base class.
			base.Start ();

            weaponManager = new WeaponManager();
            GameManager.instance.TextWeapon.text = weaponManager.getCurrentWeapon().getWeaponName();
            GameManager.instance.TextStats.text = weaponManager.getCurrentWeapon().ToString();
            GameManager.instance.ImageWeapon.sprite = GameManager.instance.items[weaponManager.getCurrentWeaponIndex()];
            updateWeaponRange();

            //GameManager.instance.TextWeapon.color = weaponManager.getCurrentWeapon().getWeaponRarity().color;
		}
		
		private void Update ()
		{
            if (!weaponBeingSwapped && Input.GetKey(KeyCode.Q))
            {
                weaponBeingSwapped = true;
                StartCoroutine(swapWeapon(false));
            }
            else if (!weaponBeingSwapped && Input.GetKey(KeyCode.E))
            {
                weaponBeingSwapped = true;
                StartCoroutine(swapWeapon(true));
            }else if(Input.GetKey(KeyCode.UpArrow)){
                facing = FacingDirection.Up;
                updateWeaponRange(facing);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                facing = FacingDirection.Right;
                updateWeaponRange(facing);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                facing = FacingDirection.Down;
                updateWeaponRange(facing);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                facing = FacingDirection.Left;
                updateWeaponRange(facing);
            }
            else if (Input.GetKey(KeyCode.Space) && !attacking)
            {
                attacking = true;
                StartCoroutine(attackRoutine());
            }
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

		}

        public override float onHit(float damageDealt)
        {
            base.onHit(damageDealt-this.armor);
            return 0;
        }

        IEnumerator attackRoutine()
        {
            int maxInt = 5;
            Vector2 playerPos = transform.position;
            int[,] attackedBlocks = weaponManager.getCurrentWeapon().getWeaponRange(facing);
            for (int i = 0; i < maxInt; i++)
            {
                for (int j = 0; j < maxInt; j++)
                {
                    if (attackedBlocks[i, j] == 1)
                    {
                        Vector2 attackedXY = new Vector2(playerPos.x + i - 2, playerPos.y + j - 2);
                        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
                        boxCollider.enabled = false;
                        RaycastHit2D hit;
                        //Cast a line from start point to end point checking collision on blockingLayer.
                        Move((int)attackedXY.x, (int)attackedXY.y, out hit);

                        //Re-enable boxCollider after linecast
                        boxCollider.enabled = true;

                        if (hit.transform != null)
                        {
                            Debug.Log(hit.transform.position.x);
                            Enemy hitComponent = hit.transform.GetComponent<Enemy>();
                            if (hitComponent != null)
                            {
                                Debug.Log(hitComponent!=null);
                                this.gainExperience(hitComponent.onHit(weaponManager.getCurrentWeapon().getWeaponDamage()));
                            }
                        }
                    }
                }
            }
                yield return new WaitForSeconds(attackDelay);
            attacking = false;
        }

        IEnumerator swapWeapon(bool positiveSwap)
        {
            if (positiveSwap) { 
                weaponManager.nextAvailable(true);
            }
            else
            {
                weaponManager.previousAvailable(true);
            }
            GameManager.instance.TextWeapon.text = weaponManager.getCurrentWeapon().getWeaponName();
            GameManager.instance.TextStats.text = weaponManager.getCurrentWeapon().ToString();
			GameManager.instance.ImageWeapon.sprite = GameManager.instance.items[weaponManager.getCurrentWeaponIndex()];
            updateWeaponRange(facing);
            yield return new WaitForSeconds(0.1f);
            weaponBeingSwapped = false;
        }

        private void updateWeaponRange(FacingDirection dir)
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    GameManager.instance.weaponRange[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>(weaponManager.getCurrentWeapon().getWeaponRange(facing)[i, j] == 1 ? "Sprites/attackRange" : "Sprites/noAttackRange");
                }
            }
        }
        private void updateWeaponRange()
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    GameManager.instance.weaponRange[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>(weaponManager.getCurrentWeapon().getWeaponRange()[i, j] == 1 ? "Sprites/attackRange" : "Sprites/noAttackRange");
                }
            }
        }

        public void gainExperience(float amount)
        {
            this.experience += amount;
            if (experience >= 50 * Mathf.Exp(0.1f * level))
            {
                this.level++;
            }
        }
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{			
			//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
			base.AttemptMove <T> (xDir, yDir);
			
			//Hit allows us to reference the result of the Linecast done in Move.
			RaycastHit2D hit;
			
			//If Move returns true, meaning Player was able to move into an empty space.
			if (Move (xDir, yDir, out hit)) 
			{
				//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
				SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}
			
			//Since the player has moved and lost food points, check if the game has ended.
			CheckIfGameOver ();
		}
		
		
		//OnCantMove overrides the abstract function OnCantMove in MovingObject.
		//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
		protected override void OnCantMove <T> (T component)
		{
			//Set hitWall to equal the component passed in as a parameter.
			Wall hitWall = component as Wall;
			
			//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
			animator.SetTrigger ("playerChop");
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
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game.
			Application.LoadLevel (Application.loadedLevel);
		}
		
		
		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseFood (int loss)
		{
			//Set the trigger for the player animator to transition to the playerHit animation.
			animator.SetTrigger ("playerHit");
			
			//Check to see if game has ended.
			CheckIfGameOver ();
		}
		
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (false) 
			{
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			}
		}
        public enum FacingDirection
        {
            Up, Right, Down, Left
        }
	}


