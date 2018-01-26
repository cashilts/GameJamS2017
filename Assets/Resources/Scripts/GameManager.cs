using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    List<Player> players = new List<Player>();
    public const int numPlayers = 2;
    int activePlayer = 2;
    public int turnCount = 0;

	// Use this for initialization
	void Start () {
        
        GameObject localPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Player"));
        localPlayer.transform.parent = transform;
        localPlayer.name = "Player0";
        Player localScript = localPlayer.GetComponent<Player>();
        players.Add(localScript);
        localScript.playerColor = new Color(255, 0, 0, 0);
        localScript.id = 0;

        for(int i = 1; i < numPlayers; i++)
        {
            GameObject aiPlayer = Instantiate((GameObject)Resources.Load("Prefabs/AIPlayer"));
            aiPlayer.transform.parent = transform;
            aiPlayer.name = "Player" + i;
            Player AIScript = aiPlayer.GetComponent<Player>();
            players.Add(AIScript);
            AIScript.playerColor = new Color(0, 0, 255, 0);
            AIScript.id = i;
        }
	}

    // Update is called once per frame
    void Update() { 
        if(activePlayer >= numPlayers)
        {
            activePlayer = 0;
            foreach (Player player in players)
            {
                player.newTurn();
            }
            turnCount++;
        }
    }

    public void endCurrentTurn(){
        while (true)
        {
            activePlayer++;
            if (activePlayer < numPlayers)
            {
                if (players[activePlayer].aiPlayer)
                {
                    players[activePlayer].doTurn();
                }
                else
                {
                    break;
                }
            }
            else
            {
                activePlayer = -1;
                foreach(Player player in players)
                {
                    player.newTurn();
                }
                turnCount++;
            }
        }
    }
}
