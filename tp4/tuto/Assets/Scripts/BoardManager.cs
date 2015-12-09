using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.


	/**
	 * Class who generate all the boardgame, its decoration, its outerwall, its floor, its lava, its exit.
	 * */
	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
		
		
		public int columns = 8; 										//Number of columns in our game board.
		public int rows = 8;											//Number of rows in our game board.
		public Count decorCount = new Count(5,14);						//Number of decoration with min and max		
		public GameObject exit;											//Prefab to spawn exit.
		public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] decorTiles;									//Array of decor prefabs.
		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
		public int lavaCount = 6;										//Number of lava tiles
		public int lavaspot = 8;										//Number of lava spot
		public GameObject[] lavaTile;									//Array of lava tile prefabs.
		public GameObject[] exitDecorBack;								//Array of back decoration prefabs.			
		public GameObject[] exitDecorFront;								//Array of front decoration prefabs.
		private List <Vector3> exitPosition = new List<Vector3> ();

		
		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place tiles.
		
		
		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();
			
			//Loop through x axis (columns).
			for(int x = 1; x < columns-1; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < rows-1; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}
		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;
			
			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = -1; x < columns + 1; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = -1; y < rows + 1; y++)
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					
					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if(x == -1 || x == columns || y == -1 || y == rows)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}

	//Represent the spot where the exit are in the game, this method check if the vector3 in parameter is in the same position of the exit tiles
	Boolean positionRestricted (Vector3 position)
	{
		bool goodRestricted = false;
		int index = 0;
		foreach(Vector3 element in exitPosition){
			if( (element - position).sqrMagnitude <= (element * 0.01f).sqrMagnitude) {
				goodRestricted = true;
			}
			
			index++;
		}

		return goodRestricted;
	}

	//Whit a vector3, check in the array of possible position the index of the position where they are almost the same (0.01 of difference max) and delete the possibility 
	void deletePosibility (Vector3 position)
	{
		List<Vector3> newList = new List<Vector3>(gridPositions);
		int index = 0;
		foreach(Vector3 element in newList){
			if( (element - position).sqrMagnitude <= (element * 0.01f).sqrMagnitude) {

				gridPositions.RemoveAt (index);
			}

			index++;
		}
	}
		
		
		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);
			
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition();
				
				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}


		}

	//generate the exit layout in the boardgame
	void LayoutExitLayout(GameObject[] tileArrayBack,GameObject[] tileArrayFront, int x, int y)
	{
		float xPosition = 0;
		float yPosition = 0;
		GameObject tileChoiceBack = tileArrayBack[Random.Range (0, tileArrayBack.Length)];
		GameObject tileChoiceFront = tileArrayFront[Random.Range (0, tileArrayFront.Length)];

		// we don't want the exit at the edge of the game, left, right, down or up
		if (y >= rows) {
			y = rows - 2;
		}
		if (y < 0) {
			y = 2;
		}
		if (x >= columns) {
			x = columns - 3;
		}
		if (x < 0) {
			x = 1;
		}

		//make the top of the exit layout
		Vector3 positionFirstColumns = new Vector3 (x, y, 0);
		//instantiate decoration 
		for (int k = 0; k <= 2; k++) {
			exitPosition.Add (positionFirstColumns);
			Instantiate(tileChoiceBack, positionFirstColumns, Quaternion.identity);
			deletePosibility(positionFirstColumns);
			positionFirstColumns.x +=1;
		}

		//make the bottom of the exit layout
		Vector3 positionSecondColumns = new Vector3(x,y-1,0);

		for (int l = 0; l <= 2; l++) {
			//instantiate decoration 
			if(l != 1){
			exitPosition.Add (positionSecondColumns);
			Instantiate(tileChoiceFront, positionSecondColumns, Quaternion.identity);
			//delete posibility, we don't want an enemy or anything in the decoration tile
			deletePosibility(positionSecondColumns);
			}
			//instantiate the exit to the next level who are between decoration
			else{
				exitPosition.Add (positionSecondColumns);
				xPosition = positionSecondColumns.x;
				yPosition = positionSecondColumns.y;
				Instantiate (exit, positionSecondColumns, Quaternion.identity);
				//delete posibility, we don't want an enemy or anything in the exit tile
				deletePosibility(positionSecondColumns);
			}
			positionSecondColumns.x +=1;

			//delete posibility near of the exit because we don't want to be blocked by lava or anything else
			Vector3 blockingExit = new Vector3(xPosition - 1, yPosition - 1, 0);
			deletePosibility(blockingExit);
			blockingExit.y -= 1;
			deletePosibility(blockingExit);
			blockingExit.y -= 1;
			deletePosibility(blockingExit);
			blockingExit.x += 1;
			deletePosibility(blockingExit);
		}




	}

	//layout random spot of lava
	void LayoutLava(GameObject[] tileArray, int numberspot, int numbertylelava)
	{
		bool first = true;
		bool goodNumber = false;
		int randomNumber = 0;
		int currentNumber = 0;
			
			//for the number of spot
			for(int i = 0; i < numberspot; i++)
			{
			//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
			Vector3 randomPosition = RandomPosition();
			
			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			
			//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
			if(positionRestricted(randomPosition) != true){
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}

					//for the number of lava
					for(int j = 0; (j < numbertylelava - 1); j++)
					{	
						//if first time generate the first random number and don't check if the previous number and the new number are the same
						if(first == true){
							randomNumber = Random.Range(0,8);
							currentNumber = randomNumber;
							first = false;
						}else{
						//do while the random number is not different of the precedent
							do{
								randomNumber = Random.Range(0,8);

								if(randomNumber == currentNumber){
									goodNumber = false;
								}else{
									goodNumber = true;
								}
							}while(goodNumber != true);
						}
						
						//for each random number, there is different position where the lava can be add to the game
						if(randomNumber == 0){
							//check if you are in the game, not in the outside
							if(randomPosition.x + 1 > 0 && randomPosition.x + 1 < columns )
							{
								randomPosition.x +=1;
								//check if the lava is not in conflict with an other gameobject
								if(positionRestricted(randomPosition) != true){
									Instantiate(tileChoice, randomPosition, Quaternion.identity);
									deletePosibility(randomPosition);
								}
							}
						}else if(randomNumber == 1){
							if(randomPosition.x - 1 > 0 && randomPosition.x - 1 < columns )
							{
							 
								randomPosition.x -=1;
							if(positionRestricted(randomPosition) != true){
								Instantiate(tileChoice, randomPosition, Quaternion.identity);
								deletePosibility(randomPosition);
								}
							}

						}else if(randomNumber == 2){
							if(randomPosition.y + 1 > 0 && randomPosition.y + 1 < rows )
							{
							
										randomPosition.y +=1;
								if(positionRestricted(randomPosition) != true){
										Instantiate(tileChoice, randomPosition, Quaternion.identity);
										deletePosibility(randomPosition);
								}
							}

						}else if(randomNumber == 3){
							if(randomPosition.y - 1 > 0 && randomPosition.y - 1 < rows )
							{
										randomPosition.y -=1;
								if(positionRestricted(randomPosition) != true){
										Instantiate(tileChoice, randomPosition, Quaternion.identity);
										deletePosibility(randomPosition);
								}
							}

						}else if(randomNumber == 4){
					if(randomPosition.y - 1 > 0 && randomPosition.y - 1 < rows && randomPosition.x - 1 > 0 && randomPosition.x - 1 < columns)
							{
								
										randomPosition.x -=1;
										randomPosition.y -=1;
								if(positionRestricted(randomPosition) != true){
										Instantiate(tileChoice, randomPosition, Quaternion.identity);
										deletePosibility(randomPosition);
								}
							}
							
						}else if(randomNumber == 5){
						if(randomPosition.y - 1 > 0 && randomPosition.y - 1 < rows && randomPosition.x + 1 > 0 && randomPosition.x + 1 < columns)
							{
									randomPosition.x +=1;
									randomPosition.y -=1;
							if(positionRestricted(randomPosition) != true){
									Instantiate(tileChoice, randomPosition, Quaternion.identity);
									deletePosibility(randomPosition);
								}		
							}
							
						}else if(randomNumber == 6){
							if(randomPosition.y + 1 > 0 && randomPosition.y + 1 < rows && randomPosition.x + 1 > 0 && randomPosition.x + 1 < columns)
							{
								
									randomPosition.x +=1;
									randomPosition.y +=1;
								if(positionRestricted(randomPosition) != true){
									Instantiate(tileChoice, randomPosition, Quaternion.identity);
									deletePosibility(randomPosition);
								}			
							}
							
						}else if(randomNumber == 7){
							if(randomPosition.y + 1 > 0 && randomPosition.y + 1 < rows && randomPosition.x - 1 > 0 && randomPosition.x - 1 < columns)
							{
									randomPosition.x -=1;
									randomPosition.y +=1;
								if(positionRestricted(randomPosition) != true){
									Instantiate(tileChoice, randomPosition, Quaternion.identity);
									deletePosibility(randomPosition);
								}
							}
							
						}
					}
			}
	}




		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			//Creates the outer walls and floor.
			BoardSetup ();
			
			//Reset our list of gridpositions.
			InitialiseList ();
		


			//Determine number of enemies based on current level number, based on a logarithmic progression
			int enemyCount = 6;

			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
			//Instantiate the exit tile in the upper right hand corner of our game board
			LayoutExitLayout (exitDecorBack,exitDecorFront, Random.Range (8, columns - 3), Random.Range (8, rows - 2));


			LayoutLava(lavaTile, lavaspot, lavaCount);
		
			//Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (decorTiles, decorCount.minimum, decorCount.maximum);

			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

			

	
			
			

		}



	}

