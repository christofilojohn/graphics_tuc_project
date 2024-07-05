using UnityEngine;

[CreateAssetMenu(fileName = "RockPositionList", menuName = "ScriptableObjects/RockPositionList", order = 1)]
public class RockPositionList : ScriptableObject
{
    public Vector3[] positions;
}