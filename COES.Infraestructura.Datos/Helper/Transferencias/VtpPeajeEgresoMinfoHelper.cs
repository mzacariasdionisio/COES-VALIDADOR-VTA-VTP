using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_PEAJE_EGRESO_MINFO
    /// </summary>
    public class VtpPeajeEgresoMinfoHelper : HelperBase
    {
        public VtpPeajeEgresoMinfoHelper() : base(Consultas.VtpPeajeEgresoMinfoSql)
        {
        }

        public VtpPeajeEgresoMinfoDTO Create(IDataReader dr)
        {
            VtpPeajeEgresoMinfoDTO entity = new VtpPeajeEgresoMinfoDTO();

            int iPegrmicodi = dr.GetOrdinal(this.Pegrmicodi);
            if (!dr.IsDBNull(iPegrmicodi)) entity.Pegrmicodi = Convert.ToInt32(dr.GetValue(iPegrmicodi));

            int iPegrcodi = dr.GetOrdinal(this.Pegrcodi);
            if (!dr.IsDBNull(iPegrcodi)) entity.Pegrcodi = Convert.ToInt32(dr.GetValue(iPegrcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iGenemprcodi = dr.GetOrdinal(this.Genemprcodi);
            if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

            int iPegrdcodi = dr.GetOrdinal(this.Pegrdcodi);
            if (!dr.IsDBNull(iPegrdcodi)) entity.Pegrdcodi = Convert.ToInt32(dr.GetValue(iPegrdcodi));

            int iCliemprcodi = dr.GetOrdinal(this.Cliemprcodi);
            if (!dr.IsDBNull(iCliemprcodi)) entity.Cliemprcodi = Convert.ToInt32(dr.GetValue(iCliemprcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iPegrmitipousuario = dr.GetOrdinal(this.Pegrmitipousuario);
            if (!dr.IsDBNull(iPegrmitipousuario)) entity.Pegrmitipousuario = Convert.ToString(dr.GetValue(iPegrmitipousuario));

            int iPegrmilicitacion = dr.GetOrdinal(this.Pegrmilicitacion);
            if (!dr.IsDBNull(iPegrmilicitacion)) entity.Pegrmilicitacion = dr.GetString(iPegrmilicitacion);

            int iPegrmipotecalculada = dr.GetOrdinal(this.Pegrmipotecalculada);
            if (!dr.IsDBNull(iPegrmipotecalculada)) entity.Pegrmipotecalculada = dr.GetDecimal(iPegrmipotecalculada);

            int iPegrmipotedeclarada = dr.GetOrdinal(this.Pegrmipotedeclarada);
            if (!dr.IsDBNull(iPegrmipotedeclarada)) entity.Pegrmipotedeclarada = dr.GetDecimal(iPegrmipotedeclarada);

            int iPegrmicalidad = dr.GetOrdinal(this.Pegrmicalidad);
            if (!dr.IsDBNull(iPegrmicalidad)) entity.Pegrmicalidad = dr.GetString(iPegrmicalidad);

            int iPegrmipreciopote = dr.GetOrdinal(this.Pegrmipreciopote);
            if (!dr.IsDBNull(iPegrmipreciopote)) entity.Pegrmipreciopote = dr.GetDecimal(iPegrmipreciopote);

            int iPegrmipoteegreso = dr.GetOrdinal(this.Pegrmipoteegreso);
            if (!dr.IsDBNull(iPegrmipoteegreso)) entity.Pegrmipoteegreso = dr.GetDecimal(iPegrmipoteegreso);

            int iPegrmimpeajeunitario = dr.GetOrdinal(this.Pegrmipeajeunitario);
            if (!dr.IsDBNull(iPegrmimpeajeunitario)) entity.Pegrmipeajeunitario = dr.GetDecimal(iPegrmimpeajeunitario);

            int iBarrcodifco = dr.GetOrdinal(this.Barrcodifco);
            if (!dr.IsDBNull(iBarrcodifco)) entity.Barrcodifco = Convert.ToInt32(dr.GetValue(iBarrcodifco));

            int iPegrmipoteactiva = dr.GetOrdinal(this.Pegrmipoteactiva);
            if (!dr.IsDBNull(iPegrmipoteactiva)) entity.Pegrmipoteactiva = dr.GetDecimal(iPegrmipoteactiva);

            int iPegrmipotereactiva = dr.GetOrdinal(this.Pegrmipotereactiva);
            if (!dr.IsDBNull(iPegrmipotereactiva)) entity.Pegrmipotereactiva = dr.GetDecimal(iPegrmipotereactiva);

            int iPegrmiusucreacion = dr.GetOrdinal(this.Pegrmiusucreacion);
            if (!dr.IsDBNull(iPegrmiusucreacion)) entity.Pegrmiusucreacion = dr.GetString(iPegrmiusucreacion);

            int iPegrmifeccreacion = dr.GetOrdinal(this.Pegrmifeccreacion);
            if (!dr.IsDBNull(iPegrmifeccreacion)) entity.Pegrmifeccreacion = dr.GetDateTime(iPegrmifeccreacion);

            return entity;

        }

        #region Mapeo de Campos

        public string Pegrmicodi = "PEGRMICODI";
        public string Pegrcodi = "PEGRCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Genemprcodi = "GENEMPRCODI";
        public string Pegrdcodi = "PEGRDCODI";
        public string Cliemprcodi = "CLIEMPRCODI";  
        public string Barrcodi = "BARRCODI";
        public string Pegrmitipousuario = "PEGRMITIPOUSUARIO";
        public string Pegrmilicitacion = "PEGRMILICITACION";
        public string Pegrmipotecalculada = "PEGRMIPOTECALCULADA";
        public string Pegrmipotedeclarada = "PEGRMIPOTEDECLARADA";
        public string Pegrmicalidad = "PEGRMICALIDAD";
        public string Pegrmipreciopote = "PEGRMIPRECIOPOTE";
        public string Pegrmipoteegreso = "PEGRMIPOTEEGRESO";
        public string Pegrmipeajeunitario = "PEGRMIPEAJEUNITARIO";
        public string Barrcodifco = "BARRCODIFCO";
        public string Pegrmipoteactiva = "PEGRMIPOTEACTIVA";
        public string Pegrmipotereactiva = "PEGRMIPOTEREACTIVA";
        public string Pegrmiusucreacion = "PEGRMIUSUCREACION";
        public string Pegrmifeccreacion = "PEGRMIFECCREACION";
        //campos adicionales
        public string Genemprnombre = "GENEMPRNOMBRE";
        public string Cliemprnombre = "CLIEMPRNOMBRE";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrnombrefco = "BARRNOMBREFCO";

        public string Pegrmipotecoincidente = "PEGRMIPOTECOINCIDENTE";
        public string Pegrmifacperdida = "PEGRMIFACPERDIDA";
        public string Coregecodvtp = "COREGECODVTP";

        // se agregan por union con la vista
        public string Pegrdpotecoincidente = "PEGRDPOTECOINCIDENTE";
        public string Pegrdfacperdida = "PEGRDFACPERDIDA";

        //código vtp generado
        public string Emprcodi = "EMPRCODI";
        public string Clicodi = "CLICODI";
        public string Tipusucodi = "TIPUSUCODI";
        public string Tipconcodi = "TIPCONCODI";
        public string Codcncodivtp = "CODCNCODIVTP";
        public string Tipusunombre = "TIPUSUNOMBRE";
        public string Tipconnombre = "TIPCONNOMBRE";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListCabecera
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO
            get { return base.GetSqlXml("ListCabecera"); }
        }

        public string SqlListEmpresa
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO
            get { return base.GetSqlXml("ListEmpresa"); }
        }

        public string SqlGetByCriteriaVista
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO
            get { return base.GetSqlXml("GetByCriteriaVista"); }
        }

        public string SqlListPotenciaValor
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO y VTP_RETIRO_POTESC
            get { return base.GetSqlXml("ListPotenciaValor"); }
        }

        public string SqlGetByCriteriaInfoFaltante
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO y VTP_RETIRO_POTESC
            get { return base.GetSqlXml("GetByCriteriaInfoFaltante"); }
        }

        public string SqlListCabeceraRecalculo
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO
            get { return base.GetSqlXml("ListCabeceraRecalculo"); }
        }
        
        public string SqlListEmpresaRecalculo
        {
            //Consulta a la vista: VW_VTP_PEAJE_EGRESO
            get { return base.GetSqlXml("ListEmpresaRecalculo"); }
        }

        public string SqlListarCodigosVTP
        {
            //Consulta a la vista: VTP_CODIGO_CONSOLIDADO
            get { return base.GetSqlXml("ListarCodigosVTP"); }
        }

        public string SqlListarCodigosByCriteria
        {
            //Consulta a la vista: VTP_CODIGO_CONSOLIDADO
            get { return base.GetSqlXml("ListarCodigosByCriteria"); }
        }

        public string SqlGetByCriteriaVistaNuevo
        {
            //Consulta a la vista: VTP_CODIGO_CONSOLIDADO
            get { return base.GetSqlXml("GetByCriteriaVistaNuevo"); }
        }
        
    }
}

