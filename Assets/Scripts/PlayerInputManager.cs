using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput{
	
	public bool UsingKeyboard = true;
	public KeyCode Left, Right, Down, Jump;
	public string ControllerHorizontal, ControllerVertical;
	
	/*public float GetHorizontal(){
		
	}
	
	public float GetVertical(){
		
	}*/
}

public static class PlayerInputManager {
	
	public static List<PlayerInput> PlayerInputs;
}
