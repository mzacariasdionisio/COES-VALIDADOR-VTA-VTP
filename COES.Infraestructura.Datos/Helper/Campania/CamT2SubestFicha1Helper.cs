using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT2SubestFicha1Helper : HelperBase
    {
        public CamT2SubestFicha1Helper() : base(Consultas.CamT2SubestFicha1Sql) { }

        public string SqlGetT2SubestFicha1
        {
            get { return base.GetSqlXml("GetCamT2SubestFicha1"); }
        }

        public string SqlSaveT2SubestFicha1
        {
            get { return base.GetSqlXml("SaveCamT2SubestFicha1"); }
        }

        public string SqlUpdateT2SubestFicha1
        {
            get { return base.GetSqlXml("UpdateCamT2SubestFicha1"); }
        }

        public string SqlGetLastT2SubestFicha1Id
        {
            get { return base.GetSqlXml("GetLastCamT2SubestFicha1Id"); }
        }

        public string SqlDeleteT2SubestFicha1ById
        {
            get { return base.GetSqlXml("DeleteCamT2SubestFicha1ById"); }
        }

        public string SqlGetT2SubestFicha1ById
        {
            get { return base.GetSqlXml("GetCamT2SubestFicha1ById"); }
        }
    }

}