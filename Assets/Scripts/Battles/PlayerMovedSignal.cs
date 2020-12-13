namespace Battles
{
    public class PlayerMovedSignal
    {
        public readonly float delta;

        public PlayerMovedSignal(float delta)
        {
            this.delta = delta;
        }
    }
}