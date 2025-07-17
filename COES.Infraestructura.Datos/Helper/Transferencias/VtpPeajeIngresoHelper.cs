using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_PEAJE_INGRESO
    /// </summary>
    public class VtpPeajeIngresoHelper : HelperBase
    {
        public VtpPeajeIngresoHelper()
            : base(Consultas.VtpPeajeIngresoSql)
        {
        }

        public VtpPeajeIngresoDTO Create(IDataReader dr)
        {
            VtpPeajeIngresoDTO entity = new VtpPeajeIngresoDTO();

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iPingtipo = dr.GetOrdinal(this.Pingtipo);
            if (!dr.IsDBNull(iPingtipo)) entity.Pingtipo = dr.GetString(iPingtipo);

            int iPingnombre = dr.GetOrdinal(this.Pingnombre);
            if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRrpecodi = dr.GetOrdinal(this.Rrpecodi);
            if (!dr.IsDBNull(iRrpecodi)) entity.Rrpecodi = Convert.ToInt32(dr.GetValue(iRrpecodi));

            int iPingpago = dr.GetOrdinal(this.Pingpago);
            if (!dr.IsDBNull(iPingpago)) entity.Pingpago = dr.GetString(iPingpago);

            int iPingtransmision = dr.GetOrdinal(this.Pingtransmision);
            if (!dr.IsDBNull(iPingtransmision)) entity.Pingtransmision = dr.GetString(iPingtransmision);

            int iPingcodigo = dr.GetOrdinal(this.Pingcodigo);
            if (!dr.IsDBNull(iPingcodigo)) entity.Pingcodigo = dr.GetString(iPingcodigo);

            int iPingpeajemensual = dr.GetOrdinal(this.Pingpeajemensual);
            if (!dr.IsDBNull(iPingpeajemensual)) entity.Pingpeajemensual = dr.GetDecimal(iPingpeajemensual);

            int iPingtarimensual = dr.GetOrdinal(this.Pingtarimensual);
            if (!dr.IsDBNull(iPingtarimensual)) entity.Pingtarimensual = dr.GetDecimal(iPingtarimensual);

            int iPingregulado = dr.GetOrdinal(this.Pingregulado);
            if (!dr.IsDBNull(iPingregulado)) entity.Pingregulado = dr.GetDecimal(iPingregulado);

            int iPinglibre = dr.GetOrdinal(this.Pinglibre);
            if (!dr.IsDBNull(iPinglibre)) entity.Pinglibre = dr.GetDecimal(iPinglibre);

            int iPinggranusuario = dr.GetOrdinal(this.Pinggranusuario);
            if (!dr.IsDBNull(iPinggranusuario)) entity.Pinggranusuario = dr.GetDecimal(iPinggranusuario);

            int iPingporctregulado = dr.GetOrdinal(this.Pingporctregulado);
            if (!dr.IsDBNull(iPingporctregulado)) entity.Pingporctregulado = dr.GetDecimal(iPingporctregulado);

            int iPingporctlibre = dr.GetOrdinal(this.Pingporctlibre);
            if (!dr.IsDBNull(iPingporctlibre)) entity.Pingporctlibre = dr.GetDecimal(iPingporctlibre);

            int iPingporctgranusuario = dr.GetOrdinal(this.Pingporctgranusuario);
            if (!dr.IsDBNull(iPingporctgranusuario)) entity.Pingporctgranusuario = dr.GetDecimal(iPingporctgranusuario);

            int iPingusucreacion = dr.GetOrdinal(this.Pingusucreacion);
            if (!dr.IsDBNull(iPingusucreacion)) entity.Pingusucreacion = dr.GetString(iPingusucreacion);

            int iPingfeccreacion = dr.GetOrdinal(this.Pingfeccreacion);
            if (!dr.IsDBNull(iPingfeccreacion)) entity.Pingfeccreacion = dr.GetDateTime(iPingfeccreacion);

            int iPingusumodificacion = dr.GetOrdinal(this.Pingusumodificacion);
            if (!dr.IsDBNull(iPingusumodificacion)) entity.Pingusumodificacion = dr.GetString(iPingusumodificacion);

            int iPingfecmodificacion = dr.GetOrdinal(this.Pingfecmodificacion);
            if (!dr.IsDBNull(iPingfecmodificacion)) entity.Pingfecmodificacion = dr.GetDateTime(iPingfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Pingcodi = "PINGCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Pingtipo = "PINGTIPO";
        public string Pingnombre = "PINGNOMBRE";
        public string Emprcodi = "EMPRCODI";
        public string Rrpecodi = "RRPECODI";
        public string Pingpago = "PINGPAGO";
        public string Pingtransmision = "PINGTRANSMISION";
        public string Pingcodigo = "PINGCODIGO";
        public string Pingpeajemensual = "PINGPEAJEMENSUAL";
        public string Pingtarimensual = "PINGTARIMENSUAL";
        public string Pingregulado = "PINGREGULADO";
        public string Pinglibre = "PINGLIBRE";
        public string Pinggranusuario = "PINGGRANUSUARIO";
        public string Pingporctregulado = "PINGPORCTREGULADO";
        public string Pingporctlibre = "PINGPORCTLIBRE";
        public string Pingporctgranusuario = "PINGPORCTGRANUSUARIO";
        public string Pingusucreacion = "PINGUSUCREACION";
        public string Pingfeccreacion = "PINGFECCREACION";
        public string Pingusumodificacion = "PINGUSUMODIFICACION";
        public string Pingfecmodificacion = "PINGFECMODIFICACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Rrpenombre = "RRPENOMBRE";

        #endregion

        public string SqlListView
        {
            get { return base.GetSqlXml("ListView"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }

        public string SqlUpdateDesarrollo
        {
            get { return base.GetSqlXml("UpdateDesarrollo"); }
        }

        public string SqlGetByNombreIngresoTarifario
        {
            get { return base.GetSqlXml("GetByNombreIngresoTarifario"); }
        }

        public string SqlListPagoSi
        {
            get { return base.GetSqlXml("ListPagoSi"); }
        }

        public string SqlListCargo
        {
            get { return base.GetSqlXml("ListCargo"); }
        }

        public string SqlListTransmisionSi
        {
            get { return base.GetSqlXml("ListTransmisionSi"); }
        }

        public string SqlListIngresoTarifarioMensual
        {
            get { return base.GetSqlXml("ListIngresoTarifarioMensual"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        #region FIT - VALORIZACION DIARIA

        public string SqlGetPeajeUnitario
        {
            get { return base.GetSqlXml("GetPeajeUnitario"); }
        }

        #endregion

        #region PrimasRER.2023
        public string SqlListCargoPrimaRER
        {
            get { return base.GetSqlXml("ListCargoPrimaRER"); }
        }
        #endregion
    }
}
