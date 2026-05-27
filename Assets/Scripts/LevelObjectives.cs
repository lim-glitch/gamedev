using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public static string GetObjective(int level)
    {
        switch (level)
        {
            case 1:
                return "Go to the Exit Elevator";
            case 2:
                return "Reach the Ventilation Shaft";
            case 3:
                return "Survive the Robot Army and Defeat Boss";
            default:
                return "";
        }
    }
}