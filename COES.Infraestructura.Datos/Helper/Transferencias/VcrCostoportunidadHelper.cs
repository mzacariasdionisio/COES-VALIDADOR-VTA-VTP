using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_COSTOPORTUNIDAD
    /// </summary>
    public class VcrCostoportunidadHelper : HelperBase
    {
        public VcrCostoportunidadHelper(): base(Consultas.VcrCostoportunidadSql)
        {
        }

        public VcrCostoportunidadDTO Create(IDataReader dr)
        {
            VcrCostoportunidadDTO entity = new VcrCostoportunidadDTO();

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrcopfecha = dr.GetOrdinal(this.Vcrcopfecha);
            if (!dr.IsDBNull(iVcrcopfecha)) entity.Vcrcopfecha = dr.GetDateTime(iVcrcopfecha);

            int iVcrcopcosto = dr.GetOrdinal(this.Vcrcopcosto);
            if (!dr.IsDBNull(iVcrcopcosto)) entity.Vcrcopcosto = dr.GetDecimal(iVcrcopcosto);

            int iVcrcopusucreacion = dr.GetOrdinal(this.Vcrcopusucreacion);
            if (!dr.IsDBNull(iVcrcopusucreacion)) entity.Vcrcopusucreacion = dr.GetString(iVcrcopusucreacion);

            int iVcrcopfeccreacion = dr.GetOrdinal(this.Vcrcopfeccreacion);
            if (!dr.IsDBNull(iVcrcopfeccreacion)) entity.Vcrcopfeccreacion = dr.GetDateTime(iVcrcopfeccreacion);

            int iVcrcopcodi = dr.GetOrdinal(this.Vcrcopcodi);
            if (!dr.IsDBNull(iVcrcopcodi)) entity.Vcrcopcodi = Convert.ToInt32(dr.GetValue(iVcrcopcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrcopfecha = "VCRCOPFECHA";
        public string Vcrcopcosto = "VCRCOPCOSTO";
        public string Vcrcopusucreacion = "VCRCOPUSUCREACION";
        public string Vcrcopfeccreacion = "VCRCOPFECCREACION";
        public string Vcrcopcodi = "VCRCOPCODI";

        //Atributos de consulta
        public string Emprcodi = "EMPRCODI";

        #endregion

        public string SqlGetByIdEmpresa
        {
            get { return base.GetSqlXml("GetByIdEmpresa"); }
        }
    }
}
