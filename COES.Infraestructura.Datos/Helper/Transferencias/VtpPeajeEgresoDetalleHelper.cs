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
    public class VtpPeajeEgresoDetalleHelper : HelperBase
    {
        public VtpPeajeEgresoDetalleHelper(): base(Consultas.VtpPeajeEgresoDetalleSql)
        {
        }

        public VtpPeajeEgresoDetalleDTO Create(IDataReader dr)
        {
            VtpPeajeEgresoDetalleDTO entity = new VtpPeajeEgresoDetalleDTO();

            int iPegrdcodi = dr.GetOrdinal(this.Pegrdcodi);
            if (!dr.IsDBNull(iPegrdcodi)) entity.Pegrdcodi = Convert.ToInt32(dr.GetValue(iPegrdcodi));

            int iPegrcodi = dr.GetOrdinal(this.Pegrcodi);
            if (!dr.IsDBNull(iPegrcodi)) entity.Pegrcodi = Convert.ToInt32(dr.GetValue(iPegrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iPegrdtipousuario = dr.GetOrdinal(this.Pegrdtipousuario);
            if (!dr.IsDBNull(iPegrdtipousuario)) entity.Pegrdtipousuario = Convert.ToString(dr.GetValue(iPegrdtipousuario));

            int iPegrdlicitacion = dr.GetOrdinal(this.Pegrdlicitacion);
            if (!dr.IsDBNull(iPegrdlicitacion)) entity.Pegrdlicitacion = dr.GetString(iPegrdlicitacion);

            int iPegrdpotecalculada = dr.GetOrdinal(this.Pegrdpotecalculada);
            if (!dr.IsDBNull(iPegrdpotecalculada)) entity.Pegrdpotecalculada = dr.GetDecimal(iPegrdpotecalculada);

            int iPegrdpotedeclarada = dr.GetOrdinal(this.Pegrdpotedeclarada);
            if (!dr.IsDBNull(iPegrdpotedeclarada)) entity.Pegrdpotedeclarada = dr.GetDecimal(iPegrdpotedeclarada);

            int iPegrdcalidad = dr.GetOrdinal(this.Pegrdcalidad);
            if (!dr.IsDBNull(iPegrdcalidad)) entity.Pegrdcalidad = dr.GetString(iPegrdcalidad);

            int iPegrdpreciopote = dr.GetOrdinal(this.Pegrdpreciopote);
            if (!dr.IsDBNull(iPegrdpreciopote)) entity.Pegrdpreciopote = dr.GetDecimal(iPegrdpreciopote);

            int iPegrdpoteegreso = dr.GetOrdinal(this.Pegrdpoteegreso);
            if (!dr.IsDBNull(iPegrdpoteegreso)) entity.Pegrdpoteegreso = dr.GetDecimal(iPegrdpoteegreso);

            int iPegrdpeajeunitario = dr.GetOrdinal(this.Pegrdpeajeunitario);
            if (!dr.IsDBNull(iPegrdpeajeunitario)) entity.Pegrdpeajeunitario = dr.GetDecimal(iPegrdpeajeunitario);

            int iBarrcodifco = dr.GetOrdinal(this.Barrcodifco);
            if (!dr.IsDBNull(iBarrcodifco)) entity.Barrcodifco = Convert.ToInt32(dr.GetValue(iBarrcodifco));

            int iPegrdpoteactiva = dr.GetOrdinal(this.Pegrdpoteactiva);
            if (!dr.IsDBNull(iPegrdpoteactiva)) entity.Pegrdpoteactiva = dr.GetDecimal(iPegrdpoteactiva);

            int iPegrdpotereactiva = dr.GetOrdinal(this.Pegrdpotereactiva);
            if (!dr.IsDBNull(iPegrdpotereactiva)) entity.Pegrdpotereactiva = dr.GetDecimal(iPegrdpotereactiva);

            int iPegrdusucreacion = dr.GetOrdinal(this.Pegrdusucreacion);
            if (!dr.IsDBNull(iPegrdusucreacion)) entity.Pegrdusucreacion = dr.GetString(iPegrdusucreacion);

            int iPegrdfeccreacion = dr.GetOrdinal(this.Pegrdfeccreacion);
            if (!dr.IsDBNull(iPegrdfeccreacion)) entity.Pegrdfeccreacion = dr.GetDateTime(iPegrdfeccreacion);

            int iCoregecodvtp = dr.GetOrdinal(this.CoregeCodVtp);
            if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = dr.GetString(iCoregecodvtp);

            int iPegrdpotecoincidente = dr.GetOrdinal(this.PegrdPoteCoincidente);
            if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = dr.GetDecimal(iPegrdpotecoincidente);

            int iPegrdfacperdida = dr.GetOrdinal(this.PegrdFacPerdida);
            if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = dr.GetDecimal(iPegrdfacperdida);

            return entity;
        }

        #region Mapeo de Campos

        public string Pegrdcodi = "PEGRDCODI";
        public string Pegrcodi = "PEGRCODI"; 
        public string Emprcodi = "EMPRCODI";    
        public string Barrcodi = "BARRCODI";
        public string Pegrdtipousuario = "PEGRDTIPOUSUARIO";
        public string Pegrdlicitacion = "PEGRDLICITACION";
        public string Pegrdpotecalculada = "PEGRDPOTECALCULADA";
        public string Pegrdpotedeclarada = "PEGRDPOTEDECLARADA";
        public string Pegrdcalidad = "PEGRDCALIDAD";
        public string Pegrdpreciopote = "PEGRDPRECIOPOTE";
        public string Pegrdpoteegreso = "PEGRDPOTEEGRESO";
        public string Pegrdpeajeunitario = "PEGRDPEAJEUNITARIO";
        public string Barrcodifco = "BARRCODIFCO";
        public string Pegrdpoteactiva = "PEGRDPOTEACTIVA";
        public string Pegrdpotereactiva = "PEGRDPOTEREACTIVA";
        public string Pegrdusucreacion = "PEGRDUSUCREACION";
        public string Pegrdfeccreacion = "PEGRDFECCREACION";
        public string Emprnomb = "EMPRNOMB";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrnombrefco = "BARRNOMBREFCO";

        //campos adicionales

        public string PeriCodi = "PERICODI";
        public string RecPotCodi = "RECPOTCODI";
        public string CoregeCodVtp = "COREGECODVTP";
        public string PegrdPoteCoincidente = "PEGRDPOTECOINCIDENTE";
        public string PegrdFacPerdida = "PEGRDFACPERDIDA";
        public string CoregeCodi = "COREGECODI";
        public string CodCnCodiVtp = "CODCNCODIVTP";
        public string TipConCondi = "TIPCONCONDI";
        public string TipConNombre = "TIPCONNOMBRE";
        #endregion

        public string SqlListView
        {
            get { return base.GetSqlXml("ListView"); }
        }

        public string SqlGetByIdMinfo
        {
            get { return base.GetSqlXml("GetByIdMinfo"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListarCodigosByEmprcodi
        {
            get { return base.GetSqlXml("ListarCodigosByEmprcodi"); }
		}
		
        public string SqlGetByPegrCodiAndCodVtp
        {
            get { return base.GetSqlXml("GetByPegrCodiAndCodVtp"); }

        }
    }
}
