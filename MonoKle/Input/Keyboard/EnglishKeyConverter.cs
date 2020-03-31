using Microsoft.Xna.Framework.Input;

namespace MonoKle.Input.Keyboard
{
    /// <summary>
    /// Keyboard input converter for EN-US QWERTY-keyboards.
    /// </summary>
    public class EnglishKeyConverter : AbstractKeyConverter
    {
        /// <summary>
        /// Converts the specified key to its character representation. If conversion failed, returns the default <see cref="char" /> value.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <param name="shift">Indicates shift modifier.</param>
        /// <param name="altgr">Indicates altgr modifier.</param>
        /// <returns>
        /// Character representation of the key; the default <see cref="char" /> value if no representation was found.
        /// </returns>
        public override char Convert(Keys key, bool shift, bool altgr)
        {
            switch (key)
            {
                case Keys.Space:
                    return ' ';
                case Keys.D0:
                    return '0';
                case Keys.D1:
                    return '1';
                case Keys.D2:
                    return '2';
                case Keys.D3:
                    return '3';
                case Keys.D4:
                    return '4';
                case Keys.D5:
                    return '5';
                case Keys.D6:
                    return '6';
                case Keys.D7:
                    return '7';
                case Keys.D8:
                    return '8';
                case Keys.D9:
                    return '9';
                case Keys.A:
                    return shift ? 'A' : 'a';
                case Keys.B:
                    return shift ? 'B' : 'b';
                case Keys.C:
                    return shift ? 'C' : 'c';
                case Keys.D:
                    return shift ? 'D' : 'd';
                case Keys.E:
                    return shift ? 'E' : 'e';
                case Keys.F:
                    return shift ? 'F' : 'f';
                case Keys.G:
                    return shift ? 'G' : 'g';
                case Keys.H:
                    return shift ? 'H' : 'h';
                case Keys.I:
                    return shift ? 'I' : 'i';
                case Keys.J:
                    return shift ? 'J' : 'j';
                case Keys.K:
                    return shift ? 'K' : 'k';
                case Keys.L:
                    return shift ? 'L' : 'l';
                case Keys.M:
                    return shift ? 'M' : 'm';
                case Keys.N:
                    return shift ? 'N' : 'n';
                case Keys.O:
                    return shift ? 'O' : 'o';
                case Keys.P:
                    return shift ? 'P' : 'p';
                case Keys.Q:
                    return shift ? 'Q' : 'q';
                case Keys.R:
                    return shift ? 'R' : 'r';
                case Keys.S:
                    return shift ? 'S' : 's';
                case Keys.T:
                    return shift ? 'T' : 't';
                case Keys.U:
                    return shift ? 'U' : 'u';
                case Keys.V:
                    return shift ? 'V' : 'v';
                case Keys.W:
                    return shift ? 'W' : 'w';
                case Keys.X:
                    return shift ? 'X' : 'x';
                case Keys.Y:
                    return shift ? 'Y' : 'y';
                case Keys.Z:
                    return shift ? 'Z' : 'z';
                case Keys.NumPad0:
                    return '0';
                case Keys.NumPad1:
                    return '1';
                case Keys.NumPad2:
                    return '2';
                case Keys.NumPad3:
                    return '3';
                case Keys.NumPad4:
                    return '4';
                case Keys.NumPad5:
                    return '5';
                case Keys.NumPad6:
                    return '6';
                case Keys.NumPad7:
                    return '7';
                case Keys.NumPad8:
                    return '8';
                case Keys.NumPad9:
                    return '9';
                case Keys.Multiply:
                    return '*';
                case Keys.Add:
                    return '+';
                //case Keys.Separator:
                //    break;
                case Keys.Subtract:
                    return '-';
                //case Keys.Decimal:
                //    break;
                case Keys.Divide:
                    return '/';
                //case Keys.OemSemicolon:
                //    break;
                //case Keys.OemPlus:
                //    break;
                //case Keys.OemComma:
                //    break;
                case Keys.OemMinus:
                    return shift ? '_' : '-';
                case Keys.OemPeriod:
                    return '.';
                //case Keys.OemQuestion:
                //    break;
                //case Keys.OemTilde:
                //    break;
                //case Keys.OemOpenBrackets:
                //    break;
                //case Keys.OemPipe:
                //    break;
                //case Keys.OemCloseBrackets:
                //    break;
                //case Keys.OemQuotes:
                //    break;
                //case Keys.Oem8:
                //    break;
                //case Keys.OemBackslash:
                //    break;
                //case Keys.OemClear:
                //    break;
                //case Keys.OemAuto:
                //    break;
                //case Keys.OemCopy:
                //    break;
                //case Keys.OemEnlW:
                //    break;
                default:
                    return default(char);
            }
        }
    }
}