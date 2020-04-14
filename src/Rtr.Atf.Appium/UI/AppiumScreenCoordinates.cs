using Rtr.Atf.Core;

namespace Rtr.Atf.Appium
{
    public class AppiumScreenCoordinates : IScreenCoordinates
    {
        internal AppiumScreenCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public override string ToString()
        {
            return $"(X = {this.X}, Y = {this.Y}) [{nameof(AppiumScreenCoordinates)}]";
        }
    }
}
