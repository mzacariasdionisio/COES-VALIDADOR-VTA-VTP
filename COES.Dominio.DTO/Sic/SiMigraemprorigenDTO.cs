using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAEMPRORIGEN
    /// </summary>
    public partial class SiMigraemprOrigenDTO : EntityBase
    {
        public int Migempcodi { get; set; }
        public int Migracodi { get; set; }
        public int Emprcodi { get; set; }
        public string Migempusucreacion { get; set; }
        public DateTime? Migempfeccreacion { get; set; }
        public string Migempusumodificacion { get; set; }
        public DateTime? Migempfecmodificacion { get; set; }

        public string MigempestadoDest { get; set; }
        public string MigempcodosinergminDest { get; set; }
        public int MigempscadacodiDest { get; set; }
        public string MigempnombrecomercialDest { get; set; }
        public string MigempdomiciliolegalDest { get; set; }
        public string MigempnumpartidaregDest { get; set; }
        public string MigempabrevDest { get; set; }
        public int? MigempordenDest { get; set; }
        public string MigemptelefonoDest { get; set; }
        public string MigempestadoregistroDest { get; set; }
        public DateTime? MigempfecinscripcionDest { get; set; }
        public string MigempcondicionDest { get; set; }
        public int MigempnroconstanciaDest { get; set; }
        public DateTime? MigempfecingresoDest { get; set; }
        public int MigempnroregistroDest { get; set; }
        public string MigempindusutramiteDest { get; set; }
    }

    public partial class SiMigraemprOrigenDTO
    {
        public int EMPRCODIDESTINO { get; set; }
        public string EMPRESANOMBREDESTINO { get; set; }

        public string EMPRESANOMBREORIGEN { get; set; }
    }
}
