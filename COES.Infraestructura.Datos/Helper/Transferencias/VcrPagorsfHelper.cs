using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_PAGORSF
    /// </summary>
    public class VcrPagorsfHelper : HelperBase
    {
        public VcrPagorsfHelper(): base(Consultas.VcrPagorsfSql)
        {
        }

        public VcrPagorsfDTO Create(IDataReader dr)
        {
            VcrPagorsfDTO entity = new VcrPagorsfDTO();

            int iVcprsfcodi = dr.GetOrdinal(this.Vcprsfcodi);
            if (!dr.IsDBNull(iVcprsfcodi)) entity.Vcprsfcodi = Convert.ToInt32(dr.GetValue(iVcprsfcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iVcprsfpagorsf = dr.GetOrdinal(this.Vcprsfpagorsf);
            if (!dr.IsDBNull(iVcprsfpagorsf)) entity.Vcprsfpagorsf = dr.GetDecimal(iVcprsfpagorsf);

            int iVcprsfusucreacion = dr.GetOrdinal(this.Vcprsfusucreacion);
            if (!dr.IsDBNull(iVcprsfusucreacion)) entity.Vcprsfusucreacion = dr.GetString(iVcprsfusucreacion);

            int iVcprsffeccreacion = dr.GetOrdinal(this.Vcprsffeccreacion);
            if (!dr.IsDBNull(iVcprsffeccreacion)) entity.Vcprsffeccreacion = dr.GetDateTime(iVcprsffeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcprsfcodi = "VCPRSFCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Vcprsfpagorsf = "VCPRSFPAGORSF";
        public string Vcprsfusucreacion = "VCPRSFUSUCREACION";
        public string Vcprsffeccreacion = "VCPRSFFECCREACION";

        #endregion

        public string SqlGetByIdUnidad
        {
            get { return base.GetSqlXml("GetByIdUnidad"); }
        }
        public string SqlGetByIdUnidadPorEmpresa
        {
            get { return base.GetSqlXml("GetByIdUnidadPorEmpresa"); }
        }

        public string SqlGetByIdUnidad2020
        {
            get { return base.GetSqlXml("GetByIdUnidad2020"); }
        }

        public string SqlGetByMigracionEquiposPorEmpresaOrigenxDestino
        {
            get { return base.GetSqlXml("GetByMigracionEquiposPorEmpresaOrigenxDestino"); }
        }
        
    }
}
