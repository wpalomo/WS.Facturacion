using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TEMPLATE: Clase para entidad de Templates.
    /// <summary>
    /// Estructura de una entidad Template
    /// </summary>

    [Serializable]
    
    public class Template
    {
        public enum TemplateFiles
        {
            [StringValue("Consolidado.xls")]
            INFORME_CONSOLIDADO,
            [StringValue("FILE_NAME")]
            PROXIMO_TEMPLATE = 1,
        }

        public Template()
        {
        }

        public Template(string Nombre, string Path, string Extension)
        {
            this.Nombre = Nombre;
            this.Path = Path;
            this.Extension = Extension;
        }

        // Nombre del Archivo
        public string Nombre { get; set; }

        // Path del Archivo
        public string Path { get; set; }

        // Extension del Archivo
        public string Extension { get; set; }

        // Tablas a llenar
        public List<string> Tables { get; set; }

        // Filtros del Template
        public List<string> CamposWhere { get; set; }

        // Campos que contienen los datos
        public List<string> CamposDatos { get; set; }

        public string GetFullName
        {
            get { return this.Nombre + this.Extension; }
        }

        public string GetFullFile
        {
            get { return this.Path+this.Nombre + this.Extension; }
        }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Estacion.
    /// </summary>
    public class TemplateL : List<Template>
    {       
    }

    #endregion
}
