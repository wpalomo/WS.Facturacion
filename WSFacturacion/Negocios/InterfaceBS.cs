using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.OleDb;
using Telectronica.Errores;
using Telectronica.Validacion;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class InterfaceBS
    {
        #region INTERFACE

        /// <summary>
        /// Importa la lista RUTA
        /// </summary>
        /// <param name="PathCertificados">Ruta del archivo de certificados que se va a importar</param>
        /// <param name="PathTarjetas">Ruta del archivo de tarjetas que se va a importar</param>
        public static void ImportarListaRUTA(string PathCertificados, string PathTarjetas, string PathMensajes)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //Conectamos a la DB correspondiente_
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    // Creamos el objeto que vamos a grabar:
                    ListaTarjetasRUTA oListaTarjetasRUTA = new ListaTarjetasRUTA();

                    // Obtenemos la fecha de modificacion del archivo de certificados:
                    DateTime FechaArchivoCertificados = new DateTime();
                    FileInfo fiCert = new FileInfo(PathCertificados);
                    FechaArchivoCertificados = fiCert.LastWriteTime;

                    // Obtenemos la fecha de modificacion del archivo de tarjetas:
                    DateTime FechaArchivoTarjetas = new DateTime();
                    FileInfo fiTarj = new FileInfo(PathTarjetas);
                    FechaArchivoTarjetas = fiTarj.LastWriteTime;

                    // Obtenemos la fecha de modificacion del archivo de mensajes:
                    DateTime FechaArchivoMensajes = new DateTime();
                    FileInfo fiMens = new FileInfo(PathMensajes);
                    FechaArchivoMensajes = fiTarj.LastWriteTime;

                    // Cargamos el objeto que vamos a grabar:
                    oListaTarjetasRUTA.FechaArchivoTarjetas = FechaArchivoTarjetas;
                    oListaTarjetasRUTA.FechaArchivoCertificados = FechaArchivoCertificados;
                    oListaTarjetasRUTA.FechaArchivoMensajes = FechaArchivoMensajes;
                    oListaTarjetasRUTA.FechaOperacion = DateTime.Now;
                    oListaTarjetasRUTA.EstacionOrigen = ConexionBs.getNumeroEstacion();
                    oListaTarjetasRUTA.Usuario = UsuarioBs.getUsuarioLogueado().ID;

                    if (!ConexionBs.getGSToEstacion())
                        oListaTarjetasRUTA.oEstacionOrigen = EstacionBs.getEstacionActual();

                    // Enviamos una sola consulta al servidor y no tantas como seria si cargamos los registros a uno.
                    InterfaceDT.addListaTarjetasRUTA(conn, oListaTarjetasRUTA, PathCertificados, PathTarjetas, PathMensajes);

                    // Ahora tenemos que grabar la auditoria:
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaListaRUTA(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oListaTarjetasRUTA),
                                                           getAuditoriaDescripcion(oListaTarjetasRUTA)),
                                                           conn);
                    conn.Commit();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtenemos la ultima lista ruta
        /// </summary>
        /// <returns></returns>
        public static ListaTarjetasRUTA GetUltimaListaRUTA()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return InterfaceDT.GetUltimaListaRUTA(conn);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exporta el archivo Detalle.txt, tomando la informacion de la quincena seleccionada 
        /// </summary>
        /// <param name="Path">Ruta donde se guardara el archivo Detalle.txt</param>
        /// <param name="Mes">Mes seleccionado</param>
        /// <param name="Quincena">Quincena seleccionada</param>
        /// <param name="Año">Año seleccionado</param>
        public static void ExportarArchivoDetalle(string Path, Int16 Mes, Int16 Quincena, Int16 Año, char SoloRUTA)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    // Nombre del archivo destino:
                    string NombreArchivo = "Detalle.txt";

                    if (SoloRUTA == 'S')
                        NombreArchivo = "Detalle Ruta.txt";

                    //Revisamos si existe el directorio, si no existe lo creamos:
                    if (!System.IO.File.Exists(Path))
                        System.IO.Directory.CreateDirectory(Path);

                    int CantidadRegistros = 0;

                    // Dataset con los registros que vamos a exportar:
                    DataSet DsRegistros = new DataSet();

                    // Conectamos a la DB correspondiente:
                    conn.ConectarConsolidado(true);

                    // Convertimos la quincena en fechas:
                    DateTime JornadaDesde = new DateTime(Año, Mes, (Quincena == 1) ? 1 : 16);
                    DateTime JornadaHasta = new DateTime(Año, Mes, (Quincena == 1) ? 15 : DateTime.DaysInMonth(Año, Mes));

                    // Obtenemos el dataset con la informacion correspondiente
                    DsRegistros = InterfaceDT.ExportarArhivoDetalle(conn, JornadaDesde, JornadaHasta, SoloRUTA);

                    // Abrimos el archivo
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(Path + NombreArchivo);

                    // Creamos el primer registro:
                    StringBuilder SBTexto = new StringBuilder();

                    if (SoloRUTA == 'N')
                    {
                        // Tipo Registro
                        SBTexto.Append("1");

                        // CODIGO DE CONCESIONARIO
                        SBTexto.Append("25");

                        // INICIO PERÍODO
                        SBTexto.Append(JornadaDesde.ToString("yyyyMMdd"));

                        // FIN PERIODO 
                        SBTexto.Append(JornadaHasta.ToString("yyyyMMdd"));

                        // VERSIÓN
                        SBTexto.Append("00V6.0");

                        // FILLER
                        SBTexto.Append(' ', 61);

                        // Anexamos el primer registro al archivo
                        sw.WriteLine(SBTexto.ToString());
                    }

                    // Recorremos el dataset
                    foreach (DataRow item in DsRegistros.Tables[0].Rows)
                    {
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        CantidadRegistros++;

                        // TIPO DE REGISTRO
                        if (SoloRUTA == 'N')
                            SBTexto.Append('2');

                        // CODIGO DE CONCESIONARIO
                        SBTexto.Append(FormatearCampo(item[0], 2));

                        // CÓDIGO DE PUESTO
                        SBTexto.Append(FormatearCampo(item[1], 3));

                        // NÚMERO DE VIA
                        SBTexto.Append(FormatearCampo(item[2], 2));

                        // SENTIDO VIA
                        SBTexto.Append(FormatearCampo(item[3], 1));

                        // TIPO DE VIA
                        SBTexto.Append(FormatearCampo(item[4], 2));

                        // FECHA DE TRANSITO
                        DateTime DtFechaYHora = (DateTime)item[5];
                        SBTexto.Append(DtFechaYHora.ToString("yyyyMMdd"));

                        // HORA DE TRANSITO
                        SBTexto.Append(DtFechaYHora.ToString("HHmmss"));

                        // MODALIDAD  DE COBRO
                        if (SoloRUTA == 'N')
                            SBTexto.Append(FormatearCampo(item[6], 2));

                        // CÓDIGO DE TARJETA 
                        SBTexto.Append(FormatearCampo(item[7], 18));

                        // PATENTE 
                        if (SoloRUTA == 'N')
                            SBTexto.Append(FormatearCampo(item[8], 10));

                        // FECHA DE VENCIMIENTO DE TARJETA 
                        SBTexto.Append(FormatearCampo(null, 8)); // TODO: Devolver el vencimiento de la tarjeta

                        // TIPO TICKET (FISCAL o NO FISCAL)
                        if (SoloRUTA == 'N')
                            SBTexto.Append(FormatearCampo(item[9], 1));

                        // NRO. TICKET 
                        if (SoloRUTA == 'N')
                            SBTexto.Append(FormatearCampo(item[10].ToString().Trim() + item[11].ToString().Trim(), 12));

                        // CATEGORÍA TABULADA
                        if (SoloRUTA == 'N')
                            SBTexto.Append(FormatearCampo(item[12], 1));

                        // CATEGORÍA SEGUN D.A.C.
                        SBTexto.Append(FormatearCampo(item[13], 1));

                        // CATEGORÍA VALIDADA
                        SBTexto.Append(FormatearCampo(item[14], 1));

                        // NUMERO DE EJES (*)
                        if (SoloRUTA == 'N')
                            SBTexto.Append(FormatearCampo(item[15], 1));

                        // CÓDIGO DE TARIFA
                        SBTexto.Append((item[16] != null) ? item[16].ToString() : "");

                        // Anexamos el registro al archivo
                        sw.WriteLine(SBTexto.ToString());
                    }

                    SBTexto.Clear();

                    if (SoloRUTA == 'N')
                    {
                        // Grabamos el pie del archivo:
                        //TIPO DE REGISTRO
                        SBTexto.Append(FormatearCampo("3", 1));

                        //CODIGO DE CONCESIONARIO
                        SBTexto.Append(FormatearCampo("25", 2));

                        // INICIO PERÍODO
                        SBTexto.Append(JornadaDesde.ToString("yyyyMMdd"));

                        // FIN PERIODO 
                        SBTexto.Append(JornadaHasta.ToString("yyyyMMdd"));

                        // TOTAL DE REGISTROS TIPO 2 
                        SBTexto.Append(FormatearCampo(CantidadRegistros, 11));

                        // FILLER
                        SBTexto.Append(' ', 56);

                        // Anexamos el registro al archivo
                        sw.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw.Close();

                    //// Ahora tenemos que grabar la auditoria:
                    //AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaListaTAPI(),
                    //                                       "A",
                    //                                       getAuditoriaCodigoRegistro(oListaTarjetasTAPI),
                    //                                       getAuditoriaDescripcion(oListaTarjetasTAPI)),
                    //                                       conn);

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para exportar los archivos de Excel de la DDJJ
        /// </summary>
        /// <param name="Path">Ruta del directorio donde se alojaran los archivos resultantes</param>
        /// <param name="Mes">Mes de la quincena</param>
        /// <param name="Quincena">Quincena seleccionada para el reporte</param>
        /// <param name="Año">Año de la quincena</param>
        /// <param name="NumeroReporte">Numero de reporte:
        ///                             1- Archivo 1
        ///                             2- Archivo 2
        ///                             3- Archivo 3
        ///                             4- Archivo 4
        ///                             5- Archivo 5
        ///                             6- Archivo 6</param>
        public static void ExportarArchivoDDJJ(string Path, Int16 Mes, Int16 Quincena, Int16 Año)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    // Nombre de los archivos destino:
                    string NombreCuadro1 = "Cuadro1.csv";
                    string NombreCuadro2 = "Cuadro2.csv";
                    string NombreCuadro3 = "Cuadro3.csv";
                    string NombreCuadro4 = "Cuadro4.csv";
                    string NombreCuadro5 = "Cuadro5.csv";
                    string NombreCuadro6 = "Cuadro6.csv";
                    StringBuilder SBTexto = new StringBuilder();

                    // Revisamos si existe el directorio, si no existe lo creamos:
                    if (!System.IO.File.Exists(Path))
                        System.IO.Directory.CreateDirectory(Path);

                    // Dataset con los registros que vamos a exportar:
                    DataSet DsRegistros = new DataSet();

                    // Conectamos a la DB correspondiente:
                    conn.ConectarConsolidado(true);

                    // Convertimos la quincena en fechas:
                    DateTime JornadaDesde = new DateTime(Año, Mes, (Quincena == 1) ? 1 : 16);
                    DateTime JornadaHasta = new DateTime(Año, Mes, (Quincena == 1) ? 15 : DateTime.DaysInMonth(Año, Mes));

                    // Obtenemos el dataset con la informacion correspondiente:
                    DsRegistros = InterfaceDT.CreateTotalDetalleTXT(conn, JornadaDesde, JornadaHasta);
                    InterfaceDT.ExportarCuadro1(conn, DsRegistros);
                    InterfaceDT.ExportarCuadro2(conn, DsRegistros);
                    InterfaceDT.ExportarCuadro3(conn, DsRegistros);
                    InterfaceDT.ExportarCuadro4(conn, DsRegistros);
                    InterfaceDT.ExportarCuadro5(conn, DsRegistros);
                    InterfaceDT.ExportarCuadro6(conn, DsRegistros);

                    //---------------------------------------------------------------------------------
                    // Cuadro 1:
                    // Abrimos / Creamos el archivo:
                    System.IO.StreamWriter sw1 = new System.IO.StreamWriter(Path + NombreCuadro1);

                    // Creamos la primer linea (Cabecera):
                    SBTexto.Clear();

                    // Recorremos las columnas para crear las cabeceras de la grilla:
                    foreach (DataColumn Columna in DsRegistros.Tables["Cuadro1"].Columns)
                        SBTexto.Append(Columna.ColumnName.ToString() + ",");
                    SBTexto.Remove(SBTexto.Length - 1, 1);

                    // Anexamos el registro al archivo:
                    sw1.WriteLine(SBTexto.ToString());

                    // Recorremos el dataset para agregar los datos:
                    foreach (DataRow item in DsRegistros.Tables["Cuadro1"].Rows)
                    {
                        // Limpiamos la variable para crear el nuevo registro:
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        SBTexto.Append(item[0].ToString() + ",");
                        SBTexto.Append(item[1].ToString() + ",");
                        SBTexto.Append(item[2].ToString() + ",");
                        SBTexto.Append(item[3].ToString() + ",");
                        SBTexto.Append(item[4].ToString() + ",");
                        SBTexto.Append(item[5].ToString() + ",");
                        SBTexto.Append(item[6].ToString() + ",");
                        SBTexto.Append(item[7].ToString() + ",");
                        SBTexto.Append(item[8].ToString() + ",");
                        SBTexto.Append(item[9].ToString() + ",");
                        SBTexto.Append(item[10].ToString() + ",");
                        SBTexto.Append(item[11].ToString() + ",");
                        SBTexto.Append(item[12].ToString() + ",");
                        SBTexto.Append(item[13].ToString() + ",");
                        SBTexto.Append(item[14].ToString());

                        // Anexamos el registro al archivo
                        sw1.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw1.Close();
                    //---------------------------------------------------------------------------------

                    //---------------------------------------------------------------------------------
                    // Cuadro 2:
                    // Abrimos / Creamos el archivo:
                    System.IO.StreamWriter sw2 = new System.IO.StreamWriter(Path + NombreCuadro2);

                    // Creamos la primer linea (Cabecera):
                    SBTexto.Clear();

                    // Recorremos las columnas para crear las cabeceras de la grilla:
                    foreach (DataColumn Columna in DsRegistros.Tables["Cuadro2"].Columns)
                        SBTexto.Append(Columna.ColumnName.ToString() + ",");
                    SBTexto.Remove(SBTexto.Length - 1, 1);

                    // Anexamos el registro al archivo:
                    sw2.WriteLine(SBTexto.ToString());

                    // Recorremos el dataset para agregar los datos:
                    foreach (DataRow item in DsRegistros.Tables["Cuadro2"].Rows)
                    {
                        // Limpiamos la variable para crear el nuevo registro:
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        SBTexto.Append(item[0].ToString() + ",");
                        SBTexto.Append(item[1].ToString() + ",");
                        SBTexto.Append(item[2].ToString() + ",");
                        SBTexto.Append(item[3].ToString() + ",");
                        SBTexto.Append(item[4].ToString() + ",");
                        SBTexto.Append(item[5].ToString() + ",");
                        SBTexto.Append(item[6].ToString() + ",");
                        SBTexto.Append(item[7].ToString() + ",");
                        SBTexto.Append(item[8].ToString());

                        // Anexamos el registro al archivo
                        sw2.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw2.Close();
                    //---------------------------------------------------------------------------------

                    //---------------------------------------------------------------------------------
                    // Cuadro 3:
                    // Abrimos / Creamos el archivo:
                    System.IO.StreamWriter sw3 = new System.IO.StreamWriter(Path + NombreCuadro3);

                    // Creamos la primer linea (Cabecera):
                    SBTexto.Clear();

                    // Recorremos las columnas para crear las cabeceras de la grilla:
                    foreach (DataColumn Columna in DsRegistros.Tables["Cuadro3"].Columns)
                        SBTexto.Append(Columna.ColumnName.ToString() + ",");
                    SBTexto.Remove(SBTexto.Length - 1, 1);

                    // Anexamos el registro al archivo:
                    sw3.WriteLine(SBTexto.ToString());

                    // Recorremos el dataset para agregar los datos:
                    foreach (DataRow item in DsRegistros.Tables["Cuadro3"].Rows)
                    {
                        // Limpiamos la variable para crear el nuevo registro:
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        SBTexto.Append(item[0].ToString() + ",");
                        SBTexto.Append(item[1].ToString() + ",");
                        SBTexto.Append(item[2].ToString() + ",");
                        SBTexto.Append(item[3].ToString() + ",");
                        SBTexto.Append(item[4].ToString() + ",");
                        SBTexto.Append(item[5].ToString() + ",");
                        SBTexto.Append(item[6].ToString() + ",");
                        SBTexto.Append(item[7].ToString() + ",");
                        SBTexto.Append(item[8].ToString());

                        // Anexamos el registro al archivo
                        sw3.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw3.Close();
                    //---------------------------------------------------------------------------------

                    //---------------------------------------------------------------------------------
                    // Cuadro 4:
                    // Abrimos / Creamos el archivo:
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(Path + NombreCuadro4);

                    // Creamos la primer linea (Cabecera):
                    SBTexto.Clear();

                    // Recorremos las columnas para crear las cabeceras de la grilla:
                    foreach (DataColumn Columna in DsRegistros.Tables["Cuadro4"].Columns)
                        SBTexto.Append(Columna.ColumnName.ToString() + ",");
                    SBTexto.Remove(SBTexto.Length - 1, 1);

                    // Anexamos el registro al archivo:
                    sw4.WriteLine(SBTexto.ToString());

                    // Recorremos el dataset para agregar los datos:
                    foreach (DataRow item in DsRegistros.Tables["Cuadro4"].Rows)
                    {
                        // Limpiamos la variable para crear el nuevo registro:
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        SBTexto.Append(item[0].ToString() + ",");
                        SBTexto.Append(item[1].ToString() + ",");
                        SBTexto.Append(item[2].ToString() + ",");
                        SBTexto.Append(item[3].ToString() + ",");
                        SBTexto.Append(item[4].ToString() + ",");
                        SBTexto.Append(item[5].ToString() + ",");
                        SBTexto.Append(item[6].ToString() + ",");
                        SBTexto.Append(item[7].ToString());

                        // Anexamos el registro al archivo
                        sw4.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw4.Close();
                    //---------------------------------------------------------------------------------

                    //---------------------------------------------------------------------------------
                    // Cuadro 5:
                    // Abrimos / Creamos el archivo:
                    System.IO.StreamWriter sw5 = new System.IO.StreamWriter(Path + NombreCuadro5);

                    // Creamos la primer linea (Cabecera):
                    SBTexto.Clear();

                    // Recorremos las columnas para crear las cabeceras de la grilla:
                    foreach (DataColumn Columna in DsRegistros.Tables["Cuadro5"].Columns)
                        SBTexto.Append(Columna.ColumnName.ToString() + ",");
                    SBTexto.Remove(SBTexto.Length - 1, 1);

                    // Anexamos el registro al archivo:
                    sw5.WriteLine(SBTexto.ToString());

                    // Recorremos el dataset para agregar los datos:
                    foreach (DataRow item in DsRegistros.Tables["Cuadro5"].Rows)
                    {
                        // Limpiamos la variable para crear el nuevo registro:
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        SBTexto.Append(item[0].ToString() + ",");
                        SBTexto.Append(item[1].ToString() + ",");
                        SBTexto.Append(item[2].ToString() + ",");
                        SBTexto.Append(item[3].ToString() + ",");
                        SBTexto.Append(item[4].ToString() + ",");
                        SBTexto.Append(item[5].ToString() + ",");
                        SBTexto.Append(item[6].ToString() + ",");
                        SBTexto.Append(item[7].ToString());

                        // Anexamos el registro al archivo
                        sw5.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw5.Close();
                    //---------------------------------------------------------------------------------

                    //---------------------------------------------------------------------------------
                    // Cuadro 6:
                    // Abrimos / Creamos el archivo:
                    System.IO.StreamWriter sw6 = new System.IO.StreamWriter(Path + NombreCuadro6);

                    // Creamos la primer linea (Cabecera):
                    SBTexto.Clear();

                    // Recorremos las columnas para crear las cabeceras de la grilla:
                    foreach (DataColumn Columna in DsRegistros.Tables["Cuadro6"].Columns)
                        SBTexto.Append(Columna.ColumnName.ToString() + ",");
                    SBTexto.Remove(SBTexto.Length - 1, 1);

                    // Anexamos el registro al archivo:
                    sw6.WriteLine(SBTexto.ToString());

                    // Recorremos el dataset para agregar los datos:
                    foreach (DataRow item in DsRegistros.Tables["Cuadro6"].Rows)
                    {
                        // Limpiamos la variable para crear el nuevo registro:
                        SBTexto.Clear();

                        // Creamos la linea que se va a grabar:
                        SBTexto.Append(item[0].ToString() + ",");
                        SBTexto.Append(item[1].ToString() + ",");
                        SBTexto.Append(item[2].ToString() + ",");
                        SBTexto.Append(item[3].ToString() + ",");
                        SBTexto.Append(item[4].ToString() + ",");
                        SBTexto.Append(item[5].ToString() + ",");
                        SBTexto.Append(item[6].ToString() + ",");
                        SBTexto.Append(item[7].ToString());

                        // Anexamos el registro al archivo
                        sw6.WriteLine(SBTexto.ToString());
                    }

                    // Cerramos el archivo:
                    sw6.Close();
                    //---------------------------------------------------------------------------------

                    //// Ahora tenemos que grabar la auditoria:
                    //AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaListaTAPI(),
                    //                                       "A",
                    //                                       getAuditoriaCodigoRegistro(oListaTarjetasTAPI),
                    //                                       getAuditoriaDescripcion(oListaTarjetasTAPI)),
                    //                                       conn);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el campo formateado, con una cantidad fija de digitos. Si el largo del dato no llega a la cantidad
        /// Maxima de digitos, entonces se completa con ceros.
        /// </summary>
        /// <param name="Dato">Valor que se va a formatear</param>
        /// <param name="LongitudCampo">Longitud maxima del campo</param>
        /// <returns></returns>
        private static string FormatearCampo(object Dato, int LongitudCampo)
        {
            StringBuilder sbResultado = new StringBuilder();
            if (Dato != null)
                sbResultado.Append(Dato.ToString());

            sbResultado.Insert(0, "0", LongitudCampo - sbResultado.Length);

            return sbResultado.ToString();
        }

        /// <summary>
        /// Importa la lista TAPI
        /// </summary>
        /// <param name="Path">Ruta del archivo que se va a importar</param>
        public static void ImportarListaTAPI(string Path)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //Conectamos a la DB correspondiente_
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    // Creamos el objeto que vamos a grabar:
                    ListaTarjetasTAPI oListaTarjetasTAPI = new ListaTarjetasTAPI();

                    // Obtenemos la fecha de modificacion del archivo:
                    DateTime FechaArchivo = new DateTime();
                    FileInfo fi = new FileInfo(Path);
                    FechaArchivo = fi.LastWriteTime;

                    // Cargamos el objeto que vamos a grabar:
                    oListaTarjetasTAPI.FechaArchivo = FechaArchivo;
                    oListaTarjetasTAPI.FechaOperacion = DateTime.Now;
                    oListaTarjetasTAPI.EstacionOrigen = ConexionBs.getNumeroEstacion();
                    oListaTarjetasTAPI.Usuario = UsuarioBs.getUsuarioLogueado().ID;

                    // Creamos la estructura del XML en memoria:
                    XmlDocument arbol = new XmlDocument();
                    XmlAttribute oAtribPatente;
                    XmlAttribute oAtribFechaVenc;
                    XmlAttribute oAtribRazonSocial;
                    XmlAttribute oAtribNroTarjeta;

                    XmlNode nodo;
                    nodo = arbol.CreateElement("Tarjetas");
                    arbol.AppendChild(nodo);

                    // Leemos el archivo:
                    StreamReader objReader = new StreamReader(Path);
                    string sLine = "";

                    while (sLine != null)
                    {
                        //Leemos una linea del archivo:
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                        {
                            // Separo los atributos:
                            string[] Datos = sLine.Split(',');

                            // Creamos el nodo:
                            nodo = arbol.CreateElement("Tarjeta");

                            // Inicializamos los atributos:
                            oAtribPatente = arbol.CreateAttribute("Patente");
                            oAtribFechaVenc = arbol.CreateAttribute("FechaVencimiento");
                            oAtribRazonSocial = arbol.CreateAttribute("RazonSocial");
                            oAtribNroTarjeta = arbol.CreateAttribute("NroTarjeta");

                            // Cargamos los atributos:
                            oAtribNroTarjeta.InnerText = Datos[0].Trim();
                            oAtribFechaVenc.InnerText = Datos[1].Trim();
                            oAtribRazonSocial.InnerText = Datos[2].Trim();
                            oAtribPatente.InnerText = Datos[3].Trim();

                            // Agregamos los atributos:
                            nodo.Attributes.Append(oAtribPatente);
                            nodo.Attributes.Append(oAtribFechaVenc);
                            nodo.Attributes.Append(oAtribRazonSocial);
                            nodo.Attributes.Append(oAtribNroTarjeta);

                            // Agregamos el nodo:
                            arbol.DocumentElement.AppendChild(nodo);
                        }
                    }

                    // Cerramos el archivo, ya tenemos todos los datos guardados en la variable arbol, en formato xml con atributos.
                    objReader.Close();

                    // Enviamos una sola consulta al servidor y no tantas como seria si cargamos los registros a uno.
                    InterfaceDT.addListaTarjetasTAPI(conn, oListaTarjetasTAPI, arbol);

                    // Ahora tenemos que grabar la auditoria:
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaListaTAPI(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oListaTarjetasTAPI),
                                                           getAuditoriaDescripcion(oListaTarjetasTAPI)),
                                                           conn);
                    conn.Commit();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtenemos la ultima lista TAPI
        /// </summary>
        /// <returns></returns>
        public static ListaTarjetasTAPI GetUltimaListaTAPI()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return InterfaceDT.GetUltimaListaTAPI(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los ultimos archivos TAPI y RUTA para mostrarlos en el dashboard.
        /// </summary>
        /// <returns></returns>
        public static ArchivoSubsidioL GetUltimosArchivosSubsidio()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return InterfaceDT.GetUltimosArchivosSubsidio(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaListaRUTA()
        {
            return "LBS";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ListaTarjetasRUTA oListaTarjetasRUTA)
        {
            string sReturn = oListaTarjetasRUTA.NumeroLista.ToString();

            if (ConexionBs.getGSToEstacion())
                sReturn = string.Concat(sReturn, "Gestion");
            else
                sReturn = string.Concat(sReturn, oListaTarjetasRUTA.EstacionOrigenDescripcion);

            return sReturn;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ListaTarjetasRUTA oListaTarjetasRUTA)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Lista Blanca RUTA ");
            AuditoriaBs.AppendCampo(sb, "Numero de Lista", oListaTarjetasRUTA.NumeroLista.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha de operacion", oListaTarjetasRUTA.FechaOperacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha del Archivo de Tarjetas", oListaTarjetasRUTA.FechaArchivoTarjetas.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha del Archivo de Certificados", oListaTarjetasRUTA.FechaArchivoCertificados.ToString());
            AuditoriaBs.AppendCampo(sb, "Status", oListaTarjetasRUTA.Status.ToString());
            AuditoriaBs.AppendCampo(sb, "Usuario", oListaTarjetasRUTA.Usuario);
            if (ConexionBs.getGSToEstacion())
                AuditoriaBs.AppendCampo(sb, "Estacion de Origen", "Gestion");
            else
                AuditoriaBs.AppendCampo(sb, "Estacion de Origen", oListaTarjetasRUTA.EstacionOrigenDescripcion);

            return sb.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaListaTAPI()
        {
            return "LBS";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ListaTarjetasTAPI oListaTarjetasTapi)
        {
            string sReturn = oListaTarjetasTapi.NumeroLista.ToString();
            if (ConexionBs.getGSToEstacion())
                sReturn = sReturn + "Gestion";
            else
                sReturn = sReturn + oListaTarjetasTapi.EstacionOrigenDescripcion;

            return sReturn;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ListaTarjetasTAPI oListaTarjetasTapi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Lista Blanca TAPI ");
            AuditoriaBs.AppendCampo(sb, "Numero de Lista", oListaTarjetasTapi.NumeroLista.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha de operacion", oListaTarjetasTapi.FechaOperacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha del Archivo", oListaTarjetasTapi.FechaArchivo.ToString());
            AuditoriaBs.AppendCampo(sb, "Status", oListaTarjetasTapi.Status.ToString());
            AuditoriaBs.AppendCampo(sb, "Usuario", oListaTarjetasTapi.Usuario);
            if (ConexionBs.getGSToEstacion())
                AuditoriaBs.AppendCampo(sb, "Estacion de Origen", "Gestion");
            else
                AuditoriaBs.AppendCampo(sb, "Estacion de Origen", oListaTarjetasTapi.EstacionOrigenDescripcion);

            return sb.ToString();
        }

        #endregion
        #endregion

        #region INTERFACE_CONTABLE

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las ListaContableL 
        /// </summary>
        /// <param name="bGST"> Permite filtrar por una fecha determinada
        /// <returns>ListaContableL</returns>
        /// ***********************************************************************************************
        public static ListaContableL getContable(string ruta, DateTime dtJornada, int? estacion)
        {
            return getContable(ConexionBs.getGSToEstacion(), ruta, dtJornada , estacion);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ListaContableL
        /// </summary>
        /// <param name="bGST"> Permite filtrar por una ListaContableL determinada
        /// <param name="Ruta">Ruta donde se guardara el archivo</param>
        /// <param name="jornada">jornada</param>
        /// <returns>ListaContableL</returns>
        /// ***********************************************************************************************
        public static ListaContableL getContable(bool bGST, string ruta, DateTime dtJornada , int? estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bGST, false);

                    return InterfaceDT.getInterfaces(conn, dtJornada,estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Exporta la interfase Contable 
        /// </summary>
        /// <param name="Path">Ruta donde se guardara el archivo</param>
        /// <param name="jornada">jornada</param>
        /// ***********************************************************************************************
        public static bool ExportarContable(string Ruta, string Path, DateTime jornada, bool GeneraTablaSQL , int? estacion )
        {
          bool retval = true;   
          //if (!System.IO.File.Exists(Ruta))
          //              System.IO.Directory.CreateDirectory(Ruta);

          //Jornada Abierta
          retval = JornadaBs.EstaCerrada(jornada);
          if (!retval)
              return retval;


          StringBuilder SBTexto = new StringBuilder();
          try
          {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                if (GeneraTablaSQL)
                {
                    InterfaceDT.setInterfaceContable(conn, jornada, ConexionBs.getUsuario() , estacion);

                }
                else
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path))
                    {

                        ListaContableL listaContable = InterfaceDT.getInterfaces(conn, jornada , estacion);
                        if (listaContable.Count > 0)
                        {
                            foreach (ListaContable oContable in listaContable)
                            {
                                SBTexto.Clear();
                                SBTexto.Append(oContable.ToString());
                                sw.WriteLine(SBTexto.ToString());

                            }
                        }
                        sw.Close();
                    }
                }
                 JornadaBs.setInterfaceJornada(jornada);


                  //Auditoria
                  using (Conexion connAud = new Conexion())
                  {
                      //conn.ConectarGSTThenPlaza();
                      connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                      // Ahora tenemos que grabar la auditoria:
                      AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaInterfase(),
                                                             "A",
                                                             getAuditoriaCodigoRegistro(jornada),
                                                             getAuditoriaDescripcionContable(jornada, GeneraTablaSQL)),
                                                             connAud);

                  }

                conn.Finalizar(true);
            }
          }
          catch (Exception ex)
          {

              throw ex;

          }

          return retval;

        }

        #endregion

        #region INTERFACE_SRI


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las ListaPuntosVentaL 
        /// </summary>
        /// <param name="bGST"> Permite filtrar por una estacion determinada
        /// <returns>ListaPuntosVentaL</returns>
        /// ***********************************************************************************************
        public static ListaPuntosVentaL getListaPuntosVenta(int estacion)
        {
            return getListaPuntosVenta(ConexionBs.getGSToEstacion(), estacion);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ListaPuntosVentaL
        /// </summary>
        /// <param name="bGST"> Permite filtrar por una getListaPuntosVenta determinada
        /// <returns>ListaPuntosVentaL</returns>
        /// ***********************************************************************************************
        public static ListaPuntosVentaL getListaPuntosVenta(bool bGST, int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bGST, false);

                    //Obtener lista ListaPuntosVenta
                    //Dmitriy 27/04/2011
                    return InterfaceDT.getInterfacesPuntosVenta(conn, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las ListaContableL 
        /// </summary>
        /// <param name="bGST"> Permite filtrar por una fecha determinada
        /// <returns>ListaContableL</returns>
        /// ***********************************************************************************************
        public static CambioAutorizacionSRI getListaAutorizacionSRI(string ruta, DateTime dtJornada, int? estacion, string PuntoVenta, bool baja)
        {
            return getListaAutorizacionSRI(ConexionBs.getGSToEstacion(), ruta, dtJornada, estacion, PuntoVenta, baja);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ListaContableL
        /// </summary>
        /// <param name="bGST"> Permite filtrar por una ListaContableL determinada
        /// <param name="Ruta">Ruta donde se guardara el archivo</param>
        /// <param name="jornada">jornada</param>
        /// <returns>ListaContableL</returns>
        /// ***********************************************************************************************
        public static CambioAutorizacionSRI getListaAutorizacionSRI(bool bGST, string ruta, DateTime dtJornada, int? estacion, string PuntoVenta, bool baja)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bGST, false);

                    //Obtener lista autorizacion
                    //Dmitriy26/04/2011
                    return InterfaceDT.getInterfacesSRI(conn, dtJornada, estacion, PuntoVenta, baja);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Exporta la interfase Contable 
        /// </summary>
        /// <param name="Path">Ruta donde se guardara el archivo</param>
        /// <param name="fechaInicioSRI">jornada</param>
        /// ***********************************************************************************************
        public static bool ExportarAutorizacionSRI(string Ruta, string Path, DateTime fechaInicioSRI, string tipoTramite, int? estacion, string PuntoVenta, string tipoTramiteDesc, bool baja)
        {
            bool retval = true;
            //if (!System.IO.File.Exists(Ruta))
            //              System.IO.Directory.CreateDirectory(Ruta);


            System.IO.StreamWriter sw = new System.IO.StreamWriter(Path);
            StringBuilder SBTexto = new StringBuilder();

            try
            {

                CambioAutorizacionSRI oCambio = getListaAutorizacionSRI(Path, fechaInicioSRI, estacion, PuntoVenta, baja);
                oCambio.codTipoTra = tipoTramite;

                //Verificamos que para cambio de software la anterior empieza antes
                if (tipoTramite == "7")
                {
                    if (oCambio.InicioAnterior == null || oCambio.InicioAnterior == new DateTime())
                    {
                        throw new Telectronica.Errores.ErrorFacturacionStatus("Para el Cambio de Software, debe haber una autorización anterior a la informada");
                    }
                    if (oCambio.Inicio < (DateTime)oCambio.InicioAnterior)
                    {
                        throw new Telectronica.Errores.ErrorFacturacionStatus("Para el Cambio de Software, la nueva autorización no puede empezar antes que la autorización anterior");
                    }
                }
                //Verificamos que para renovacion falten menos de 30 dias para el vencimiento
                if (tipoTramite == "8")
                {
                    if (oCambio.VencimientoAnterior == null || oCambio.VencimientoAnterior == new DateTime())
                    {
                        throw new Telectronica.Errores.ErrorFacturacionStatus("Para la renovación, debe haber una autorización anterior a la informada");
                    }
                    if (oCambio.Inicio < ((DateTime)oCambio.VencimientoAnterior).AddDays(-30))
                    {
                        throw new Telectronica.Errores.ErrorFacturacionStatus("Para la renovación, la nueva autorización no puede empezar más de 30 días antes del vencimiento de la autorización anterior");
                    }
                }
                //La fecha es la de hoy salvo que caiga fuera de la vigencia
                DateTime fecha = DateTime.Today;
                if (fecha > oCambio.Vencimiento)
                    fecha = oCambio.Vencimiento;

                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                //Guardamos las autorizaciones SRI
                if (oCambio != null)
                {
                    //string cadena = xmlUtil.SerializeObject(oCambio);
                    //sw.WriteLine(cadena);
                    
                    
                    sw.WriteLine("<autorizacion>");
                    sw.WriteLine("<codTipoTra>" + oCambio.codTipoTra + "</codTipoTra>");
                    sw.WriteLine("<ruc>" + oCambio.ruc + "</ruc>");
                   
                    //demas campos
                    //si tipo de tramite es 6, 10, 11, 9
                        //numAut
                    //sino 
                        //autOld
                        //autNew



                    if (tipoTramite == "6" || tipoTramite == "10" || tipoTramite == "11" || tipoTramite == "9")
                     {

                         sw.WriteLine("<numAut>" + oCambio.autNew + "</numAut>");
                         sw.WriteLine("<fecha>" + fecha.ToShortDateString() + "</fecha>");

                     }
                    else
                    {
                        sw.WriteLine("<fecha>" + fecha.ToShortDateString() + "</fecha>");
                        if (oCambio.autOld != null)
                            sw.WriteLine("<autOld>" + oCambio.autOld + "</autOld>");
                        sw.WriteLine("<autNew>" + oCambio.autNew + "</autNew>");

                    }


                    sw.WriteLine("<detalles>");
                    foreach (ListaAutorizacionDetalle item in oCambio.detalles)
                    {
                        sw.WriteLine("<detalle>");
                        sw.WriteLine("<codDoc>" + item.codDoc + "</codDoc>");
                        sw.WriteLine("<estab>" + item.estab +   "</estab>");
                        sw.WriteLine("<ptoEmi>" + item.ptoEmi + "</ptoEmi>");

                        //Demas campos   
                        //si tipo de tramite es 6, 10, 11, 9
                        if (tipoTramite == "6" || tipoTramite == "10")
                        {
                            //inicio
                            if (item.iniNew != null)
                                sw.WriteLine("<inicio>" + item.iniNew + "</inicio>");
                        }
                        else if (tipoTramite == "11" || tipoTramite == "9")
                        {
                            //fin
                            if (item.finNew != null)
                                sw.WriteLine("<fin>" + item.finNew + "</fin>");
                        }
                        //sino 
                        else
                        {
                            //finOld
                            //iniNew
                            if (item.finOld != null)
                                sw.WriteLine("<finOld>" + item.finOld + "</finOld>");
                            if (item.iniNew != null)
                                sw.WriteLine("<iniNew>" + item.iniNew + "</iniNew>");

                        }
                        sw.WriteLine("</detalle>");
                    }
                    sw.WriteLine("</detalles>");
                    sw.WriteLine("</autorizacion>");
                }

                //Auditoria
                using (Conexion conn = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    // Ahora tenemos que grabar la auditoria:
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaInterfase(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(fechaInicioSRI),
                                                           getAuditoriaDescripcionAutorizacionSRI(fechaInicioSRI,tipoTramiteDesc, PuntoVenta)),
                                                           conn);

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                sw.Close();
            }

            return retval;

        }

       

        #endregion

        #region INTERFACE_EXPORTACION


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta todas las interfases el OCCOVI
        /// </summary>
        /// <param name="Path">Ruta donde se guardara el archivo Detalle.txt</param>
        /// <param name="Mes">Mes seleccionado</param>
        /// <param name="Quincena">Quincena seleccionada</param>
        /// <param name="Anio">Año seleccionado</param>
        /// ***********************************************************************************************
        public static void ExportarOCCOVI(string Path, Int16 Mes, Int16 Quincena, Int16 Anio)
        {
            try
            {
                //Revisamos si existe el directorio, si no existe lo creamos:
                if (!System.IO.File.Exists(Path))
                    System.IO.Directory.CreateDirectory(Path);

                using (Conexion conn = new Conexion())
                {
                        // Conectamos a la DB correspondiente:
                        conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                        // Exportamos Detalle.txt  conn, 
                        InterfaceBS.ExportarArchivoDetalle(Path, Mes, Quincena, Anio, 'S');

                        // Exportamos Detalle Ruta.txt conn, 
                        InterfaceBS.ExportarArchivoDetalle(Path, Mes, Quincena, Anio, 'N');

                        // Exportamos los 6 archivos de Excel de la DDJJ conn, 
                        InterfaceBS.ExportarArchivoDDJJ(Path, Mes, Quincena, Anio);
                }

                //Auditoria
                using (Conexion conn = new Conexion())
                {
                    // Conectamos a la DB correspondiente:
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    // Ahora tenemos que grabar la auditoria:
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaExportacionArchivosSubsidio(),
                                                            "A",
                                                            getAuditoriaCodigoRegistroSubsidio("RUTA"),
                                                            getAuditoriaDescripcionSubsidio(Path, Mes, Quincena, Anio)),
                                                            conn);

                    conn.Finalizar(true);                      
                }
            }

            catch (Exception ex)
            {


                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// ***********************************************************************************************
        public static string ToDate(string Fecha)
        {
            string Resultado = "20" + Fecha.Substring(4, 2) + Fecha.Substring(2, 2) + Fecha.Substring(0, 2);
            return Resultado;
        }

        public static string toJornada(DateTime fecha)
        {

            return (fecha.Day.ToString("00") + fecha.Month.ToString("00") + fecha.Year.ToString()).Trim();

        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaExportacionArchivosSubsidio()
        {
            return "GIS";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroSubsidio(string tipo)
        {
            return tipo;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionSubsidio(string Path, Int16 Mes, Int16 Quincena, Int16 Anio)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Ruta de los archivos", Path);
            AuditoriaBs.AppendCampo(sb, "Año", Anio.ToString());
            AuditoriaBs.AppendCampo(sb, "Mes", Mes.ToString());
            AuditoriaBs.AppendCampo(sb, "Quincena", Quincena.ToString());


            return sb.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaExportacionCuadros()
        {
            return "GIC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroContable(string tipo)
        {
            return tipo;
        }



        #endregion

        #endregion

        #region INTERFACE_FACTURAS


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de facturas 
        /// </summary>
        /// <param name="dtDesde"> Fecha Desde
        /// <param name="dtHasta"> Fecha Hasta
        /// <returns>ListaFacturasL</returns>
        /// ***********************************************************************************************
        public static ListaFacturasL getFacturas(int? establecimiento, int? puntoventa, int? facturaDesde, int? facturaHasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return InterfaceDT.getFacturas(conn, establecimiento, puntoventa, facturaDesde, facturaHasta);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta la interfase de Factruas 
        /// </summary>
        /// <param name="Path">Ruta donde se guardara el archivo</param>
        /// <param name="desde">Fecha Desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// ***********************************************************************************************
        public static bool ExportarFacturas(string Ruta, string Path, string extension, int? establecimiento, int? puntoventa, int? facturaDesde, int? facturaHasta)
        {
            bool retval = true;


            System.IO.StreamWriter sw = null;
            StringBuilder SBTexto = new StringBuilder();
            Decimal baseIva0, baseIva12,montoNeto, montoIVA, montoTotal, precioUnitario, precioTotal;

            try
            {
                ListaFacturasL listaFacturas = getFacturas(establecimiento,puntoventa, facturaDesde, facturaHasta);

                foreach (ListaFacturas oFactura in listaFacturas)
                {
                    string archivo = Path + oFactura.Establecimiento.ToString("D03") + "_" + oFactura.PuntoVenta.ToString("D03") + "_" + oFactura.SecuencialFactura.ToString("D09") + ".xml";
                    sw = new System.IO.StreamWriter(archivo);
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

                    //Grabar la factura
                    sw.WriteLine("<factura version=\"2.00\">");
                    sw.WriteLine("<infoTributaria>");
                    sw.WriteLine("<razonSocial>" + oFactura.RazonSocialConcesionario +"</razonSocial>");
                    sw.WriteLine("<ruc>" + oFactura.RucConcesionario + "</ruc>");
                    sw.WriteLine("<numAut>" + oFactura.AutorizacionSRI + "</numAut>");
                    sw.WriteLine("<codDoc>" + oFactura.TipoComprobante + "</codDoc>");
                    sw.WriteLine("<estab>" + oFactura.Establecimiento.ToString("D03") + "</estab>");
                    sw.WriteLine("<ptoEmi>" + oFactura.PuntoVenta.ToString("D03") + "</ptoEmi>");
                    sw.WriteLine("<secuencial>" + oFactura.SecuencialFactura.ToString() + "</secuencial>");
                    sw.WriteLine("<fechaAutorizacion>" + oFactura.InicioSRI.ToShortDateString() + "</fechaAutorizacion>");
                    sw.WriteLine("<caducidad>" + oFactura.VencimientoSRI.ToShortDateString() + "</caducidad>");
                    sw.WriteLine("<fechaEmision>" + oFactura.FechaEmision.ToShortDateString() + "</fechaEmision>");
                    sw.WriteLine("<dirMatriz>" + oFactura.DireccionMatriz + "</dirMatriz>");
                    sw.WriteLine("<razonSocialComprador>" + oFactura.NombreCliente + "</razonSocialComprador>");
                    sw.WriteLine("<rucCedulaComprador>" + oFactura.RucCliente + "</rucCedulaComprador>");
                    if( oFactura.ContribuyenteEspecial != null && oFactura.ContribuyenteEspecial != "" )
                        sw.WriteLine("<contribuyenteEspecial>" + oFactura.ContribuyenteEspecial + "</contribuyenteEspecial>");
                    sw.WriteLine("<obligado>" + "Obligado a Llevar Contabilidad" + "</obligado>");
                    if (oFactura.Anulado)
                    {
                        montoNeto = 0;
                        montoIVA = 0;
                        montoTotal = 0;
                    }
                    else
                    {
                        montoNeto = oFactura.MontoNeto;
                        montoIVA = oFactura.MontoIVA;
                        montoTotal = oFactura.MontoTotal;
                    }
                    if (oFactura.Identificado == "S")
                    {
                        sw.WriteLine("<totalSinImpuestos>" + Conversiones.MoneyToString(montoNeto) + "</totalSinImpuestos>");
                        baseIva0 = 0;
                        baseIva12 = 0;
                        if ((double)montoIVA > 0.005)
                        {
                            baseIva12 = montoNeto;
                        }
                        else
                        {
                            baseIva0 = montoNeto;
                        }
                        sw.WriteLine("<baseIVA0>" + Conversiones.MoneyToString(baseIva0) + "</baseIVA0>");
                        sw.WriteLine("<baseIVA12>" + Conversiones.MoneyToString(baseIva12) + "</baseIVA12>");
                        sw.WriteLine("<IVA12>" + Conversiones.MoneyToString(montoIVA) + "</IVA12>");
                    }
                    sw.WriteLine("<totalConImpuestos>" + Conversiones.MoneyToString(montoTotal) + "</totalConImpuestos>");
                    sw.WriteLine("</infoTributaria>");

                    sw.WriteLine("<detalles>");
                    foreach (ListaFacturasDetalle item in oFactura.detalles)
                    {
                        if (oFactura.Anulado)
                        {
                            precioUnitario = 0;
                            precioTotal = 0;
                        }
                        else
                        {
                            precioUnitario = item.PrecioUnitario;
                            precioTotal = item.PrecioUnitario * item.Cantidad;
                        }
                        sw.WriteLine("<detalle>");
                        sw.WriteLine("<concepto>" + item.DescripcionItem + "</concepto>");
                        sw.WriteLine("<cantidad>" + Conversiones.MoneyToString(item.Cantidad) + "</cantidad>");
                        sw.WriteLine("<precioUnitario>" + Conversiones.MoneyToString(precioUnitario) + "</precioUnitario>");
                        sw.WriteLine("<descuentos>" + "0.00" + "</descuentos>");
                        sw.WriteLine("<precioTotal>" + Conversiones.MoneyToString(precioTotal) + "</precioTotal>");
                        sw.WriteLine("</detalle>");
                    }
                    sw.WriteLine("</detalles>");

                    sw.WriteLine("<infoAdicional>");
                    sw.WriteLine("<campoAdicional nombre = \"Direccion Establecimiento\">" + oFactura.DireccionEstablecimiento + "</campoAdicional>");
                    sw.WriteLine("</infoAdicional>");
                    sw.WriteLine("</factura>");
                    sw.Close();

                }

                //Auditoria
                using (Conexion conn = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    // Ahora tenemos que grabar la auditoria:
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaInterfase(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(establecimiento),
                                                           getAuditoriaDescripcionFacturas(establecimiento, puntoventa, facturaDesde, facturaHasta)),
                                                           conn);

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
            }

            return retval;

        }



        #endregion
        
        #region INTERFACE_INFORME_CONSOLIDADO

        /// <summary>
        /// Funcion para exportar a excel el informe consolidado de transitos y recaudos
        /// </summary>
        /// <param name="Path">Ruta del directorio donde se alojara el archivo resultante</param>
        /// <param name="Mes">Mes del reporte</param>
        /// <param name="Año">Año del reporte</param>
        /// <param name="Zona">La zona de la cual se obtienen los datos del reporte, NULL = TODAS</param>
        /// <param name="Estacion">La estación de la cual se obtienen los datos del reporte, NULL = TODAS</param>
        public static void ExportarListadoConsolidadoAExcel(string strTemplate, string strNewName, int estacion,Int16 Mes, Int16 Año, string nombreEstacion)
        {
            string a = string.Empty;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    // Nombre del archivo resultante
                    //Template tmpInforme = TemplateBs.getTemplate(StringEnum.GetStringValue(Template.TemplateFiles.INFORME_CONSOLIDADO),Path);

                    // Dataset con los registros que vamos a exportar:
                    DataSet DsRegistros = new DataSet();

                    // Convertimos el mes/año en fechas:
                    DateTime JornadaDesde = new DateTime(Año, Mes,1);
                    DateTime JornadaHasta = new DateTime(Año, Mes,DateTime.DaysInMonth(Año, Mes));

                    // Obtenemos el dataset con la informacion correspondiente:
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    DsRegistros = InterfaceDT.getTransitoInformeConsolidado(conn, JornadaDesde, estacion, DsRegistros);
                    InterfaceDT.getRecaudoInformeConsolidado(conn, JornadaDesde, estacion,  ref DsRegistros);
                    InterfaceDT.getResultadoInformeConsolidado(conn, JornadaDesde, estacion, ref DsRegistros);
                    InterfaceDT.getRecargasConsolidado(conn, JornadaDesde, estacion, ref DsRegistros);
                    //Uso el metodo de negocios porque el SP está en Gestion y no en Consolidado
                    Estacion oEst =  EstacionBs.getEstaciones(estacion)[0];
                    //InterfaceDT.getFechaInformeConsolidado(JornadaDesde, JornadaHasta, DateTime.DaysInMonth(Año, Mes), ref DsRegistros);

                    //Copiamos el archivo
                    // Copia el archivo a una nueva ubicacion y lo sobreescribe si existe
                    System.IO.File.Copy(strTemplate,strNewName, true);
                    System.IO.File.SetAttributes(strNewName, FileAttributes.Normal);


                    //string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                    //        + "Data Source=" + strNewName
                    //        + ";" + "Extended Properties=Excel 8.0;";
                    string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewName
                        + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";

                    OleDbConnection objConn = new OleDbConnection(connectionString);

                    objConn.Open();

                    InterfaceDT.ActualizarTransitoConsolidado(objConn, DsRegistros.Tables["Datos_Trafico"]);
                    InterfaceDT.ActualizarRecaudoConsolidado(objConn, DsRegistros.Tables["Datos_Recaudo"]);
                    InterfaceDT.ActualizarResultadoConsolidado(objConn, DsRegistros.Tables["Datos_Resultado"]);
                    InterfaceDT.ActualizarRecargasConsolidado(objConn, DsRegistros.Tables["Datos_Recarga"]);
                    string dirEstacion = oEst.Direccion;
                    InterfaceDT.ActualizarFechasConsolidado(objConn, JornadaDesde, JornadaHasta, DateTime.DaysInMonth(Año, Mes), estacion, nombreEstacion, dirEstacion);
                    //conn.oleConection.Close();
                    //conn.Dispose();

                    objConn.Close();

                    //TemplateBs.UpdateFile(ref tmpInforme, DsRegistros, true, strDirectoryToSaveFile,strNewName);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ANEXO TRANSACCIONAL
        public static void generarAnexoTransaccional(DateTime dtDesde, DateTime dtHasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    InterfaceDT.GenerarAnexoTransaccional(conn, dtDesde, dtHasta, ConexionBs.getUsuario());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el Anexo Transaccional
        /// </summary>
        /// <param name="Path">Ruta donde se guardara el archivo</param>
        /// <param name="desde">Fecha Desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// ***********************************************************************************************
        public static bool ExportarAnexoTransaccional(DateTime desde, DateTime hasta)
        {
            bool retval = true;



            try
            {
                generarAnexoTransaccional(desde, hasta);

                //Auditoria
                using (Conexion conn = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    // Ahora tenemos que grabar la auditoria:
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaInterfase(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(desde),
                                                           getAuditoriaDescripcionAnexoTransaccional(desde, hasta)),
                                                           conn);

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

            return retval;

        }
        #endregion
        
        #region AUDITORIA
        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaInterfase()
        {
            return "GIC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(DateTime fecha)
        {
            return fecha.ToShortDateString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(int? establecimiento)
        {
            string ret = "";
            if( establecimiento != null )
                ret = ((int)establecimiento).ToString("D03");
            return ret;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionContable(DateTime fecha, bool generaTablaSQL)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Interfase Contable");
            AuditoriaBs.AppendCampo(sb, "Jornada", fecha.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Destino", generaTablaSQL?"Tabla SQL":"Archivo TXT");

            return sb.ToString();
        }


        private static string getAuditoriaDescripcionAutorizacionSRI(DateTime fecha, string tramite, string puntosVenta)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Autorizacion SRI");
            AuditoriaBs.AppendCampo(sb, "Inicio Autorización", fecha.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Trámite", tramite);
            if( puntosVenta != null && puntosVenta != "")
                AuditoriaBs.AppendCampo(sb, "Puntos Emisión", puntosVenta);

            return sb.ToString();
        }

        private static string getAuditoriaDescripcionFacturas(int? establecimiento, int? puntoventa, int? facturaDesde, int? facturaHasta)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Detalle Facturas");
            if( establecimiento != null )
                AuditoriaBs.AppendCampo(sb, "Establecimiento", ((int)establecimiento).ToString("D03"));
            if (puntoventa != null)
                AuditoriaBs.AppendCampo(sb, "Punto Emisión", ((int)puntoventa).ToString("D03"));
            if (facturaDesde != null)
                AuditoriaBs.AppendCampo(sb, "Secuencial Desde", ((int)facturaDesde).ToString("D03"));
            if (facturaHasta != null)
                AuditoriaBs.AppendCampo(sb, "Secuencial Hasta", ((int)facturaHasta).ToString("D03"));

            return sb.ToString();
        }

        private static string getAuditoriaDescripcionAnexoTransaccional(DateTime desde, DateTime hasta)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Anexo Transaccional");
            AuditoriaBs.AppendCampo(sb, "Fecha Desde", desde.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Fecha Hasta", hasta.ToShortDateString());

            return sb.ToString();
        }


        #endregion

       
    }
}
