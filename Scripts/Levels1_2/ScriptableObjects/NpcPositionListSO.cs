using UnityEngine;

[CreateAssetMenu(fileName = "NpcPositionList", menuName = "ScriptableObjects/NpcPositionList", order = 1)]
public class NpcPositionList : ScriptableObject
{
    public Vector3[] positions;
}