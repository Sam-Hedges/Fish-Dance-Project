using UnityEngine;

[CreateAssetMenu(fileName = "BuoyancyScriptableObject", menuName = "ScriptableObjects/WaterSystem/BuoyancyScriptableObject")]
public class BuoyancyScriptableObject : ScriptableObject
{
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int numberOfPoints = 4;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
}
