using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class AutorizacionBs
    {

      #region Autorizacion: Metodos de la Clase de Negocios de la entidad Autorizacion.


            /// ***********************************************************************************************
            /// <summary>
            /// Devuelve la lista de todas las Autorizacion definidas. 
            /// </summary>
            /// <param name="bGST">Int - Permite filtrar por una Autorizacion determinada
            /// <returns>Lista de Autorizacion</returns>
            /// ***********************************************************************************************
            public static AutorizacionL getAutorizaciones()
            {
                return getAutorizaciones(ConexionBs.getGSToEstacion());
            }


            /// ***********************************************************************************************
            /// <summary>
            /// Devuelve la lista de Autorizacion definidas
            /// </summary>
            /// <param name="bGST">Int - Permite filtrar por una Autorizacion determinada
            /// <returns>Lista de Autorizacion</returns>
            /// ***********************************************************************************************
            public static AutorizacionL getAutorizaciones(bool bGST)
            {
                try
                {
                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        conn.ConectarGSTPlaza(bGST, false);

                        return AutorizacionDt.getASRI(conn);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// ***********************************************************************************************
            /// <summary>
            /// Devuelve la lista de Autorizacion definidas
            /// </summary>
            /// <param name="bGST">Int - Permite filtrar por una Autorizacion determinada
            /// <returns>Lista de Autorizacion</returns>
            /// ***********************************************************************************************
            public static Autorizacion getAutorizacion(bool bGST, DateTime? fecha, out bool PudoGST)
            {

                try
                {
                    PudoGST = false;

                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        if (bGST)
                            PudoGST = conn.ConectarGSTThenPlaza();
                        else
                            conn.ConectarPlaza(false);

                        return AutorizacionDt.getUnASRI(conn, fecha);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //  return getAutorizacion(ConexionBs.getGSToEstacion(), fecha, null);


            }

          
            /// ***********************************************************************************************
            /// <summary>
            /// Devuelve la Autorizacion Vigente. 
            /// </summary>
            /// <returns>Autorizacion Viogente</returns>
            /// ***********************************************************************************************
            public static Autorizacion getAutorizacionVigente()
            {
                //Dmitriy: 14/04/2011
                Autorizacion oVigente = null;
                AutorizacionL oAuts = getAutorizaciones(ConexionBs.getGSToEstacion());
                foreach (Autorizacion item in oAuts)
                {
                    if (item.FechaProgramacion <= DateTime.Now)
                    {
                        oVigente = item;
                        break;          //Estan en orden descendente la primera es la valida
                    }

                }

                return oVigente;
            }
            /// ***********************************************************************************************
            /// <summary>
            /// Baja de una Autorizacion sri
            /// </summary>
            /// <param name="oAutorizacion">Autorizacion - Objeto Autorizacion
            /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
            /// <returns></returns>
            /// ***********************************************************************************************
            public static void delAutorizacion(Autorizacion oAutorizacion, bool nocheck)
            {
                try
                {
                    //iniciamos una transaccion
                    using (Conexion conn = new Conexion())
                    {
                        //No puede haber empezado
                        if (oAutorizacion.FechaProgramacion < DateTime.Now)
                            throw new ArgumentException("La autorización ya empezó");

                        //Siempre en Gestion, con transaccion
                        conn.ConectarGST(true);

                        //Verificar que no haya registros con FK a este
                        MantenimientoBS.checkReferenciasFK(conn, "AUTSRI",
                                                           new string[] { oAutorizacion.FechaProgramacion.ToString() },
                                                           new string[] { },
                                                           nocheck);

                        //eliminamos la oAutorizacion
                        AutorizacionDt.delASRI(oAutorizacion, conn);


                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutorizacion(),
                                                               "B",
                                                               getAuditoriaCodigoRegistroAutorizacionSRI(oAutorizacion),
                                                               getAuditoriaDescAutorizacionSRI(oAutorizacion)),
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
            /// Agregar una Autorizacion
            /// </summary>
            /// <param name="bGST">Int - Autorizacion a agregar
            /// <returns></returns>
            /// ***********************************************************************************************
            public static int addAutorizacion(Autorizacion oAutorizacion, bool confirmado)
            {

                int retval = 0; 
                try
                {
                    //iniciamos una transaccion
                    using (Conexion conn = new Conexion())
                    {
                        if (oAutorizacion.FechaProgramacion < DateTime.Now)
                            throw new ArgumentException("La autorización ya empezó");

                        //Siempre en Gestion, con transaccion
                        conn.ConectarGST(true);

                        //Verificamos que cumpla las condiciones
                        // el numero de autorizacion no existe
                        // ninguna termina luego que esta empieza
                        if (!confirmado)
                        {
                            string msg = "";
                            bool OK = true, error = false;
                            AutorizacionL oAutorizacionesAnteriores = AutorizacionDt.getASRI(conn);
                            foreach (Autorizacion item in oAutorizacionesAnteriores)
	                        {
		                        if( item.NumeroAutorizacion == oAutorizacion.NumeroAutorizacion )
                                {
                                    //No lo dejamos hacer, si se equivocan tendran que cargarlo por afuera
                                    msg += Traduccion.Traducir(string.Format("El número de Autorización ya fue usado en la Autorización que empieza el {0}<BR/>", item.FechaInicio.ToShortDateString()));
                                    OK = false;
                                    error = true;
                                }
                                //La nueva se puede ingresar desde 30 dias antes
                                if( item.FechaCaducidad.AddDays(-30) > oAutorizacion.FechaInicio )
                                {
                                    msg += Traduccion.Traducir(string.Format("La fecha de inicio es anterior en más de 30 días a la caducidad de la Autorización que caduca el {0}<BR/>", item.FechaCaducidad.ToShortDateString()));
                                    OK = false;
                                }
	                        }
                            if( error )
                            {
                                throw new Telectronica.Errores.ErrorFacturacionStatus(msg);
                            }
                            if (!OK)
                            {
                                throw new Telectronica.Errores.WarningException(msg);
                            }


                        }                        


                        //Agregamos Autorizacion
                        retval = AutorizacionDt.addSRI(oAutorizacion, conn);

                        
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutorizacion(),
                                                               "M",
                                                               getAuditoriaCodigoRegistroAutorizacionSRI(oAutorizacion),
                                                               getAuditoriaDescAutorizacionSRI(oAutorizacion)),
                                                               conn);


                        //Grabo OK hacemos COMMIT
                        conn.Finalizar(true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return retval;
            }

            /// ***********************************************************************************************
            /// <summary>
            /// Devuelve la lista de todas las Autorizacion definidas. 
            /// </summary>
            /// <param name="bGST">Int - Permite filtrar por una Autorizacion determinada
            /// <returns>Lista de Autorizacion</returns>
            /// ***********************************************************************************************
            public static Autorizacion getAutorizacion(DateTime laFecha)
            {
                return getAutorizacion(ConexionBs.getGSToEstacion(),laFecha);
            }


            /// ***********************************************************************************************
            /// <summary>
            /// Devuelve la lista de Autorizacion definidas
            /// </summary>
            /// <param name="bGST">Int - Permite filtrar por una Autorizacion determinada
            /// <returns>Lista de Autorizacion</returns>
            /// ***********************************************************************************************
            public static Autorizacion getAutorizacion(bool bGST, DateTime laFecha)
            {
                try
                {
                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        conn.ConectarGSTPlaza(bGST, false);

                        return AutorizacionDt.getUnASRI(conn, laFecha);
                        //return AutorizacionDt.getASRI(conn)[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// ***********************************************************************************************
            /// <summary>
            /// Modificacion de una Autorizacion
            /// </summary>
            /// <param name="bGST">Int - Autorizacion a modificar
            /// <returns></returns>
            /// ***********************************************************************************************
            public static int updAutorizacion(Autorizacion oAutorizacion, bool confirmado )
            {
                int retval = 0;
                try
                {
                    //iniciamos una transaccion
                    using (Conexion conn = new Conexion())
                    {
                        //Siempre en Gestion, con transaccion
                        conn.ConectarGST(true);

                        if (oAutorizacion.FechaProgramacion < DateTime.Now)
                            throw new ArgumentException("La autorización ya empezó");

                        //Verificamos que cumpla las condiciones
                        // el numero de autorizacion no existe
                        // ninguna termina luego que esta empieza
                        if (!confirmado)
                        {
                            string msg = "";
                            bool OK = true;
                            AutorizacionL oAutorizacionesAnteriores = AutorizacionDt.getASRI(conn);
                            foreach (Autorizacion item in oAutorizacionesAnteriores)
                            {
                                //No validamos al mismo registro que modificamos
                                if (item.FechaProgramacion != oAutorizacion.FechaProgramacion)
                                {
                                    if (item.NumeroAutorizacion == oAutorizacion.NumeroAutorizacion)
                                    {
                                        msg += Traduccion.Traducir(string.Format("El número de Autorización ya fue usado en la Autorización que empieza el {0}<BR/>", item.FechaInicio.ToShortDateString()));
                                        OK = false;
                                    }
                                    if (item.FechaCaducidad.AddDays(-30) > oAutorizacion.FechaInicio)
                                    {
                                        msg += Traduccion.Traducir(string.Format("La fecha de inicio es anterior a la caducidad de la Autorización que caduca el {0}<BR/>", item.FechaCaducidad.ToShortDateString()));
                                        OK = false;
                                    }
                                }
                            }
                            if (!OK)
                            {
                                throw new Telectronica.Errores.WarningException(msg);
                            }


                        }                        


                        //Modificamos Autorizacion
                        retval = AutorizacionDt.updASRI(oAutorizacion, conn);

                        // Revisamos el retorno 
                        if (retval != 0)
                        {
                            string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                            if (retval == -101)
                                msg = Traduccion.Traducir("El registro ya existe.");

                            //throw new WarningException(msg);
                            throw new Telectronica.Errores.ErrorSPException(msg);

                        }                        

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutorizacion(),
                                                               "M",
                                                               getAuditoriaCodigoRegistroAutorizacionSRI(oAutorizacion),
                                                               getAuditoriaDescAutorizacionSRI(oAutorizacion)),
                                                               conn);


                        //Grabo OK hacemos COMMIT
                        conn.Finalizar(true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return retval;
            }



            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaAutorizacion()
            {
                return "ASR";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistroAutorizacionSRI(Autorizacion oAutorizacion)
            {
                return oAutorizacion.FechaInicio.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescAutorizacionSRI(Autorizacion oAutorizacion)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Número de Autorización ", oAutorizacion.NumeroAutorizacion.ToString());
                AuditoriaBs.AppendCampo(sb, "Fecha de Inicio ", oAutorizacion.FechaInicio.ToString());
                AuditoriaBs.AppendCampo(sb, "Fecha de Caducidad ", oAutorizacion.FechaCaducidad.ToString());
                AuditoriaBs.AppendCampo(sb, "Fecha de Programación ", oAutorizacion.FechaProgramacion.ToString());

                return sb.ToString();
            }

            #endregion


       #endregion


    }
}
