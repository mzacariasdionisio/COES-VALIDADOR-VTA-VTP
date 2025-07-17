using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_CENTRAL
    /// </summary>
    public class RerCentralHelper : HelperBase
    {
        public RerCentralHelper() : base(Consultas.RerCentralSql)
        {
        }

        public RerCentralDTO Create(IDataReader dr)
        {
            RerCentralDTO entity = new RerCentralDTO();

            int iRercencodi = dr.GetOrdinal(this.Rercencodi);
            if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iRercenestado = dr.GetOrdinal(this.Rercenestado);
            if (!dr.IsDBNull(iRercenestado)) entity.Rercenestado = dr.GetString(iRercenestado);

            int iRercenfechainicio = dr.GetOrdinal(this.Rercenfechainicio);
            if (!dr.IsDBNull(iRercenfechainicio)) entity.Rercenfechainicio = dr.GetDateTime(iRercenfechainicio);

            int iRercenfechafin = dr.GetOrdinal(this.Rercenfechafin);
            if (!dr.IsDBNull(iRercenfechafin)) entity.Rercenfechafin = dr.GetDateTime(iRercenfechafin);

            int iRercenenergadj = dr.GetOrdinal(this.Rercenenergadj);
            if (!dr.IsDBNull(iRercenenergadj)) entity.Rercenenergadj = dr.GetDecimal(iRercenenergadj);

            int iRercenprecbase = dr.GetOrdinal(this.Rercenprecbase);
            if (!dr.IsDBNull(iRercenprecbase)) entity.Rercenprecbase = dr.GetDecimal(iRercenprecbase);

            int iRerceninflabase = dr.GetOrdinal(this.Rerceninflabase);
            if (!dr.IsDBNull(iRerceninflabase)) entity.Rerceninflabase = dr.GetDecimal(iRerceninflabase);

            int iRercendesccontrato = dr.GetOrdinal(this.Rercendesccontrato);
            if (!dr.IsDBNull(iRercendesccontrato)) entity.Rercendesccontrato = dr.GetString(iRercendesccontrato);

            int iCodentcodi = dr.GetOrdinal(this.Codentcodi);
            if (!dr.IsDBNull(iCodentcodi)) entity.Codentcodi = Convert.ToInt32(dr.GetValue(iCodentcodi));

            int iPingnombre = dr.GetOrdinal(this.Pingnombre);
            if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iRercenusucreacion = dr.GetOrdinal(this.Rercenusucreacion);
            if (!dr.IsDBNull(iRercenusucreacion)) entity.Rercenusucreacion = dr.GetString(iRercenusucreacion);

            int iRercenfeccreacion = dr.GetOrdinal(this.Rercenfeccreacion);
            if (!dr.IsDBNull(iRercenfeccreacion)) entity.Rercenfeccreacion = dr.GetDateTime(iRercenfeccreacion);

            int iRercenusumodificacion = dr.GetOrdinal(this.Rercenusumodificacion);
            if (!dr.IsDBNull(iRercenusumodificacion)) entity.Rercenusumodificacion = dr.GetString(iRercenusumodificacion);

            int iRercenfecmodificacion = dr.GetOrdinal(this.Rercenfecmodificacion);
            if (!dr.IsDBNull(iRercenfecmodificacion)) entity.Rercenfecmodificacion = dr.GetDateTime(iRercenfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rercencodi = "RERCENCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Famcodi = "FAMCODI";
        public string Rercenestado = "RERCENESTADO";
        public string Rercenfechainicio = "RERCENFECHAINICIO";
        public string Rercenfechafin = "RERCENFECHAFIN";
        public string Rercenenergadj = "RERCENENERGADJ";
        public string Rercenprecbase = "RERCENPRECBASE";
        public string Rerceninflabase = "RERCENINFLABASE";
        public string Rercendesccontrato = "RERCENDESCCONTRATO";
        public string Codentcodi = "CODENTCODI";
        public string Pingnombre = "PINGNOMBRE";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Rercenusucreacion = "RERCENUSUCREACION";
        public string Rercenfeccreacion = "RERCENFECCREACION";
        public string Rercenusumodificacion = "RERCENUSUMODIFICACION";
        public string Rercenfecmodificacion = "RERCENFECMODIFICACION";

        public string Equinomb = "EQUINOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Codentcodigo = "CODENTCODIGO";
        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";
        #endregion

        public string SqlListNombreCentralEmpresaBarra
        {
            get { return base.GetSqlXml("ListNombreCentralEmpresaBarra"); }
        }

        public string SqlListByFiltros
        {
            get { return base.GetSqlXml("ListByFiltros"); }
        }

        public string SqlListCentralREREmpresas
        {
            get { return base.GetSqlXml("ListCentralREREmpresas"); }
        }
        
        public string SqlListByEmprcodi
        {
            get { return base.GetSqlXml("ListByEmprcodi"); }
        }

        public string SqlListByEquiEmprFecha
        {
            get { return base.GetSqlXml("ListByEquiEmprFecha"); }
        }

        public string SqlListByFechasEstado
        {
            get { return base.GetSqlXml("ListByFechasEstado"); }
        }

        //CU21
        public string SqlListCentralByFecha
        {
            get { return base.GetSqlXml("ListCentralByFecha"); }
        }

        public string SqlListCodigoEntregaYBarraTransferencia
        {
            get { return base.GetSqlXml("ListCodigoEntregaYBarraTransferencia"); }
        }


        public string SqlListCentralByIds
        {
            get { return base.GetSqlXml("ListCentralByIds"); }
        }

        public string SqlListCentralByFechaLVTP
        {
            get { return base.GetSqlXml("ListCentralByFechaLVTP"); }
        }
    }
}