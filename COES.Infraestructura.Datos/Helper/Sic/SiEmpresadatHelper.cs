using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESADAT
    /// </summary>
    public class SiEmpresadatHelper : HelperBase
    {
        public SiEmpresadatHelper()
            : base(Consultas.SiEmpresadatSql)
        {
        }

        public SiEmpresadatDTO Create(IDataReader dr)
        {
            SiEmpresadatDTO entity = new SiEmpresadatDTO();

            int iEmpdatfecha = dr.GetOrdinal(this.Empdatfecha);
            if (!dr.IsDBNull(iEmpdatfecha)) entity.Empdatfecha = dr.GetDateTime(iEmpdatfecha);

            int iConsiscodi = dr.GetOrdinal(this.Consiscodi);
            if (!dr.IsDBNull(iConsiscodi)) entity.Consiscodi = Convert.ToInt32(dr.GetValue(iConsiscodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmpdatusucreacion = dr.GetOrdinal(this.Empdatusucreacion);
            if (!dr.IsDBNull(iEmpdatusucreacion)) entity.Empdatusucreacion = dr.GetString(iEmpdatusucreacion);

            int iEmpdatfeccreacion = dr.GetOrdinal(this.Empdatfeccreacion);
            if (!dr.IsDBNull(iEmpdatfeccreacion)) entity.Empdatfeccreacion = dr.GetDateTime(iEmpdatfeccreacion);

            int iEmpdatusumodificacion = dr.GetOrdinal(this.Empdatusumodificacion);
            if (!dr.IsDBNull(iEmpdatusumodificacion)) entity.Empdatusumodificacion = dr.GetString(iEmpdatusumodificacion);

            int iEmpdatfecmodificacion = dr.GetOrdinal(this.Empdatfecmodificacion);
            if (!dr.IsDBNull(iEmpdatfecmodificacion)) entity.Empdatfecmodificacion = dr.GetDateTime(iEmpdatfecmodificacion);

            int iEmpdatdeleted = dr.GetOrdinal(this.Empdatdeleted);
            if (!dr.IsDBNull(iEmpdatdeleted)) entity.Empdatdeleted = Convert.ToInt32(dr.GetValue(iEmpdatdeleted));

            int IEmpdatvalor = dr.GetOrdinal(this.Empdatvalor);
            if (!dr.IsDBNull(IEmpdatvalor)) entity.Empdatvalor = dr.GetString(IEmpdatvalor);

            return entity;
        }

        #region Mapeo de Campos

        public string Empdatfecha = "EMPDATFECHA";
        public string Consiscodi = "CONSISCODI";
        public string Emprcodi = "EMPRCODI";
        public string Empdatusucreacion = "EMPDATUSUCREACION";
        public string Empdatfeccreacion = "EMPDATFECCREACION";
        public string Empdatusumodificacion = "EMPDATUSUMODIFICACION";
        public string Empdatfecmodificacion = "EMPDATFECMODIFICACION";
        public string Empdatdeleted = "EMPDATDELETED";
        public string Empdatvalor = "EMPDATVALOR";

        public string Emprabrev = "EMPRABREV";
        public string Emprnomb = "EMPRNOMB";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Emprestado = "EMPRESTADO";
        public string EmprestadoFecha = "EMPRESTADOFECHA";

        #endregion

        #region Mapeo de Consultas

        public string SqlListByEmpresaYConcepto
        {
            get { return base.GetSqlXml("ListByEmpresaYConcepto"); }
        }

        #endregion

    }
}
