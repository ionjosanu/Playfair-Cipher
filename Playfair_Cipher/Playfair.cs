using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playfair_Cipher
{
    internal class Playfair
    {
        public string Text;
        public string Key;

        private string Cipher(string input, string key, bool encipher)
        {
            string retVal = string.Empty;
            char[,] keySquare = KeyGenerator(key);
            string tempInput = CharsRemover(input);
            int e = encipher ? 1 : -1;

            if ((tempInput.Length % 2) != 0)
                tempInput += "X";

            for (int i = 0; i < tempInput.Length; i += 2)
            {
                int row1 = 0;
                int col1 = 0;
                int row2 = 0;
                int col2 = 0;

                PositionGetter(ref keySquare, char.ToUpper(tempInput[i]), ref row1, ref col1);
                PositionGetter(ref keySquare, char.ToUpper(tempInput[i + 1]), ref row2, ref col2);

                if (row1 == row2 && col1 == col2)
                    retVal += new string(SameRowCol(ref keySquare, row1, col1, e));
                else if (row1 == row2)
                    retVal += new string(SameRow(ref keySquare, row1, col1, col2, e));
                else if (col1 == col2)
                    retVal += new string(SameCol(ref keySquare, col1, row1, row2, e));
                else
                    retVal += new string(DifferentRowCol(ref keySquare, row1, col1, row2, col2));
            }

            retVal = OutputAdjuster(input, retVal);

            return retVal;
        }

        public string Encrypt(string text, string key)
        {
            return Cipher(text, key, true);
        }

        public string Decrypt(string text, string key)
        {
            return Cipher(text, key, false);
        }


        private string OutputAdjuster(string input, string output)
        {
            StringBuilder retVal = new StringBuilder(output);

            for (int i = 0; i < input.Length; ++i)
            {
                if (!char.IsLetter(input[i]))
                    retVal = retVal.Insert(i, input[i].ToString());

                if (char.IsLower(input[i]))
                    retVal[i] = char.ToLower(retVal[i]);
            }

            return retVal.ToString();
        }

        private string CharsRemover(string text)
        {
            string output = text;
            for (int i = 0; i < output.Length; i++)
                if (!char.IsLetter(output[i]))
                    output = output.Remove(i, 1);
            return output;
        }

        private void PositionGetter(ref char[,] keySquare, char ch, ref int row, ref int col)
        {
            if (ch == 'J')
                PositionGetter(ref keySquare, 'I', ref row, ref col);
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (keySquare[i, j] == ch)
                    {
                        row = i;
                        col = j;
                    }
        }

        private char[] SameRow(ref char[,] keySquare, int row, int col1, int col2, int encipher)
        {
            return new char[] { keySquare[row, Modulus((col1 + encipher), 5)], keySquare[row, Modulus((col2 + encipher), 5)] };
        }

        private char[] SameCol(ref char[,] keySquare, int col, int row1, int row2, int encipher)
        {
            return new char[] { keySquare[Modulus((row1 + encipher), 5), col], keySquare[Modulus((row2 + encipher), 5), col] };
        }

        private char[] SameRowCol(ref char[,] keySquare, int row, int col, int encipher)
        {
            return new char[] { keySquare[Modulus((row + encipher), 5), Modulus((col + encipher), 5)], keySquare[Modulus((row + encipher), 5), Modulus((col + encipher), 5)] };
        }

        private static char[] DifferentRowCol(ref char[,] keySquare, int row1, int col1, int row2, int col2)
        {
            return new char[] { keySquare[row1, col2], keySquare[row2, col1] };
        }
        private char[,] KeyGenerator(string key)
        {
            char[,] keySquare = new char[5, 5];
            string defaultKey = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            string tempKey = string.IsNullOrEmpty(key) ? "CIPHER" : key.ToUpper();
            tempKey = tempKey.Replace("J", "");
            tempKey += defaultKey;
            for (int i = 0; i < 25; i++)
            {
                List<int> indexes = OccurrencesGetter(tempKey, defaultKey[i]);
                tempKey = DuplicateRemover(tempKey, indexes);
            }
            tempKey = tempKey.Substring(0, 25);
            for (int i = 0; i < 25; i++)
                keySquare[(i / 5), (i % 5)] = tempKey[i];
            return keySquare;

        }

        private string DuplicateRemover(string text, List<int> indexes)
        {
            string newText = text;
            for (int i = indexes.Count - 1; i >= 1; i--)
                newText = newText.Remove(indexes[i], 1);
            return newText;
        }

        private int Modulus(int a, int b)
        {
            return (a % b + b) % b;
        }

        private List<int> OccurrencesGetter(string text, char value)
        {
            List<int> indexes = new List<int>();
            int index = 0;
            while ((index = text.IndexOf(value, index)) != -1)
                indexes.Add(index++);
            return indexes;
        }

    }
}
