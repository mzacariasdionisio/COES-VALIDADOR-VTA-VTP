// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 21/03/2017
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
    public class VceArrparIncredGenHelper : HelperBase
    {

        public VceArrparIncredGenHelper()
            : base(Consultas.VceArrparIncredGenSql)
        {
        }

        public VceArrparIncredGenDTO Create(IDataReader dr)
        {
            VceArrparIncredGenDTO entity = new VceArrparIncredGenDTO();

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

            int iApinrefecha = dr.GetOrdinal(this.Apinrefecha);
            if (!dr.IsDBNull(iApinrefecha)) entity.Apinrefecha = dr.GetDateTime(iApinrefecha);

            int iApinrenuminc = dr.GetOrdinal(this.Apinrenuminc);
            if (!dr.IsDBNull(iApinrenuminc)) entity.Apinrenuminc = dr.GetInt32(iApinrenuminc);

            int iApinrenumdis = dr.GetOrdinal(this.Apinrenumdis);
            if (!dr.IsDBNull(iApinrenumdis)) entity.Apinrenumdis = dr.GetInt32(iApinrenumdis);

            int iApinreusucreacion = dr.GetOrdinal(this.Apinreusucreacion);
            if (!dr.IsDBNull(iApinreusucreacion)) entity.Apinreusucreacion = dr.GetString(iApinreusucreacion);

            int iApinrefeccreacion = dr.GetOrdinal(this.Apinrefeccreacion);
            if (!dr.IsDBNull(iApinrefeccreacion)) entity.Apinrefeccreacion = dr.GetDateTime(iApinrefeccreacion);

            int iApinreusumodificacion = dr.GetOrdinal(this.Apinreusumodificacion);
            if (!dr.IsDBNull(iApinreusumodificacion)) entity.Apinreusumodificacion = dr.GetString(iApinreusumodificacion);

            int iApinrefecmodificacion = dr.GetOrdinal(this.Apinrefecmodificacion);
            if (!dr.IsDBNull(iApinrefecmodificacion)) entity.Apinrefecmodificacion = dr.GetDateTime(iApinrefecmodificacion);

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            return entity;
        }

        public string Pecacodi = "PECACODI";

        public string Grupocodi = "GRUPOCODI";

        public string Apinrefecha = "APINREFECHA";

        public string Apinrenuminc = "APINRENUMINC";

        public string Apinrenumdis = "APINRENUMDIS";

        public string Apinreusucreacion = "APINREUSUCREACION";

        public string Apinrefeccreacion = "APINREFECCREACION";

        public string Apinreusumodificacion = "APINREUSUMODIFICACION";

        public string Apinrefecmodificacion = "APINREFECMODIFICACION";

        public string Gruponomb = "GRUPONOMB";

        public string SqlGetListaPorPeriodo
        {
            get { return GetSqlXml("SqlGetListaPorPeriodo"); }
        }

    }
}
