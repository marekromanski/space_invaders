namespace Battles.Entities.Player
{
    public interface IPlayerConfiguration : ICharacterConfiguration
    {
        int LivesTotal { get; }

        float InvulnerabilityDuration { get; }
    }
}