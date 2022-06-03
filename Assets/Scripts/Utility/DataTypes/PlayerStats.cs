[System.Serializable]
public class PlayerStats
{
    public float JumpTimeout;
    public float JumpForce;
    public float MaxHealth;
    public float RegenRate;
    public float RunSpeed;

    public PlayerStats(PlayerDataHandler playerData)
    {
        RunSpeed = playerData.RunSpeed;
        JumpForce = playerData.JumpForce;
        JumpTimeout = playerData.JumpTimeout;
        MaxHealth = playerData.MaxHealth;
        RegenRate = playerData.RegenRate;
    }
}
