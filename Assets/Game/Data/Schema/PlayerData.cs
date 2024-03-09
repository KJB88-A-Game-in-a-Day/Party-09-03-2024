using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Custom/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxPower = 1;
    [SerializeField] private float speedMod = 20.0f;
    [SerializeField] private float dizzyTimeMax = 2.5f;
    [SerializeField] private float bumpedTimeMax = 1.0f;
    [SerializeField] private float thrustPower = 40.0f;
    [SerializeField] private float thrustStep = 0.1f;
    public int MaxHealth => maxHealth;
    public int MaxPower => maxPower;
    public float SpeedMod => speedMod;
    public float DizzyTimeMax => dizzyTimeMax;
    public float BumpedTimeMax => bumpedTimeMax;
    public float ThrustPower => thrustPower;
    public float ThrustStep => thrustStep;
}
