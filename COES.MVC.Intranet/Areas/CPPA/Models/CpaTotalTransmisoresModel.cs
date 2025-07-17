using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CPPA.Models
{
    public class CpaTotalTransmisoresModel : BaseModel
    {
        public int Cpattcodi { get; set; }
        public int Cpatdanio { get; set; }
        public string Cpatdajuste { get; set; }
        public int Cparcodi { get; set; }


        #region Para la entidad TotalTransmisores
        public int Anio { get; set; }
        public string Ajuste { get; set; }
        public int IdRevision { get; set; }
        public CpaTotalTransmisoresDTO EntidadTotalTransmisores { get; set; }
        public List<CpaTotalTransmisoresDTO> ListaTotalTransmisores { get; set; }
        #endregion

        #region Para la entidad TotalTransmisoresDetalle
        public int IdEmpresa { get; set; }
       
        public CpaTotalTransmisoresDetDTO EntidadTotalTransmisoresDetalle { get; set; }
        public List<CpaTotalTransmisoresDetDTO> ListaTotalTransmisoresDetalle { get; set; }
        #endregion

        #region Para los campos auditores
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        #endregion

        #region Para los campos auxiliares
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string HtmlList { get; set; }

        public int Visible { get; set; }
        #endregion
    }
}