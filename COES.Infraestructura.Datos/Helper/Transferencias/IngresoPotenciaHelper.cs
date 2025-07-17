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
    /// Clase que contiene el mapeo de la tabla trn_ing_potencia
    /// </summary>
    public class IngresoPotenciaHelper : HelperBase
    {
        public IngresoPotenciaHelper() : base(Consultas.IngresoPotenciaSql)
        {
        }
        public IngresoPotenciaDTO Create(IDataReader dr)
        {
            IngresoPotenciaDTO entity = new IngresoPotenciaDTO();

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            int iIngrPoteCodi = dr.GetOrdinal(this.IngrPoteCodi);
            if (!dr.IsDBNull(iIngrPoteCodi)) entity.IngrPoteCodi = dr.GetInt32(iIngrPoteCodi);

            int iIngrPoteVers = dr.GetOrdinal(this.IngrPoteVers);
            if (!dr.IsDBNull(iIngrPoteVers)) entity.IngrPoteVersion = dr.GetInt32(iIngrPoteVers);

            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);
         
            int iIngrPoteImporte = dr.GetOrdinal(this.IngrPoteImporte);
            if (!dr.IsDBNull(iIngrPoteImporte)) entity.IngrPoteImporte = dr.GetDecimal(iIngrPoteImporte);

            int iIngrPoteUsername = dr.GetOrdinal(this.IngrPoteUsername);
            if (!dr.IsDBNull(iIngrPoteUsername)) entity.IngrPoteUserName = dr.GetString(iIngrPoteUsername);

            int iIngrFecins = dr.GetOrdinal(this.IngrPoteFecins);
            if (!dr.IsDBNull(iIngrFecins)) entity.IngrPoteFecIns = dr.GetDateTime(iIngrFecins);

            int iIngrPoteFecact = dr.GetOrdinal(this.IngrPoteFecact);
            if (!dr.IsDBNull(iIngrPoteFecact)) entity.IngrPoteFecAct = dr.GetDateTime(iIngrPoteFecact);

            return entity;
        }

        #region Mapeo de Campos

        public string IngrPoteCodi = "INGPOTCODI";
        public string PeriCodi = "PERICODI";
        public string EmprCodi = "EMPRCODI";
        public string IngrPoteVers = "INGPOTVERSION";
        public string IngrPoteImporte = "INGPOTIMPORTE";
        public string IngrPoteUsername = "INGPOTUSERNAME";
        public string IngrPoteFecins = "INGPOTFECINS";
        public string IngrPoteFecact = "INGPOTFECACT";
        public string EmprNombre = "NOMBEMPRESA";
        public string PeriNombre = "NOMBPERIODO";
        public string Total = "TOTAL";      

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

        public string SqlGetByPeriodoVersionEmpresa
        {
            get { return base.GetSqlXml("GetByPeriodoVersionEmpresa"); }
        }
    }

}
