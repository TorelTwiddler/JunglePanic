using UnityEngine;
using System.Collections;
using System;

public class PlayerSlots : MonoBehaviour {
	
	public GameObject[] characters;
	GlobalOptions options;
	
	private int winSlotIndex = 0;
	private int loseSlotIndex = 0;
	
	// Use this for initialization
	void Start () {
		options = GlobalOptions.Instance;
		SlotPlayers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SlotPlayers(){
		Team team = options.GetMostRecentWinningTeam();
		// get the team index from the team name
		int teamIndex = Array.IndexOf(options.TeamNames, team.teamName);
		for (int playerIndex = 0; playerIndex < 4; playerIndex++){
			// if the player is on the winning team
			if (options.PlayerTeamAssignment[playerIndex] == teamIndex) {
				// Winning team
				GameObject slot = GameObject.Find("WinSlot" + winSlotIndex.ToString());
				winSlotIndex++;
				AddToSlot(playerIndex, slot, true);
			}
			else if (options.PlayerTeamAssignment[playerIndex] != -1){
				// Losing team
				GameObject slot = GameObject.Find("LoseSlot" + loseSlotIndex.ToString());
				loseSlotIndex++;
				AddToSlot(playerIndex, slot, false);
			}
		}
	}
	
	void AddToSlot(int playerIndex, GameObject slot, bool celebrate) {
		// Get which player it should look like
		int characterIndex = options.GetPlayerCharacters()[playerIndex];
		characters[characterIndex].transform.parent = slot.transform;
		characters[characterIndex].transform.position = slot.transform.position;
		if (celebrate) {
			characters[characterIndex].GetComponent<Celebrate>().Play();
		} else {
			characters[characterIndex].GetComponent<Player>().PlayAnimation("Walk");
		}
	}
}
