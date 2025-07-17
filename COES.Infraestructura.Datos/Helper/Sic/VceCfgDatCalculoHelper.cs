// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 02/03/2017
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
    public class VceCfgDatCalculoHelper : HelperBase
    {

        public VceCfgDatCalculoHelper()
            : base(Consultas.VceCfgDatcalculoSql)
        {
        }

        public VceCfgDatCalculoDTO Create(IDataReader dr)
        {
            VceCfgDatCalculoDTO entity = new VceCfgDatCalculoDTO();

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = dr.GetInt32(iConcepcodi);

            int iCfgdccamponomb = dr.GetOrdinal(this.Cfgdccamponomb);
            if (!dr.IsDBNull(iCfgdccamponomb)) entity.Cfgdccamponomb = dr.GetString(iCfgdccamponomb);

            int iCfgdctipoval = dr.GetOrdinal(this.Cfgdctipoval);
            if (!dr.IsDBNull(iCfgdctipoval)) entity.Cfgdctipoval = dr.GetString(iCfgdctipoval);

            int iCfgdccondsql = dr.GetOrdinal(this.Cfgdccondsql);
            if (!dr.IsDBNull(iCfgdccondsql)) entity.Cfgdccondsql = dr.GetString(iCfgdccondsql);

            int iCfgdcestregistro = dr.GetOrdinal(this.Cfgdcestregistro);
            if (!dr.IsDBNull(iCfgdcestregistro)) entity.Cfgdcestregistro = dr.GetString(iCfgdcestregistro);

            return entity;
        }

        public string Fenergcodi = "FENERGCODI";

        public string Concepcodi = "CONCEPCODI";

        public string Cfgdccamponomb = "CFGDCCAMPONOMB";

        public string Cfgdctipoval = "CFGDCTIPOVAL";

        public string Cfgdccondsql = "CFGDCCONDSQL";

        public string Cfgdcestregistro = "CFGDCESTREGISTRO";

        public string SqlGetCfgsCamp
        {
            get { return GetSqlXml("SqlGetCfgsCamp"); }
        }

    }
}
