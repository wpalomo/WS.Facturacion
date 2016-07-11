using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
     public class ValePedagio
    {

        

         public class Encabezado
        {
                public string tipo { get; set; }
                public string iDPaisConcesionaria { get; set; }
                public string iDConcesionaria { get; set; }
                public int numeroSecuencia { get; set; }
                public DateTime fechaGeneracion { get; set; }
                public int totalRegistros { get; set; }
                public decimal valorTotal { get; set; }
        }


         public abstract class Detalle
         {
             public string tipo { get; set; }            
             public int numeroSecuencia { get; set; }    
             public string idPaisEmisor { get; set; }    
             public string idEmisorTag { get; set; }     
             public string numeroTag { get; set; }       
             public int estacion { get; set; }           
             public Int64 numeroViaje { get; set; }        
             public decimal valorViaje { get; set; }     
             public int categoria { get; set; }          
             public DateTime fechaPasada { get; set; }   
         }
        

        public class PasadaRealizada
        {
            public Encabezado encabezado;
            public List<Detalle> detalle;

            //Constructor
            public PasadaRealizada()
            {
                encabezado = new Encabezado();
                detalle = new List<Detalle>();
            }

            public class Encabezado : ValePedagio.Encabezado
            {}
           

            public class Detalle : ValePedagio.Detalle
            {
                public Int64 idTrn { get; set; }            
                public int via { get; set; }
                public DateTime mesProtocoloFinanciero { get; set; }
            }

            //Se reutiliza el metodo para ambas clases
            public bool checkIntegridad()
            {
                decimal totalDetalle = detalle.Sum(p => p.valorViaje);

                return ValePedagio.checkIntegridad(encabezado.valorTotal, totalDetalle, detalle.Count,encabezado.totalRegistros);
            }
        }   


        public class PasadaCobrada
        {
            public Encabezado encabezado;
            public List<Detalle> detalle;

            //Constructor
            public PasadaCobrada()
            {
                encabezado = new Encabezado();
                detalle = new List<Detalle>();
            }

            //Tiene el mismo encabezado para ambos Registros
            public class Encabezado : ValePedagio.Encabezado { }

            public class Detalle : ValePedagio.Detalle 
            {
                public char statusPasada { get; set; }
                public DateTime? fechaCancelacion { get; set; }

            }


            //Se reutiliza el metodo para ambas clases
            public bool checkIntegridad()
            {
               decimal totalDetalle = detalle.Sum(p =>p.valorViaje);

               return ValePedagio.checkIntegridad(encabezado.valorTotal, totalDetalle,detalle.Count,encabezado.totalRegistros);
            }
        }


        protected static bool checkIntegridad(decimal valorTotal, decimal registrosReal,int countRegistros, int countEncabezado)
        {
            decimal TotalEncabezado = valorTotal;
            decimal TotalRegistros = registrosReal;
                //pasadaRealizada.detalle.Sum(p => p.valorViaje);

            if (TotalEncabezado != TotalRegistros)
            {
                return false;
            }

            if (countEncabezado!= countRegistros)
            {
                return false;
            }
            return true;

        }
    }

}
