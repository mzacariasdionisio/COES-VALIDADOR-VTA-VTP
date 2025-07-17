using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrnClasificacionDTO : EntityBase
    {
        public int Prnclscodi { get; set; }
        public int Lectcodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Prnclsclasificacion { get; set; }
        public decimal Prnclsporcerrormin { get; set; }
        public decimal Prnclsporcerrormax { get; set; }
        public decimal Prnclsmagcargamin { get; set; }
        public decimal Prnclsmagcargamax { get; set; }
        public string Prnclsestado { get; set; }
        public DateTime Prnclsfecha { get; set; }
        public int Prnclsperfil { get; set; }
        public decimal Prnclsvariacion { get; set; }

        public string Prnclsusucreacion { get; set; }
        public DateTime Prnclsfeccreacion { get; set; }
        public string   Prnclsusumodificacion { get; set; }
        public DateTime Prnclsfecmodificacion { get; set; }

        #region Adicionales

        public string Prnclsmagcarga { get; set; }
        public string Prnclsdepuracion { get; set; }
        public decimal Prnclsporcerror { get; set; }

        public string Ptomedidesc { get; set; }
        public string Ptomedielenomb { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprabrev { get; set; }
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public string Areaabrev { get; set; }
        public DateTime Medifecha { get; set; }
        public decimal Meditotal { get; set; }
        public int Prnmestado { get; set; }

        //27-11-2018
        public string Tareaabrev { get; set; }
        public string Famnomb { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Tipoemprdesc { get; set; }
        public int Famcodi { get; set; }

        //09-01-2018
        public int Agrupfactor { get; set; }
        public bool EsPronostico { get; set; }

        //26-11-2019
        public string FullPtomedidesc { get; set; }

        //05-12-2019
        public string StrPrncfgfecha { get; set; }

        //16-12-2019
        public bool Prnflgclasificacion { get; set; }

        //18-07-2020
        public string Subcausadesc { get; set; }

        //30-09-2020
        public int Subcausacodi { get; set; }
        public string StrSubcausacodi { get; set; }
        public List<string> ListSubcausacodi { get; set; }
        #endregion
    }
}
