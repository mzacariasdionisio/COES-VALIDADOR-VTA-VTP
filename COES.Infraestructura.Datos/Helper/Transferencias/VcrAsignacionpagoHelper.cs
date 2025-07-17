using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_ASIGNACIONPAGO
    /// </summary>
    public class VcrAsignacionpagoHelper : HelperBase
    {
        public VcrAsignacionpagoHelper(): base(Consultas.VcrAsignacionpagoSql)
        {
        }

        public VcrAsignacionpagoDTO Create(IDataReader dr)
        {
            VcrAsignacionpagoDTO entity = new VcrAsignacionpagoDTO();

            int iVcrapcodi = dr.GetOrdinal(this.Vcrapcodi);
            if (!dr.IsDBNull(iVcrapcodi)) entity.Vcrapcodi = Convert.ToInt32(dr.GetValue(iVcrapcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iVcrapfecha = dr.GetOrdinal(this.Vcrapfecha);
            if (!dr.IsDBNull(iVcrapfecha)) entity.Vcrapfecha = dr.GetDateTime(iVcrapfecha);

            int iVcrapasignpagorsf = dr.GetOrdinal(this.Vcrapasignpagorsf);
            if (!dr.IsDBNull(iVcrapasignpagorsf)) entity.Vcrapasignpagorsf = dr.GetDecimal(iVcrapasignpagorsf);

            int iVcrapusucreacion = dr.GetOrdinal(this.Vcrapusucreacion);
            if (!dr.IsDBNull(iVcrapusucreacion)) entity.Vcrapusucreacion = dr.GetString(iVcrapusucreacion);

            int iVcrapfeccreacion = dr.GetOrdinal(this.Vcrapfeccreacion);
            if (!dr.IsDBNull(iVcrapfeccreacion)) entity.Vcrapfeccreacion = dr.GetDateTime(iVcrapfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrapcodi = "VCRAPCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Vcrapfecha = "VCRAPFECHA";
        public string Vcrapasignpagorsf = "VCRAPASIGNPAGORSF";
        public string Vcrapusucreacion = "VCRAPUSUCREACION";
        public string Vcrapfeccreacion = "VCRAPFECCREACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcen = "EQUINOMBCEN";
        public string Equinombuni = "EQUINOMBUNI";

        #endregion

        public string SqlGetByIdMesUnidad
        {
            get { return base.GetSqlXml("GetByIdMesUnidad"); }
        }

        public string SqlListEmpresaMes
        {
            get { return base.GetSqlXml("ListEmpresaMes"); }
        }
        
    }
}
