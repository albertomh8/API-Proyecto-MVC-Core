using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ApiHospital_Alberto.Helpers
{
    public class HelperCifrado
    {
        public static string CifrarPassword(string texto, string salt)
        {
            string contenido = texto + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenido);
            for (int i = 1; i <= 20; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            return HttpUtility.UrlEncode(Convert.ToBase64String(salida));
        }
        public static string GenerarSalt()
        {
            Random rnd = new Random();
            string salt = "";
            for (int i = 1; i <= 30; i++)
            {
                int aleatorio = rnd.Next(1, 255);
                char letra = Convert.ToChar(aleatorio);
                salt += letra;
            }
            return salt;
        }
        public static bool CompararBytes(string array1, string array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            bool iguales = true;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Equals(array2[i]) == false)
                {
                    iguales = false;
                    break;
                }
            }
            return iguales;
        }
    }
}
