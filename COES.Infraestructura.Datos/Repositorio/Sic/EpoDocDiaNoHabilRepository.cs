using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{/// <summary>
 /// Clase de acceso a datos de la tabla EPO_DOCDIAHABIL
 /// </summary>
    public class EpoDocDiaNoHabilRepository : RepositoryBase, IEpoDocDiaNoHabilRepository
    {
        public EpoDocDiaNoHabilRepository(string strConn) : base(strConn)
        {
        }

        EpoDocDiaNoHabilHelper helper = new EpoDocDiaNoHabilHelper();


        public DateTime ObtenerDiasNoHabiles(DateTime fInicio, int Dias)
        {
            //NOTA : el conteo comienza a parte de la siguiente fecha de inicio 

            DateTime fCompare = fInicio.AddDays(1);
            bool bDiasH = false;
            int Conteo = 1;


            while (Conteo <= Dias)
            {
                //fCompare = fInicio.AddDays(cDias);

                bDiasH = this.VerificarDiasNoHabiles(fCompare);
                if (!bDiasH)
                    Conteo++;
                fCompare = fCompare.AddDays(1);
            }

            return fCompare.AddDays(-1);
        }

        private bool VerificarDiasNoHabiles(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            //verifica la fecha en la tabla 
            //"select diafecha,diafrec from doc_dia_esp t where diatipo = 0";
            //var query = base.TraerTodos().Where(x => x.DIATIPO == 0).ToList();
            //var q = query.SingleOrDefault(x => x.DIAFECHA.Value.ToShortDateString() == fecha.ToShortDateString());
            //return (q == null);
            var lstFeriados = this.List();
            if (EsNoHabil(fecha, ref lstFeriados))
            {
                return true;
            }
            return false;


            //return false;
        }
        private bool VerificarDiasNoHabSinFS(DateTime fecha)
        {
            //verifica la fecha en la tabla 
            //"select diafecha,diafrec from doc_dia_esp t where diatipo = 0";
            //var query = base.TraerTodos().Where(x => x.DIATIPO == 0).ToList();
            //var q = query.SingleOrDefault(x => x.DIAFECHA.Value.ToShortDateString() == fecha.ToShortDateString());
            //return (q == null);
            var lstFeriados = this.List();
            if (EsNoHabil(fecha, ref lstFeriados))
            {
                return true;
            }
            return false;


            //return false;
        }

        private bool EsNoHabil(DateTime adt_fecha, ref List<EpoDocDiaNoHabilDTO> an_TablaDiaNoHabil)
        {
            bool lb_noHabil = false;
            DateTime ldt_fechaIni, ldt_fechaFin;

            foreach (var ln_dr in an_TablaDiaNoHabil)
            {
                ldt_fechaIni = Convert.ToDateTime(ln_dr.DIANHFECHAINI);
                ldt_fechaFin = Convert.ToDateTime(ln_dr.DIANHFECHAFIN);

                if (adt_fecha >= ldt_fechaIni && adt_fecha <= ldt_fechaFin)
                {
                    lb_noHabil = true;
                    break;
                }


            }

            return lb_noHabil;
        }


        public List<EpoDocDiaNoHabilDTO> List()
        {
            List<EpoDocDiaNoHabilDTO> entitys = new List<EpoDocDiaNoHabilDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerDiasNoHabiles);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public DateTime ObtenerDiasNoHabSinFS(DateTime fInicio, int Dias)
        {
            //NOTA : el conteo comienza a parte de la siguiente fecha de inicio 

            DateTime fCompare = fInicio.AddDays(1);
            bool bDiasH = false;
            int Conteo = 1;


            while (Conteo <= Dias)
            {
                //fCompare = fInicio.AddDays(cDias);

                bDiasH = this.VerificarDiasNoHabSinFS(fCompare);
                if (!bDiasH)
                    Conteo++;
                fCompare = fCompare.AddDays(1);
            }

            return fCompare.AddDays(-1);
        }
    }
}
