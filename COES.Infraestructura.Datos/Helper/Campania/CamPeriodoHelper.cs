using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamPeriodoHelper : HelperBase
    {
        public CamPeriodoHelper() : base(Consultas.CamPeriodoSql) { }

        public string SqlGetListaPeriodos
        {
            get { return base.GetSqlXml("GetListaPeriodos"); }
        }

        public string SqlGetListaPeriodosByAnioAndEstado
        {
            get { return base.GetSqlXml("GetListaPeriodosByAnioAndEstado"); }
        }

        public string SqlGetListaPeriodosByAnio
        {
            get { return base.GetSqlXml("GetListaPeriodosByAnio"); }
        }

        public string SQLSave
        {
            get { return base.GetSqlXml("Save"); }
        }

        public string SqlGetPeriodoId
        {
            get { return base.GetSqlXml("GetLastPeriodoID"); }
        }

        public string SqlDeletePeriodoById
        {
            get { return base.GetSqlXml("DeletePeriodoById"); }
        }

        public string SqlGetPeriodoById
        {
            get { return base.GetSqlXml("GetPeriodoById"); }
        }

        public string SqlUpdatePeriodo
        {
            get { return base.GetSqlXml("UpdatePeriodo"); }
        }

        public string SqlGetPeriodoByDate
        {
            get { return base.GetSqlXml("GetPeriodoByDate"); }
        }
    }
}
