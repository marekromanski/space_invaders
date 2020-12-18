namespace Battles.Entities.Player
{
    internal class PlayerLivesAmountChangedSignal
    {
        public readonly int livesRemaining;

        public PlayerLivesAmountChangedSignal(int livesRemaining)
        {
            this.livesRemaining = livesRemaining;
        }
    }
}