// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 06/04/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class VceArrparRampaCfgHelper : HelperBase
    {

        public VceArrparRampaCfgHelper()
            : base(Consultas.VceArrparRampaCfgSql)
        {
        }

        public VceArrparRampaCfgDTO Create(IDataReader dr)
        {
            VceArrparRampaCfgDTO entity = new VceArrparRampaCfgDTO();

            int iApramcodi = dr.GetOrdinal(this.Apramcodi);
            if (!dr.IsDBNull(iApramcodi)) entity.Apramcodi = dr.GetInt32(iApramcodi);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

//            int iApramtipo = dr.GetOrdinal(this.Apramtipo);
//            if (!dr.IsDBNull(iApramtipo)) entity.Apramtipo = dr.GetString(iApramtipo);

            int iApstocodi = dr.GetOrdinal(this.Apstocodi);
            if (!dr.IsDBNull(iApstocodi)) entity.Apstocodi = dr.GetString(iApstocodi);

            int iApramhorasacum = dr.GetOrdinal(this.Apramhorasacum);
            if (!dr.IsDBNull(iApramhorasacum)) entity.Apramhorasacum = dr.GetDecimal(iApramhorasacum);

            int iAprampotenciabruta = dr.GetOrdinal(this.Aprampotenciabruta);
            if (!dr.IsDBNull(iAprampotenciabruta)) entity.Aprampotenciabruta = dr.GetDecimal(iAprampotenciabruta);

            int iApramenergiaacum = dr.GetOrdinal(this.Apramenergiaacum);
            if (!dr.IsDBNull(iApramenergiaacum)) entity.Apramenergiaacum = dr.GetDecimal(iApramenergiaacum);

            int iApramconsumobloqued2 = dr.GetOrdinal(this.Apramconsumobloqued2);
            if (!dr.IsDBNull(iApramconsumobloqued2)) entity.Apramconsumobloqued2 = dr.GetDecimal(iApramconsumobloqued2);

            int iApramconsumobloquecarb = dr.GetOrdinal(this.Apramconsumobloquecarb);
            if (!dr.IsDBNull(iApramconsumobloquecarb)) entity.Apramconsumobloquecarb = dr.GetDecimal(iApramconsumobloquecarb);

            int iApramconsumoacumd2 = dr.GetOrdinal(this.Apramconsumoacumd2);
            if (!dr.IsDBNull(iApramconsumoacumd2)) entity.Apramconsumoacumd2 = dr.GetDecimal(iApramconsumoacumd2);

            int iApramconsumoacumcarb = dr.GetOrdinal(this.Apramconsumoacumcarb);
            if (!dr.IsDBNull(iApramconsumoacumcarb)) entity.Apramconsumoacumcarb = dr.GetDecimal(iApramconsumoacumcarb);

            int iApramusucreacion = dr.GetOrdinal(this.Apramusucreacion);
            if (!dr.IsDBNull(iApramusucreacion)) entity.Apramusucreacion = dr.GetString(iApramusucreacion);

            int iApramfeccreacion = dr.GetOrdinal(this.Apramfeccreacion);
            if (!dr.IsDBNull(iApramfeccreacion)) entity.Apramfeccreacion = dr.GetDateTime(iApramfeccreacion);

            int iApramusumodificacion = dr.GetOrdinal(this.Apramusumodificacion);
            if (!dr.IsDBNull(iApramusumodificacion)) entity.Apramusumodificacion = dr.GetString(iApramusumodificacion);

            int iApramfecmodificacion = dr.GetOrdinal(this.Apramfecmodificacion);
            if (!dr.IsDBNull(iApramfecmodificacion)) entity.Apramfecmodificacion = dr.GetDateTime(iApramfecmodificacion);

            return entity;
        }

        public string Apramcodi = "APRAMCODI";

        public string Grupocodi = "GRUPOCODI";

        //public string Apramtipo = "APRAMTIPO";

        public string Apstocodi = "APSTOCODI";

        public string Apramhorasacum = "APRAMHORASACUM";

        public string Aprampotenciabruta = "APRAMPOTENCIABRUTA";

        public string Apramenergiaacum = "APRAMENERGIAACUM";

        public string Apramconsumobloqued2 = "APRAMCONSUMOBLOQUED2";

        public string Apramconsumobloquecarb = "APRAMCONSUMOBLOQUECARB";

        public string Apramconsumoacumd2 = "APRAMCONSUMOACUMD2";

        public string Apramconsumoacumcarb = "APRAMCONSUMOACUMCARB";

        public string Apramusucreacion = "APRAMUSUCREACION";

        public string Apramfeccreacion = "APRAMFECCREACION";

        public string Apramusumodificacion = "APRAMUSUMODIFICACION";

        public string Apramfecmodificacion = "APRAMFECMODIFICACION";

        public string SqlRangoInferiorPar
        {
            get { return GetSqlXml("getRangoInferiorPar"); }
        }

        public string SqlRangoSuperiorPar
        {
            get { return GetSqlXml("getRangoSuperiorPar"); }
        }

        public string SqlRangoInferiorArr
        {
            get { return GetSqlXml("getRangoInferiorArr"); }
        }

        public string SqlRangoSuperiorArr
        {
            get { return GetSqlXml("getRangoSuperiorArr"); }
        }

    }
}

