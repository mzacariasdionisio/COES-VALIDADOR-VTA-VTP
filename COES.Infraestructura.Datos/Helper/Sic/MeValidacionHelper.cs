using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_VALIDACION
    /// </summary>
    public class MeValidacionHelper : HelperBase
    {
        public MeValidacionHelper()
            : base(Consultas.MeValidacionSql)
        {
        }

        public MeValidacionDTO Create(IDataReader dr)
        {
            MeValidacionDTO entity = new MeValidacionDTO();

            int iValidcodi = dr.GetOrdinal(this.Validcodi);
            if (!dr.IsDBNull(iValidcodi)) entity.Validcodi = Convert.ToInt32(dr.GetValue(iValidcodi));

            int iValidcomentario = dr.GetOrdinal(this.Validcomentario);
            if (!dr.IsDBNull(iValidcomentario)) entity.Validcomentario = dr.GetString(iValidcomentario);

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValidfechaperiodo = dr.GetOrdinal(this.Validfechaperiodo);
            if (!dr.IsDBNull(iValidfechaperiodo)) entity.Validfechaperiodo = dr.GetDateTime(iValidfechaperiodo);

            int iValidestado = dr.GetOrdinal(this.Validestado);
            if (!dr.IsDBNull(iValidestado)) entity.Validestado = Convert.ToInt32(dr.GetValue(iValidestado));

            int iValidusumodificaion = dr.GetOrdinal(this.Validusumodificacion);
            if (!dr.IsDBNull(iValidusumodificaion)) entity.Validusumodificacion = dr.GetString(iValidusumodificaion);

            int iValidfecmodificacion = dr.GetOrdinal(this.Validfecmodificacion);
            if (!dr.IsDBNull(iValidfecmodificacion)) entity.Validfecmodificacion = dr.GetDateTime(iValidfecmodificacion);

            int iValidplazo = dr.GetOrdinal(this.Validplazo);
            if (!dr.IsDBNull(iValidplazo)) entity.Validplazo = dr.GetString(iValidplazo);

            int iValiddataconsiderada = dr.GetOrdinal(this.Validdataconsiderada);
            if (!dr.IsDBNull(iValiddataconsiderada)) entity.Validdataconsiderada = dr.GetDecimal(iValiddataconsiderada);

            int iValiddatainformada = dr.GetOrdinal(this.Validdatainformada);
            if (!dr.IsDBNull(iValiddatainformada)) entity.Validdatainformada = dr.GetDecimal(iValiddatainformada);

            int iValiddatasinobs = dr.GetOrdinal(this.Validdatasinobs);
            if (!dr.IsDBNull(iValiddatasinobs)) entity.Validdatasinobs = dr.GetDecimal(iValiddatasinobs);

            return entity;
        }


        #region Mapeo de Campos

        public string Formatcodi = "FORMATCODI";
        public string Emprcodi = "EMPRCODI";
        public string Validfechaperiodo = "VALIDFECHAPERIODO";
        public string Validestado = "VALIDESTADO";
        public string Validusumodificacion = "VALIDUSUMODIFICACION";
        public string Validfecmodificacion = "VALIDFECMODIFICACION";
        public string Validcodi = "VALIDCODI";
        public string Validcomentario = "VALIDCOMENTARIO";
        public string Validplazo = "VALIDPLAZO";
        public string Validdataconsiderada = "VALIDDATACONSIDERADA";
        public string Validdatainformada = "VALIDDATAINFORMADA";
        public string Validdatasinobs = "VALIDDATASINOBS";

        public string Emprnomb = "EMPRNOMB";
        public string Formatnombre = "FORMATNOMBRE";

        #endregion

        public string SqlValidarEmpresa
        {
            get { return base.GetSqlXml("ValidarEmpresa"); }
        }

		public string SqlDeleteAllEmpresa
        {
            get { return base.GetSqlXml("DeleteAllEmpresa"); }
        }

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListarValidacionXFormatoYFecha
        {
            get { return base.GetSqlXml("ListarValidacionXFormatoYFecha"); }
        }

        public string SqlUpdateById
        {
            get { return base.GetSqlXml("UpdateById"); }
        }
    }
}
