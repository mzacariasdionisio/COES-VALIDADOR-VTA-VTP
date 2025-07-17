using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_CENTRAL
    /// </summary>
    public class CpaCentralDTO
    {
        public int Cpacntcodi { get; set; }            // Mapeo de CPACNTCODI
        public int Cpaempcodi { get; set; }            // Mapeo de CPAEMPCODI
        public int Cparcodi { get; set; }              // Mapeo de CPARCODI
        public int Equicodi { get; set; }              // Mapeo de EQUICODI
        public int? Barrcodi { get; set; }             // Mapeo de BARRCODI (nullable)
        public int? Ptomedicodi { get; set; }          // Mapeo de PTOMEDICODI (nullable)
        public string Cpacntestado { get; set; }      // Mapeo de CPACNTESTADO
        public string Cpacnttipo { get; set; }        // Mapeo de CPACNTTIPO
        public DateTime? Cpacntfecejecinicio { get; set; } // Mapeo de CPACNTFECEJECINICIO (nullable)
        public DateTime? Cpacntfecejecfin { get; set; } // Mapeo de CPACNTFECEJECFIN (nullable)
        public DateTime? Cpacntfecproginicio { get; set; } // Mapeo de CPACNTFECPROGINICIO (nullable)
        public DateTime? Cpacntfecprogfin { get; set; } // Mapeo de CPACNTFECPROGFIN (nullable)
        public string Cpacntusucreacion { get; set; } // Mapeo de CPACNTUSUCREACION
        public DateTime Cpacntfeccreacion { get; set; } // Mapeo de CPACNTFECCREACION
        public string Cpacntusumodificacion { get; set; } // Mapeo de CPACNTUSUMODIFICACION (nullable)
        public DateTime? Cpacntfecmodificacion { get; set; } // Mapeo de CPACNTFECMODIFICACION (nullable)
        //CU03
        public string Equinomb { get; set; }
        public string Equinombconcatenado { get; set; }

        //CU04
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Barrnombre { get; set; }
        public string Barrbarratransferencia { get; set; }
        public DateTime? Equifechiniopcom { get; set; }
        public DateTime? Equifechfinopcom { get; set; }
        public string Ptomedidesc { get; set; }
        public string Centralespmpo { get; set; }
        //Additionals
        public string Cpaemptipo { get; set; }
        public string Cpaempestado { get; set; }
        public DateTime? Cpacntfecproginicioantesfecejec { get; set; } // Fecha Inicio Programada antes Fechas Ejecutadas
        public DateTime? Cpacntfecprogfinantesfecejec { get; set; } // Fecha Fin Programada antes Fechas Ejecutadas

        //Agregado para poder registrar varias veces una empresa 
        public int Cpacntcorrelativo { get; set; }
    }
}

