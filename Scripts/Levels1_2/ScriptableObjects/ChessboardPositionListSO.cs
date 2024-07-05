using UnityEngine;

[CreateAssetMenu(fileName = "ChessboardPositionList", menuName = "ScriptableObjects/ChessboardPositionList", order = 1)]
public class ChessboardPositionList : ScriptableObject
{
    public Vector3[] positions;
}
