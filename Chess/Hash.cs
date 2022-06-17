using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Hash
    {
        public Hash()
        {

        }
        public string CriptografarSenha(string senha)
        {
            byte[] tmpsenha = ASCIIEncoding.ASCII.GetBytes(senha);
            byte[] tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsenha);


            return ByteArrayToString(tmphash);
        }
        string ByteArrayToString(byte[] tmpHash)
        {
            int i;
            StringBuilder senhafinal = new StringBuilder(tmpHash.Length);
            for (i = 0; i < tmpHash.Length; i++)
            {
                senhafinal.Append(tmpHash[i].ToString("X2"));
            }
            return senhafinal.ToString();
        }
        public bool VerificarSenha(string senhadigitada, string senhabanco)
        {
            //criptografar senha digitada
            byte[] tmpsenha = ASCIIEncoding.ASCII.GetBytes(senhadigitada);
            byte[] tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsenha);

            bool eVerifica = false;
            string senhanova = ByteArrayToString(tmphash);

            if (senhanova.Length == senhabanco.Length)
            {
                if (senhanova == senhabanco)
                {
                    eVerifica = true;
                }

            }

            return eVerifica;
        }
    }
}
