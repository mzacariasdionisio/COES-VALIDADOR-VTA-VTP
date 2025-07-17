using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Tools
{
    public class Encriptacion
    {
        /// <summary>
        /// Permite encriptar una cadena
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string Encripta(string texto)
        {
            string vector = "Aig1qck7zbPFVAz7XgZ0Zg==";
            string key = "RBgH0sBP/p70B5CGXo0JRxilDiwelxaKc+obUHGLZWk=";
            byte[] by_iv;
            byte[] by_key;

            by_iv = Convert.FromBase64String(vector);
            by_key = Convert.FromBase64String(key);

            return Seguridad.Criptografia.Rijndaelx.EncriptarStringtoBase64(texto, by_key, by_iv);
        }

        /// <summary>
        /// Permite desencriptar el texto encriptado
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string Desencripta(string texto)
        {
            string vector = "Aig1qck7zbPFVAz7XgZ0Zg==";
            string key = "RBgH0sBP/p70B5CGXo0JRxilDiwelxaKc+obUHGLZWk=";
            byte[] by_iv;
            byte[] by_key;

            by_iv = Convert.FromBase64String(vector);
            by_key = Convert.FromBase64String(key);

            return Seguridad.Criptografia.Rijndaelx.DesencriptaStringBase64toString(texto, by_key, by_iv);
        }
    }
}
