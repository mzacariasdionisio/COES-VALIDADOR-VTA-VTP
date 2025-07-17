using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CPPA.Models
{
    public class CpaTotalDemandaModel : BaseModel
    {
        public int Cpatdanio { get; set; }
        public string Cpatdajuste { get; set; }
        public int Cparcodi { get; set; }
        public string Cpatdtipo { get; set; }
        public int Cpatdmes { get; set; }


        #region Para la entidad TotalDemanda
        public int Anio { get; set; }
        public string Ajuste { get; set; }
        public int IdCabecera { get; set; }
        public int IdRevision { get; set; }
        public string IdTipoParticipacion { get; set; }
        public int Mes { get; set; }
        #endregion

        #region Para la entidad TotalDemandaDetalle
        public int IdDetalle { get; set; }
        public int IdEmpresa { get; set; }
        public decimal TotalDemandaEnergiaMwh { get; set; }
        public decimal TotalDemandaEnergiaSoles { get; set; }
        public decimal TotalDemandaPotenciaMw { get; set; }
        public decimal TotalDemandaPotenciaSoles { get; set; }
        #endregion

        #region Para los campos auditores
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        #endregion

        public CpaTotalDemandaDTO EntidadTotalDemanda { get; set; }
        public List<CpaTotalDemandaDTO> ListaTotalDemanda { get; set; }
        public CpaTotalDemandaDetDTO EntidadTotalDemandaDetalle { get; set; }
        public List<CpaTotalDemandaDetDTO> ListaTotalDemandaDetalle { get; set; }

        public VtpIngresoPotefrDetalleDTO Entidad { get; set; }
        public List<VtpIngresoPotefrDetalleDTO> ListaIngresoPotefrDetalle { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string HtmlList { get; set; }

        public int Visible { get; set; }
    }
}