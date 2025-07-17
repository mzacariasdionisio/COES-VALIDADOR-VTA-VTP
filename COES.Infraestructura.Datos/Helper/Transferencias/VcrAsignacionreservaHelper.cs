using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_ASIGNACIONRESERVA
    /// </summary>
    public class VcrAsignacionreservaHelper : HelperBase
    {
        public VcrAsignacionreservaHelper(): base(Consultas.VcrAsignacionreservaSql)
        {
        }

        public VcrAsignacionreservaDTO Create(IDataReader dr)
        {
            VcrAsignacionreservaDTO entity = new VcrAsignacionreservaDTO();

            int iVcrarcodi = dr.GetOrdinal(this.Vcrarcodi);
            if (!dr.IsDBNull(iVcrarcodi)) entity.Vcrarcodi = Convert.ToInt32(dr.GetValue(iVcrarcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrarfecha = dr.GetOrdinal(this.Vcrarfecha);
            if (!dr.IsDBNull(iVcrarfecha)) entity.Vcrarfecha = dr.GetDateTime(iVcrarfecha);

            int iVcrarrapbf = dr.GetOrdinal(this.Vcrarrapbf);
            if (!dr.IsDBNull(iVcrarrapbf)) entity.Vcrarrapbf = dr.GetDecimal(iVcrarrapbf);

            int iVcrarasignreserva = dr.GetOrdinal(this.Vcrarasignreserva);
            if (!dr.IsDBNull(iVcrarasignreserva)) entity.Vcrarasignreserva = dr.GetDecimal(iVcrarasignreserva);

            int iVcrarprbf = dr.GetOrdinal(this.Vcrarprbf);
            if (!dr.IsDBNull(iVcrarprbf)) entity.Vcrarprbf = dr.GetDecimal(iVcrarprbf);

            int iVcrarrama = dr.GetOrdinal(this.Vcrarrama);
            if (!dr.IsDBNull(iVcrarrama)) entity.Vcrarrama = dr.GetDecimal(iVcrarrama);

            int iVcrarmpa = dr.GetOrdinal(this.Vcrarmpa);
            if (!dr.IsDBNull(iVcrarmpa)) entity.Vcrarmpa = dr.GetDecimal(iVcrarmpa);

            int iVcrarramaursra = dr.GetOrdinal(this.Vcrarramaursra);
            if (!dr.IsDBNull(iVcrarramaursra)) entity.Vcrarramaursra = dr.GetDecimal(iVcrarramaursra);

            int iVcrarusucreacion = dr.GetOrdinal(this.Vcrarusucreacion);
            if (!dr.IsDBNull(iVcrarusucreacion)) entity.Vcrarusucreacion = dr.GetString(iVcrarusucreacion);

            int iVcrarfeccreacion = dr.GetOrdinal(this.Vcrarfeccreacion);
            if (!dr.IsDBNull(iVcrarfeccreacion)) entity.Vcrarfeccreacion = dr.GetDateTime(iVcrarfeccreacion);

            //202010
            int iVcrarrapbfbajar = dr.GetOrdinal(this.Vcrarrapbfbajar);
            if (!dr.IsDBNull(iVcrarrapbfbajar)) entity.Vcrarrapbfbajar = dr.GetDecimal(iVcrarrapbfbajar);

            int iVcrarprbfbajar = dr.GetOrdinal(this.Vcrarprbfbajar);
            if (!dr.IsDBNull(iVcrarprbfbajar)) entity.Vcrarprbfbajar = dr.GetDecimal(iVcrarprbfbajar);

            int iVcrarramabajar = dr.GetOrdinal(this.Vcrarramabajar);
            if (!dr.IsDBNull(iVcrarramabajar)) entity.Vcrarramabajar = dr.GetDecimal(iVcrarramabajar);

            int iVcrarmpabajar = dr.GetOrdinal(this.Vcrarmpabajar);
            if (!dr.IsDBNull(iVcrarmpabajar)) entity.Vcrarmpabajar = dr.GetDecimal(iVcrarmpabajar);

            int iVcrarramaursrabajar = dr.GetOrdinal(this.Vcrarramaursrabajar);
            if (!dr.IsDBNull(iVcrarramaursrabajar)) entity.Vcrarramaursrabajar = dr.GetDecimal(iVcrarramaursrabajar);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrarcodi = "VCRARCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrarfecha = "VCRARFECHA";
        public string Vcrarrapbf = "VCRARRAPBF";
        public string Vcrarprbf = "VCRARPRBF";
        public string Vcrarrama = "VCRARRAMA";
        public string Vcrarmpa = "VCRARMPA";
        public string Vcrarramaursra = "VCRARRAMAURSRA";
        public string Vcrarasignreserva = "VCRARASIGNRESERVA";
        public string Vcrarusucreacion = "VCRARUSUCREACION";
        public string Vcrarfeccreacion = "VCRARFECCREACION";
        //202010
        public string Vcrarrapbfbajar = "VCRARRAPBFBAJAR";
        public string Vcrarprbfbajar = "VCRARPRBFBAJAR";
        public string Vcrarramabajar = "VCRARRAMABAJAR";
        public string Vcrarmpabajar = "VCRARMPABAJAR";
        public string Vcrarramaursrabajar = "VCRARRAMAURSRABAJAR";

        //Atributos de consulta
        public string Emprcodi = "EMPRCODI";
        #endregion

        public string SqlListPorMesURS
        {
            get { return base.GetSqlXml("ListPorMesURS"); }
        }

        public string SqlExportarReservaAsignadaSEV2020
        {
            get { return base.GetSqlXml("ExportarReservaAsignadaSEV2020"); }
        }

        public string SqlExportarReservaAsignadaSEV
        {
            get { return base.GetSqlXml("ExportarReservaAsignadaSEV"); }
        }

        public string SqlGetByIdEmpresa
        {
            get { return base.GetSqlXml("GetByIdEmpresa"); }
        }

        public string SqlGetByCriteriaOferta
        {
            get { return base.GetSqlXml("GetByCriteriaOferta"); }
        }

        public string SqlGetByMPA2020
        {
            get { return base.GetSqlXml("GetByMPA2020"); }
        }

        public string SqlGetByMPA
        {
            get { return base.GetSqlXml("GetByMPA"); }
        }
    }
}
