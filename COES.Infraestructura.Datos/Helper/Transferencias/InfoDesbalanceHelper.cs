using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la consulta para el desbalance
    /// </summary>
    public class InfoDesbalanceHelper : HelperBase
    {
        public InfoDesbalanceHelper() : base(Consultas.InfoDesbalanceSql)
        {
        }

        public InfoDesbalanceDTO Create(IDataReader dr)
        {
            InfoDesbalanceDTO entity = new InfoDesbalanceDTO();

            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            int iBarrTransferencia = dr.GetOrdinal(this.BarrTransferencia);
            if (!dr.IsDBNull(iBarrTransferencia)) entity.BarrTransferencia = dr.GetString(iBarrTransferencia);

            int iDia = dr.GetOrdinal(this.Dia);
            if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetInt32(iDia);

            int iEnergiaDesbalance = dr.GetOrdinal(this.EnergiaDesbalance);
            if (!dr.IsDBNull(iEnergiaDesbalance)) entity.EnergiaDesbalance = dr.GetDecimal(iEnergiaDesbalance);

            int iEnergiaEntrega = dr.GetOrdinal(this.EnergiaEntrega);
            if (!dr.IsDBNull(iEnergiaEntrega)) entity.EnergiaEntrega = dr.GetDecimal(iEnergiaEntrega);

            int iEnergiaRetiro = dr.GetOrdinal(this.EnergiaRetiro);
            if (!dr.IsDBNull(iEnergiaRetiro)) entity.EnergiaRetiro = dr.GetDecimal(iEnergiaRetiro);

            return entity;
        }

        #region Mapeo de Campos

        public string BarrCodi = "BARRCODI";
        public string BarrTransferencia = "BARRBARRATRANSFERENCIA";
        public string Dia = "DIA";
        public string EnergiaDesbalance = "ENERGIADESBALANCE";
        public string EnergiaEntrega = "ENERGIAENTREGA";
        public string EnergiaRetiro = "ENERGIARETIRO";
        
        public string Pericodi = "PERICODI";
        public string Version = "VERSION";
        public string Desbalance = "DESBALANCE";
        #endregion

        public string SqlGetByListaBarrasTrans
        {
            get { return base.GetSqlXml("GetByListaBarrasTrans"); }

        }

        public string SqlGetByListaInfoDesbalanceByBarra
        {
            get { return base.GetSqlXml("GetByListaInfoDesbalanceByBarra"); }

        }

    }
}
