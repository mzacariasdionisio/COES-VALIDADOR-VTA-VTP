using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT2SubestFicha1Det3Helper : HelperBase
    {
        public CamT2SubestFicha1Det3Helper() : base(Consultas.CamT2SubestFicha1Det3Sql) { }

        public string SqlGetT2SubestFicha1Det3
        {
            get { return base.GetSqlXml("GetCamT2SubestFicha1Det3"); }
        }

        public string SqlSaveT2SubestFicha1Det3
        {
            get { return base.GetSqlXml("SaveCamT2SubestFicha1Det3"); }
        }


        public string SqlGetLastT2SubestFicha1Det3Id
        {
            get { return base.GetSqlXml("GetLastCamT2SubestFicha1Det3Id"); }
        }

        public string SqlDeleteT2SubestFicha1Det3ById
        {
            get { return base.GetSqlXml("DeleteCamT2SubestFicha1Det3ById"); }
        }

        public string SqlGetT2SubestFicha1Det3ById
        {
            get { return base.GetSqlXml("GetCamT2SubestFicha1Det3ById"); }
        }
    }
}