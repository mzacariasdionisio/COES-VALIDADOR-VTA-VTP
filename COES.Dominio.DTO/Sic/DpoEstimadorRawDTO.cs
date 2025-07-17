using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class DpoEstimadorRawDTO : EntityBase
    {
        public int Ptomedicodi { get; set; }
        public int Prnvarcodi { get; set; }
        public int Dporawfuente { get; set; }
        public int Dporawtipomedi { get; set; }
        public DateTime Dporawfecha { get; set; }
        public decimal? Dporawvalor { get; set; }

        public decimal? Dporawvalorh1 { get; set; }
        public decimal? Dporawvalorh2 { get; set; }
        public decimal? Dporawvalorh3 { get; set; }
        public decimal? Dporawvalorh4 { get; set; }
        public decimal? Dporawvalorh5 { get; set; }
        public decimal? Dporawvalorh6 { get; set; }
        public decimal? Dporawvalorh7 { get; set; }
        public decimal? Dporawvalorh8 { get; set; }
        public decimal? Dporawvalorh9 { get; set; }
        public decimal? Dporawvalorh10 { get; set; }
        public decimal? Dporawvalorh11 { get; set; }
        public decimal? Dporawvalorh12 { get; set; }
        public decimal? Dporawvalorh13 { get; set; }
        public decimal? Dporawvalorh14 { get; set; }
        public decimal? Dporawvalorh15 { get; set; }
        public decimal? Dporawvalorh16 { get; set; }
        public decimal? Dporawvalorh17 { get; set; }
        public decimal? Dporawvalorh18 { get; set; }
        public decimal? Dporawvalorh19 { get; set; }
        public decimal? Dporawvalorh20 { get; set; }
        public decimal? Dporawvalorh21 { get; set; }
        public decimal? Dporawvalorh22 { get; set; }
        public decimal? Dporawvalorh23 { get; set; }
        public decimal? Dporawvalorh24 { get; set; }
        public decimal? Dporawvalorh25 { get; set; }
        public decimal? Dporawvalorh26 { get; set; }
        public decimal? Dporawvalorh27 { get; set; }
        public decimal? Dporawvalorh28 { get; set; }
        public decimal? Dporawvalorh29 { get; set; }
        public decimal? Dporawvalorh30 { get; set; }
        public decimal? Dporawvalorh31 { get; set; }
        public decimal? Dporawvalorh32 { get; set; }
        public decimal? Dporawvalorh33 { get; set; }
        public decimal? Dporawvalorh34 { get; set; }
        public decimal? Dporawvalorh35 { get; set; }
        public decimal? Dporawvalorh36 { get; set; }
        public decimal? Dporawvalorh37 { get; set; }
        public decimal? Dporawvalorh38 { get; set; }
        public decimal? Dporawvalorh39 { get; set; }
        public decimal? Dporawvalorh40 { get; set; }
        public decimal? Dporawvalorh41 { get; set; }
        public decimal? Dporawvalorh42 { get; set; }
        public decimal? Dporawvalorh43 { get; set; }
        public decimal? Dporawvalorh44 { get; set; }
        public decimal? Dporawvalorh45 { get; set; }
        public decimal? Dporawvalorh46 { get; set; }
        public decimal? Dporawvalorh47 { get; set; }
        public decimal? Dporawvalorh48 { get; set; }
        public decimal? Dporawvalorh49 { get; set; }
        public decimal? Dporawvalorh50 { get; set; }
        public decimal? Dporawvalorh51 { get; set; }
        public decimal? Dporawvalorh52 { get; set; }
        public decimal? Dporawvalorh53 { get; set; }
        public decimal? Dporawvalorh54 { get; set; }
        public decimal? Dporawvalorh55 { get; set; }
        public decimal? Dporawvalorh56 { get; set; }
        public decimal? Dporawvalorh57 { get; set; }
        public decimal? Dporawvalorh58 { get; set; }
        public decimal? Dporawvalorh59 { get; set; }
        public decimal? Dporawvalorh60 { get; set; }

        #region Adicionales
        public string Ptomedidesc { get; set; }
        public string Prnvarnom { get; set; }
        public int Ieod { get; set; }
        #endregion
    }

    #region Tabla temporal - Proceso Automatico
    public class DpoEstimadorRawTmpDTO : EntityBase
    {
        public int Ptomedicodi { get; set; }
        public int Prnvarcodi { get; set; }
        public int Dporawfuente { get; set; }
        public int Dporawtipomedi { get; set; }
        public DateTime Dporawfecha { get; set; }
        public decimal Dporawvalor { get; set; }
    }
    #endregion

    #region Log
    public class DpoEstimadorRawLogDTO : EntityBase
    {
        public string NomArchivoRaw { get; set; }
        public DateTime FechaArchivoRaw { get; set; }
    }
    #endregion

    #region Historico
    public class DpoEstimadorRawFilesDTO : EntityBase
    {
        public string NomArchivoRaw { get; set; }
        public DateTime FechaArchivoRaw { get; set; }
        public string Tipo { get; set; }
        public string Flag { get; set; }
    }
    #endregion

    #region Tabla temporal - Proceso Manual
    public class DpoEstimadorRawManualDTO : EntityBase
    {
        public int Ptomedicodi { get; set; }
        public int Prnvarcodi { get; set; }
        public int Dporawfuente { get; set; }
        public int Dporawtipomedi { get; set; }
        public DateTime Dporawfecha { get; set; }
        public decimal Dporawvalor { get; set; }

        public string Hora { get; set; }
        public string Minuto { get; set; }
    }
    #endregion
}
