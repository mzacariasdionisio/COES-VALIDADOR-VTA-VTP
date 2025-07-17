using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_FORMATO
    /// </summary>
    public partial class MeFormatoDTO : EntityBase, ICloneable
    {
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public int? Areacode { get; set; } 
        public int? Formatresolucion { get; set; } 
        public int? Formatperiodo { get; set; } 
        public string Formatnombre { get; set; } 
        public int Formatcodi { get; set; }
        public int Formathorizonte { get; set; }
        public int Cabcodi { get; set; }
        public int Lectcodi { get; set; }
        public int Formatsecundario { get; set; }

        public int Modcodi { get; set; }
        public int Formatdiaplazo { get; set; }
        public int Formatminplazo { get; set; }
        public string Formatdescrip { get; set; }
        public int Formatcheckplazo { get; set; }
        public int Formatcheckblanco { get; set; }
        public int Formatallempresa { get; set; }
        public int Formatcheckplazopunto { get; set; }
        public int? Formatdependeconfigptos { get; set; }

        //- Aplicativo PMPO
        public int Formatdiafinplazo { get; set; } // Agregado por STS
        public int Formatminfinplazo { get; set; } // Agregado por STS
        public int Formatnumbloques { get; set; } // Agregado por STS

        public int Formatdiafinfueraplazo { get; set; }
        public int Formatminfinfueraplazo { get; set; }

        public int Formatmesplazo { get; set; }
        public int Formatmesfinplazo { get; set; }
        public int Formatmesfinfueraplazo { get; set; }

        public string Formatenviocheckadicional { get; set; }
    }

    public partial class MeFormatoDTO
    {
        public string Formatextension { get; set; }
        public int? Formatversion { get; set; }
        public int Formatrows { get; set; }
        public int Formatcols { get; set; }
        public string Formatquery { get; set; }
        public string Formatheaderrow { get; set; }
        public string Formatheadercol { get; set; }

        public string Areaname { get; set; }

        public string Periodo { get; set; }
        public string Resolucion { get; set; }
        public string Horizonte { get; set; }
        public List<MeHojaDTO> ListaHoja { get; set; }

        public string FormatnombreOrigen { get; set; }
        public string LastdateDesc { get; set; }

        public int RowPorDia { get; set; }
        public int Lecttipo { get; set; }
        public string Lectnomb { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaProceso { get; set; }
        public DateTime FechaPlazoIni { get; set; }//Fecha de inicio del Plazo
        public DateTime FechaPlazo { get; set; }// solo se podra enviar informacion esta entre FechaPlazoIni < fecha < FechaPlazo
        public DateTime FechaPlazoFuera { get; set; }
        public int IdEnvio { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public bool FlagUtilizaHoja { get; set; }
        public int IdFormatoNuevo { get; set; }

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }  // Agregado por STS

        public string FechaPlazoIniDesc { get; set; }
        public string FechaPlazoDesc { get; set; }
        public string FechaPlazoFueraDesc { get; set; }
        public bool EsCerrado { get; set; }

        public int TipoAgregarTiempoAdicional { get; set; }

        public int? Enviobloquehora { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
