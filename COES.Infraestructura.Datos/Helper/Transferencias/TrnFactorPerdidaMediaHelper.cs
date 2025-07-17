using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_FACTOR_PERDIDA_MEDIA
    /// </summary>
    public class TrnFactorPerdidaMediaHelper : HelperBase
    {
        public TrnFactorPerdidaMediaHelper(): base(Consultas.TrnFactorPerdidaMediaSql)
        {
        }

        public TrnFactorPerdidaMediaDTO Create(IDataReader dr)
        {
            TrnFactorPerdidaMediaDTO entity = new TrnFactorPerdidaMediaDTO();

            int iTrnfpmcodi = dr.GetOrdinal(this.Trnfpmcodi);
            if (!dr.IsDBNull(iTrnfpmcodi)) entity.Trnfpmcodi = Convert.ToInt32(dr.GetValue(iTrnfpmcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCodentcodi = dr.GetOrdinal(this.Codentcodi);
            if (!dr.IsDBNull(iCodentcodi)) entity.Codentcodi = Convert.ToInt32(dr.GetValue(iCodentcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iTrnfpmversion = dr.GetOrdinal(this.Trnfpmversion);
            if (!dr.IsDBNull(iTrnfpmversion)) entity.Trnfpmversion = Convert.ToInt32(dr.GetValue(iTrnfpmversion));

            int iTrnfpmvalor = dr.GetOrdinal(this.Trnfpmvalor);
            if (!dr.IsDBNull(iTrnfpmvalor)) entity.Trnfpmvalor = dr.GetDecimal(iTrnfpmvalor);

            int iTrnfpmobserv = dr.GetOrdinal(this.Trnfpmobserv);
            if (!dr.IsDBNull(iTrnfpmobserv)) entity.Trnfpmobserv = dr.GetString(iTrnfpmobserv);

            int iTrnfpmusername = dr.GetOrdinal(this.Trnfpmusername);
            if (!dr.IsDBNull(iTrnfpmusername)) entity.Trnfpmusername = dr.GetString(iTrnfpmusername);

            int iTrnfpmfecins = dr.GetOrdinal(this.Trnfpmfecins);
            if (!dr.IsDBNull(iTrnfpmfecins)) entity.Trnfpmfecins = dr.GetDateTime(iTrnfpmfecins);

            return entity;
        }


        #region Mapeo de Campos

        public string Trnfpmcodi = "TRNFPMCODI";
        public string Pericodi = "PERICODI";
        public string Barrcodi = "BARRCODI";
        public string Codentcodi = "CODENTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Trnfpmversion = "TRNFPMVERSION";
        public string Trnfpmvalor = "TRNFPMVALOR";
        public string Trnfpmobserv = "TRNFPMOBSERV";
        public string Trnfpmusername = "TRNFPMUSERNAME";
        public string Trnfpmfecins = "TRNFPMFECINS";
        //atrbutos adicionales para consulta:
        public string Codentcodigo = "CODENTCODIGO";
        public string Barrnombre = "BARRNOMBRE";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string EntregaMes = "ENTREGAMES";
        public string MedidoresMes = "MEDIDORESMES";
        #endregion

        public string SqlListDesvEntregas
        {
            get { return base.GetSqlXml("ListDesvEntregas"); }
        }
    }
}
