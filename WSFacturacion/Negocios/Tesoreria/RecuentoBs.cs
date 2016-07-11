using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data;
using System.Xml;
using System.IO;
using Telectronica.Errores;

namespace Telectronica.Tesoreria
{

    public static class RecuentoBs
    {

        /// <summary>
        /// Obtiene los recuentos que se encuentran dentro de las fechas desde y hasta
        /// </summary>
        /// <param name="Desde"></param>
        /// <param name="Hasta"></param>
        /// <returns>Devuelve una lista de de</returns>
        public static RecuentoL getRecuentos(DateTime? Desde, DateTime? Hasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RecuentoDt.getRecuentos(conn, Desde, Hasta, EstacionBs.getEstacionActual().Numero);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///*****************************************************************************************************
        /// <summary>
        /// Elimina un recuento
        /// </summary>
        /// <param name="oRecuento">Recuento a eliminar</param>
        ///*****************************************************************************************************
        public static void delRecuento(Recuento oRecuento)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    oRecuento.Detalles = getDetalleRecuento(oRecuento.NumeroRecuento);

                    //Verficamos que puede Anular el Deposito
                    if (!PuedeAnular(conn, oRecuento, out causa))
                    {
                        throw new ErrorParteStatus(causa);
                    }

                    RecuentoDt.delRecuento(conn, oRecuento);                    

                    foreach (RecuentoDetalle item in oRecuento.Detalles)
                    {
                        RecuentoDt.delDetalleRecuento(conn, item.estacion, item.ParteRecuento);                        
                    }

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecuento(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oRecuento),
                                                           getAuditoriaDescripcion(oRecuento)),
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

        
        /// <summary>
        /// Obtiene los detalles de un recuento
        /// </summary>
        /// <param name="iNumeroRecuento"></param>
        /// <returns></returns>
        public static RecuentoDetalleL getDetalleRecuento(int iNumeroRecuento)
        {
            try
            {
                RecuentoDetalleL oRecuntoDetalle = new RecuentoDetalleL();

                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RecuentoDt.getDetalleRecuentos(conn, iNumeroRecuento);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        

        /// <summary>
        /// Procesa el archivo enviado, y devuelve la tabla con los recuentos y los depositos correspondientes
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <returns></returns>
        public static RecuentosDepositosL getDepositosRecuentos(string RutaArchivo, DateTime FechaDesde, int? Estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    // Creamos la estructura del XML en memoria:
                    XmlDocument arbol = new XmlDocument();
                    XmlAttribute oAtribRecuento;
                    XmlAttribute oAtribFecha;
                    XmlAttribute oAtribLetra;
                    XmlAttribute oAtribF22;
                    XmlAttribute oAtribSucursal;
                    XmlAttribute oAtribEstacion;
                    XmlAttribute oAtribSobre;
                    XmlAttribute oAtribMedioDePago;
                    XmlAttribute oAtribMoneda;
                    XmlAttribute oAtribIngresado;
                    XmlAttribute oAtribRecontado;
                    XmlAttribute oAtribDiferencia;
                    XmlAttribute oAtribObservaciones;

                    XmlNode nodo;
                    nodo = arbol.CreateElement("Recuentos");
                    arbol.AppendChild(nodo);

                    //// convert string to stream
                    //byte[] byteArray = Encoding.ASCII.GetBytes(ContenidoArchivo);
                    //MemoryStream stream = new MemoryStream(byteArray);

                    // Leemos el archivo:
                    //RutaArchivo = "C:" + (char)92 + "Users" + (char)92 + "msuarez" + (char)92 + "Desktop" + (char)92 + "Auxiliar";
                    StreamReader objReader = new StreamReader(RutaArchivo);
                    string sLine = "";

                    //La primer linea del archivo corresponde al encabezado, asi que hay que empezar a cargar los datos desde la segunda linea.
                    sLine = objReader.ReadLine();

                    while (sLine != null)
                    {
                        //Leemos una linea del archivo:
                        sLine = objReader.ReadLine();

                        if (sLine != null)
                        {
                            // Separo los atributos:
                            string[] Datos = sLine.Split(',');

                            // Creamos el nodo:
                            nodo = arbol.CreateElement("Recuentos");

                            // Inicializamos los atributos:
                            oAtribRecuento = arbol.CreateAttribute("Recuento");
                            oAtribFecha = arbol.CreateAttribute("Fecha");
                            oAtribLetra = arbol.CreateAttribute("Letra");
                            oAtribF22 = arbol.CreateAttribute("F22");
                            oAtribSucursal = arbol.CreateAttribute("Sucursal");
                            oAtribEstacion = arbol.CreateAttribute("Estacion");
                            oAtribSobre = arbol.CreateAttribute("Sobre");
                            oAtribMedioDePago = arbol.CreateAttribute("MedioDePago");
                            oAtribMoneda = arbol.CreateAttribute("Moneda");
                            oAtribIngresado = arbol.CreateAttribute("Ingresado");
                            oAtribRecontado = arbol.CreateAttribute("Recontado");
                            oAtribDiferencia = arbol.CreateAttribute("Diferencia");
                            oAtribObservaciones = arbol.CreateAttribute("Observaciones");

                            // Cargamos los atributos:
                            oAtribRecuento.InnerText = Datos[0].Trim();
                            oAtribFecha.InnerText = Datos[1].Trim();
                            oAtribLetra.InnerText = Datos[2].Trim();
                            oAtribF22.InnerText = Datos[3].Trim();
                            oAtribSucursal.InnerText = Datos[4].Trim();
                            oAtribEstacion.InnerText = Datos[5].Trim();
                            oAtribSobre.InnerText = Datos[6].Trim();
                            oAtribMedioDePago.InnerText = Datos[7].Trim();
                            oAtribMoneda.InnerText = Datos[8].Trim();
                            oAtribIngresado.InnerText = Datos[9].Trim();
                            oAtribRecontado.InnerText = Datos[10].Trim();
                            oAtribDiferencia.InnerText = Datos[11].Trim();
                            oAtribObservaciones.InnerText = Datos[12].Trim();

                            // Agregamos los atributos:
                            nodo.Attributes.Append(oAtribRecuento);
                            nodo.Attributes.Append(oAtribFecha);
                            nodo.Attributes.Append(oAtribLetra);
                            nodo.Attributes.Append(oAtribF22);
                            nodo.Attributes.Append(oAtribSucursal);
                            nodo.Attributes.Append(oAtribEstacion);
                            nodo.Attributes.Append(oAtribSobre);
                            nodo.Attributes.Append(oAtribMedioDePago);
                            nodo.Attributes.Append(oAtribMoneda);
                            nodo.Attributes.Append(oAtribIngresado);
                            nodo.Attributes.Append(oAtribRecontado);
                            nodo.Attributes.Append(oAtribDiferencia);
                            nodo.Attributes.Append(oAtribObservaciones);

                            // Agregamos el nodo:
                            arbol.DocumentElement.AppendChild(nodo);
                        }
                    }

                    // Cerramos el archivo, ya tenemos todos los datos guardados en la variable arbol, en formato xml con atributos.
                    objReader.Close();

                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //Obtenemos la lista de DepositosRecuentos:
                    RecuentosDepositosL oRecuentosDepositosL = RecuentoDt.getDepositosRecuentos(conn, arbol, FechaDesde, Estacion);

                    //Recorremos la lista y actualizamos el status segun corresponda:
                    for (int i = 0; i < oRecuentosDepositosL.Count; i++)
                    {
                        RecuentosDepositos item = oRecuentosDepositosL[i];

                        //------------------------------------------------------------------------------------\\
                        //Depositada no tiene datos	                       | Rojo |        Sobre Inexistente  \\
                        //------------------------------------------------------------------------------------\\
                        //Recontada no tiene datos                                                            \\
                        //y existe registro con F22 = a esta Guia          | Rojo |        Depósito Incompleto\\
                        //------------------------------------------------------------------------------------\\
                        //Recontado hay registro y                                                            \\
                        //Existe otro registro con = Guia e igual Bolsa    | Rojo |        Bolsa Repetida     \\
                        //------------------------------------------------------------------------------------\\
                        //Monto <> Ingresado                               | Amarillo |    No coincide Monto  \\
                        //------------------------------------------------------------------------------------\\
                        //SI NO ENVIA EL RECUENTO SIGNIFICA QUE ESTA RECONTADO OK
                        //SI LO ENVIAN SIGNIFICA QUE TIENE ALGUN PROBLEMA

                        //Status 0 VERDE OK (Coincide bolsa, remito, monto y es unico)
                        //Status 1 ROJO (impide recibir)
                        //Status 2 AMARILLO (Coincide bolsa, remito y es unico)
                        //Status 3 GRIS (Deposito sin recuento, o movimiento sin deposito)

                        item.Status = 0;
                        item.Causa = "";

                        //Tiene Recuento (HAY DIFERENCIA)
                        if (item.oRecuento != null && item.oRecuento.CodigoRecuento != null)
                        {
                            //sin Bolsa depositada
                            if (item.oBolsaDeposito == null || item.oBolsaDeposito.Bolsa == null)
                            {
                                item.Status = 1;
                                item.Causa = "Sobre Inexistente";
                            }
                            else
                            {
                                //Si esta depositado
                                //Monto ingresado no coincide
                                if (item.oRecuento.Recontado != item.oBolsaDeposito.MontoEquivalente)
                                {
                                    item.Status = 2;
                                    item.Causa = "No coincide Monto";
                                }

                                //Revisamos que no este repetida
                                for (int i2 = 0; i2 < oRecuentosDepositosL.Count - 1; i2++)
                                {
                                    RecuentosDepositos item2 = oRecuentosDepositosL[i2];
                                    //Repetido en bolsas depositadas
                                    if (i != i2 //que no sea el mismo
                                        && item2.oBolsaDeposito != null && item2.oBolsaDeposito.Remito == item.oBolsaDeposito.Remito
                                        && item2.oBolsaDeposito.Bolsa == item.oBolsaDeposito.Bolsa)
                                    {
                                        item.Status = 1;
                                        item.Causa = "Bolsa Repetida";
                                        break;
                                    }

                                    //Repetido en el archivo
                                    if (i != i2 //que no sea el mismo
                                        && item2.oRecuento != null && item2.oRecuento.F22Recuento == item.oRecuento.F22Recuento
                                        && item2.oRecuento.SobreRecuento == item.oRecuento.SobreRecuento)
                                    {
                                        item.Status = 1;
                                        item.Causa = "Bolsa Repetida";
                                        break;
                                    }
                                }
                            }
                        }
                        // SI NO VINO EN EL ARCHIVO DE RECUENTO
                        else
                        {
                            // SI NO VINO EN EL ARCHIVO DE RECIENTO PERO TIENE BOLSA.... DEBERIA DEJARLO COMO RECONTADO OK
                            if (item.oBolsaDeposito != null && item.oBolsaDeposito.Remito != null)
                            {
                                item.Status = 0;
                                item.Causa = "";
                            }

                            //// Indicamos que no tiene recuento:
                            //item.Status = 3;
                            // item.Causa = "";

                            ////Si no hay recuento busco si hay un recuento de este mismo deposito
                            //if (item.oBolsaDeposito != null && item.oBolsaDeposito.Remito != null)
                            //{
                            //    for (int i2 = 0; i2 < oRecuentosDepositosL.Count - 1; i2++)
                            //    {
                            //        RecuentosDepositos item2 = oRecuentosDepositosL[i2];
                            //        if (i != i2 //que no sea el mismo
                            //            && item2.oRecuento != null && item2.oRecuento.F22Recuento == item.oBolsaDeposito.Remito)
                            //        {
                            //            item.Status = 1;
                            //            item.Causa = "Depósito Incompleto";
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                    }

                    return oRecuentosDepositosL;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Agrega el recuento con todos sus detalles
        /// </summary>
        /// <param name="oRecuentosDepositosL"></param>
        public static void addRecuento(Recuento oRecuento)
        {
            //Si la lista es vacia no hacemos nada
            if (oRecuento.Detalles.Count > 0)
            {
                try
                {
                    //iniciamos una transaccion
                    using (Conexion conn = new Conexion())
                    {
                        // Siempre en la plaza, con transaccion
                        conn.ConectarPlaza(true);

                        oRecuento.Estacion = new Estacion(ConexionBs.getNumeroEstacion(), ConexionBs.getNombreEstacion());
                        oRecuento.Usuario = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre()); /* ID del usuario que registra el recuento */
                        oRecuento.Fecha = DateTime.Now; /* Fecha de registro del recuento */

                        // Agregamos la cabecera:
                        RecuentoDt.addRecuentoCabecera(conn, oRecuento); /* Numero de registro de Recuento */

                        // Por cada item de recuento, con status = 1 generamos un registro
                        // Tener en cuenta, que si en esta estacion hay tesorero, el idmoc va en null
                        // porque se engancha directamente con la apropiacion de bolsa y no contra mocaja

                        
                        foreach (RecuentoDetalle item in oRecuento.Detalles)
                        {
                        
                            RecuentoDt.addRecuentoDetalle(conn, item, oRecuento);

                            // CALCULO LA DIFERENCIA
                            decimal Diferencia = 0;

                            if (item.Recontado < item.Ingresado)
                            {
                                Diferencia = item.Ingresado - item.Recontado;
                            }


                            // GENERO UNA REPOSISION FINANCIERA SI TUBE DIFERENCIAS NEGATIVAS
                            if (Diferencia > 0)
                            {

                                ReposicionPedida oReposicion = new ReposicionPedida();

                                    oReposicion.Parte = item.ParteRecuento;
                                    oReposicion.TipoDeReposicion = new TipoDeReposicion();
                                    oReposicion.TipoDeReposicion.TipoCodigo = "F";
                                    oReposicion.Estacion = oRecuento.Estacion;
                                    oReposicion.Monto = Diferencia; //Paso el monto a positivo
                                    oReposicion.Peajista = oRecuento.Usuario.ID;
                                    oReposicion.bolsa = item.BolsaDepositada.Bolsa.ToString();

                                    //Crear metodo que genere la Reposicion Financiera
                                    RendicionDt.addReposicionFinanciera(conn, oReposicion);


                                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicionPedida(),
                                                                               "A",
                                                                               getAuditoriaCodigoRegistro(oReposicion),
                                                                               getAuditoriaDescripcion(oReposicion)),
                                                                               conn);      
                                

                            }

                            //Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecuento(),
                                                                   "A",
                                                                   getAuditoriaCodigoRegistro(oRecuento),
                                                                   getAuditoriaDescripcion(oRecuento)),
                                                                   conn);
                        }


                        
                        //Grabo OK hacemos COMMIT
                        conn.Finalizar(true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



            //


            ////////Tabla RECBAN
            /////////* Recuentos del Banco */
            ////////rec_coest tinyint not null					/* Código de estación */
            ////////rec_numer int identity  					/* Numero de registro de Recuento */
            ////////rec_id varchar(10) not null					/* ID del usuario que registra el recuento */
            ////////rec_fecha datetime null						/* Fecha de registro del recuento */
            ////////rec_archivo varchar(1000) null				/* Nombre del archivo procesado */
            ////////primary key clustered(rec_coest,rec_numer)
            ////////foreign key FK_RECBAN_ESTACI RECBAN(rec_coest) ESTACI(est_codig)
            ////////foreign key FK_RECBAN_USERS RECBAN(rec_id) USERS(use_id)

            ////////Tabla DETRECMOC
            /////////* Detalles de Recuentos realizados por el banco a los depositos */
            ////////dec_coest tinyint not null					/* Código de Estación */
            ////////dec_idrec int not null					    /* Numero de registro de recuento */
            ////////dec_iddep int not null					    /* Numero de deposito */
            ////////dec_idmoc int not null						/* Numero de MOCAJA */
            ////////dec_valoringresado money not null			/* Valor declarado como depositado */
            ////////dec_valorrecontado money not null			/* Valor recontado por el banco (ES EL QUE VALE) */
            ////////dec_diferencia money not null				/* Valor de diferencia Informado por el banco, NO tomarlo como definitivo, recalcularlo siempre */
            ////////dec_observacion varchar(1000) null			/* Observacion registrada por la entidad recaudadora en el archivo que retorna */
            ////////primary key clustered(dec_coest,dec_idrec,dec_iddetdep)
            ////////foreign key FK_DETRECMOC_RECBAN DETRECMOC(dec_coest,dec_idrec) RECBAN(rec_coest,rec_numer)
            ////////foreign key FK_DETRECMOC_DETDEPMOC DETRECMOC(dec_coest,dec_iddep,dec_idmoc) DETDEPMOC(dem_coest,dem_iddep,dem_idmoc)

        }

        /// <summary>
        /// Indica si este recuento se puede anular.
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oRecuento"></param>
        /// <param name="Causa"></param>
        /// <returns></returns>
        private static bool PuedeAnular(Conexion oConn, Recuento oRecuento, out string Causa)
        {
            Causa = "";
            bool verif = true;

            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);
                verif = RecuentoDt.VerificarReposicionPaga(oRecuento, conn, out Causa);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }

            return verif;
        }

        #region AUDITORIA


        ///**********************************************************************************
        /// <summary>
        /// Codigo de la audigoria
        /// </summary>
        /// <returns></returns>
        ///***********************************************************************************
        private static string getAuditoriaCodigoAuditoriaReposicionPedida()
        {
            return "REP";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ReposicionPedida oRep)
        {
            return oRep.Parte.Numero.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ReposicionPedida oRep)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estacion", oRep.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Parte", oRep.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha Movimiento", oRep.FechaIngreso.ToString("yyyyMMdd hh:mm:ss"));
            AuditoriaBs.AppendCampo(sb, "Peajista", oRep.Peajista.ToString());
            AuditoriaBs.AppendCampo(sb, "Bolsa", oRep.bolsa.ToString());
            AuditoriaBs.AppendCampo(sb, "Monto a Reponer", oRep.Monto.ToString());
            AuditoriaBs.AppendCampo(sb, "Tipo", oRep.TipoDeReposicion.TipoCodigo.ToString());

            return sb.ToString();
        }

        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaRecuento()
        {
            return "RTO";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Recuento oRecuento)
        {
            return oRecuento.NumeroRecuento.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Recuento oRecuento)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Archivo", oRecuento.NombreArchivo);
            if (oRecuento.Detalles != null)
            {
                StringBuilder sb2 = new StringBuilder();
                foreach (RecuentoDetalle item in oRecuento.Detalles)
                {
                    AuditoriaBs.AppendCampo(sb2, "F22", item.F22Recuento.ToString());
                    AuditoriaBs.AppendCampo(sb2, "Sobre", item.SobreRecuento.ToString());
                    AuditoriaBs.AppendCampo(sb2, "Ingresado", item.Ingresado.ToString("C"));
                    AuditoriaBs.AppendCampo(sb2, "Recontado", item.Recontado.ToString("C"));
                    AuditoriaBs.AppendCampo(sb2, "Diferencia", item.Diferencia.ToString("C"));
                    AuditoriaBs.AppendCampo(sb2, "Observación", item.Observacion);
                    sb2.Append("\n");
                }
                AuditoriaBs.AppendCampo(sb, "Detalles", sb2.ToString());
            }


            return sb.ToString();
        }
        #endregion

    }
}
