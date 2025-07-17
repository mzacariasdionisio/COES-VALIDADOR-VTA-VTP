using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_FORZADO_CAB
    /// </summary>
    public class CpForzadoCabHelper : HelperBase
    {
        public CpForzadoCabHelper() : base(Consultas.CpForzadoCabSql)
        {
        }

        public CpForzadoCabDTO Create(IDataReader dr)
        {
            CpForzadoCabDTO entity = new CpForzadoCabDTO();

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iCpfzfecha = dr.GetOrdinal(this.Cpfzfecha);
            if (!dr.IsDBNull(iCpfzfecha)) entity.Cpfzfecha = dr.GetDateTime(iCpfzfecha);

            int iCpfzbloquehorario = dr.GetOrdinal(this.Cpfzbloquehorario);
            if (!dr.IsDBNull(iCpfzbloquehorario)) entity.Cpfzbloquehorario = Convert.ToInt32(dr.GetValue(iCpfzbloquehorario));

            int iCpfzusuregistro = dr.GetOrdinal(this.Cpfzusuregistro);
            if (!dr.IsDBNull(iCpfzusuregistro)) entity.Cpfzusuregistro = dr.GetString(iCpfzusuregistro);

            int iCpfzfecregistro = dr.GetOrdinal(this.Cpfzfecregistro);
            if (!dr.IsDBNull(iCpfzfecregistro)) entity.Cpfzfecregistro = dr.GetDateTime(iCpfzfecregistro);

            int iCpfzcodi = dr.GetOrdinal(this.Cpfzcodi);
            if (!dr.IsDBNull(iCpfzcodi)) entity.Cpfzcodi = Convert.ToInt32(dr.GetValue(iCpfzcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Topcodi = "TOPCODI";
        public string Cpfzfecha = "CPFZFECHA";
        public string Cpfzbloquehorario = "CPFZBLOQUEHORARIO";
        public string Cpfzusuregistro = "CPFZUSUREGISTRO";
        public string Cpfzfecregistro = "CPFZFECREGISTRO";
        public string Cpfzcodi = "CPFZCODI";

        #endregion

        public string SqlGetByDate => GetSqlXml("GetByDate");
        public string SqlGetListByDate => GetSqlXml("GetListByDate");
    }
}
