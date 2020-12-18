using JetBrains.Annotations;
using Zenject;

namespace Battles.Scoring
{
    public class ScoreCalculator : IScoreProvider
    {
        private readonly SignalBus signalBus;
        private readonly IScoringConfiguration scoringConfiguration;

        private int currentScore = 0;

        [UsedImplicitly]
        public ScoreCalculator(SignalBus signalBus, IScoringConfiguration scoringConfiguration)
        {
            this.signalBus = signalBus;
            this.scoringConfiguration = scoringConfiguration;
            
            signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            currentScore += scoringConfiguration.GetPointsFor(signal.type);
            signalBus.Fire(new PlayerScoreChangedSignal(currentScore));
        }

        public int GetScore()
        {
            return currentScore;
        }
    }

    internal class PlayerScoreChangedSignal
    {
        public readonly int newScore;

        public PlayerScoreChangedSignal(int score)
        {
            newScore = score;
        }
    }
}