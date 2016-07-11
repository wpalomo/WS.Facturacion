using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class FormatoTicketBs
    {

        #region FORMATO TICKET: Metodos de negocios de la Clase FormatoTicket.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de Tickets cargados
        /// base de datos.
        /// </summary>
        /// <returns>Tipos de tickets</returns>
        /// ***********************************************************************************************
        public static TipoTicketL getTiposDeTicket()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return FormatoTicketDt.getTiposDeTicket(conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formatos de tickets
        /// metodo pero con valor NULL para que retorne todos los registros
        /// </summary>
        /// <returns>Lista de Tarifas Diferenciadas</returns>
        /// ***********************************************************************************************
        public static FormatoTicketL getFormatosTicket(int? codFrmTicket)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return FormatoTicketDt.getFormatosTicket(conn, codFrmTicket);
            }
        }

        public static FormatoTicketL getFormatosTicket()
        {
            return getFormatosTicket(null);
        }


        public static DataSet getRptFormatoTicket()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return FormatoTicketDt.getRptFormatoTicket(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de los Formatos de Tickets. Si el tipo de ticket no tiene formato lo inserta.
        /// </summary>
        /// <param name="oFormatoTicket">Formato de Ticket
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updFormatoTicket(FormatoTicket oFormatoTicket)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //No tocamos la cabecera del cambio de tarifa
                    FormatoTicketDt.updFormatoTicket(oFormatoTicket, conn);

                    //Grabamos auditoria de la cabecera (para que no confundan tantos datos repetidos)
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaFormatoTicket(),
                                                           "M",
                                                           getAuditoriaCodigoRegistroFRM(oFormatoTicket),
                                                           getAuditoriaDescripcionFRM(oFormatoTicket)),
                                                           conn);




                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un string con las variables posibles que se pueden utilizar en el ticket configurable
        /// </summary>
        /// ***********************************************************************************************
        public static string getVariablesTickets()
        {
            try
            {
                string sVariables = "";


                sVariables = sVariables + "--- DATOS GENERALES ---" + "\n";       //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "~CONCESIONARIO~" + "\n";               //          - " + Traduccion.Traducir("Nombre de la Concesionaria") + "\n";
                sVariables = sVariables + "~DIRECCIONSUCRUSAL~" + "\n";           //          - " + Traduccion.Traducir("Dirección de la Sucursal") + "\n";
                sVariables = sVariables + "~NUMESTACION~" + "\n";                 //          - " + Traduccion.Traducir("Número de Estación") + "\n";
                sVariables = sVariables + "~NOMBREESTACION~" + "\n";              //          - " + Traduccion.Traducir("Nombre de la Estación") + "\n";
                sVariables = sVariables + "~VIA~" + "\n";                         //          - " + Traduccion.Traducir("Número de Vía") + "\n";
                sVariables = sVariables + "~VIANOMBRE~" + "\n";                   //          - " + Traduccion.Traducir("Nombre de la Vía") + "\n";
                sVariables = sVariables + "~MODELOVIA~" + "\n";                   //          - " + Traduccion.Traducir("Modelo de la Vía") + "\n";
                sVariables = sVariables + "~HASH~" + "\n";                        //          - " + Traduccion.Traducir("Modelo de la Vía") + "\n";

                sVariables = sVariables + "" + "\n";                              //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "--- DATOS TURNO ---" + "\n";           //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "~CAJERO~" + "\n";                      //          - " + Traduccion.Traducir("Nombre del Cajero") + "\n";
                sVariables = sVariables + "~IDCAJERO~" + "\n";                    //          - " + Traduccion.Traducir("ID del Cajero") + "\n";
                sVariables = sVariables + "~PARTE~" + "\n";                       //          - " + Traduccion.Traducir("Número de Parte del Turno") + "\n";
                sVariables = sVariables + "~JORNADA~" + "\n";                     //          - " + Traduccion.Traducir("Jornada Contable") + "\n";
                sVariables = sVariables + "~BLOQUE~" + "\n";                      //          - " + Traduccion.Traducir("Número de Bloque / Turno / Mini-turno") + "\n";
                sVariables = sVariables + "~APERTURA~" + "\n";                    //          - " + Traduccion.Traducir("Fecha y Hora de Apertura del Turno") + "\n";

                sVariables = sVariables + "" + "\n";                              //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "--- DATOS BOLSA ---" + "\n";           //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "~BOLSA~" + "\n";                       //          - " + Traduccion.Traducir("Número de Bolsa ingresado/generado") + "\n";
                sVariables = sVariables + "~PRECINTO~" + "\n";                    //          - " + Traduccion.Traducir("Número de Precinto ingresado") + "\n";
                sVariables = sVariables + "~MALOTE~" + "\n";                      //          - " + Traduccion.Traducir("Idem Bolsa") + "\n";

                sVariables = sVariables + "" + "\n";                              //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "--- DATOS TRANSITO ---" + "\n";        //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "~FECHA~" + "\n";                       //          - " + Traduccion.Traducir("Fecha del Tránsito") + "\n";
                sVariables = sVariables + "~FECHAAMD~" + "\n";                    //          - " + Traduccion.Traducir("Fecha del Tránsito con formato") + "\n";
                sVariables = sVariables + "~HORA~" + "\n";                        //          - " + Traduccion.Traducir("Hora del Tránsito") + "\n";
                sVariables = sVariables + "~CATEGO~" + "\n";                      //          - " + Traduccion.Traducir("Número de Categoría") + "\n";
                sVariables = sVariables + "~CATEGODESC~" + "\n";                  //          - " + Traduccion.Traducir("Descripción de la Categoría") + "\n";
                sVariables = sVariables + "~CATEGODESCLARGA~" + "\n";             //          - " + Traduccion.Traducir("Descripción Larga de la Categoría") + "\n";
                sVariables = sVariables + "~EJESADIC~" + "\n";                    //          - " + Traduccion.Traducir("Cantidad de Ejes Adicionales") + "\n";
                sVariables = sVariables + "~TARIFA~" + "\n";                      //          - " + Traduccion.Traducir("Valor de la Tarifa cobrada") + "\n";
                sVariables = sVariables + "~IMPORTE~" + "\n";                     //          - " + Traduccion.Traducir("Valor de la Tarifa cobrada (mismo formato??)") + "\n";
                sVariables = sVariables + "~NETO~" + "\n";                        //          - " + Traduccion.Traducir("Valor Neto de la Tarifa cobrada, sin IVA") + "\n";
                sVariables = sVariables + "~IVA~" + "\n";                         //          - " + Traduccion.Traducir("Valor de IVA de la Tarifa cobrada") + "\n";
                sVariables = sVariables + "~PORCIVA~" + "\n";                     //          - " + Traduccion.Traducir("Porcentaje de IVA de la Tarifa cobrada") + "\n";
                sVariables = sVariables + "~FORMAPAGO~" + "\n";                   //          - " + Traduccion.Traducir("Forma de Pago utilizada") + "\n";
                sVariables = sVariables + "~NROTRANSITO~" + "\n";                 //          - " + Traduccion.Traducir("Número de Tránsito generado") + "\n";
                sVariables = sVariables + "~PUNTOVENTA~" + "\n";                  //          - " + Traduccion.Traducir("Número de Punto de Venta asignado a la vía") + "\n";
                sVariables = sVariables + "~TICKET~" + "\n";                      //          - " + Traduccion.Traducir("Número de Ticket Fiscal generado en el Tránsito") + "\n";
                sVariables = sVariables + "~TICKETNF~" + "\n";                    //          - " + Traduccion.Traducir("Número de Ticket NO Fiscal generado en el Tránsito") + "\n";
                sVariables = sVariables + "~TIPOCOMPROBANTE~" + "\n";             //          - " + Traduccion.Traducir("Tipo de Comprobante utilizado (Boleta, Factura)") + "\n";
                sVariables = sVariables + "~NRODEUDA~" + "\n";                    //          - " + Traduccion.Traducir("Número de Pago Diferido registrado o pagado?") + "\n";
                sVariables = sVariables + "~RUC~" + "\n";                         //          - " + Traduccion.Traducir("Número de RUC del usuario pagante") + "\n";
                sVariables = sVariables + "~NUMCLIENTE~" + "\n";                  //          - " + Traduccion.Traducir("Número de Cliente en el sistema") + "\n";
                sVariables = sVariables + "~RAZONSOCIAL~" + "\n";                 //          - " + Traduccion.Traducir("Razón Social del Cliente") + "\n";
                sVariables = sVariables + "~DIRECCION~" + "\n";                   //          - " + Traduccion.Traducir("Dirección registrada del cliente ??") + "\n";
                sVariables = sVariables + "~TELEFONO~" + "\n";                    //          - " + Traduccion.Traducir("Teléfono registrado del cliente ??") + "\n";
                sVariables = sVariables + "~CODIGOBARRAS~" + "\n";                //          - " + Traduccion.Traducir("Código de Barras generado (pago diferido?? algo mas ??)") + "\n";
                sVariables = sVariables + "~LISTA-VIOLACIONES~" + "\n";           //          - " + Traduccion.Traducir("Lista de Violaciones del usuario)") + "\n";
                sVariables = sVariables + "~LISTA-PAGOSDIFERIDOS~" + "\n";        //          - " + Traduccion.Traducir("Lista de Pagos Diferidos del usuario)") + "\n";

                sVariables = sVariables + "" + "\n";                              //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "---- TAG / CHIP ----" + "\n";          //          - " + Traduccion.Traducir("") + "\n";
                sVariables = sVariables + "~PLACA~" + "\n";                       //          - " + Traduccion.Traducir("Placa registrada en el Tránsito") + "\n";
                sVariables = sVariables + "~TAG~" + "\n";                         //          - " + Traduccion.Traducir("Número de Tag utilizado en el Tránsito") + "\n";
                sVariables = sVariables + "~TARIFAPASO~" + "\n";                  //          - " + Traduccion.Traducir("Tarifa de Paso de la Recarga") + "\n";
                sVariables = sVariables + "~SALDOINICIAL~" + "\n";                //          - " + Traduccion.Traducir("Saldo Inicial del medio, antes de la Recarga") + "\n";
                sVariables = sVariables + "~SALDOFINAL~" + "\n";                  //          - " + Traduccion.Traducir("Saldo Final del medio luego de la Recarga") + "\n";




                return sVariables;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaFormatoTicket()
            {
                return "FTC";
            }
            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistroFRM(FormatoTicket oFormatoTicket)
            {
                return oFormatoTicket.TipoTicket.Codigo.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(FormatoTicket oFormatoTicket)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Tipo de Ticket", oFormatoTicket.TipoTicket.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Cuerpo del Ticket", oFormatoTicket.CuerpoTicket);
                AuditoriaBs.AppendCampo(sb, "Copias", oFormatoTicket.NumeroCopias.ToString("F02"));

                
                return sb.ToString();
            }
            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion de la cabecera del registro afectado 
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcionFRM(FormatoTicket oFormatoTicket)
            {
                StringBuilder sb = new StringBuilder();

                // Registro de auditoria
                AuditoriaBs.AppendCampo(sb, "Tipo de Ticket", oFormatoTicket.TipoTicket.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Cuerpo del Ticket", oFormatoTicket.CuerpoTicket);
                AuditoriaBs.AppendCampo(sb, "Copias", oFormatoTicket.NumeroCopias.ToString("F02"));


                return sb.ToString();
            }

            #endregion


    }
}


