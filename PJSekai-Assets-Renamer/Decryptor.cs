using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJSekai_Assets_Renamer
{
    internal class Decryptor
    {
        public static byte[] DecryptAsset(byte[] sourceBytes)
        {
            byte[] decryptedBytes = sourceBytes;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (j < 5)
                    {
                        decryptedBytes[4 + i * 8 + j] = (byte)(~decryptedBytes[4 + i * 8 + j]);
                    }
                }
            }

            return decryptedBytes;
        }
    }
}
