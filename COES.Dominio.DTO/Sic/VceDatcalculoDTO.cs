using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_DATCALCULO
    /// </summary>
    public class VceDatcalculoDTO : EntityBase
    {
        public decimal? Crdcgcmarr_dol { get; set; }
        public decimal? Crdcgcmarr_sol { get; set; }
        public decimal? Crdcgccbefparrampa { get; set; }
        public decimal? Crdcgccbefpar { get; set; }
        public decimal? Crdcgccbefarrtoma { get; set; }
        public decimal? Crdcgccbefarr { get; set; }
        public decimal? Crdcgpotmin { get; set; }
        public decimal? Crdcgconcompp4 { get; set; }
        public decimal? Crdcgpotpar4 { get; set; }
        public decimal? Crdcgconcompp3 { get; set; }
        public decimal? Crdcgpotpar3 { get; set; }
        public decimal? Crdcgconcompp2 { get; set; }
        public decimal? Crdcgpotpar2 { get; set; }
        public decimal? Crdcgconcompp1 { get; set; }
        public decimal? Crdcgpotpar1 { get; set; }
        public decimal? Crdcgccpotefe { get; set; }
        public decimal? Crdcgpotefe { get; set; }
        public decimal? Crdcgnumarrpar { get; set; }
        public string Crdcgprecioaplicunid { get; set; }
        public decimal? Crdcgprecioaplic { get; set; }
        public string Crdcgprecombunid { get; set; }
        public decimal? Crdcgprecomb { get; set; }
        public decimal? Crdcgcvncsol { get; set; }
        public decimal? Crdcgcvncdol { get; set; }
        public decimal? Crdcgtratquim { get; set; }
        public decimal? Crdcgtratmec { get; set; }
        public decimal? Crdcgtranspor { get; set; }
        public decimal? Crdcglhv { get; set; }
        public string Crdcgtipcom { get; set; }
        public DateTime Crdcgfecmod { get; set; }
        public int Grupocodi { get; set; }
        public int PecaCodi { get; set; }
        public int? Crdcgdiasfinanc { get; set; }
        public decimal? Crdcgtiempo { get; set; }
        public decimal? Crdcgenergia { get; set; }
        public int? Crdcgconsiderapotnom { get; set; }
        public int? Barrcodi { get; set; }

        //Adicionales       
        public String Gruponomb { get; set; }
        public String Fenergnomb { get; set; }
        public String Barradiaper { get; set; }
        public String Considerarpotnominal { get; set; }
        public decimal Vcedcmenergia { get; set; }
        public decimal Vcedcmtiempo { get; set; }
        public int Edit { get; set; }
        public String Periodo { get; set; }
        public String Vcedcmconsiderapotnom { get; set; }

        //- compensaciones.HDT - Inicio 27/02/2017: Cambio para atender el requerimiento.
        public String Emprnomb { get; set; }

        public decimal? Crdcgconspotefearr { get; set; }

        public decimal? Crdcgconspotefepar { get; set; }

        public decimal? Crdcgprecioaplicxarr { get; set; }

        public decimal? Crdcgprecioaplicxpar { get; set; }

        public decimal? Crdcgprecioaplicxincgen { get; set; }

        public decimal? Crdcgprecioaplicxdisgen { get; set; }

        public decimal? CrdcgccpotefeNumerador { get; set; }

        public decimal? CrdcgccpotefeDenominador { get; set; }
        public decimal? Crdcgimpuesto { get; set; }
        /// <summary>
        /// Indica si el registro se actualizará o no producto del procesamiento automático
        /// de cálculo.
        /// </summary>
        private bool actualizarRegistro = false;

        public bool ActualizarRegistro
        {
            get { return actualizarRegistro; }
            set { actualizarRegistro = value; }
        }

        /// <summary>
        /// Propiedad que se usa para indicar si el registro se actualizará o no.
        /// </summary>
        

        //- HDT Fin
        public decimal? Crdcgcombarsin { get; set; }

    }
}
