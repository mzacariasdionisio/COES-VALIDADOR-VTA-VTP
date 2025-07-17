// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 29/03/2017
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
    public class VceTextoReporteHelper : HelperBase
    {

        public VceTextoReporteHelper() : base(Consultas.VceTextoReporteSql)
        {
        }

        public VceTextoReporteDTO Create(IDataReader dr)
        {
            VceTextoReporteDTO entity = new VceTextoReporteDTO();

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

            int iTxtrepcodreporte = dr.GetOrdinal(this.Txtrepcodreporte);
            if (!dr.IsDBNull(iTxtrepcodreporte)) entity.Txtrepcodreporte = dr.GetString(iTxtrepcodreporte);

            int iTxtrepcodtexto = dr.GetOrdinal(this.Txtrepcodtexto);
            if (!dr.IsDBNull(iTxtrepcodtexto)) entity.Txtrepcodtexto = dr.GetString(iTxtrepcodtexto);

            int iTxtreptexto = dr.GetOrdinal(this.Txtreptexto);
            if (!dr.IsDBNull(iTxtreptexto)) entity.Txtreptexto = dr.GetString(iTxtreptexto);

            int iTxtrepusucreacion = dr.GetOrdinal(this.Txtrepusucreacion);
            if (!dr.IsDBNull(iTxtrepusucreacion)) entity.Txtrepusucreacion = dr.GetString(iTxtrepusucreacion);

            int iTxtrepfeccreacion = dr.GetOrdinal(this.Txtrepfeccreacion);
            if (!dr.IsDBNull(iTxtrepfeccreacion)) entity.Txtrepfeccreacion = dr.GetDateTime(iTxtrepfeccreacion);

            int iTxtrepusumodificacion = dr.GetOrdinal(this.Txtrepusumodificacion);
            if (!dr.IsDBNull(iTxtrepusumodificacion)) entity.Txtrepusumodificacion = dr.GetString(iTxtrepusumodificacion);

            int iTxtrepfecmodificacion = dr.GetOrdinal(this.Txtrepfecmodificacion);
            if (!dr.IsDBNull(iTxtrepfecmodificacion)) entity.Txtrepfecmodificacion = dr.GetDateTime(iTxtrepfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Pecacodi = "PECACODI";

        public string Txtrepcodreporte = "TXTREPCODREPORTE";

        public string Txtrepcodtexto = "TXTREPCODTEXTO";

        public string Txtreptexto = "TXTREPTEXTO";

        public string Txtrepusucreacion = "TXTREPUSUCREACION";

        public string Txtrepfeccreacion = "TXTREPFECCREACION";

        public string Txtrepusumodificacion = "TXTREPUSUMODIFICACION";

        public string Txtrepfecmodificacion = "TXTREPFECMODIFICACION";

        #endregion

        public string SqlListByPeriodo
        {
            get { return base.GetSqlXml("ListByPeriodo"); }
        }

        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }
    }
}
