[System.Serializable]
public class PlayerStats
{
    public float RunSpeed;
    public float JumpForce;
    public float JumpTimeout;
    public float MaxHealth;
    public float RegenRate;

    public PlayerStats(PlayerDataHandler playerData)
    {
        RunSpeed = playerData.RunSpeed;
        JumpForce = playerData.JumpForce;
        JumpTimeout = playerData.JumpTimeout;
        MaxHealth = playerData.MaxHealth;
        RegenRate = playerData.RegenRate;
    }
}
