using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_ENVIO
    /// </summary>
    public class ReEnvioHelper : HelperBase
    {
        public ReEnvioHelper(): base(Consultas.ReEnvioSql)
        {
        }

        public ReEnvioDTO Create(IDataReader dr)
        {
            ReEnvioDTO entity = new ReEnvioDTO();

            int iReenvcodi = dr.GetOrdinal(this.Reenvcodi);
            if (!dr.IsDBNull(iReenvcodi)) entity.Reenvcodi = Convert.ToInt32(dr.GetValue(iReenvcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReenvtipo = dr.GetOrdinal(this.Reenvtipo);
            if (!dr.IsDBNull(iReenvtipo)) entity.Reenvtipo = dr.GetString(iReenvtipo);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReenvfecha = dr.GetOrdinal(this.Reenvfecha);
            if (!dr.IsDBNull(iReenvfecha)) entity.Reenvfecha = dr.GetDateTime(iReenvfecha);

            int iReenvplazo = dr.GetOrdinal(this.Reenvplazo);
            if (!dr.IsDBNull(iReenvplazo)) entity.Reenvplazo = dr.GetString(iReenvplazo);

            int iReenvestado = dr.GetOrdinal(this.Reenvestado);
            if (!dr.IsDBNull(iReenvestado)) entity.Reenvestado = dr.GetString(iReenvestado);

            int iReenvindicador = dr.GetOrdinal(this.Reenvindicador);
            if (!dr.IsDBNull(iReenvindicador)) entity.Reenvindicador = dr.GetString(iReenvindicador);

            int iReenvcomentario = dr.GetOrdinal(this.Reenvcomentario);
            if (!dr.IsDBNull(iReenvcomentario)) entity.Reenvcomentario = dr.GetString(iReenvcomentario);

            int iReenvusucreacion = dr.GetOrdinal(this.Reenvusucreacion);
            if (!dr.IsDBNull(iReenvusucreacion)) entity.Reenvusucreacion = dr.GetString(iReenvusucreacion);

            int iReenvfeccreacion = dr.GetOrdinal(this.Reenvfeccreacion);
            if (!dr.IsDBNull(iReenvfeccreacion)) entity.Reenvfeccreacion = dr.GetDateTime(iReenvfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reenvcodi = "REENVCODI";
        public string Repercodi = "REPERCODI";
        public string Reenvtipo = "REENVTIPO";
        public string Emprcodi = "EMPRCODI";
        public string Reenvfecha = "REENVFECHA";
        public string Reenvplazo = "REENVPLAZO";
        public string Reenvestado = "REENVESTADO";
        public string Reenvindicador = "REENVINDICADOR";
        public string Reenvcomentario = "REENVCOMENTARIO";
        public string Reenvusucreacion = "REENVUSUCREACION";
        public string Reenvfeccreacion = "REENVFECCREACION";

        #endregion

        public string SqlListarPorPeriodoYEmpresa
        {
            get { return GetSqlXml("ListarPorPeriodoYEmpresa"); }
        }
    }
}
