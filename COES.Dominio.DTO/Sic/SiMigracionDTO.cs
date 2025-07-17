using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRACION
    /// </summary>
    public class SiMigracionDTO : EntityBase
    {
        public int Migracodi { get; set; }
        public int Emprcodi { get; set; }
        public int Tmopercodi { get; set; }
        public string Migradescripcion { get; set; }
        public DateTime Migrafeccorte { get; set; }
        public DateTime MigrafeccorteSTR { get; set; }
        public string Migrausucreacion { get; set; }
        public DateTime? Migrafeccreacion { get; set; }
        public string Migrausumodificacion { get; set; }
        public DateTime? Migrafecmodificacion { get; set; }
        public int? Migradeleted { get; set; }
        public int? Migrareldeleted { get; set; }
        public int Migraflagstr { get; set; }
        //ASSETEC 202108 TIEE
        public int MigraflagPM { get; set; }

        public int Emprcodiorigen { get; set; }
        public string Emprnomborigen { get; set; }
        public string Emprnombdestino { get; set; }
        public string Emprabrevorigen { get; set; }
        public string Emprestadoorigen { get; set; }
        public string EmprestadoorigenDesc { get; set; }

        public string Tmoperdescripcion { get; set; }
        public string MigrafeccorteDesc { get; set; }
        public string MigrafeccreacionDesc { get; set; }
        public string MigrafecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        #region siosein2
        public string EmpresaNombreOrigen { get; set; }
        public string EmpresaNombreDestino { get; set; }
        public string DescripcionTipoOperacion { get; set; }
        #endregion

        public int Total { get; set; }
        public string TotalDesc { get; set; }
        public string ColorFila { get; set; }
    }
}
