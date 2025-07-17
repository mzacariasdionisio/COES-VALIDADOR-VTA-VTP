using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_INGRESO_TARIFARIO
    /// </summary>
    public class VtpIngresoTarifarioHelper : HelperBase
    {
        public VtpIngresoTarifarioHelper()
            : base(Consultas.VtpIngresoTarifarioSql)
        {
        }

        public VtpIngresoTarifarioDTO Create(IDataReader dr)
        {
            VtpIngresoTarifarioDTO entity = new VtpIngresoTarifarioDTO();

            int iIngtarcodi = dr.GetOrdinal(this.Ingtarcodi);
            if (!dr.IsDBNull(iIngtarcodi)) entity.Ingtarcodi = Convert.ToInt32(dr.GetValue(iIngtarcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iEmprcodiping = dr.GetOrdinal(this.Emprcodiping);
            if (!dr.IsDBNull(iEmprcodiping)) entity.Emprcodiping = Convert.ToInt32(dr.GetValue(iEmprcodiping));

            int iIngtartarimensual = dr.GetOrdinal(this.Ingtartarimensual);
            if (!dr.IsDBNull(iIngtartarimensual)) entity.Ingtartarimensual = dr.GetDecimal(iIngtartarimensual);

            int iEmprcodingpot = dr.GetOrdinal(this.Emprcodingpot);
            if (!dr.IsDBNull(iEmprcodingpot)) entity.Emprcodingpot = Convert.ToInt32(dr.GetValue(iEmprcodingpot));

            int iIngtarporcentaje = dr.GetOrdinal(this.Ingtarporcentaje);
            if (!dr.IsDBNull(iIngtarporcentaje)) entity.Ingtarporcentaje = dr.GetDecimal(iIngtarporcentaje);

            int iIngtarimporte = dr.GetOrdinal(this.Ingtarimporte);
            if (!dr.IsDBNull(iIngtarimporte)) entity.Ingtarimporte = dr.GetDecimal(iIngtarimporte);

            int iIngtarsaldoanterior = dr.GetOrdinal(this.Ingtarsaldoanterior);
            if (!dr.IsDBNull(iIngtarsaldoanterior)) entity.Ingtarsaldo = dr.GetDecimal(iIngtarsaldoanterior);

            int iIngtarajuste = dr.GetOrdinal(this.Ingtarajuste);
            if (!dr.IsDBNull(iIngtarajuste)) entity.Ingtarajuste = dr.GetDecimal(iIngtarajuste);

            int iIngtarsaldo = dr.GetOrdinal(this.Ingtarsaldo);
            if (!dr.IsDBNull(iIngtarsaldo)) entity.Ingtarsaldo = dr.GetDecimal(iIngtarsaldo);

            int iIngtarpericodidest = dr.GetOrdinal(this.Ingtarpericodidest);
            if (!dr.IsDBNull(iIngtarpericodidest)) entity.Ingtarpericodidest = Convert.ToInt32(dr.GetValue(iIngtarpericodidest));

            int iIngtarusucreacion = dr.GetOrdinal(this.Ingtarusucreacion);
            if (!dr.IsDBNull(iIngtarusucreacion)) entity.Ingtarusucreacion = dr.GetString(iIngtarusucreacion);

            int iIngtarfeccreacion = dr.GetOrdinal(this.Ingtarfeccreacion);
            if (!dr.IsDBNull(iIngtarfeccreacion)) entity.Ingtarfeccreacion = dr.GetDateTime(iIngtarfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ingtarcodi = "INGTARCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Pingcodi = "PINGCODI";
        public string Emprcodiping = "EMPRCODIPING";
        public string Ingtartarimensual = "INGTARTARIMENSUAL";
        public string Emprcodingpot = "EMPRCODINGPOT";
        public string Ingtarporcentaje = "INGTARPORCENTAJE";
        public string Ingtarimporte = "INGTARIMPORTE";
        public string Ingtarsaldoanterior = "INGTARSALDOANTERIOR";
        public string Ingtarajuste = "INGTARAJUSTE";
        public string Ingtarsaldo = "INGTARSALDO";
        public string Ingtarpericodidest = "INGTARPERICODIDEST";
        public string Ingtarusucreacion = "INGTARUSUCREACION";
        public string Ingtarfeccreacion = "INGTARFECCREACION";
        //ATRIBUTOS ADICIONALES EMPLEADOS EN EL RESULTADO DE LAS CONSULTAS
        public string Emprnombingpot = "EMPRNOMBINGPOT";
        public string Emprnombping = "EMPRNOMBPING";
        public string Emprruc = "EMPRRUC";
        #region SIOSEIN
        public string Emprcodosinergminingpot = "EMPRCODOSINERGMININGPOT";
        public string Emprcodosinergminping = "EMPRCODOSINERGMINPING";
        public string Pingtipo = "PINGTIPO";
        #endregion
        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListEmpresaPago
        {
            get { return base.GetSqlXml("ListEmpresaPago"); }
        }

        public string SqlListEmpresaCobro
        {
            get { return base.GetSqlXml("ListEmpresaCobro"); }
        }

        public string SqlGetByIdSaldo
        {
            get { return base.GetSqlXml("GetByIdSaldo"); }
        }

        public string SqlUpdatePeriodoDestino
        {
            get { return base.GetSqlXml("UpdatePeriodoDestino"); }
        }

        public string SqlGetSaldoAnterior
        {
            get { return base.GetSqlXml("GetSaldoAnterior"); }
        }

        public string SqlGetByCriteriaIngresoTarifarioSaldo
        {
            get { return base.GetSqlXml("GetByCriteriaIngresoTarifarioSaldo"); }
        }

        #region SIOSEIN2

        public string SqlListEmpresaCobroParaMultEmprcodingpot
        {
            get { return base.GetSqlXml("ListEmpresaCobroParaMultEmprcodingpot"); }
        }

        #endregion

    }
}
