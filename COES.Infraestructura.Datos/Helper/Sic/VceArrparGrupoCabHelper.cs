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
    public class VceArrparGrupoCabHelper : HelperBase
    {

        public VceArrparGrupoCabHelper()
            : base(Consultas.VceArrparGrupoCabSql)
        {
        }

        public VceArrparGrupoCabDTO Create(IDataReader dr)
        {
            VceArrparGrupoCabDTO entity = new VceArrparGrupoCabDTO();

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

            int iApgcfccodi = dr.GetOrdinal(this.Apgcfccodi);
            if (!dr.IsDBNull(iApgcfccodi)) entity.Apgcfccodi = dr.GetString(iApgcfccodi);

            int iApgcabccbef = dr.GetOrdinal(this.Apgcabccbef);
            if (!dr.IsDBNull(iApgcabccbef)) entity.Apgcabccbef = dr.GetDecimal(iApgcabccbef);

            int iApgcabccmarr = dr.GetOrdinal(this.Apgcabccmarr);
            if (!dr.IsDBNull(iApgcabccmarr)) entity.Apgcabccmarr = dr.GetDecimal(iApgcabccmarr);

            int iApgcabccadic = dr.GetOrdinal(this.Apgcabccadic);
            if (!dr.IsDBNull(iApgcabccadic)) entity.Apgcabccadic = dr.GetDecimal(iApgcabccadic);

            int iApgcabflagcalcmanual = dr.GetOrdinal(this.Apgcabflagcalcmanual);
            if (!dr.IsDBNull(iApgcabflagcalcmanual)) entity.Apgcabflagcalcmanual = dr.GetString(iApgcabflagcalcmanual);

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            // Inicio de Agregado 31/05/2017 - Sistema de Compensaciones
            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iApgcabtotal = dr.GetOrdinal(this.Apgcabtotal);
            if (!dr.IsDBNull(iApgcabtotal)) entity.Apgcabtotal = dr.GetDecimal(iApgcabtotal);
            // Fin de Agregado
            return entity;
        }

        public string Pecacodi = "PECACODI";

        public string Grupocodi = "GRUPOCODI";

        public string Apgcfccodi = "APGCFCCODI";

        public string Apgcabccbef = "APGCABCCBEF";

        public string Apgcabccmarr = "APGCABCCMARR";

        public string Apgcabccadic = "APGCABCCADIC";

        public string Apgcabflagcalcmanual = "APGCABFLAGCALCMANUAL";

        public string Gruponomb = "GRUPONOMB";

        public string Emprnomb = "EMPRNOMB";
        
        public string Apgcabtotal = "APGCABTOTAL";

        public string SqlGetListaPorPeriodo
        {
            get { return GetSqlXml("SqlGetListaPorPeriodo"); }
        }

        public string SqlDeleteCabecera
        {
            get { return GetSqlXml("DeleteCabecera"); }
        }

        public string SqlDeleteDetalle
        {
            get { return GetSqlXml("DeleteDetalle"); }
        }
        public string SqlDeleteCabeceraByVersion
        {
            get { return GetSqlXml("DeleteCabeceraByVersion"); }
        }

        public string SqlDeleteDetalleByVersion
        {
            get { return GetSqlXml("DeleteDetalleByVersion"); }
        }
        
        public string SqlSaveCabeceraFromOtherVersion
        {
            get { return base.GetSqlXml("SaveCabeceraFromOtherVersion"); }
        }
        public string SqlSaveDetalleFromOtherVersion
        {
            get { return base.GetSqlXml("SaveDetalleFromOtherVersion"); }
        }

        public string SqlListByGroupCompArrpar
        {
            get { return GetSqlXml("ListByGroupCompArrpar"); }
        }

    }
}
