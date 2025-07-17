using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class IndTransporteDetalleHelper : HelperBase
    {
        public IndTransporteDetalleHelper() : base(Consultas.IndTransporteDetalleSql)
        {
        }

        #region Mapeo de Campos
        public string Tnsdetcodi = "TNSDETCODI";
        public string Cpctnscodi = "CPCTNSCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprnombalter = "EMPRNOMBALTER";
        public string Tnsdetfecha = "TNSDETFECHA";
        public string Tnsdetcntadquirida = "TNSDETCNTADQUIRIDA";
        public string Tnsdetprctransferencia = "TNSDETPRCTRANSFERENCIA";
        public string Tnsdetptosuministro = "TNSDETPTOSUMINISTRO";
        public string Tnsdetcompraventa = "TNSDETCOMPRAVENTA";
        public string Tnsdetusucreacion = "TNSDETUSUCREACION";
        public string Tnsdetfeccreacion = "TNSDETFECCREACION";
        public string Tnsdetusumodificacion = "TNSDETUSUMODIFICACION";
        public string Tnsdetfecmodificacion = "TNSDETFECMODIFICACION";

        //Adicionales
        public string Emprnomb = "EMPRNOMB";
        public string Tnsdetdescripcion = "TNSDETDESCRIPCION";
        //reporte cumplimiento
        public string Fechacumplimiento = "FECHACUMPLIMIENTO";
        public string Empresacompra = "EMPRESACOMPRA";
        public string Cantidadcompra = "CANTIDADCOMPRA";
        public string Empresaventa = "EMPRESAVENTA";
        public string Cantidadventa = "CANTIDADVENTA";
        #endregion

        #region Consultas
        public string SqlListTransporteDetalle
        {
            get { return base.GetSqlXml("ListTransporteDetalle"); }
        }
        public string SqlListTransDetalleJoinCapacidadTrans
        {
            get { return base.GetSqlXml("ListTransDetalleJoinCapacidadTrans"); }
        }
        public string SqlDeleteByCapacidadTransporte
        {
            get { return base.GetSqlXml("DeleteByCapacidadTransporte"); }
        }
        public string SqlReporteIncumplimientoByPeriodo
        {
            get { return base.GetSqlXml("ReporteIncumplimientoByPeriodo"); }
        }
        #endregion
    }
}
