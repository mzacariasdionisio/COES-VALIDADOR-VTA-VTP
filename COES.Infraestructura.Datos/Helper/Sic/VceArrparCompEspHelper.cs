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
    public class VceArrparCompEspHelper : HelperBase
    {

        public VceArrparCompEspHelper()
            : base(Consultas.VceArrparCompEspSql)
        {
        }

        public VceArrparCompEspDTO Create(IDataReader dr)
        {
            VceArrparCompEspDTO entity = new VceArrparCompEspDTO();

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

            int iApstocodi = dr.GetOrdinal(this.Apstocodi);
            if (!dr.IsDBNull(iApstocodi)) entity.Apstocodi = dr.GetString(iApstocodi);

            int iApespcargafinal = dr.GetOrdinal(this.Apespcargafinal);
            if (!dr.IsDBNull(iApespcargafinal)) entity.Apespcargafinal = dr.GetDecimal(iApespcargafinal);

            int iApespenergprod = dr.GetOrdinal(this.Apespenergprod);
            if (!dr.IsDBNull(iApespenergprod)) entity.Apespenergprod = dr.GetDecimal(iApespenergprod);

            int iApesprendvigente = dr.GetOrdinal(this.Apesprendvigente);
            if (!dr.IsDBNull(iApesprendvigente)) entity.Apesprendvigente = dr.GetDecimal(iApesprendvigente);

            int iApesppreciocomb = dr.GetOrdinal(this.Apesppreciocomb);
            if (!dr.IsDBNull(iApesppreciocomb)) entity.Apesppreciocomb = dr.GetDecimal(iApesppreciocomb);

            int iApespcombbase = dr.GetOrdinal(this.Apespcombbase);
            if (!dr.IsDBNull(iApespcombbase)) entity.Apespcombbase = dr.GetDecimal(iApespcombbase);

            int iApespcombrampa = dr.GetOrdinal(this.Apespcombrampa);
            if (!dr.IsDBNull(iApespcombrampa)) entity.Apespcombrampa = dr.GetDecimal(iApespcombrampa);

            int iApespcombreconocxtransf = dr.GetOrdinal(this.Apespcombreconocxtransf);
            if (!dr.IsDBNull(iApespcombreconocxtransf)) entity.Apespcombreconocxtransf = dr.GetDecimal(iApespcombreconocxtransf);

            int iApesppreciocombalt = dr.GetOrdinal(this.Apesppreciocombalt);
            if (!dr.IsDBNull(iApesppreciocombalt)) entity.Apesppreciocombalt = dr.GetDecimal(iApesppreciocombalt);

            int iApespcombbasealt = dr.GetOrdinal(this.Apespcombbasealt);
            if (!dr.IsDBNull(iApespcombbasealt)) entity.Apespcombbasealt = dr.GetDecimal(iApespcombbasealt);

            int iApespcombrampaalt = dr.GetOrdinal(this.Apespcombrampaalt);
            if (!dr.IsDBNull(iApespcombrampaalt)) entity.Apespcombrampaalt = dr.GetDecimal(iApespcombrampaalt);

            int iApespcombreconocxtransfalt = dr.GetOrdinal(this.Apespcombreconocxtransfalt);
            if (!dr.IsDBNull(iApespcombreconocxtransfalt)) entity.Apespcombreconocxtransfalt = dr.GetDecimal(iApespcombreconocxtransfalt);

            int iApespcompensacion = dr.GetOrdinal(this.Apespcompensacion);
            if (!dr.IsDBNull(iApespcompensacion)) entity.Apespcompensacion = dr.GetDecimal(iApespcompensacion);

            //- Adicionales

            int iGrupoNomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

            int iAptopcodi = dr.GetOrdinal(this.Aptopcodi);
            if (!dr.IsDBNull(iAptopcodi)) entity.Aptopcodi = dr.GetString(iAptopcodi);

            int iAptopnombre = dr.GetOrdinal(this.Aptopnombre);
            if (!dr.IsDBNull(iAptopnombre)) entity.Aptopnombre = dr.GetString(iAptopnombre);

            int iApstonombre = dr.GetOrdinal(this.Apstonombre);
            if (!dr.IsDBNull(iApstonombre)) entity.Apstonombre = dr.GetString(iApstonombre);

            int iApespfechadesc = dr.GetOrdinal(this.Apespfechadesc);
            if (!dr.IsDBNull(iApespfechadesc)) entity.Apespfechadesc = dr.GetString(iApespfechadesc);
          
            return entity;
        }

        public string Pecacodi = "PECACODI";

        public string Grupocodi = "GRUPOCODI";

        public string Apespfecha = "APESPFECHA";

        public string Apstocodi = "APSTOCODI";

        public string Apespcargafinal = "APESPCARGAFINAL";

        public string Apespenergprod = "APESPENERGPROD";

        public string Apesprendvigente = "APESPRENDVIGENTE";

        public string Apesppreciocomb = "APESPPRECIOCOMB";

        public string Apespcombbase = "APESPCOMBBASE";

        public string Apespcombrampa = "APESPCOMBRAMPA";

        public string Apespcombreconocxtransf = "APESPCOMBRECONOCXTRANSF";

        public string Apesppreciocombalt = "APESPPRECIOCOMBALT";

        public string Apespcombbasealt = "APESPCOMBBASEALT";

        public string Apespcombrampaalt = "APESPCOMBRAMPAALT";

        public string Apespcombreconocxtransfalt = "APESPCOMBRECONOCXTRANSFALT";

        public string Apespcompensacion = "APESPCOMPENSACION";

        //- Complementarios

        public string Gruponomb = "GRUPONOMB";

        public string Aptopcodi = "APTOPCODI";

        public string Aptopnombre = "APTOPNOMBRE";

        public string Apstonombre = "APSTONOMBRE";

        public string Apespfechadesc = "APESPFECHADESC";

        public string Apesptipodesc = "APESPTIPODESC";

        public string SqlListByPeriod
        {
            get { return GetSqlXml("ListByPeriod"); }
        }

    }
}
