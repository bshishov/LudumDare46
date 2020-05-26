using Gameplay.States;
using Utils;

namespace Messages
{
    public class GameStateChanged : GenericTinyMessage<GameState>
    {
        public GameStateChanged(object sender, GameState content) : base(sender, content)
        {
        }
    }
}