using UnityEngine;

public class Price : MonoBehaviour
{
    //This script handles the objects that are picked up by the player and give him points, namely the gems
    internal GameObject GameController; //Used to hold the game controller object, if it's in the scene
    internal GameObject Player; //Used to hold the player object, if it's in the scene
    
    public float SpinSpeed = 90; //The spinning speed of the object. It rotates around the up axis 
    
    public int Value = 1; //How much is added to the player's score when this object is picked up
    public float PickupRange = 1f; //The range from which the droppable is picked up by the Player
    
    public Transform PickupEffect; //The effect displayed when the droppable is picked up
    internal Transform PickUpEffectCopy; //A copy of the pickup effect
    
    void Start()
    {
    	GameController = GameObject.FindWithTag("GameController"); //Find the game controller in the scene and put it in a variable, for later use
    	Player = GameObject.FindWithTag("Player"); //Find the player in the scene and put it in a variable, for later use
		transform.Rotate (Vector3.right , 90f ,Space.Self);
    }
    
    void Update() 
    {
    	//Rotate the object around the UP axis at a speed set by SpinSpeed
    	transform.Rotate(Vector3.up, SpinSpeed * Time.deltaTime, Space.World);
    	
    	if ( Player ) //If the Player object exists, do the following
    	{
    		if ( Vector3.Distance(transform.position, Player.transform.position) < PickupRange ) //If the Player is close enough to the object, pick it up
    		{
    			if ( PickupEffect ) //If a pickup effect is defined
    			{
    				PickUpEffectCopy = Instantiate(PickupEffect, transform.position, transform.rotation) as Transform; //Create a nice pickup effect
    				PickUpEffectCopy.transform.parent = transform.parent; //Attach the pickup effect to the player
    			}
    			
    			//Add to the player's gem score
    			GameController.GetComponent<GameController>().TotalPrices += Value; 
    			
    			Destroy(gameObject); //remove the object
    		}
    	}
    }
}
