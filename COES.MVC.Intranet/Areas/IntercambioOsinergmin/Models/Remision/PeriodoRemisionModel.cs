using System;
using System.ComponentModel.DataAnnotations;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Remision
{
    /// <summary>
    /// Datos de la remisión para el listado de remisiones
    /// </summary>
    public class PeriodoRemisionModel
    {
        [Display(Name = "Periodo")]
        public string Periodo { get; set; }
        [Display(Name = "Fec. Último Envío")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? UltimoEnvioDate { get; set; }
        [Display(Name = "Fec. Primer Envío")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? PrimerEnvioDate { get; set; }

        public string fechaVacia { get; set; }

        /// <summary>
        /// Mapeo de campos entre el DTO y el view model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns> </returns>
        public static PeriodoRemisionModel Create(IioPeriodoSeinDTO dto)
        {
            int ianho = Int32.Parse(dto.PseinAnioMesPerrem.Substring(0, 4));
            int imes = Int32.Parse(dto.PseinAnioMesPerrem.Substring(4));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("MM yyyy");

            return new PeriodoRemisionModel
            {   
                fechaVacia = "",
                Periodo = date,
                UltimoEnvioDate = dto.PseinFecUltEnvio,
                PrimerEnvioDate = dto.PseinFecPriEnvio
            };
        }
    }
}

//model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");