using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Util
{
    public class ManttoByQuery
    {
        private string _ls_query1;
        private string _ls_query2;

        private DateTime _dt_FechaInicial;
        private DateTime _dt_FechaFinal;

        public string UpdateQuery1
        {
            get { return _ls_query1; }
            set { _ls_query1 = value; }
        }

        public string UpdateQuery2
        {
            get { return _ls_query2; }
            set { _ls_query2 = value; }
        }

        public DateTime FechaHoraInicial
        {
            get { return _dt_FechaInicial; }
            set { _dt_FechaInicial = value; }
        }

        public DateTime FechaHoraFinal
        {
            get { return _dt_FechaFinal; }
            set { _dt_FechaFinal = value; }
        }
    }
}