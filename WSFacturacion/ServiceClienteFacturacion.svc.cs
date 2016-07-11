using System;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;

using System.Configuration;

using System.Text;
using System.IO;
using Telectronica.Peaje;
using Telectronica.Facturacion;
using Telectronica.Tesoreria;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.EntidadesSL;
using Telectronica.Validacion;

namespace ClienteFacturacion.Web.Servicios
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceClienteFacturacion 
    {
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }
        private int maxRegistrosClientes = 400;
        private int maxRegistrosVehiculos = 400;
        #region CLIENTE: Servicio para acceso a datos de Clientes

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista de ClientesSL 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 18/12/2009
        // ULT.FEC.MODIF. ..: 18/12/2009
        // OBSERVACIONES ...: Parametros:
        //                  bool - bGST tratar de obtener los datos de GST
        //                  int? - numeroCliente - Permite filtrar por un codigo de cliente en particular
        //                  string - patente - permite filtrar por una patente determinada
        //                  int? - tipoDocumento - permite filtrar por un tipo y numero de documento
        //                  string - nombre - Filtra por una cadena similar a la ingresada
        //                  string - numeroTag - permite filtrar por un numero de tag de un vehiculo
        //                  string - numeroTarjeta - permite filtrar por un numero de tarjeta chip de un vehiculo
        //                  int? - numeroTarjetaExterno - permite filtrar por el numero externo de la tarjeta chip de un vehiculo
        //                  string - expediente - permite filtrar por el numero de expediente a tramitar
        //                  bool? - locales - permite incluir en la lista los clientes locales a la estacion
        //                  out bool - PudoGST - true si pudo obtener los datos de GST
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteSLL obtenerClientesSL(bool bGST, int? numeroCliente,
                                        string patente, int? tipoDocumento,
                                        string documento, string nombre,
                                        string numeroTag, string numeroTarjeta,
                                        int? numeroTarjetaExterno, string expediente,
                                        bool locales, out errorCapaSilver exout, out bool PudoGST, out bool llegoAlTope, bool conConsumidorFinal, int? numeroTagExterno)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return (ClienteBS.getClientes(bGST, numeroCliente, patente,
                                             tipoDocumento, documento,
                                             nombre, numeroTag,
                                             numeroTarjeta, numeroTarjetaExterno, expediente, locales, out PudoGST, maxRegistrosClientes, out  llegoAlTope, conConsumidorFinal, numeroTagExterno)).ConvertEnSL();
            }
            catch (Exception ex)
            {
                llegoAlTope = false;
                exout = new errorCapaSilver(ex);
                return null;
            }
        }




        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista de Clientes 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 18/12/2009
        // ULT.FEC.MODIF. ..: 18/12/2009
        // OBSERVACIONES ...: Parametros:
        //                  bool - bGST tratar de obtener los datos de GST
        //                  int? - numeroCliente - Permite filtrar por un codigo de cliente en particular
        //                  string - patente - permite filtrar por una patente determinada
        //                  int? - tipoDocumento - permite filtrar por un tipo y numero de documento
        //                  string - nombre - Filtra por una cadena similar a la ingresada
        //                  string - numeroTag - permite filtrar por un numero de tag de un vehiculo
        //                  string - numeroTarjeta - permite filtrar por un numero de tarjeta chip de un vehiculo
        //                  int? - numeroTarjetaExterno - permite filtrar por el numero externo de la tarjeta chip de un vehiculo
        //                  string - expediente - permite filtrar por el numero de expediente a tramitar
        //                  bool? - locales - permite incluir en la lista los clientes locales a la estacion
        //                  out bool - PudoGST - true si pudo obtener los datos de GST
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteL obtenerClientes(bool bGST, int? numeroCliente,
                                        string patente, int? tipoDocumento,
                                        string documento, string nombre,
                                        string numeroTag, string numeroTarjeta,
                                        int? numeroTarjetaExterno, string expediente,
                                        bool locales, bool conConsumidorFinal, int? numeroTagExterno,
                                        out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return (ClienteBS.getClientes(bGST, numeroCliente, patente,
                                            tipoDocumento, documento,
                                             nombre, numeroTag,
                                            numeroTarjeta, numeroTarjetaExterno, expediente, locales, conConsumidorFinal, numeroTagExterno, out PudoGST));
          
            // return new ClienteL();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con el Cliente (no deberia ser mas de uno) que posean el vehiculo con la placa recibida
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 13/05/2010
        // ULT.FEC.MODIF. ..: 13/05/2010
        // OBSERVACIONES ...: Parametros:
        //                  string - patente - permite filtrar por una patente determinada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteL obtenerClientesPorPatente(string patente, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getClientesPorPatente(patente);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con los Cliente (desagrupado por sus vehiculos) 
        // AUTOR ...........: Nicolas Tarlao
        // FECHA CREACIÓN ..: 29/04/2011
        // ULT.FEC.MODIF. ..: 29/04/2011
        // OBSERVACIONES ...: Parametros:
        //                  string - patente - permite filtrar por una patente determinada
        //                  string - razonSocial
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteL obtenerClientesAgrupadoPorVehiculos(string patente, string razonSocial, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getClientesAgrupadoPorVehiculos(patente, razonSocial);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con el Cliente (no deberia ser mas de uno) con sus datos y listas de vehiculos, cuentas y saldos
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 15/06/2010
        // ULT.FEC.MODIF. ..: 15/06/2010
        // OBSERVACIONES ...: Parametros:
        //                          int - numeroCliente - permite filtrar por un cliente puntual
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteL obtenerClientesCompleto(bool bGST, int numeroCliente, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;
            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getClientesCompleto(bGST, numeroCliente, true, true, true, out PudoGST);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con el Cliente (no deberia ser mas de uno) con sus datos y listas de vehiculos, cuentas y saldos
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 15/06/2010
        // ULT.FEC.MODIF. ..: 15/06/2010
        // OBSERVACIONES ...: Parametros:
        //                          int - numeroCliente - permite filtrar por un cliente puntual
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteL obtenerClientesParcial(bool bGST,
                                                int numeroCliente,
                                               bool incluirListaVehiculos,
                                               bool incluirListaCuentas,
                                               bool incluirListaSaldos,
                                               out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getClientesCompleto(bGST, numeroCliente, incluirListaVehiculos, incluirListaCuentas, incluirListaSaldos, out PudoGST);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Agrega un Cliente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 05/10/2010
        // ULT.FEC.MODIF. ..: 05/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Cliente - oCliente - Datos del Cliente
        //                    Retorno: Numero de Cliente agregado
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public int agregarCliente(Cliente oCliente,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.addCliente(oCliente);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return -1;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Modifica un Cliente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 05/10/2010
        // ULT.FEC.MODIF. ..: 05/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Cliente - oCliente - Datos del Cliente
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool guardarCliente(Cliente oCliente,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.updCliente(oCliente);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);

                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Elimina un Cliente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 05/10/2010
        // ULT.FEC.MODIF. ..: 05/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Cliente - oCliente - Datos del Cliente
        //                          bool - confirmado - Indica que se confirmó la FK
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool eliminarCliente(Cliente oCliente, bool confirmado,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.delCliente(oCliente, confirmado, "\n");

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);

                return false;
            }
        }

        //Retorma el titulo del reporte
        [OperationContract]
        public string imprimirClienteDetalle(int numeroCliente, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return reporteClienteDetalle(numeroCliente, "PDFo");

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        //Retorma el titulo del reporte
        private string reporteClienteDetalle(int numeroCliente, string tipo)
        {
            //Los datos los salva en la sesión para que el reporte lo vea
            List<DataSet> dsl = new List<DataSet>();
            dsl.Add(RptClienteBs.getDetalleClienteCabecera(numeroCliente));
            dsl.Add(RptClienteBs.getDetalleClienteVehiculos(numeroCliente));
            dsl.Add(RptClienteBs.getDetalleClienteCuentas(numeroCliente));

            //SalvarDataSetSchema(ds, "c:\\FUENTES\\Peaje.WEB\\Reportes\\App_Code\\DataSets\\");

            string titulo = Traduccion.Traducir("Detalle del Cliente") + " " + numeroCliente.ToString();
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", titulo);
            //Directo en PDF
            VerReporte(titulo, dsl, pars, tipo, "~/Reportes/rptDetalleDeCliente.rdlc");

            return titulo;
        }

        #endregion

        #region CUENTA: Servicio para acceso a datos de Cuentas

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Agrega un Cliente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/10/2010
        // ULT.FEC.MODIF. ..: 08/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Cuenta - oCuenta - Datos de la Cuenta
        //                    Retorno: Numero de Cuenta agregado
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public int agregarCuenta(Cuenta oCuenta,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.addCuenta(oCuenta);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return -1;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Modifica una Cuenta
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/10/2010
        // ULT.FEC.MODIF. ..: 08/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Cuenta - oCuenta - Datos de la Cuenta
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool guardarCuenta(Cuenta oCuenta,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.updCuenta(oCuenta);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);

                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Elimina una Cuenta
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/10/2010
        // ULT.FEC.MODIF. ..: 08/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Cuenta - oCuenta - Datos de la Cuenta
        //                          bool - confirmado - Indica que se confirmó la FK
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool eliminarCuenta(Cuenta oCuenta, bool confirmado,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.delCuenta(oCuenta, confirmado, "\n");

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);

                return false;
            }
        }

        #endregion

        #region VEHICULO: Servicio para acceso a datos de Vehiculos
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con los datos de un vehiculo y todos sus datos asociados (Cliente,
        //                    cuenta, tag, chip, etc)
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 13/05/2010
        // ULT.FEC.MODIF. ..: 13/05/2010
        // OBSERVACIONES ...: Parametros:
        //                  string - patente - permite filtrar por una patente determinada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoSLL obtenerVehiculoSLPatenteEstacion(string patente, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

               return ClienteBS.getVehiculosPatenteEstacion(patente,null).ConvertEnSL();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }



        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con los datos de un vehiculo y todos sus datos asociados (Cliente,
        //                    cuenta, tag, chip, etc)
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 13/05/2010
        // ULT.FEC.MODIF. ..: 13/05/2010
        // OBSERVACIONES ...: Parametros:
        //                  string - patente - permite filtrar por una patente determinada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoL obtenerVehiculoPatenteEstacion(string patente, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getVehiculosPatenteEstacion(patente,null);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con los datos de un vehiculo y todos sus datos asociados (Cliente,
        //                    cuenta, tag, chip, etc)
        //                    Se puede filtrar por nombre de cliente
        //                    Se agrega un vehiculo trucho por cada cuenta que puede tener tarjeta chip no asociada a un vehiculo
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 13/05/2010
        // ULT.FEC.MODIF. ..: 13/05/2010
        // OBSERVACIONES ...: Parametros:
        //                  string - patente - permite filtrar por una patente determinada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoSLL obtenerVehiculoSLPatenteEstacionCliente(string patente, string nombre, out errorCapaSilver exout , out bool llegoAlTope)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getVehiculosPatenteEstacionCliente(patente, nombre, maxRegistrosVehiculos,400, out llegoAlTope).ConvertEnSL();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                llegoAlTope = false;
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna una lista con los datos de un vehiculo y todos sus datos asociados (Cliente,
        //                    cuenta, tag, chip, etc)
        //                    Se puede filtrar por nombre de cliente
        //                    Se agrega un vehiculo trucho por cada cuenta que puede tener tarjeta chip no asociada a un vehiculo
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 13/05/2010
        // ULT.FEC.MODIF. ..: 13/05/2010
        // OBSERVACIONES ...: Parametros:
        //                  string - patente - permite filtrar por una patente determinada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoL obtenerVehiculoPatenteEstacionCliente(string patente, string nombre, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getVehiculosPatenteEstacionCliente(patente,nombre,null);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Agrega un Vehiculo
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 06/10/2010
        // ULT.FEC.MODIF. ..: 06/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Vehiculo - oVehiculo - Datos del Vehiculo
        //                    Retorno: true OK
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool agregarVehiculo(Vehiculo oVehiculo,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.addVehiculo(oVehiculo);

                return true;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Modifica un Vehiculo
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 06/10/2010
        // ULT.FEC.MODIF. ..: 06/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Vehiculo - oVehiculo - Datos del Vehiculo
        //                    Retorno: true OK
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool guardarVehiculo(Vehiculo oVehiculo,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.updVehiculo(oVehiculo);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);

                return false;
            }
        }
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Elimina un Vehiculo
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 06/10/2010
        // ULT.FEC.MODIF. ..: 06/10/2010
        // OBSERVACIONES ...: Parametros:
        //                          Vehiculo - oVehiculo - Datos del Vehiculo
        //                          bool - confirmado - Indica que se confirmó la FK
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool eliminarVehiculo(Vehiculo oVehiculo, bool confirmado,
                                               out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.delVehiculo(oVehiculo, confirmado, "\n");

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);

                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Modifica la patente de un vehiculo
        // AUTOR ...........: Fernando
        // FECHA CREACIÓN ..: 01/06/2011
        // ULT.FEC.MODIF. ..: 01/06/2011
        // OBSERVACIONES ...: Parametros:
        //                  int? -   codigo estacion
        //                  string - User Id
        //                  int64 -  Cliente
        //                  string - Patente Vieja
        //                  string - Patente Nueva

        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool setPatenteVehiculo(int? intCodEst, string strUsrId, Cliente oCliente, Vehiculo oVehiculo, string strNuevaPatente, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.setPatenteVehiculo(intCodEst,strUsrId,oCliente,oVehiculo,strNuevaPatente);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Modifica un vehiculo del cliente
        // AUTOR ...........: Fernando
        // FECHA CREACIÓN ..: 03/06/2011
        // ULT.FEC.MODIF. ..: 03/06/2011
        // OBSERVACIONES ...: Parametros:
        //                  int? -   codigo estacion
        //                  string - User Id
        //                  Cliente -  Cliente
        //                  Vehiculo - Vehiculo
        //                  string - Patente Nueva
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        /*
         @Coest TINYINT = NULL,
        @UsrId VARCHAR(10) = NULL,
        @Cliente INT = NULL,
        @OldPaten VARCHAR(8) = NULL,
        @OldDescMarca VARCHAR(30) = NULL,
        @OldDescModelo VARCHAR(30) = NULL,
        @OldDescColor VARCHAR(30) = NULL,
        @OldCatego VARCHAR(30) = NULL,
        @NewPaten VARCHAR(8) = NULL,
        @NewDescMarca VARCHAR(30) = NULL,
        @NewDescModelo VARCHAR(30) = NULL,
        @NewDescColor VARCHAR(30) = NULL,
        @NewCatego VARCHAR(30) = NULL,
        @NewFecVenc DATETIME = NULL,
        @NewCodMarca INT = NULL,
        @NewCodModelo INT = NULL,
        @NewCodColor INT = NULL,
         */
        public bool setVehiculo(int? nCoest, string sUsrId, Cliente oCliente, Vehiculo oVehiculo, Vehiculo oNewVehiculo, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteBS.setVehiculo(nCoest, sUsrId, oCliente, oVehiculo, oNewVehiculo);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        #endregion

        #region TURNOSTRABAJO: Servicio para consulta de turnos de trabajo definidos

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Setea la terminal fisica de facturacion a la terminal logica indicada
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 11/01/2010
        // ULT.FEC.MODIF. ..: 11/01/2010
        // OBSERVACIONES ...: Parametros:
        //                      terminal (short): Terminal logica que asigno a la fisica 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TurnoTrabajoL obtenerTurnosTrabajo(out DateTime jornadaActual, out int turnoActual, out errorCapaSilver exout)
        {
            exout = null;

            TurnoTrabajoL oTurnos = new TurnoTrabajoL();
            jornadaActual = DateTime.Today;
            turnoActual = 1;

            try
            {
                ConexionBs.verificarSesion();

                oTurnos = EstacionConfiguracionBs.getTurnosTrabajo();

                oTurnos.FindTurnoActual(TurnoTrabajo.getToleranciaAsignacionPlaza(), out jornadaActual, out turnoActual);

                return oTurnos;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        #endregion

        #region TERMINALES: Servicio para consulta de terminales de facturacion

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de terminales de facturacion
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 08/01/2010
        // ULT.FEC.MODIF. ..: 08/01/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TerminalFacturacionL obtenerTerminalesFacturacion(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ClienteFacturacionBs.getTerminales();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region LECTORES DE TARJETAS CHIP: Servicio para consulta de lectores de tarjetas chip

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de lectores de tarjetas chip
        // AUTOR ...........: Damián Gurski
        // FECHA CREACIÓN ..: 02/02/2010
        // ULT.FEC.MODIF. ..: 02/02/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public LectorChipL obtenerLectoresChip(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ClienteFacturacionBs.getLectoresChip();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region CONFIGURACIONCLIENTEFACTURACION: Servicio para consulta de la configuracion del cliente de facturacion

        [OperationContract]
        public void obtenerDatosEmpresa(out string nombre, out string logo, out string logoNombre)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Retorna el nombre de la empresa (Telectronica o Telsis) y los logos
            // AUTOR ...........: Damian Jachniuk
            // FECHA CREACIÓN ..: 20/08/2010
            // ULT.FEC.MODIF. ..: 
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------
            nombre = ConexionBs.getNombreEmpresa();
            logo = ConexionBs.getLogoEmpresa();
            logoNombre = ConexionBs.getLogoNombreEmpresa();

        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna un objeto con la configuracion del cliente de facturacion
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 18/12/2009
        // ULT.FEC.MODIF. ..: 18/12/2009
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ConfiguracionClienteFacturacion obtenerConfiguracionClienteFacturacion(out errorCapaSilver exout)
        {
            ConfiguracionClienteFacturacion oConfig = null;

            exout = null;
            try
            {
                ConexionBs.verificarSesion();
                //False = gestion
                oConfig = ClienteFacturacionBs.getConfiguracionClienteFacturacion(false); 

                // Version del sistema
                oConfig.VersionCliente = "Ver. 1.0.2SP2";

                // Nombre del servidor web
                oConfig.NombreServidorWeb = Convert.ToString(WebConfigurationManager.AppSettings["nombreServidorWeb"]);

                //Fecha y Hora del Servidor
                oConfig.FechayHoraServidor = DateTime.Now;

                return oConfig;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                //Para que aunque de un error no me quede mal la fecha
                //Fecha y Hora del Servidor
                if (oConfig != null)
                    oConfig.FechayHoraServidor = DateTime.Now;

                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Abre la terminal de facturacion colocanco al vendedor a cargo de la misma
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 08/01/2010
        // ULT.FEC.MODIF. ..: 08/01/2010
        // OBSERVACIONES ...: Parametros:
        //                      jornada(datetime): Jornada contable que se asigna
        //                      turno(int): Junto con la jornada conforman el parte a asignar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool SetVendedorACargo(DateTime jornada, int turno, TerminalFacturacion oTerminal, out errorCapaSilver exout, out bool bMismoParteAnterior)
        {
            exout = null;
            bMismoParteAnterior = false;

            try
            {
                ConexionBs.verificarSesion();

                Parte oParte = new Parte(0, jornada, turno);
                oParte.Estacion = new Estacion(ConexionBs.getNumeroEstacion(), "");
                oParte.Peajista = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());


                ClienteFacturacionBs.setVendedorACargo(oParte, oTerminal, out bMismoParteAnterior);

                return true;
            }


            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Cierra la terminal de facturacion actualmente asignada
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 08/01/2010
        // ULT.FEC.MODIF. ..: 08/01/2010
        // OBSERVACIONES ...: Parametros:
        //                      propia(bool): Si la terminal que se intenta cerrar esta abierta por el mismo 
        //                                    usuario que se loguea.
        //                      confirmado(bool): En caso de necesitar confirmacion (anulacion de operaciones
        //                                        pendientes) se vuelve a llamar a este metodo pero confirmado                        
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool CerrarTerminal(bool propia, bool confirmado, Parte oParte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ClienteFacturacionBs.CerrarTerminal(propia, confirmado, oParte);
                return true;
            }

            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Setea la terminal fisica de facturacion a la terminal logica indicada
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 11/01/2010
        // ULT.FEC.MODIF. ..: 11/01/2010
        // OBSERVACIONES ...: Parametros:
        //                      terminal (short): Terminal logica que asigno a la fisica 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool SetearTerminalLogica(short terminal, LectorChip lectorChip, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                //Seteamos la terminal actual
                ClienteFacturacionBs.updTerminalActual(ConexionBs.getNumeroEstacion(), terminal, lectorChip);

                return true;
            }

            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        #endregion

        #region TAGS: Servicio para consulta de Tags y operaciones relacionadas a Tags

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de entregas de tags no facturadas 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 05/05/2010
        // ULT.FEC.MODIF. ..: 05/05/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EntregaTagL obtenerEntregasTagsNoFacturadas(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return TagBS.getEntregasNoFacturadas(parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna el numero de Tag correspondiente a la patente ingresada 
        // AUTOR ...........: Germán Mauro
        // FECHA CREACIÓN ..: 01/04/2015
        // ULT.FEC.MODIF. ..: 01/04/2015
        // OBSERVACIONES ...: Parametros: parte (int) - Número de patente buscada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Tag getTagPatente(string patente, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return TagBS.getTagPatente(patente);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        /*
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de entregas de tags no facturadas que coinciden con la patente y el tag
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 08/05/2010
        // ULT.FEC.MODIF. ..: 08/05/2010
        // OBSERVACIONES ...: Parametros: patente (string) - Patente para la cual deseo consultar las entregas
        //                                tag (string) - Tag para el cual deseo consultar las entregas                      
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EntregaTagL obtenerEntregasTagsNoFacturadasValid(string patente, string tag, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return TagBS.getEntregasNoFacturadasValid(patente, tag);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }
        */

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la entrega de tags
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 04/06/2010
        // ULT.FEC.MODIF. ..: 04/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaTag): Objeto de la entrega a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string GuardarEntregaTag(EntregaTag oEntrega, RecargaSupervision oRecarga, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                //TagBS.GuardarEntrega(oEntrega, oRecarga);

                return reporteEntregaTag(oEntrega, "PDFo");

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: MUESTRA EL REPORTE
        // AUTOR ...........: Alvaro Cardenas
        // FECHA CREACIÓN ..: 07/09/2011
        // ULT.FEC.MODIF. ..: 07/09/2011
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaChip): Objeto de la entrega a grabar
        // RETORNA .........: dispositivo
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string ReporteEntregaTag(EntregaTag oEntrega, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return reporteEntregaTag(oEntrega, "PDFo");
                
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula una entrega de tags
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 08/06/2010
        // ULT.FEC.MODIF. ..: 08/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaTag): Objeto de la entrega a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularEntregaTag(EntregaTag oEntrega, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                TagBS.AnularEntrega(oEntrega);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula un tag
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/10/2010
        // ULT.FEC.MODIF. ..: 08/10/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTag(Tag): Objeto del tag a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularTag(Tag oTag, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                TagBS.AnularTag(oTag);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        //Retorma el titulo del reporte
        private string reporteEntregaTag(EntregaTag oEntrega, string tipo)
        {
            //Los datos los salva en la sesión para que el reporte lo vea
            DataSet ds = TagBS.getEntregaDetalle(oEntrega, 2); // Por duplicado

            SalvarDataSetSchema(ds, "c:\\FUENTES\\Peaje.WEB\\Reportes\\App_Code\\DataSets\\");

            // abrir el fichero, que se llamará prueba.txt
            string fichero = HttpContext.Current.Server.MapPath("~/Reportes/rptAdhesionTag.txt");
            System.IO.StreamReader sr = new System.IO.StreamReader(fichero);
            string textoContrato = sr.ReadToEnd();

            string titulo = Traduccion.Traducir("Entrega de Tag") + " " + oEntrega.Patente + " " + oEntrega.NumeroTag;
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", titulo);
            pars.Add("Texto_Contrato", textoContrato);
            //Directo en PDF
            VerReporte(titulo, ds, pars, tipo, "~/Reportes/rptAdhesionTag.rdlc");

            return titulo;
        }
        #endregion

        #region TAGLISTANEGRA: Servicio para consulta de estado de Lista Negra de los Tags
        [OperationContract]
        public bool GrabarTagListaNegra(TagListaNegra oTagListaNegra, out errorCapaSilver exout)
        {
            exout = null;
            try
            {
                ConexionBs.verificarSesion();
                TagBS.GuardarTagListaNegra(oTagListaNegra);
                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }

        }

        [OperationContract]
        public bool EliminarTagListaNegra(TagListaNegra oTagListaNegra, out errorCapaSilver exout)
        {
            exout = null;
            try
            {
                ConexionBs.verificarSesion();
                TagBS.EliminarTagListaNegra(oTagListaNegra);
                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }

        }
        
        /*

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de Tags en Lista negra que coindican con el pedido
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 03/06/2010
        // ULT.FEC.MODIF. ..: 03/06/2010
        // OBSERVACIONES ...: Parametros: numeroTag (string) - Numero de tag que deseo consultar el estado de LN
        //                                bGestion (bool) - Se puede forzar la busqueda de los datos en Gestion o Estacion.
        //                                                  (por defecto el metodo busca en la base de datos del ambito en que esta)                        
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TagListaNegraL obtenerTagsListaNegra(bool bGestion, string numeroTag, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return TagBS.getTagListaNegra(bGestion, numeroTag);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        */
        #endregion

        #region CHIPLISTANEGRA: Servicio para consulta de estado de Lista Negra de los Chips
        [OperationContract]
        public bool GrabarChipListaNegra(ChipListaNegra oChipListaNegra, out errorCapaSilver exout)
        {
            exout = null;
            try
            {
                ConexionBs.verificarSesion();
                ChipBS.GuardarChipListaNegra(oChipListaNegra);
                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }

        }

        [OperationContract]
        public bool EliminarChipListaNegra(ChipListaNegra oChipListaNegra, out errorCapaSilver exout)
        {
            exout = null;
            try
            {
                ConexionBs.verificarSesion();
                ChipBS.EliminarChipListaNegra(oChipListaNegra);
                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }

        }

        /*

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de Tags en Lista negra que coindican con el pedido
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 03/06/2010
        // ULT.FEC.MODIF. ..: 03/06/2010
        // OBSERVACIONES ...: Parametros: numeroTag (string) - Numero de tag que deseo consultar el estado de LN
        //                                bGestion (bool) - Se puede forzar la busqueda de los datos en Gestion o Estacion.
        //                                                  (por defecto el metodo busca en la base de datos del ambito en que esta)                        
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TagListaNegraL obtenerTagsListaNegra(bool bGestion, string numeroTag, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return TagBS.getTagListaNegra(bGestion, numeroTag);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        */
        #endregion

        #region CHIPS: Servicio para consulta de Chips y operaciones relacionadas a Chips

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de entregas de chips no facturadas 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 09/06/2010
        // ULT.FEC.MODIF. ..: 09/06/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EntregaChipL obtenerEntregasChipsNoFacturadas(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ChipBS.getEntregasNoFacturadas(parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        /*
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de entregas de chips no facturadas que coinciden con la patente y la tarjeta chip
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 08/05/2010
        // ULT.FEC.MODIF. ..: 08/05/2010
        // OBSERVACIONES ...: Parametros: patente (string) - Patente para la cual deseo consultar las entregas
        //                                nChip (string) - Numero de tarjeta chip para la cual deseo consultar las entregas                      
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EntregaChipL obtenerEntregasChipsNoFacturadasValid(string patente, int nChip, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ChipBS.getEntregasNoFacturadasValid(patente, nChip);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }
        */

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la entrega de chips
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 09/06/2010
        // ULT.FEC.MODIF. ..: 09/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaChip): Objeto de la entrega a grabar
        // RETORNA .........: Numero de tarjeta grabada
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string GuardarEntregaChip(EntregaChip oEntrega, Vehiculo oVehiculo, RecargaSupervision oRecarga, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                //ChipBS.GuardarEntrega(oEntrega, oVehiculo, oRecarga);

                reporteEntregaChip(oEntrega, "PDFo");

                return oEntrega.Dispositivo;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: MUESTRA EL REPORTE
        // AUTOR ...........: ariel stendel
        // FECHA CREACIÓN ..: 08/08/2011
        // ULT.FEC.MODIF. ..: 08/08/2011
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaChip): Objeto de la entrega a grabar
        // RETORNA .........: dispositivo
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string ReporteEntregaChip(EntregaChip oEntrega, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                 reporteEntregaChip(oEntrega, "PDFo");

                return oEntrega.Dispositivo;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }



        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula una entrega de chip
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 09/06/2010
        // ULT.FEC.MODIF. ..: 09/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaChip): Objeto de la entrega a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularEntregaChip(EntregaChip oEntrega, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ChipBS.AnularEntrega(oEntrega);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula un chip
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/10/2010
        // ULT.FEC.MODIF. ..: 08/10/2010
        // OBSERVACIONES ...: Parametros:
        //                      oChip(Chip): Objeto del chip a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularChip(Chip oChip, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ChipBS.AnularChip(oChip);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna el proximo numero interno de tarjeta chip a generar
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 10/06/2010
        // ULT.FEC.MODIF. ..: 10/06/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string obtenerProximoNumeroTarjetaChip(out errorCapaSilver exout)
        {
            string ProxChip;
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ProxChip = ChipBS.getProximoNumeroTarjeta();

                return ProxChip;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        //GENERA EL REPORTE DE ENTREGA CHIP
        private string reporteEntregaChip(EntregaChip oEntrega, string tipo)
        {
            //Los datos los salva en la sesión para que el reporte lo vea
            DataSet ds = ChipBS.getEntregaDetalle(oEntrega, 2);     // Por duplicado

            SalvarDataSetSchema(ds, "c:\\FUENTES\\Peaje.WEB\\Reportes\\App_Code\\DataSets\\");
            // abrir el fichero, que se llamará prueba.txt
            string fichero = HttpContext.Current.Server.MapPath("~/Reportes/rptAdhesionChip.txt");
            System.IO.StreamReader sr = new System.IO.StreamReader(fichero);
            string textoContrato = sr.ReadToEnd();
            string titulo = Traduccion.Traducir("Entrega de Chip") + " " + oEntrega.Patente + " " + oEntrega.NumeroExterno;
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", titulo);
           pars.Add("Texto_Contrato", textoContrato);
            //Directo en PDF
            VerReporte(titulo, ds, pars, tipo, "~/Reportes/rptAdhesionChip.rdlc");

            return titulo;
        }

        [OperationContract]
        public string PuedeEntregarChip(EntregaChip oEntrega, out errorCapaSilver exout)
        {
            exout = null;
            string causa = "";

            try
            {

                ConexionBs.verificarSesion();
                ChipBS.PuedeEntregarChip(oEntrega, out causa);

                return causa;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return causa;
            }
        }

        #endregion

        #region VINCULACION_CHIPS: Servicio para consulta de Chips y operaciones relacionadas a Vinculacion de Chips
        /*
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de vinculaciones de chips no facturadas 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 10/06/2010
        // ULT.FEC.MODIF. ..: 10/06/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EntregaChipL obtenerVinculacionesChipsNoFacturadas(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ChipBS.getVinculacionesNoFacturadas(parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de vinculaciones de chips no facturadas que coinciden con la patente y la tarjeta chip
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 10/05/2010
        // ULT.FEC.MODIF. ..: 10/05/2010
        // OBSERVACIONES ...: Parametros: patente (string) - Patente para la cual deseo consultar las Vinculaciones
        //                                nChip (string) - Numero de tarjeta chip para la cual deseo consultar las Vinculaciones
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EntregaChipL obtenerVinculacionesChipsNoFacturadasValid(string patente, int nChip, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ChipBS.getVinculacionesNoFacturadasValid(patente, nChip);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la vinculacion de chip
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 10/06/2010
        // ULT.FEC.MODIF. ..: 10/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaChip): Objeto de la Vinculacion a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool GuardarVinculacionChip(EntregaChip oEntrega, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                // A traves de este metodo hacemos ambas operaciones, grabamos la vinculacion en la estacion
                // y la habilitacion de la tarjeta chip en Gestion

                ChipBS.GuardarVinculacion(oEntrega);



                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula una Vinculacion de chip
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 10/06/2010
        // ULT.FEC.MODIF. ..: 10/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oEntrega(EntregaChip): Objeto de la Vinculacion a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularVinculacionChip(EntregaChip oEntrega, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ChipBS.AnularVinculacion(oEntrega);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        */
        #endregion

        #region MEDIOSPAGO: Servicio para consulta de Medios de Pago

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de medios de pago para un tipo de dispositivo
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 05/05/2010
        // ULT.FEC.MODIF. ..: 05/05/2010
        // OBSERVACIONES ...: Parametros: tipoDispositivo (string) - Tipo de dispositivo que se quiere consultar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public MedioPagoConfiguracionL obtenerMediosPago(string tipoDispositivo, string tipoBoleto, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return MedioPagoBs.getMedioPago(tipoDispositivo, tipoBoleto);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region RECARGAS_SUPERVISION: Servicio para consulta de Recargas de supervision

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de recargas de supervision no facturadas 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 14/06/2010
        // ULT.FEC.MODIF. ..: 14/06/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public RecargaSupervisionL obtenerRecargasNoFacturadas(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return PrepagoBS.getRecargasNoFacturadas(parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la recarga de prepago
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 14/06/2010
        // ULT.FEC.MODIF. ..: 14/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oRecarga(RecargaSupervision): Objeto de la recarga a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool GuardarRecarga(RecargaSupervision oRecarga, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                PrepagoBS.GuardarRecarga(oRecarga);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula una recarga de Prepago
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 14/06/2010
        // ULT.FEC.MODIF. ..: 14/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oRecarga(RecargaSupervision): Objeto de la recarga a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularRecarga(RecargaSupervision oRecarga, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                PrepagoBS.AnularRecarga(oRecarga);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        #endregion

        #region ESTACIONES: Servicio para consulta de Estaciones

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Estaciones definidos
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 07/10/2010
        // ULT.FEC.MODIF. ..: 07/10/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public EstacionL obtenerEstaciones(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            EstacionL oEstaciones = new EstacionL();
            try
            {
                ConexionBs.verificarSesion();

                oEstaciones = EstacionBs.getEstaciones(bGST, null, null, out PudoGST);

                return oEstaciones;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        #endregion

        #region TIPOSDOCUMENTO: Servicio para consulta de Tipos de Documento definidos

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Tipos de Documento definidos
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 16/06/2010
        // ULT.FEC.MODIF. ..: 16/06/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TipoDocumentoL obtenerTiposDocumento(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            TipoDocumentoL oTiposDoc = new TipoDocumentoL();
            try
            {
                ConexionBs.verificarSesion();

                oTiposDoc = ClienteBS.getTiposDocumento(bGST, null, out PudoGST);

                return oTiposDoc;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        #endregion

        #region TIPOSIVA: Servicio para consulta de Tipos de IVA definidos

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Tipos de IVA definidos
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 20/09/2010
        // ULT.FEC.MODIF. ..: 20/09/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TipoIVAL obtenerTiposIVA(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            TipoIVAL oTipos = new TipoIVAL();
            try
            {
                ConexionBs.verificarSesion();

                oTipos = ClienteBS.getTiposIVA(bGST, null, out PudoGST);

                return oTipos;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        #endregion

        #region PROVINCIAS: Servicio para consulta de Provincias

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Provincias
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 23/09/2010
        // ULT.FEC.MODIF. ..: 23/09/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ProvinciaL obtenerProvincias(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            ProvinciaL oProvincias = new ProvinciaL();
            try
            {
                ConexionBs.verificarSesion();

                oProvincias = ClienteBS.getProvincias(bGST, out PudoGST);

                return oProvincias;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        #endregion

        #region MONEDAS: Servicio para consulta de Monedas

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la moneda de referencia
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 17/06/2010
        // ULT.FEC.MODIF. ..: 17/06/2010
        // OBSERVACIONES ...: Parametros: 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Moneda obtenerMonedaReferencia(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return MonedaBs.getMonedaReferencia();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region CATEGORIAS: Servicio para acceso a datos de Categorias

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de categorias definidas        
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 23/06/2010
        // ULT.FEC.MODIF. ..: 23/06/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public CategoriaManualL obtenerCategoriasManuales(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return CategoriaBs.getCategorias(bGST, null, false, out PudoGST);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region MARCAS_MODELOS_COLORES: Servicio para acceso a datos de Marcas Modelos y Colores

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de marcas definidas        
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 05/10/2010
        // ULT.FEC.MODIF. ..: 05/10/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoMarcaL obtenerMarcas(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getMarcas(bGST, null, out PudoGST);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de modelos definidos        
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 05/10/2010
        // ULT.FEC.MODIF. ..: 05/10/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoModeloL obtenerModelos(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getModelos(bGST, null, null, out PudoGST);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de colores definidos        
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 05/10/2010
        // ULT.FEC.MODIF. ..: 05/10/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public VehiculoColorL obtenerColores(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            try
            {
                ConexionBs.verificarSesion();

                return ClienteBS.getColores(bGST, null, out PudoGST);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region TIPOSCUENTA - AGRUPACION: Servicio para consulta de Tipos y Agrupaciones de Cuentas

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Tipos de Cuenta definidos
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 07/10/2010
        // ULT.FEC.MODIF. ..: 07/10/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TipoCuentaL obtenerTiposCuenta(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            TipoCuentaL oTipos = new TipoCuentaL();
            try
            {
                ConexionBs.verificarSesion();

                oTipos = ClienteBS.getTipoCuenta(bGST, out PudoGST, false);

                return oTipos;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Agrupaciones de Cuenta definidos
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 07/10/2010
        // ULT.FEC.MODIF. ..: 07/10/2010
        // OBSERVACIONES ...: Parametros:
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public AgrupacionCuentaL obtenerAgrupacionesCuenta(bool bGST, out errorCapaSilver exout, out bool PudoGST)
        {
            exout = null;
            PudoGST = false;

            AgrupacionCuentaL oTipos = new AgrupacionCuentaL();
            try
            {
                ConexionBs.verificarSesion();

                oTipos = ClienteBS.getAgrupacionesCuentas(bGST, null, null, out PudoGST);

                return oTipos;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        #endregion

        #region VALESPREPAGOS_VENTA: Servicio para consulta de Venta de Vales Prepagos y operaciones relacionadas a Venta de Vales Prepagos

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de venta de vales prepagos no facturados 
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 24/06/2010
        // ULT.FEC.MODIF. ..: 24/06/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ValePrepagoVentaL obtenerVentaValesPrepagosNoFacturadas(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ValePrepagoBS.getVentaValesNoFacturadas(parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Varifica que los vales prepagos se puedan vender y devuelve precio, cuenta y estaciones habilitadas
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/09/2010
        // ULT.FEC.MODIF. ..: 08/09/2010
        // OBSERVACIONES ...: Parametros: cliente (int) - Numero de cliente para el cual fueron personalizados los vales
        //                                serieDesde (int?) - Numero de Serie inicial a buscar
        //                                serieHasta (int?) - Numero de Serie final a buscar
        //                                categoria (byte) - Numero de categoria del vale 
        //                                zona (byte) - Codigo de zona para la cual se personalizo el vale
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public decimal obtenerPrecioTandaValesPrepagos(int cliente, int serieDesde,
                                                              int serieHasta, byte categoria,
                                                              out Cuenta cuenta,
                                                              out string estacionesHabilitadas,
                                                              out errorCapaSilver exout)
        {
            exout = null;
            estacionesHabilitadas = "";
            cuenta = null;

            try
            {
                ConexionBs.verificarSesion();
                return ValePrepagoBS.getPrecioTandaVales(cliente, serieDesde, serieHasta, categoria, out cuenta, out estacionesHabilitadas);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return 0;
            }
        }

        /*
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de vales prepagos vendidos
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 29/06/2010
        // ULT.FEC.MODIF. ..: 29/06/2010
        // OBSERVACIONES ...: Parametros: cliente (int) - Numero de cliente para el cual fueron personalizados los vales
        //                                serieDesde (int?) - Numero de Serie inicial a buscar
        //                                serieHasta (int?) - Numero de Serie final a buscar
        //                                categoria (byte) - Numero de categoria del vale 
        //                                zona (byte) - Codigo de zona para la cual se personalizo el vale
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ValePrepagoVentaL obtenerValesPrepagosVendidos(int cliente,      int serieDesde, 
                                                              int serieHasta,   byte categoria, 
                                                              out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ValePrepagoBS.getVentaValesVendidos(cliente, serieDesde, serieHasta, categoria);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }
        */

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la venta de vales prepagos
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 24/06/2010
        // ULT.FEC.MODIF. ..: 24/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oVales(ValesPrepagosL): Lista de vales a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool GuardarVentaValesPrepagos(ValePrepagoVentaL oVales, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ValePrepagoBS.GuardarVentaValesPrepagos(oVales);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula una venta de Vale Prepago
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 24/06/2010
        // ULT.FEC.MODIF. ..: 24/06/2010
        // OBSERVACIONES ...: Parametros:
        //                      oVale(ValePrepago): Objeto de la venta de vale a eliminar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularVentaVale(ValePrepagoVenta oVale, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                ValePrepagoBS.AnularVentaVale(oVale);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        /*
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de habilitaciones de estaciones de un lote y su precio para ser vendidos
        // AUTOR ...........: Jorge Luis Bongianino
        // FECHA CREACIÓN ..: 30/06/2010
        // ULT.FEC.MODIF. ..: 30/06/2010
        // OBSERVACIONES ...: Parametros: lote (int) - Numero de lote que se consultar la habilitacion
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ValePrepagoHabilitacionL obtenerHabilitacionVentaVale(int lote, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return ValePrepagoBS.getHabilitacionVentaVale(lote);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }
        */

        #endregion

        #region PEAJISTAS

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de peajistas
        // AUTOR ...........: Ezequiel Alberto Ramirez
        // FECHA CREACIÓN ..: 07/01/2011
        // ULT.FEC.MODIF. ..: 07/01/2011
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de estacion en el cual esta trabajando 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public UsuarioL ObtenerPeajistas(int iEstacion, out errorCapaSilver exout)
        {
            exout = null;
            UsuarioL oUsuarios = new UsuarioL();

            try
            {
                if (iEstacion > 0)
                    oUsuarios = UsuarioBs.getUsuariosEstPeajista(iEstacion);
                else
                    oUsuarios = UsuarioBs.getUsuarios();

                return oUsuarios;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        #endregion

        #region SESION
        [OperationContract]
        public string obtenerDatosUsuario()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: devuelve los datos del usuario necesarios para iniciar una nueva sesión
            // AUTOR ...........: Damián Jachniuk
            // FECHA CREACIÓN ..: 19/08/2010
            // ULT.FEC.MODIF. ..: 
            // OBSERVACIONES ...: Parametros:
            // ----------------------------------------------------------------------------------------------

            return ConexionBs.getDatosUsuario();

        }

        [OperationContract]
        public bool obtenerVerificarGST()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: devuelve true si la conexion con GST está OK
            // AUTOR ...........: Damián Jachniuk
            // FECHA CREACIÓN ..: 04/10/2010
            // ULT.FEC.MODIF. ..: 
            // OBSERVACIONES ...: Parametros:
            // ----------------------------------------------------------------------------------------------
            bool bRet = false;
            try
            {
                ConexionBs.verificarGST();
                bRet = true;
            }
            catch (Exception ex)
            {
            }
            return bRet;
        }

        [OperationContract]
        public bool obtenerVerificarSesion()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: devuelve true si la sesion está OK
            // AUTOR ...........: Damián Jachniuk
            // FECHA CREACIÓN ..: 19/08/2010
            // ULT.FEC.MODIF. ..: 
            // OBSERVACIONES ...: Parametros:
            // ----------------------------------------------------------------------------------------------
            return (ConexionBs.getUsuario() != null);
        }

        [OperationContract]
        public bool iniciarSesion(string datosUsuario)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Inicia de nuevo la sesion
            // AUTOR ...........: Damián Jachniuk
            // FECHA CREACIÓN ..: 19/08/2010
            // ULT.FEC.MODIF. ..: 
            // OBSERVACIONES ...: Parametros:
            //                    Datos del Usuario
            // ----------------------------------------------------------------------------------------------
            string usuario = "", password = "";
            ConexionBs.getUsuarioPassword(datosUsuario, ref usuario, ref password);

            return (UsuarioBs.ValidarLogin(usuario, password) != null);

        }

        #endregion

        #region FACTURAS
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de facturas del parte 
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 15/09/2010
        // ULT.FEC.MODIF. ..: 15/09/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Telectronica.Facturacion.FacturaL obtenerFacturasParte(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getFacturasParte((byte)ConexionBs.getNumeroEstacion(), parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de Tipos de Factura
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 17/09/2010
        // ULT.FEC.MODIF. ..: 17/09/2010
        // OBSERVACIONES ...: Parametros: 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public TipoFacturaL obtenerTiposFactura(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getTiposFactura();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de Impuestos
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 23/09/2010
        // ULT.FEC.MODIF. ..: 23/09/2010
        // OBSERVACIONES ...: Parametros: 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Impuesto obtenerImpuestos(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getImpuestos();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna numero factura 
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 17/09/2010
        // ULT.FEC.MODIF. ..: 17/09/2010
        // OBSERVACIONES ...: Parametros: tipoFactura TipoFactura
        //                                impresora int
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public int obtenerNumeroFactura(TipoFactura tipoFactura, int impresora, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getNumeroFactura(tipoFactura, impresora);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return 0;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de Cliente con Facturacion Pendiente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 15/09/2010
        // ULT.FEC.MODIF. ..: 15/09/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public ClienteL obtenerClientesNoFacturados(int parte, char IncluyeFallos, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getClientesNoFacturados(parte, IncluyeFallos);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de items no facturadas de un cliente 
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 17/09/2010
        // ULT.FEC.MODIF. ..: 17/09/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        //                                cliente (Cliente) - CLiente para el que se va a facturar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public FacturaItemL obtenerItemsNoFacturados(int parte, Cliente cliente, char IncluyeFallos,  out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getItemsNoFacturados(parte, cliente, IncluyeFallos);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de items de una factura
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 17/09/2010
        // ULT.FEC.MODIF. ..: 17/09/2010
        // OBSERVACIONES ...: Parametros: factura (Factura) - Factura de la que se quieren los items
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public FacturaItemL obtenerItemsFacturados(Telectronica.Facturacion.Factura factura, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getItemsFacturados(factura);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la Factura
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 21/09/2010
        // ULT.FEC.MODIF. ..: 21/09/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTerminal(TerminalFacturacion): Objeto de la Terminal donde se factura
        //                      oFactura(Factura): Objeto de la factura a grabar
        //                    Retorna titulo del reporte de la factura
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string GuardarFactura(TerminalFacturacion oTerminal, Telectronica.Facturacion.Factura oFactura, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                FacturacionBs.addFactura(oTerminal, oFactura);

                //Imprimimos la factura
                return reporteFactura(oFactura, "PDFo");

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anular la Factura
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 22/09/2010
        // ULT.FEC.MODIF. ..: 21/09/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTerminal(TerminalFacturacion): Objeto de la Terminal donde se factura
        //                      oFactura(Factura): Objeto de la factura a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularFactura(TerminalFacturacion oTerminal, Telectronica.Facturacion.Factura oFactura, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                FacturacionBs.anularFactura(oTerminal, oFactura);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de cobros a cuenta del parte 
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 27/09/2010
        // ULT.FEC.MODIF. ..: 27/09/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public CobroACuentaL obtenerCobrosACuentaParte(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getCobrosACuentaParte((byte)ConexionBs.getNumeroEstacion(), parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de Facturas con Cobro Pendiente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 27/09/2010
        // ULT.FEC.MODIF. ..: 27/09/2010
        // OBSERVACIONES ...: Parametros: 
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Telectronica.Facturacion.FacturaL obtenerFacturasPendientes(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getFacturasPendientes();
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba el Cobro A Cuenta
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 23/09/2010
        // ULT.FEC.MODIF. ..: 23/09/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTerminal(TerminalFacturacion): Objeto de la Terminal donde se factura
        //                      oCobroACuenta(CobroACuenta): Objeto del Cobro A Cuenta a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool GuardarCobroACuenta(TerminalFacturacion oTerminal, CobroACuenta oCobroACuenta, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                FacturacionBs.addCobroACuenta(oTerminal, oCobroACuenta);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Elimina el Cobro A Cuenta
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 23/09/2010
        // ULT.FEC.MODIF. ..: 23/09/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTerminal(TerminalFacturacion): Objeto de la Terminal donde se factura
        //                      oCobroACuenta(CobroACuenta): Objeto del Cobro A Cuenta a anular
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularCobroACuenta(TerminalFacturacion oTerminal, CobroACuenta oCobroACuenta, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                FacturacionBs.delCobroACuenta(oTerminal, oCobroACuenta);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de notas de credito del parte 
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 28/09/2010
        // ULT.FEC.MODIF. ..: 28/09/2010
        // OBSERVACIONES ...: Parametros: parte (int) - Numero de parte en el cual esta trabajando
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public NotaCreditoL obtenerNotasCreditoParte(int parte, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getNotasCreditoParte((byte)ConexionBs.getNumeroEstacion(), parte);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de facturas de un cliente
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 29/09/2010
        // ULT.FEC.MODIF. ..: 29/09/2010
        // OBSERVACIONES ...: Parametros: cliente nombre parcial del cliente
        //                                ParteActual parte que está trabajando (para excluir)
        //                                ultimas cantidad de facturas a traer
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Telectronica.Facturacion.FacturaL obtenerUltimasFacturas(string cliente, Parte parteActual, int ultimas, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getUltimasFacturas(cliente, parteActual.Numero, ultimas);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Retorna la lista de facturas de un cliente entre fechas
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 29/09/2010
        // ULT.FEC.MODIF. ..: 29/09/2010
        // OBSERVACIONES ...: Parametros: cliente nombre parcial del cliente
        //                                desde hasta fechas desde y hasta
        //                                ParteActual parte que está trabajando (para excluir)
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public Telectronica.Facturacion.FacturaL obtenerFacturasPorFecha(string cliente, DateTime desde, DateTime hasta, Parte parteActual, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();
                return FacturacionBs.getFacturasPorFecha(cliente, desde, hasta, parteActual.Numero);
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Graba la Nota de Credito
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 29/09/2010
        // ULT.FEC.MODIF. ..: 29/09/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTerminal(TerminalFacturacion): Objeto de la Terminal donde se factura
        //                      oNotaCredito(NotaCredito): Objeto de la NC a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public string GuardarNotaCredito(TerminalFacturacion oTerminal, NotaCredito oNotaCredito, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                FacturacionBs.addNotaCredito(oTerminal, oNotaCredito);

                //Imprimimos la factura
                return reporteNotaCredito(oNotaCredito, "PDFo");
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Anula la Nota de Credito
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 29/09/2010
        // ULT.FEC.MODIF. ..: 29/09/2010
        // OBSERVACIONES ...: Parametros:
        //                      oTerminal(TerminalFacturacion): Objeto de la Terminal donde se factura
        //                      oNotaCredito(NotaCredito): Objeto de la NC a grabar
        // ----------------------------------------------------------------------------------------------
        [OperationContract]
        public bool AnularNotaCredito(TerminalFacturacion oTerminal, NotaCredito oNotaCredito, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                FacturacionBs.anularNotaCredito(oTerminal, oNotaCredito);

                return true;
            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return false;
            }
        }


        //Retorma el titulo del reporte
        [OperationContract]
        public string imprimirFacturacion(DateTime DtDesde, DateTime DtHasta, string sUsuario, Cliente cliente, string tipo, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                if (tipo == null)
                    tipo = "PDFo";

                return reporteFactura(DtDesde, DtHasta, sUsuario, cliente, tipo);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }


        //Imprime la facturta por impresora serie
        //Dmitriy 13/04/2010
        //[OperationContract]
        //public string ImprimirFactura(FacturaImpresora oFactura)
        //{
        //    string ret = "";

        //    ImpresoraSerie oImpresora = new ImpresoraSerie("COM4");
        //    ImpresoraSerie.Estado estado = oImpresora.ImprimirFactura(oFactura);

        //    //TODO convertira estado a un texto de error
        //    ret = Convert.ToString(estado);
        //    return ret;
        //}

        //Retorma el titulo del reporte
        [OperationContract]
        public string imprimirFactura(Telectronica.Facturacion.Factura oFactura, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return reporteFactura(oFactura, "PDFo");

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        //Retorma el titulo del reporte
        private string reporteFactura(DateTime DtDesde, DateTime DtHasta, string sUsuario, Cliente cliente, string tipo)
        {

            // Armamos los parametros:
            StringBuilder filtro = new StringBuilder();
            string titulo = "Reporte de facturacion";

            filtro.Append(Traduccion.Traducir("Desde") + ":" + DtDesde.ToShortDateString() + ", ");
            filtro.Append(Traduccion.Traducir("Hasta") + ":" + DtHasta.ToShortDateString());

            if (cliente != null)
                filtro.Append(", " + Traduccion.Traducir("Cliente") + ":" + cliente.RazonSocial);

            if (sUsuario != null)
                filtro.Append(", " + Traduccion.Traducir("Usuario") + ":" + sUsuario);

            //Los datos los salva en la sesión para que el reporte lo vea
            DataSet dsFacturacion = RptClienteBs.getFacturacion(DtDesde, DtHasta, cliente.NumeroCliente, sUsuario, null, (byte?) ConexionBs.getNumeroEstacion());

            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", filtro.ToString());

            VerReporte(titulo, dsFacturacion, pars, tipo, "~/Reportes/rptFacturacion.rdlc");

            return titulo;
        }

        //Retorma el titulo del reporte
        private string reporteFactura(Telectronica.Facturacion.Factura oFactura, string tipo)
        {
            //Los datos los salva en la sesión para que el reporte lo vea
            DataSet ds = FacturacionBs.getFacturaDetalle(oFactura);

            SalvarDataSetSchema(ds, "c:\\FUENTES\\Peaje.WEB\\Reportes\\App_Code\\DataSets\\");

            string titulo = oFactura.TipoFacturaDescr + " " + Traduccion.Traducir("Número") + " " + oFactura.NumeroFactura.ToString();
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", titulo);
            //Directo en PDF
            VerReporte(titulo, ds, pars, tipo, "~/Reportes/rptFactura.rdlc");

            return titulo;
        }

        //Retorma el titulo del reporte
        [OperationContract]
        public string imprimirNotaCredito(NotaCredito oNotaCredito, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return reporteNotaCredito(oNotaCredito, "PDFo");

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        //Retorma el titulo del reporte
        private string reporteNotaCredito(NotaCredito oNotaCredito, string tipo)
        {
            //Los datos los salva en la sesión para que el reporte lo vea
            DataSet ds = FacturacionBs.getNotaCreditoDetalle(oNotaCredito);

            SalvarDataSetSchema(ds, "c:\\FUENTES\\Peaje.WEB\\Reportes\\App_Code\\DataSets\\");

            string titulo = Traduccion.Traducir("Nota de Crédito") + " " + Traduccion.Traducir("Número") + " " + oNotaCredito.NumeroNC.ToString();
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", titulo);
            //Directo en PDF
            VerReporte(titulo, ds, pars, tipo, "~/Reportes/rptNotaCredito.rdlc");

            return titulo;
        }

        #endregion

        #region REPORTES
        /// ***********************************************************************************************
        /// <summary>
        /// Metodo que prepara la sesion para Mostrar un reporte
        /// </summary>
        /// <param name="titulo">string - Titulo del Reporte</param>
        /// <param name="dsDatos">DataSet - datos del reporte</param>
        /// <param name="pars">Dictionary<string, string> - parametros del reporte</param>
        /// <param name="tipo">string - Indica si se va a ver con el report viewer, pdf o excel</param>
        /// <param name="reportPath">string - path relativo del reporte</param>
        /// <returns></returns>
        /// ***********************************************************************************************    
        private void VerReporte(string titulo, DataSet dsDatos, Dictionary<string, string> pars,
                    string tipo, string reportPath)
        {

            //Salvamos en la sesion los datos del reporte
            HttpContext.Current.Session["Reporte_Titulo"] = titulo;
            HttpContext.Current.Session["Reporte_DataSource_Object"] = dsDatos;

            HttpContext.Current.Session["Reporte_Imprimir"] = tipo;

            HttpContext.Current.Session["Reporte_Path"] = HttpContext.Current.Server.MapPath(reportPath);
            //HttpContext.Current.Session["Reporte_Path"] = reportPath;

            HttpContext.Current.Session["Reporte_Parametros"] = pars;

        }
        /// ***********************************************************************************************
        /// <summary>
        /// Metodo que prepara la sesion para Mostrar un reporte
        /// </summary>
        /// <param name="titulo">string - Titulo del Reporte</param>
        /// <param name="dsDatosL">List/<DataSet/> - datos del reporte</param>
        /// <param name="pars">Dictionary<string, string> - parametros del reporte</param>
        /// <param name="tipo">string - Indica si se va a ver con el report viewer, pdf o excel</param>
        /// <param name="reportPath">string - path relativo del reporte</param>
        /// <returns></returns>
        /// ***********************************************************************************************    
        private void VerReporte(string titulo, List<DataSet> dsDatosL, Dictionary<string, string> pars,
                    string tipo, string reportPath)
        {

            //Salvamos en la sesion los datos del reporte
            HttpContext.Current.Session["Reporte_Titulo"] = titulo;
            HttpContext.Current.Session["Reporte_DataSource_Object"] = dsDatosL;

            HttpContext.Current.Session["Reporte_Imprimir"] = tipo;

            HttpContext.Current.Session["Reporte_Path"] = HttpContext.Current.Server.MapPath(reportPath);

            HttpContext.Current.Session["Reporte_Parametros"] = pars;

        }
        /// ***********************************************************************************************
        /// <summary>
        /// Metodo para Salvar el esquema de un DataSet
        /// para poder diseñar reportes
        /// </summary>
        /// <param name="ds">DataSet - DataSet a salvar</param>
        /// <param name="path">string - Path donde se salva (sin nombre archivo)</param>
        /// <returns></returns>
        /// ***********************************************************************************************    
        protected void SalvarDataSetSchema(DataSet ds, string path)
        {
            //COMENTAR EN PRODUCCION!!!!
            try
            {
                //Salvamos el Dataset una vez (para diseñar el reporte)
                ds.WriteXmlSchema(path + ds.DataSetName + ".xsd");
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region TRADUCCION
        [OperationContract]
        public List<Mensaje> obtenerMensajes(out string idioma, out bool marcarIdioma, out errorCapaSilver exout)
        {
            exout = null;
            idioma = "es";
            marcarIdioma = false;

            try
            {

                return Traduccion.getListMensajes(out idioma, out marcarIdioma);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        [OperationContract]
        public void grabarLog(string texto, string archivo)
        {
            try
            {
                // Grabamos en un archivo el mensaje que no se encontro, con el formato listo para completar la traduccion
                StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Logs/" + archivo), true);

                sw.WriteLine(texto);
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region OPERACIONES

        [OperationContract]
        public TipoItemVentaL ObtenerTiposDeOperaciones(out errorCapaSilver exout)
        {
            exout = null;

            TipoItemVentaL oTipoItemVenta = new TipoItemVentaL();
            try
            {
                ConexionBs.verificarSesion();

                oTipoItemVenta = FacturacionBs.getTiposItemsVentas();

                return oTipoItemVenta;

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return null;
            }
        }

        //Retorma el titulo del reporte
        [OperationContract]
        public string imprimirOperaciones(DateTime DtDesde, DateTime DtHasta, string sUsuario, Cliente cliente, int? iOperacion, string tipo, out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                if (tipo == null)
                    tipo = "PDFo";

                return reporteOperaciones(DtDesde, DtHasta, sUsuario, cliente, iOperacion, tipo);

            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
                return "";
            }
        }

        //Retorma el titulo del reporte
        private string reporteOperaciones(DateTime DtDesde, DateTime DtHasta, string sUsuario, Cliente cliente, int? iOperacion, string tipo)
        {

            // Armamos los parametros:
            StringBuilder filtro = new StringBuilder();
            string titulo = "Reporte de Operaciones";

            filtro.Append(Traduccion.Traducir("Desde") + ":" + DtDesde.ToShortDateString() + ", ");
            filtro.Append(Traduccion.Traducir("Hasta") + ":" + DtHasta.ToShortDateString());

            if (cliente != null)
                filtro.Append(", " + Traduccion.Traducir("Cliente") + ":" + cliente.RazonSocial);

            if (sUsuario != null)
                filtro.Append(", " + Traduccion.Traducir("Usuario") + ":" + sUsuario);

            //Los datos los salva en la sesión para que el reporte lo vea
            DataSet dsOperaciones = RptClienteBs.getOperaciones(DtDesde, DtHasta, cliente.NumeroCliente, sUsuario, iOperacion);

            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("Filtro_Descripcion", filtro.ToString());

            VerReporte(titulo, dsOperaciones, pars, tipo, "~/Reportes/rptOperaciones.rdlc");

            return titulo;
        }

        #endregion

        #region TRIBUTARIA


        //Dmitriy: 14/04/2011
        [OperationContract]
        public Autorizacion ObtenerAutorizacionSRIVigente(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return AutorizacionBs.getAutorizacionVigente();


            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
            }
            return null;
        }

        [OperationContract]
        public ConfiguracionTributaria ObtenerConfiguracionTribuitaria(out errorCapaSilver exout)
        {
            exout = null;

            try
            {
                ConexionBs.verificarSesion();

                return GestionConfiguracionBs.getConfigtrb();


            }
            catch (Exception ex)
            {
                exout = new errorCapaSilver(ex);
            }
            return null;
        }
        #endregion
    }
}
