using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PagSeguro
{
    public static class sValidar
    {
        // Validar conexão com a internet
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);        
        public static bool IsConnected()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
        // Validar e-mail
        public static bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, ("(?<user>[^@]+)@(?<host>.+)"));
        }
        // Valida texto como decimal
        public static bool ValidarDecimal(string valor = "")
        {
            bool retorno = true;
            try
            {
                var tmp = Convert.ToDecimal(valor);
            }
            catch
            {
                retorno = false;
            }
            return retorno;
        }
        // Validar campo texto com números, virgula e traço
        public static bool AjustarValorMonetario(TextBox obj, KeyPressEventArgs e, bool negativo = false)
        {
            var ret = false;
            // Já foi digitado a telca ,
            if (obj.Text.Contains(",") && e.KeyChar == (char)44)
                ret = true;
            // Já foi digitado a tecla -
            else if (obj.Text.Contains("-") && e.KeyChar == (char)45 && negativo)
                ret = true;
            else
            {
                if (negativo)
                    // Caso for validar um valor negativo
                    ret = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',' &&
                                                                                      e.KeyChar != '-'
                        ? true
                        : false
                        );
                else
                    // Caso for validar um valor positivo
                    ret = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','
                        ? true
                        : false
                        );
            }
            return ret;
        }
        // Retornar valor em formato monetáio
        public static string ValidarMoeda(string valor = "", bool casadecimal = true)
        {
            try
            {
                // Verifica se o valor passado é um valor numérico
                if (!ValidarDecimal(valor))
                    return casadecimal ? "0,00" : "0";
                // Se tiver mais de um sinal negativo, remove do valor
                if (valor.Contains("-") && valor.IndexOf("-") > 0)
                    valor = valor.Replace("-", "");
                // Se não for um valor válido, retorna 0
                if (valor.Equals("") || valor.Equals("-,") || valor.Equals(",-")) valor = casadecimal ? "0,00" : "0";
                if (!valor.Equals("") && Convert.ToDecimal(valor).Equals(0)) return casadecimal ? "0,00" : "0";
                // Retornar o valor validado
                return valor.Equals("")
                    ? casadecimal ? "0,00" : "0"
                    : Math.Round(Convert.ToDecimal(valor), 2).ToString("N" + (casadecimal ? "2" : "0"));
            }
            catch
            {
                return "0,00";
            }
        }
    }
}
