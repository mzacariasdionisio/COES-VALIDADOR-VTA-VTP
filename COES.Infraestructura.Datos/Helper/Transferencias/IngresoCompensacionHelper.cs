using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_ing_compensacion
    /// </summary>
    public class IngresoCompensacionHelper : HelperBase
    {
        public IngresoCompensacionHelper()
            : base(Consultas.IngresoCompensacionSql)
        {
        }

        public IngresoCompensacionDTO Create(IDataReader dr)
        {
            IngresoCompensacionDTO entity = new IngresoCompensacionDTO();

            int iIngrCompCodi = dr.GetOrdinal(this.IngrCompCodi);
            if (!dr.IsDBNull(iIngrCompCodi)) entity.IngrCompCodi = dr.GetInt32(iIngrCompCodi);

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            int iCompCodi = dr.GetOrdinal(this.CompCodi);
            if (!dr.IsDBNull(iCompCodi)) entity.CompCodi = dr.GetInt32(iCompCodi);

            int iIngrCompVers = dr.GetOrdinal(this.IngrCompVers);
            if (!dr.IsDBNull(iIngrCompVers)) entity.IngrCompVersion = dr.GetInt32(iIngrCompVers);

            int iIngrCompImporte = dr.GetOrdinal(this.IngrCompImporte);
            if (!dr.IsDBNull(iIngrCompImporte)) entity.IngrCompImporte = dr.GetDecimal(iIngrCompImporte);

            int iIngrCompusername = dr.GetOrdinal(this.IngrCompusername);
            if (!dr.IsDBNull(iIngrCompusername)) entity.IngrCompUserName = dr.GetString(iIngrCompusername);

            int iIngrCompfecins = dr.GetOrdinal(this.IngrCompfecins);
            if (!dr.IsDBNull(iIngrCompfecins)) entity.IngrCompFecIns = dr.GetDateTime(iIngrCompfecins);

            return entity;

        }

        #region Mapeo de Campos

        public string IngrCompCodi = "INGCOMCODI";
        public string PeriCodi = "PERICODI";
        public string EmprCodi = "EMPRCODI";
        public string CompCodi = "CABCOMCODI";
        public string IngrCompVers = "INGCOMVERSION";
        public string IngrCompImporte = "INGCOMIMPORTE";
        public string IngrCompusername = "INGCOMUSERNAME";
        public string IngrCompfecins = "INGCOMFECINS";
        public string EmprNombre = "NOMBEMPRESA";
        public string PeriNombre = "NOMBPERIODO";
        public string CabeCompNombre = "NOMBCOMPENSACION";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetByCodigo
        {
            get { return base.GetSqlXml("GetByCodigo"); }

        }

        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("ListByPeriodoVersion"); }

        }

        public string SqlGetByPeriVersCabCompEmpr
        {
            get { return base.GetSqlXml("GetByPeriVersCabCompEmpr"); }

        }

        public string SqlBuscarListaEmpresas
        {
            get { return base.GetSqlXml("BuscarListaEmpresas"); }

        }

        public string SqlGetByPeriVersRentasCongestion
        {
            get { return base.GetSqlXml("GetByPeriVersRentasCongestion"); }

        }
    }
}
