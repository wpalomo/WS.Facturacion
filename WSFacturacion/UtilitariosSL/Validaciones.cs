using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Telectronica.Utilitarios.SL
{
    public class Validaciones
    {

        #region CUIT

        /// <summary>
        /// Calcula el dígito verificador dado un CUIT completo o sin él.
        /// </summary>
        /// <param name="cuit">El CUIT como String sin guiones</param>
        /// <returns>El valor del dígito verificador calculado.</returns>
        public static int CalcularDigitoCuit(string cuit)
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuit.ToCharArray();
            int total = 0;

            for (int i = 0; i < mult.Length; i++)
                total += int.Parse(nums[i].ToString()) * mult[i];

            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        /// <summary>
        /// Valida el CUIT ingresado.
        /// </summary>
        /// <param name="cuit" />Número de CUIT como string con o sin guiones
        /// <returns>True si el CUIT es válido y False si no.</returns>
        public static bool ValidaCuit(string cuit)
        {
            if (cuit == null)
                return false;

            //Quito los guiones, el cuit resultante debe tener 11 caracteres.
            cuit = cuit.Replace("-", string.Empty);

            if (cuit.Length != 11)
                return false;
            else
            {
                int calculado = CalcularDigitoCuit(cuit);
                int digito = int.Parse(cuit.Substring(10));
                return calculado == digito;
            }
        }

        #endregion

        #region Numeros: Validaciones con numeros

        public static bool IsNumeric(object Expression)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Valida si el objeto recibido es un número
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 11/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            bool isNum;
            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        /// <summary>
        /// Funcion a llamar en el KeyDown de un textbox para que solo acepte teclas numericas
        /// </summary>
        /// <param name="e">KeyEventArgs - parametro del evento</param>
        public static void SoloNumeroEnteroPositivo(KeyEventArgs e)
        {
            if (!(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) &&
                !(e.Key >= Key.D0 && e.Key <= Key.D9) &&
                e.Key != Key.Back && e.Key != Key.Tab)
            {
                e.Handled = true;
                return;
            }



        }




        #endregion

        #region Fechas: Validaciones con fechas

        public static bool IsDate(object Expression)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Valida si el objeto recibido es una fecha
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 11/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            try
            {
                Convert.ToDateTime(Expression);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsTime(int xiHora, int xiMinuto, int xiSegundo)
        {
            if (xiHora < 0 || xiHora > 23) { return false; }
            if (xiMinuto < 0 || xiMinuto > 59) { return false; }
            if (xiSegundo < 0 || xiSegundo > 59) { return false; }

            return true;
        }

        /// <summary>
        /// Funcion a llamar en el KeyDown de un textbox para que solo acepte horas y minutos
        /// </summary>
        /// <param name="e">KeyEventArgs - parametro del evento</param>
        public static void SoloHorasMinutos(KeyEventArgs e)
        {
            if (!(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) && 
                    !(e.Key >= Key.D0 && e.Key <= Key.D9 ) &&
                    e.Key != Key.Back    && e.Key != Key.Unknown && e.Key != Key.Tab )

            {
                e.Handled = true;
                return;
            }
    

            System.Windows.Controls.TextBox txtControl = (System.Windows.Controls.TextBox) e.OriginalSource;
            // Presiono la tecla de dos puntos (":")
            // Analizo si ya presiono o no la tecla
            if (e.Key == Key.Unknown)
            {
                // Estamos ante el tipo double, y presiono el ".", analizo que no lo haya presionado antes
                if (txtControl.Text.Contains(":"))
                {
                    e.Handled = true;
                    return;
                }

                // No puede empezar con dos puntos
                if (txtControl.Text.Length == 0)
                {
                    e.Handled = true;
                    return;
                }

            }
        }
        #endregion
    }
}
