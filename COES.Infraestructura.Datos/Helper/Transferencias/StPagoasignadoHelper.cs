using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_Pagoasignado
    /// </summary>
    public class StPagoasignadoHelper : HelperBase
    {
        public StPagoasignadoHelper()
            : base(Consultas.StPagoasignadoSql)
        {
        }

        public StPagoasignadoDTO Create(IDataReader dr)
        {
            StPagoasignadoDTO entity = new StPagoasignadoDTO();

            int iPagasgcodi = dr.GetOrdinal(this.Pagasgcodi);
            if (!dr.IsDBNull(iPagasgcodi)) entity.Pagasgcodi = Convert.ToInt32(dr.GetValue(iPagasgcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iStcntgcodi = dr.GetOrdinal(this.Stcntgcodi);
            if (!dr.IsDBNull(iStcntgcodi)) entity.Stcntgcodi = Convert.ToInt32(dr.GetValue(iStcntgcodi));

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iPagasgcmggl = dr.GetOrdinal(this.Pagasgcmggl);
            if (!dr.IsDBNull(iPagasgcmggl)) entity.Pagasgcmggl = dr.GetDecimal(iPagasgcmggl);

            int iPagasgcmgglp = dr.GetOrdinal(this.Pagasgcmgglp);
            if (!dr.IsDBNull(iPagasgcmgglp)) entity.Pagasgcmgglp = dr.GetDecimal(iPagasgcmgglp);

            int iPagasgcmgglfinal = dr.GetOrdinal(this.Pagasgcmgglfinal);
            if (!dr.IsDBNull(iPagasgcmgglfinal)) entity.Pagasgcmgglfinal = dr.GetDecimal(iPagasgcmgglfinal);

            int iPagasgusucreacion = dr.GetOrdinal(this.Pagasgusucreacion);
            if (!dr.IsDBNull(iPagasgusucreacion)) entity.Pagasgusucreacion = dr.GetString(iPagasgusucreacion);

            int iPagasgfeccreacion = dr.GetOrdinal(this.Pagasgfeccreacion);
            if (!dr.IsDBNull(iPagasgfeccreacion)) entity.Pagasgfeccreacion = dr.GetDateTime(iPagasgfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pagasgcodi = "PAGASGCODI";
        public string Strecacodi = "STRECACODI";
        public string Stcntgcodi = "STCNTGCODI";
        public string Stcompcodi = "STCOMPCODI";
        public string Pagasgcmggl = "PAGASGCMGGL";
        public string Pagasgcmgglp = "PAGASGCMGGLP";
        public string Pagasgcmgglfinal = "PAGASGCMGGLFINAL";
        public string Pagasgusucreacion = "PAGASGUSUCREACION";
        public string Pagasgfeccreacion = "PAGASGFECCREACION";
        //Variables para los reportes
        public string Equinomb = "EQUINOMB";
        public string Stcompcodelemento = "STCOMPCODELEMENTO";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprruc = "EMPRRUC";
        public string Sistrncodi = "SISTRNCODI";
        public string Sistrnnombre = "SISTRNNOMBRE";

        #endregion

        public string SqlGetByCriteriaReporte
        {
            get { return base.GetSqlXml("GetByCriteriaReporte"); }
        }

        public string SqlListEmpresaGeneradores
        {
            get { return base.GetSqlXml("ListEmpresaGeneradores"); }
        }

        public string SqlListEmpresaSistemas
        {
            get { return base.GetSqlXml("ListEmpresaSistemas"); }
        }

        public string SqlGetPagoGeneradorXSistema
        {
            get { return base.GetSqlXml("GetPagoGeneradorXSistema"); }
        }

        public string SqlDeleteByCompensacion
        {
            get { return base.GetSqlXml("DeleteByCompensacion"); }
        }
    }
}
