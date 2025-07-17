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
    public class VtdCargosEneReacHelper : HelperBase
    {
        public VtdCargosEneReacHelper() : base(Consultas.VtdCargosEneReacSql)
        {
        }
        public VtdCargoEneReacDTO Create(IDataReader dr)
        {
            VtdCargoEneReacDTO entity = new VtdCargoEneReacDTO();

            int iCaercodi = dr.GetOrdinal(this.Caercodi);
            if (!dr.IsDBNull(iCaercodi)) entity.Caercodi = Convert.ToInt32(dr.GetValue(iCaercodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCaerfecha = dr.GetOrdinal(this.Caerfecha);
            if (!dr.IsDBNull(iCaerfecha)) entity.Caerfecha = dr.GetDateTime(iCaerfecha);

            int iCaermonto = dr.GetOrdinal(this.Caermonto);
            if (!dr.IsDBNull(iCaermonto)) entity.Caermonto = Convert.ToDecimal(dr.GetValue(iCaermonto));

            int iCaerdeleted = dr.GetOrdinal(this.Caerdeleted);
            if (!dr.IsDBNull(iCaerdeleted)) entity.Caerdeleted = Convert.ToInt32(dr.GetValue(iCaerdeleted));

            int iCaersucreacion = dr.GetOrdinal(this.Caersucreacion);
            if (!dr.IsDBNull(iCaersucreacion)) entity.Caersucreacion = dr.GetString(iCaersucreacion);

            int iCaerfeccreacion = dr.GetOrdinal(this.Caerfeccreacion);
            if (!dr.IsDBNull(iCaerfeccreacion)) entity.Caerfeccreacion = dr.GetDateTime(iCaerfeccreacion);

            int iCaerusumodificacion = dr.GetOrdinal(this.Caerusumodificacion);
            if (!dr.IsDBNull(iCaerusumodificacion)) entity.Caerusumodificacion = dr.GetString(iCaerusumodificacion);

            int iCaerfecmodificacion = dr.GetOrdinal(this.Caerfecmodificacion);
            if (!dr.IsDBNull(iCaerfecmodificacion)) entity.Caerfecmodificacion = dr.GetDateTime(iCaerfecmodificacion);
            return entity;
        }


        #region Mapeo de Campos

        public string Caercodi = "CAERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Caerfecha = "CAERFECHA";
        public string Caermonto = "CAERMONTO";
        public string Caerdeleted = "CAERDELETED";
        public string Caersucreacion = "CAERSUCREACION";
        public string Caerfeccreacion = "CAERFECCREACION";
        public string Caerusumodificacion = "CAERUSUMODIFICACION";
        public string Caerfecmodificacion = "CAERFECMODIFICACION";
        #endregion

        public string SqlGetMontoByEmpresa
        {
            get { return GetSqlXml("GetMontoByEmpresa"); }
        }

        public string SqlDeletedFisicByDate
        {
            get { return base.GetSqlXml("DeletedFisicByDate"); }
        }

        public string SqlUpdateByResultDate
        {
            get { return base.GetSqlXml("UpdateByResultDate"); }
        }

        public string SqlSaveNowDate
        {
            get { return base.GetSqlXml("SaveNowDate"); }
        }

        public string SqlObtenerCargosPorConsumoExcesoEnergiaReactiva
        {
            get { return base.GetSqlXml("ObtenerCargosPorConsumoExcesoEnergiaReactiva"); }
        }

        public string SqlListByDate
        {
            get { return GetSqlXml("ListByDate"); }
        }
    }
}