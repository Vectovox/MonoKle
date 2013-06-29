namespace MonoKle.State
{
    public abstract class GameState
    {
        public virtual void Activated()
        {
        }

        public virtual void Deactivated()
        {
        }

        public abstract void Draw(double seconds);

        public virtual void Removed()
        {
        }

        public abstract void Update(double seconds);
    }
}