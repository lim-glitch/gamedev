using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for single + multiplayer result
public class GameResult : MonoBehaviour
{
    // Single player
    public static int score;
    public static float timeTaken;
    public static int stars;
    public static int levelNumber;

    // Multiplayer
    public static int player1Score;
    public static int player2Score;

    public static bool isMultiplayer;

}
