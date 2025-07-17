using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamBioHojaAHelper : HelperBase
    {
        public CamBioHojaAHelper() : base(Consultas.CamBioHojaASql) { }


        public string SqlGetBioHojaAProyCodi
        {
            get { return base.GetSqlXml("GetBioHojaAProyCodi"); }
        }

        public string SqlSaveBioHojaA
        {
            get { return base.GetSqlXml("SaveBioHojaA"); }
        }

        public string SqlUpdateBioHojaA
        {
            get { return base.GetSqlXml("UpdateBioHojaA"); }
        }

        public string SqlGetLastBioHojaAId
        {
            get { return base.GetSqlXml("GetLastBioHojaAId"); }
        }

        public string SqlDeleteBioHojaAById
        {
            get { return base.GetSqlXml("DeleteBioHojaAById"); }
        }

        public string SqlGetBioHojaAById
        {
            get { return base.GetSqlXml("GetBioHojaAById"); }
        }
    }
}
