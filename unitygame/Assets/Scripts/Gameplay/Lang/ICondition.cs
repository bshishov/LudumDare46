namespace Gameplay.Lang
{
    public interface ICondition
    {
        bool Call(GameContext context);
    }
}