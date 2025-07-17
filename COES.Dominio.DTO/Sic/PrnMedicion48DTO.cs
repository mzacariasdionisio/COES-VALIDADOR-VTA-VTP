using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Campos para la tabla PRN_MEDICION48
    /// </summary>
    public class PrnMedicion48DTO
    {
        public int Ptomedicodi { get; set; }
        public int Prnm48tipo { get; set; }
        public DateTime Medifecha { get; set; }
        public int Prnm48estado { get; set; }
        public decimal? Meditotal { get; set; }
        public decimal? H1 { get; set; }
        public decimal? H2 { get; set; }
        public decimal? H3 { get; set; }
        public decimal? H4 { get; set; }
        public decimal? H5 { get; set; }
        public decimal? H6 { get; set; }
        public decimal? H7 { get; set; }
        public decimal? H8 { get; set; }
        public decimal? H9 { get; set; }
        public decimal? H10 { get; set; }
        public decimal? H11 { get; set; }
        public decimal? H12 { get; set; }
        public decimal? H13 { get; set; }
        public decimal? H14 { get; set; }
        public decimal? H15 { get; set; }
        public decimal? H16 { get; set; }
        public decimal? H17 { get; set; }
        public decimal? H18 { get; set; }
        public decimal? H19 { get; set; }
        public decimal? H20 { get; set; }
        public decimal? H21 { get; set; }
        public decimal? H22 { get; set; }
        public decimal? H23 { get; set; }
        public decimal? H24 { get; set; }
        public decimal? H25 { get; set; }
        public decimal? H26 { get; set; }
        public decimal? H27 { get; set; }
        public decimal? H28 { get; set; }
        public decimal? H29 { get; set; }
        public decimal? H30 { get; set; }
        public decimal? H31 { get; set; }
        public decimal? H32 { get; set; }
        public decimal? H33 { get; set; }
        public decimal? H34 { get; set; }
        public decimal? H35 { get; set; }
        public decimal? H36 { get; set; }
        public decimal? H37 { get; set; }
        public decimal? H38 { get; set; }
        public decimal? H39 { get; set; }
        public decimal? H40 { get; set; }
        public decimal? H41 { get; set; }
        public decimal? H42 { get; set; }
        public decimal? H43 { get; set; }
        public decimal? H44 { get; set; }
        public decimal? H45 { get; set; }
        public decimal? H46 { get; set; }
        public decimal? H47 { get; set; }
        public decimal? H48 { get; set; }
        public string Prnm48usucreacion { get; set; }
        public DateTime Prnm48feccreacion { get; set; }
        public string Prnm48usumodificacion { get; set; }
        public DateTime Prnm48fecmodificacion { get; set; }
        public int Prnm48relacion { get; set; }

        #region Adicionales

        public string Ptomedidesc { get; set; }
        public int Equicodi { get; set; }        
        public int Prnclsclasificacion { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public int Prnmestado { get; set; }
        public string Prnmestdesc { get; set; }//Version string del Prnestado
        public int Prnmintervalo { get; set; }
        public string Prnmitvdesc { get; set; }//Version string del Prnmintervalo
        public int Lectcodi { get; set; }
        public int Tipoinfocodi { get; set; }

        //Old - Inicio
        public string Parptoporcerrormin { get; set; }//Parametro de Configuración
        public string Parptoporcerrormax { get; set; }//Parametro de Configuración
        public string Parptoporcmuestra { get; set; }//Parametro de Configuración
        public string Parptoporcdesvia { get; set; }//Parametro de Configuración
        public string Parptomagcargamin { get; set; }//Parametro de Configuración
        public string Parptomagcargamax { get; set; }//Parametro de Configuración
        public string Parptoferiado { get; set; }//Parametro de Configuración
        public string Parptoatipico { get; set; }//Parametro de Configuración
        public string Parptodepurauto { get; set; }//Parametro de Configuración
        public string Parptopatron { get; set; }//Parametro de Configuración
        public string Parptoveda { get; set; }//Parametro de Configuración
        public string Parptonumcoincidencia { get; set; }//Parametro de Configuración
        //Old - Fin

        //agregado reporte
        public decimal? Equitension { get; set; }

        //Parametros Barra
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public int Grupocodi { get; set; }
        public int Prnredbarracp { get; set; }
        public int Prnredbarrapm { get; set; }
        //20200511
        public List<int> ListaIntervalos { get; set; }
        //20220125
        public decimal[] ArrayMediciones { get; set; }
        #endregion
    }

}
