using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_MODELO_ENVIO
    /// </summary>
    public class TrnModeloEnvioHelper : HelperBase
    {
        public TrnModeloEnvioHelper(): base(Consultas.TrnModeloEnvioSql)
        {
        }

        public TrnModeloEnvioDTO Create(IDataReader dr)
        {
            TrnModeloEnvioDTO entity = new TrnModeloEnvioDTO();

            int iModenvcodi = dr.GetOrdinal(this.Modenvcodi);
            if (!dr.IsDBNull(iModenvcodi)) entity.Modenvcodi = Convert.ToInt32(dr.GetValue(iModenvcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iModenvversion = dr.GetOrdinal(this.Modenvversion);
            if (!dr.IsDBNull(iModenvversion)) entity.Modenvversion = Convert.ToInt32(dr.GetValue(iModenvversion));

            int iModenvusuario = dr.GetOrdinal(this.Modenvusuario);
            if (!dr.IsDBNull(iModenvusuario)) entity.Modenvusuario = dr.GetString(iModenvusuario);

            int iModenvfecenvio = dr.GetOrdinal(this.Modenvfecenvio);
            if (!dr.IsDBNull(iModenvfecenvio)) entity.Modenvfecenvio = dr.GetDateTime(iModenvfecenvio);

            int iModenvestado = dr.GetOrdinal(this.Modenvestado);
            if (!dr.IsDBNull(iModenvestado)) entity.Modenvestado = dr.GetString(iModenvestado);

            int iModenvextension = dr.GetOrdinal(this.Modenvextension);
            if (!dr.IsDBNull(iModenvextension)) entity.Modenvextension = dr.GetString(iModenvextension);

            int iModendfile = dr.GetOrdinal(this.Modendfile);
            if (!dr.IsDBNull(iModendfile)) entity.Modendfile = dr.GetString(iModendfile);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTrnmodcodi = dr.GetOrdinal(this.Trnmodcodi);
            if (!dr.IsDBNull(iTrnmodcodi)) entity.Trnmodcodi = Convert.ToInt32(dr.GetValue(iTrnmodcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Modenvcodi = "MODENVCODI";
        public string Pericodi = "PERICODI";
        public string Modenvversion = "MODENVVERSION";
        public string Modenvusuario = "MODENVUSUARIO";
        public string Modenvfecenvio = "MODENVFECENVIO";
        public string Modenvestado = "MODENVESTADO";
        public string Modenvextension = "MODENVEXTENSION";
        public string Modendfile = "MODENDFILE";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Perinomb = "PERINOMB";
        public string Versionnomb = "VERSIONNOMB";
        public string Trnmodcodi = "TRNMODCODI";
        public string Trnmodnombre = "TRNMODNOMBRE";

        #endregion
    }
}
