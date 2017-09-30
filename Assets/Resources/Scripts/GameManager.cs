using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    List<Player> players;
    public const int numPlayers = 2;
    int activePlayer = -1;
    public int turnCount = 0;

	// Use this for initialization
	void Start () {
        players = new List<Player>();
        GameObject localPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Player"));
        localPlayer.transform.parent = transform;
        localPlayer.name = "Player0";
        Player localScript = localPlayer.GetComponent<Player>();
        players.Add(localScript);

        for(int i = 1; i < numPlayers; i++)
        {
            GameObject aiPlayer = Instantiate((GameObject)Resources.Load("Prefabs/AIPlayer"));
            aiPlayer.transform.parent = transform;
            aiPlayer.name = "Player" + i;
            Player AIScript = aiPlayer.GetComponent<Player>();
            players.Add(AIScript);
        }
	}

    // Update is called once per frame
    void Update() { 
    }

    void endCurrentTurn(){
        while (true)
        {
            activePlayer++;
            if (activePlayer < numPlayers)
            {
                if (players[activePlayer].aiPlayer)
                {
                    players[activePlayer].doTurn();
                }
                break;
            }
            else
            {
                activePlayer = 0;
            }
        }
    }
}
