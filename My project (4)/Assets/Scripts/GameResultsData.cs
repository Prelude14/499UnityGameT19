using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "GameResultsData", menuName = "ScriptableObjects/GameResultsData", order = 1)]
public class GameResultsData : ScriptableObject
{
    public int p1Damage = 0; // Track player 1's damage.
    public int p2Damage = 0; // Track player 2's damage.
    public char p1Result = '\0'; // Track player 1's result (win/loss).
    public char p2Result = '\0'; // Track player 2's result (win/loss).

    public void ResetData()
    {
        p1Damage = 0;
        p2Damage = 0;
        p1Result = '\0';
        p2Result = '\0';
    }
}