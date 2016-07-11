using System;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class MantenimientoBS
    {
        #region CONFIGURACION CONTRASEÑA: Metodos de la Clase de Negocios de la entidad Configuracion de Contraseña.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Configuracion de Contraseñas definidas. 
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configuracion de Contraseña determinada
        /// <returns>Lista de  Configuracion de Contraseñas</returns>
        /// ***********************************************************************************************
        public static PasswordConfiguracion getConfiguracionContraseñas()
        {
            return getConfiguracionContraseñas(ConexionBs.getGSToEstacion());
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Contraseñas definidas
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configuracion de Contraseñas determinada
        /// <returns>Lista de Configuracion de Contraseñas</returns>
        /// ***********************************************************************************************
        public static PasswordConfiguracion getConfiguracionContraseñas(bool bGST)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return MantenimientoDT.getConfiguracionContraseña(conn);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Configuracion de Contraseñas
        /// </summary>
        /// <param name="oConfiguracionContraseña">PasswordConfiguracion - Estructura de la Configuracion de Contraseña para modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfiguracionContraseña(PasswordConfiguracion oConfiguracionContraseña)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Configuracion de Contraseña

                //Modificamos ConfiguracionContraseña
                MantenimientoDT.updConfiguracionContraseña(oConfiguracionContraseña, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfigContraseña(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oConfiguracionContraseña),
                                                        getAuditoriaDescripcion(oConfiguracionContraseña)),
                                                        conn);
                    
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaConfigContraseña()
        {
            return "PWD";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(PasswordConfiguracion oConfiguracionContraseña)
        {
            return oConfiguracionContraseña.IdContraseña.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(PasswordConfiguracion oConfiguracionContraseña)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Vencimiento (dias)", oConfiguracionContraseña.Vencimiento.ToString());
            AuditoriaBs.AppendCampo(sb, "Control de repetición", oConfiguracionContraseña.ControlRepeticion.ToString());
            AuditoriaBs.AppendCampo(sb, "Largo mínimo", oConfiguracionContraseña.LargoMinimo.ToString());
            AuditoriaBs.AppendCampo(sb, "Caracteres repetidos", oConfiguracionContraseña.CaracteresRepetidos.ToString());

            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region CONFIGURACION LISTADO: Metodos de la Clase de Negocios de la entidad Configuracion de Listado.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Configuracion de Listados. 
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configuracion de Listados determinada
        /// <returns>Lista de  Configuracion de Listados</returns>
        /// ***********************************************************************************************
        public static ListaActualizacionL getConfiguracionListados()
        {
            return getConfiguracionListados(null, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Listados
        /// </summary>
        /// <param name="CodListado">string - Permite filtrar por una Configuracion de Listados
        /// <returns>Lista de Configuracion de Listados</returns>
        /// ***********************************************************************************************
        public static ListaActualizacionL getConfiguracionListados(string CodListado, string Nivel)
        {
            return getConfiguracionListados(ConexionBs.getGSToEstacion(), CodListado, Nivel);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Listados
        /// </summary>
        /// <param name="CodListado">string - Permite filtrar por una Configuracion de Listados
        /// <returns>Lista de Configuracion de Listados</returns>
        /// ***********************************************************************************************
        public static ListaActualizacionL getConfiguracionListados(bool bGST, string CodListado, string Nivel)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return MantenimientoDT.getConfiguracionListado(conn, CodListado, Nivel);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Configuracion de Listados
        /// </summary>
        /// <param name="oConfiguracionListados">ListaActualizacion - Estructura de la Configuracion de Listados para modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfiguracionListados(ListaActualizacion oConfiguracionListados)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Configuracion de Listados

                //Modificamos ConfiguracionListado
                MantenimientoDT.updConfiguracionListas(oConfiguracionListados, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfigListados(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oConfiguracionListados),
                                                        getAuditoriaDescripcion(oConfiguracionListados)),
                                                        conn);
                    
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaConfigListados()
        {
            return "LIS";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ListaActualizacion oConfiguracionListados)
        {
            return oConfiguracionListados.TipoLista.Codigo;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ListaActualizacion oConfiguracionListados)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Tipo de lista", oConfiguracionListados.TipoLista.Codigo);
            AuditoriaBs.AppendCampo(sb, "Modo", oConfiguracionListados.Modo.Codigo);
            if (oConfiguracionListados.Modo.Codigo == "H")
            {
                AuditoriaBs.AppendCampo(sb, "Frecuencia horaria (min)", oConfiguracionListados.FrecuenciaHoraria.ToString());
            }
            else
            {
                AuditoriaBs.AppendCampo(sb, "Horario de consulta", oConfiguracionListados.HorariodeConsulta);
            }
            AuditoriaBs.AppendCampo(sb, "Diferencia entre vías (seg)", oConfiguracionListados.DiferenciaEntreVias.ToString());
            AuditoriaBs.AppendCampo(sb, "Reintento (min)", oConfiguracionListados.Reintento.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

        #region MODO: Metodos de la Clase de Negocios de la entidad ListaActualizacion.

        /// <summary>
        /// Devuelve la lista de todos los Modos
        /// </summary>
        /// <returns>Lista de tipos de Modos</returns>
        /// ***********************************************************************************************
        public static ListaActualizacionModoL getModos()
        {
            ListaActualizacionModoL oTipoModoL = new ListaActualizacionModoL();
            oTipoModoL = MantenimientoDT.getModos();

            return oTipoModoL;
        }

        #endregion

        #region ReferenciaFK

        //TODO recibir una lista adicional de tablas y campos referenciados (en Consolidado)

        /// ***********************************************************************************************
        /// <summary>
        /// Chequea que un registro a dar de baja no este referenciado a traves de un FK
        /// Si nochequear es true se saltea el chequeo
        /// Si nochequear es false y encuentra una referencia genera una excepcion
        /// Si la referencia es de una tabla no excluyente genera un warning 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="tabla">string - Tabla de la que se quiere eliminar el registro (incluir nombre de Schema)</param>
        /// <param name="claves">string[] - valores de la PK del registro que se quiere eliminar</param>
        /// <param name="excluirTablas">string[] - Tablas con FK que igual permiten la baja</param>
        /// <param name="nochequear">bool - true cuando ya se confirmó</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void checkReferenciasFK(Conexion conn, string tabla, string[] claves, string[] excluirTablas, bool nochequear)
        {
            checkReferenciasFK(conn, tabla, claves, excluirTablas, nochequear, "<BR/>");
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Chequea que un registro a dar de baja no este referenciado a traves de un FK
        /// Si nochequear es true se saltea el chequeo
        /// Si nochequear es false y encuentra una referencia genera una excepcion
        /// Si la referencia es de una tabla no excluyente genera un warning 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="tabla">string - Tabla de la que se quiere eliminar el registro (incluir nombre de Schema)</param>
        /// <param name="claves">string[] - valores de la PK del registro que se quiere eliminar</param>
        /// <param name="excluirTablas">string[] - Tablas con FK que igual permiten la baja</param>
        /// <param name="nochequear">bool - true cuando ya se confirmó</param>
        /// <param name="NewLine">string - caracteres usados para separar lineas</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void checkReferenciasFK(Conexion conn, string tabla, string[] claves, string[] excluirTablas, bool nochequear, string NewLine)
        {
            if (!nochequear)
            {
                ReferenciaFKL oRFKs = MantenimientoDT.getReferenciasFK(conn, tabla, claves, excluirTablas);

                if (oRFKs.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("");
                    sb.Append(Traduccion.Traducir("El registro está referenciado en las tablas"));
                    sb.Append(NewLine);
                    sb.Append(NewLine);

                    bool bHay = false;
                    //Revisamos por tablas excluyentes
                    foreach (ReferenciaFK item in oRFKs)
                    {
                        if (item.Excluyente)
                        {
                            bHay = true;

                            if (item.CantidadCampos == 1)
                                sb.Append(string.Format(Traduccion.Traducir("Tabla: {0}, Campo: {1}, usado en {2} registros"), item.Tabla, item.Campos[0], item.CantidadRegistros));
                            else if (item.CantidadCampos == 2)
                                sb.Append(string.Format(Traduccion.Traducir("Tabla: {0}, Campos: {1} {2}, usado en {3} registros"), item.Tabla, item.Campos[0], item.Campos[1], item.CantidadRegistros));
                            else if (item.CantidadCampos == 3)
                                sb.Append(string.Format(Traduccion.Traducir("Tabla: {0}, Campos: {1} {2} {3}, usado en {4} registros"), item.Tabla, item.Campos[0], item.Campos[1], item.Campos[2], item.CantidadRegistros));
                            sb.Append(NewLine);

                        }
                    }

                    if (bHay)
                    {
                        throw new Telectronica.Errores.ErrorFKException(sb.ToString());
                    }
                    else
                    {
                        //Revisamos por tablas no excluyentes
                        foreach (ReferenciaFK item in oRFKs)
                        {
                            if (!item.Excluyente)
                            {
                                bHay = true;

                                if (item.CantidadCampos == 1)
                                    sb.Append(string.Format(Traduccion.Traducir("Tabla: {0}, Campo: {1}, usado en {2} registros"), item.Tabla, item.Campos[0], item.CantidadRegistros));
                                else if (item.CantidadCampos == 2)
                                    sb.Append(string.Format(Traduccion.Traducir("Tabla: {0}, Campos: {1} {2}, usado en {3} registros"), item.Tabla, item.Campos[0], item.Campos[1], item.CantidadRegistros));
                                else if (item.CantidadCampos == 3)
                                    sb.Append(string.Format(Traduccion.Traducir("Tabla: {0}, Campos: {1} {2} {3}, usado en {4} registros"), item.Tabla, item.Campos[0], item.Campos[1], item.Campos[2], item.CantidadRegistros));
                                sb.Append(NewLine);

                            }
                        }
                        if (bHay)
                        {
                            throw new Telectronica.Errores.WarningFKException(sb.ToString());
                        }
                    }
                }

            }
        }

        #endregion

        #region Otros

        /// <summary>
        /// Devuelve la fecha y hora del servidor
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeServer()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarPlaza(false);
                return MantenimientoDT.GetDateTimeServer(conn);
            }
        }

        #endregion
    }
}
