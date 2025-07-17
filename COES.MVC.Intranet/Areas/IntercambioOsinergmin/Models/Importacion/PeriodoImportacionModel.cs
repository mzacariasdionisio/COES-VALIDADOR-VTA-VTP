using System;
using System.ComponentModel.DataAnnotations;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Importacion
{
    /// <summary>
    /// Datos de la importación
    /// </summary>
    public class PeriodoImportacionModel
    {
        [Display(Name = "Periodo")]
        public string Periodo { get; set; }
        [Display(Name = "Última Actualización COES")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime FechaActualCoes { get; set; }
        [Display(Name = "Última Actualización Osinergmin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime FechaActualOsinergmin { get; set; }

        public string fechaVacia { get; set; }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public int PsicliCodi { get; set; }
        
        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public string PsicliCerrado { get; set; }

        public DateTime FechaSincronizacionCoes { get; set; }

        public int TablasEmpresasProcesar { get; set; }

        /// <summary>
        /// Mapeo de campos entre el DTO y el view model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns> </returns>
        public static PeriodoImportacionModel Create(IioPeriodoSicliDTO dto)
        {
            int ianho = Int32.Parse(dto.PsicliAnioMesPerrem.Substring(0, 4));
            int imes = Int32.Parse(dto.PsicliAnioMesPerrem.Substring(4));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("MM yyyy");

            return new PeriodoImportacionModel
            {
                fechaVacia = "",
                Periodo = date,
                FechaActualCoes = dto.PsicliFecUltActCoes,
                FechaActualOsinergmin = dto.PsicliFecUltActOsi,
                //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
                PsicliCodi = dto.PsicliCodi,
                PsicliCerrado = dto.PSicliCerrado
                //- HDT Fin

                ,FechaSincronizacionCoes = dto.PSicliFecSincronizacion
                ,TablasEmpresasProcesar = dto.TablasEmpresasProcesar
            };
        }
    }
}

//model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");