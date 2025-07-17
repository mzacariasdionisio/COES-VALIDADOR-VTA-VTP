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
    public class VtpCodigoConsolidadoHelper: HelperBase
    {
        public VtpCodigoConsolidadoHelper() : base(Consultas.VtpCodigoConsolidadoSql)
        {
        }

        public VtpCodigoConsolidadoDTO Create(IDataReader dr)
        {
            VtpCodigoConsolidadoDTO entity = new VtpCodigoConsolidadoDTO();

            int iCodcncodi = dr.GetOrdinal(this.CodCnCodi);
            if (!dr.IsDBNull(iCodcncodi)) entity.Codcncodi = Convert.ToInt32(dr.GetValue(iCodcncodi));

            int iEmprcodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmpresa = dr.GetOrdinal(this.Empresa);
            if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

            int iClicodi = dr.GetOrdinal(this.CliCodi);
            if (!dr.IsDBNull(iClicodi)) entity.Clicodi = Convert.ToInt32(dr.GetValue(iClicodi));

            int iCliente = dr.GetOrdinal(this.Cliente);
            if (!dr.IsDBNull(iCliente)) entity.Cliente = Convert.ToString(dr.GetValue(iCliente));

            int iBarrcodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iBarra = dr.GetOrdinal(this.Barra);
            if (!dr.IsDBNull(iBarra)) entity.Barra = Convert.ToString(dr.GetValue(iBarra));

            int iCodcncodivtp = dr.GetOrdinal(this.CodCnCodiVtp);
            if (!dr.IsDBNull(iCodcncodivtp)) entity.Codcncodivtp = Convert.ToString(dr.GetValue(iCodcncodivtp));

            int iCodcnpotegre = dr.GetOrdinal(this.CodCnPotEgre);
            if (!dr.IsDBNull(iCodcnpotegre)) entity.Codcnpotegre = Convert.ToInt32(dr.GetValue(iCodcnpotegre));

            int iTipUsuNombre = dr.GetOrdinal(this.TipUsuNombre);
            if (!dr.IsDBNull(iTipUsuNombre)) entity.TipUsuNombre = Convert.ToString(dr.GetValue(iTipUsuNombre));

            int iTipConNombre = dr.GetOrdinal(this.TipConNombre);
            if (!dr.IsDBNull(iTipConNombre)) entity.TipConNombre = Convert.ToString(dr.GetValue(iTipConNombre));

            return entity;
        }

        #region Mapeo de Campos

        public string CodCnCodi = "CODCNCODI";
        public string EmprCodi = "EMPRCODI";
        public string Empresa = "EMPRESA";
        public string CliCodi = "CLICODI";
        public string Cliente = "CLIENTE";
        public string BarrCodi = "BARRCODI";
        public string Barra = "BARRA";
        public string CodCnCodiVtp = "CODCNCODIVTP";
        public string CodCnPotEgre = "CODCNPOTEGRE";
        public string TipUsuNombre = "TIPUSUNOMBRE";
        public string TipConNombre = "TIPCONNOMBRE";

        #endregion

        public string SqlGetByCodigoVTP
        {
            get { return base.GetSqlXml("GetByCodigoVTP"); }
        }
    }
}
