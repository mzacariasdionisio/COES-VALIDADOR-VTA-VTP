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
    public class VceArrparTipoOperaHelper : HelperBase
    {

        public VceArrparTipoOperaHelper()
            : base(Consultas.VceArrparTipoOperaSql)
        {
        }

        public VceArrparTipoOperaDTO Create(IDataReader dr)
        {
            VceArrparTipoOperaDTO entity = new VceArrparTipoOperaDTO();

            int iAptopcodi = dr.GetOrdinal(this.Aptopcodi);
            if (!dr.IsDBNull(iAptopcodi)) entity.Aptopcodi = dr.GetString(iAptopcodi);

            int iApstocodi = dr.GetOrdinal(this.Apstocodi);
            if (!dr.IsDBNull(iApstocodi)) entity.Apstocodi = dr.GetString(iApstocodi);

            int iApstonombre = dr.GetOrdinal(this.Apstonombre);
            if (!dr.IsDBNull(iApstonombre)) entity.Apstonombre = dr.GetString(iApstonombre);

            int iApstoconscomb_concepto = dr.GetOrdinal(this.Apstoconscomb_concepto);
            if (!dr.IsDBNull(iApstoconscomb_concepto)) entity.Apstoconscomb_concepto = dr.GetDecimal(iApstoconscomb_concepto);
          
            return entity;
        }

        public string Aptopcodi = "APTOPCODI";

        public string Apstocodi = "APSTOCODI";

        public string Apstonombre = "APSTONOMBRE";

        public string Apstoconscomb_concepto = "APSTOCONSCOMB_CONCEPTO";

        public string SqlListByType
        {
            get { return GetSqlXml("ListByType"); }
        }

        public string SqlGetConceptoVceArrparTipoOpera
        {
            get { return GetSqlXml("getConceptoVceArrparTipoOpera"); }
        }

    }
}
