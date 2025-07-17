using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_INTERRUP_SUMINISTRO
    /// </summary>
    public class AfInterrupSuministroHelper : HelperBase
    {
        public AfInterrupSuministroHelper() : base(Consultas.AfInterrupSuministroSql)
        {
        }

        public AfInterrupSuministroDTO Create(IDataReader dr)
        {
            AfInterrupSuministroDTO entity = new AfInterrupSuministroDTO();

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iIntsummwred = dr.GetOrdinal(this.Intsummwred);
            if (!dr.IsDBNull(iIntsummwred)) entity.Intsummwred = dr.GetDecimal(iIntsummwred);

            int iIntsummwfin = dr.GetOrdinal(this.Intsummwfin);
            if (!dr.IsDBNull(iIntsummwfin)) entity.Intsummwfin = dr.GetDecimal(iIntsummwfin);

            int iIntsumsuministro = dr.GetOrdinal(this.Intsumsuministro);
            if (!dr.IsDBNull(iIntsumsuministro)) entity.Intsumsuministro = dr.GetString(iIntsumsuministro);

            int iIntsumobs = dr.GetOrdinal(this.Intsumobs);
            if (!dr.IsDBNull(iIntsumobs)) entity.Intsumobs = dr.GetString(iIntsumobs);

            int iIntsumnumetapa = dr.GetOrdinal(this.Intsumnumetapa);
            if (!dr.IsDBNull(iIntsumnumetapa)) entity.Intsumnumetapa = Convert.ToInt32(dr.GetValue(iIntsumnumetapa));

            int iIntsumduracion = dr.GetOrdinal(this.Intsumduracion);
            if (!dr.IsDBNull(iIntsumduracion)) entity.Intsumduracion = dr.GetDecimal(iIntsumduracion);

            int iIntsumfuncion = dr.GetOrdinal(this.Intsumfuncion);
            if (!dr.IsDBNull(iIntsumfuncion)) entity.Intsumfuncion = dr.GetString(iIntsumfuncion);

            int iIntsumfechainterrfin = dr.GetOrdinal(this.Intsumfechainterrfin);
            if (!dr.IsDBNull(iIntsumfechainterrfin)) entity.Intsumfechainterrfin = dr.GetDateTime(iIntsumfechainterrfin);

            int iIntsumfechainterrini = dr.GetOrdinal(this.Intsumfechainterrini);
            if (!dr.IsDBNull(iIntsumfechainterrini)) entity.Intsumfechainterrini = dr.GetDateTime(iIntsumfechainterrini);

            int iIntsummw = dr.GetOrdinal(this.Intsummw);
            if (!dr.IsDBNull(iIntsummw)) entity.Intsummw = dr.GetDecimal(iIntsummw);

            int iIntsumsubestacion = dr.GetOrdinal(this.Intsumsubestacion);
            if (!dr.IsDBNull(iIntsumsubestacion)) entity.Intsumsubestacion = dr.GetString(iIntsumsubestacion);

            int iIntsumempresa = dr.GetOrdinal(this.Intsumempresa);
            if (!dr.IsDBNull(iIntsumempresa)) entity.Intsumempresa = dr.GetString(iIntsumempresa);

            int iIntsumzona = dr.GetOrdinal(this.Intsumzona);
            if (!dr.IsDBNull(iIntsumzona)) entity.Intsumzona = dr.GetString(iIntsumzona);

            int iIntsumcodi = dr.GetOrdinal(this.Intsumcodi);
            if (!dr.IsDBNull(iIntsumcodi)) entity.Intsumcodi = Convert.ToInt32(dr.GetValue(iIntsumcodi));

            int iAfecodi = dr.GetOrdinal(this.Afecodi);
            if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = Convert.ToInt32(dr.GetValue(iAfecodi));

            int iIntsumfeccreacion = dr.GetOrdinal(this.Intsumfeccreacion);
            if (!dr.IsDBNull(iIntsumfeccreacion)) entity.Intsumfeccreacion = dr.GetDateTime(iIntsumfeccreacion);

            int iIntsumusucreacion = dr.GetOrdinal(this.Intsumusucreacion);
            if (!dr.IsDBNull(iIntsumusucreacion)) entity.Intsumusucreacion = dr.GetString(iIntsumusucreacion);

            int iIntsumestado = dr.GetOrdinal(this.Intsumestado);
            if (!dr.IsDBNull(iIntsumestado)) entity.Intsumestado = Convert.ToInt32(dr.GetValue(iIntsumestado));

            return entity;
        }


        #region Mapeo de Campos

        public string Enviocodi = "ENVIOCODI";
        public string Intsummwred = "INTSUMMWRED";
        public string Intsummwfin = "INTSUMMWFIN";
        public string Intsumsuministro = "INTSUMSUMINISTRO";
        public string Intsumobs = "INTSUMOBS";
        public string Intsumnumetapa = "INTSUMNUMETAPA";
        public string Intsumduracion = "INTSUMDURACION";
        public string Intsumfuncion = "INTSUMFUNCION";
        public string Intsumfechainterrfin = "INTSUMFECHAINTERRFIN";
        public string Intsumfechainterrini = "INTSUMFECHAINTERRINI";
        public string Intsummw = "INTSUMMW";
        public string Intsumsubestacion = "INTSUMSUBESTACION";
        public string Intsumempresa = "INTSUMEMPRESA";
        public string Intsumzona = "INTSUMZONA";
        public string Intsumcodi = "INTSUMCODI";
        public string Afecodi = "AFECODI";
        public string Intsumfeccreacion = "INTSUMFECCREACION";
        public string Intsumusucreacion = "INTSUMUSUCREACION";
        public string Intsumestado = "INTSUMESTADO";

        public string Emprcodi = "EMPRCODI";
        public string Fdatcodi = "FDATCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Evencodi = "EVENCODI";
        public string Evenini = "EVENINI";
        public string Eracmfsuministrador = "ERACMFSUMINISTRADOR";
        public string Afemprnomb = "AFEMPRNOMB"; 

        #endregion

        public string SqlObtenerUltimoEnvio
        {
            get { return GetSqlXml("ObtenerUltimoEnvio"); }
        }

        public string SqlListarReporteExtranetCTAF
        {
            get { return GetSqlXml("ListarReporteExtranetCTAF"); }
        }

        public string SqlUpdateAEstadoBaja
        {
            get { return GetSqlXml("UpdateAEstadoBaja"); }
        }

        public string SqlListarReporteInterrupcionesCTAF
        {
            get { return GetSqlXml("ListarReporteInterrupcionesCTAF"); }
        }
    }
}
