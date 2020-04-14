using Rtr.Atf.Core;

namespace Rtr.Atf.CodedUI.UI
{
    public class CodedUiScreenCoordinates : IScreenCoordinates
    {
        internal CodedUiScreenCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public override string ToString()
        {
            return $"(X = {this.X}, Y = {this.Y}) [{nameof(CodedUiScreenCoordinates)}]";
        }
    }
}
