using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnAreamedicionHelper : HelperBase
    {
        public PrnAreamedicionHelper()
            : base(Consultas.PrnAreamedicionSql)
        {
        }

        public PrnAreamedicionDTO Create(IDataReader dr)
        {
            PrnAreamedicionDTO entity = new PrnAreamedicionDTO();

            int iAreamedcodi = dr.GetOrdinal(this.Areamedcodi);
            if (!dr.IsDBNull(iAreamedcodi)) entity.Areamedcodi = Convert.ToInt32(dr.GetValue(iAreamedcodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iAreamedfecinicial = dr.GetOrdinal(this.Areamedfecinicial);
            if (!dr.IsDBNull(iAreamedfecinicial)) entity.Areamedfecinicial = dr.GetDateTime(iAreamedfecinicial);

            int iAreamedfecfinal = dr.GetOrdinal(this.Areamedfecfinal);
            if (!dr.IsDBNull(iAreamedfecfinal)) entity.Areamedfecfinal = dr.GetDateTime(iAreamedfecfinal);

            int iAreamedestado = dr.GetOrdinal(this.Areamedestado);
            if (!dr.IsDBNull(iAreamedestado)) entity.Areamedestado = dr.GetString(iAreamedestado);

            int iAreamedfeccreacion = dr.GetOrdinal(this.Areamedfeccreacion);
            if (!dr.IsDBNull(iAreamedfeccreacion)) entity.Areamedfeccreacion = dr.GetDateTime(iAreamedfeccreacion);

            int iAreamedusucreacion = dr.GetOrdinal(this.Areamedusucreacion);
            if (!dr.IsDBNull(iAreamedusucreacion)) entity.Areamedusucreacion = dr.GetString(iAreamedusucreacion);

            int iAreamedfecmodificacion = dr.GetOrdinal(this.Areamedfecmodificacion);
            if (!dr.IsDBNull(iAreamedfecmodificacion)) entity.Areamedfecmodificacion = dr.GetDateTime(iAreamedfecmodificacion);

            int iAreamedusumodificacion = dr.GetOrdinal(this.Areamedusumodificacion);
            if (!dr.IsDBNull(iAreamedusumodificacion)) entity.Areamedusumodificacion = dr.GetString(iAreamedusumodificacion);

            return entity;
        }

        public PrnAreamedicionDTO ListVarexoCiudad(IDataReader dr)
        {
            PrnAreamedicionDTO entity = new PrnAreamedicionDTO();

            int iAreamedcodi = dr.GetOrdinal(this.Areamedcodi);
            if (!dr.IsDBNull(iAreamedcodi)) entity.Areamedcodi = Convert.ToInt32(dr.GetValue(iAreamedcodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iAreamedfecinicial = dr.GetOrdinal(this.Areamedfecinicial);
            if (!dr.IsDBNull(iAreamedfecinicial)) entity.Areamedfecinicial = dr.GetDateTime(iAreamedfecinicial);

            int iAreamedfecfinal = dr.GetOrdinal(this.Areamedfecfinal);
            if (!dr.IsDBNull(iAreamedfecfinal)) entity.Areamedfecfinal = dr.GetDateTime(iAreamedfecfinal);

            int iAreamedestado = dr.GetOrdinal(this.Areamedestado);
            if (!dr.IsDBNull(iAreamedestado)) entity.Areamedestado = dr.GetString(iAreamedestado);

            int iAreamedfeccreacion = dr.GetOrdinal(this.Areamedfeccreacion);
            if (!dr.IsDBNull(iAreamedfeccreacion)) entity.Areamedfeccreacion = dr.GetDateTime(iAreamedfeccreacion);

            int iAreamedusucreacion = dr.GetOrdinal(this.Areamedusucreacion);
            if (!dr.IsDBNull(iAreamedusucreacion)) entity.Areamedusucreacion = dr.GetString(iAreamedusucreacion);

            int iAreamedfecmodificacion = dr.GetOrdinal(this.Areamedfecmodificacion);
            if (!dr.IsDBNull(iAreamedfecmodificacion)) entity.Areamedfecmodificacion = dr.GetDateTime(iAreamedfecmodificacion);

            int iAreamedusumodificacion = dr.GetOrdinal(this.Areamedusumodificacion);
            if (!dr.IsDBNull(iAreamedusumodificacion)) entity.Areamedusumodificacion = dr.GetString(iAreamedusumodificacion);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

            return entity;
        }

        #region Mapeo de los campos
        public string Areamedcodi = "AREMEDCODI";
        public string Areacodi = "AREACODI";
        public string Areamedfecinicial = "AREMEDFECINICIAL";
        public string Areamedfecfinal = "AREMEDFECFINAL";
        public string Areamedestado = "AREMEDESTADO";
        public string Areamedfeccreacion = "AREMEDFECCREACION";
        public string Areamedusucreacion = "AREMEDUSUCREACION";
        public string Areamedfecmodificacion = "AREMEDFECMODIFICACION";
        public string Areamedusumodificacion = "AREMEDUSUMODIFICACION";
        public string Areaabrev = "AREAABREV";
        public string Areanomb = "AREANOMB";

        #endregion

        #region Consultas

        public string SqlListVarexoCiudad
        {
            get { return base.GetSqlXml("ListVarexoCiudad"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }

        #endregion

    }
}
