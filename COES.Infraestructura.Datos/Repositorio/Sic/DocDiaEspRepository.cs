using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla DOC_DIA_ESP
    /// </summary>
    public class DocDiaEspRepository: RepositoryBase, IDocDiaEspRepository
    {
        public DocDiaEspRepository(string strConn): base(strConn)
        {
        }

        DocDiaEspHelper helper = new DocDiaEspHelper();

         public List<DocDiaEspDTO> List()
        {
            List<DocDiaEspDTO> entitys = new List<DocDiaEspDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DocDiaEspDTO> GetByCriteria()
        {
            List<DocDiaEspDTO> entitys = new List<DocDiaEspDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public bool EsFeriado(DateTime adt_fecha, ref List<DocDiaEspDTO> an_TablaFeriados)
        {
            bool lb_feriado;
            lb_feriado = false;
            DateTime ldt_feriado;
            string ls_frecuencia;

            foreach (var ln_dr in an_TablaFeriados)
            {
                ldt_feriado = Convert.ToDateTime(ln_dr.Diafecha);
                ls_frecuencia = ln_dr.Diafrec;

                if (ldt_feriado.Day == adt_fecha.Day && ldt_feriado.Month == adt_fecha.Month) //Es un dia Feriado
                {
                    //Preguntamos si es frecuente para todos los años
                    if (adt_fecha.Year >= ldt_feriado.Year && ls_frecuencia == "S")
                    {
                        lb_feriado = true;
                        break;
                    }
                    else if (adt_fecha.Year < ldt_feriado.Year && ls_frecuencia == "S")
                    {
                        lb_feriado = true;
                        break;
                    }
                    //Fechas donde el feriado es valido solo ese año
                    else if (adt_fecha.Year == ldt_feriado.Year && ls_frecuencia == "N")
                    {
                        lb_feriado = true;
                        break;
                    }
                    else
                    {
                        lb_feriado = false;
                    }
                }
            }
            return lb_feriado;
        }

        public int NumDiasHabiles(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            DateTime dtFechaAux = dtFechaInicio;//Convert.ToDateTime(dtFechaInicio.ToString("dd/MM/yyyy"));
            DateTime dtFechaAuxFin = dtFechaFin; //Convert.ToDateTime(dtFechaFin.ToString("dd/MM/yyyy"));
            var lsDiasFeriados = List();
            int i = 0, DiasHabiles = 0;
            while (dtFechaAux <= dtFechaAuxFin)
            {
                var bFeriado = EsFeriado(dtFechaAux, ref lsDiasFeriados);
                if (!bFeriado && dtFechaAux.DayOfWeek != DayOfWeek.Saturday && dtFechaAux.DayOfWeek != DayOfWeek.Sunday)
                {
                    DiasHabiles++;
                }
                dtFechaAux = dtFechaAux.AddDays(1);
                i++;
            }
            return DiasHabiles;
        }


        private bool VerificarDias(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            //verifica la fecha en la tabla 
            //"select diafecha,diafrec from doc_dia_esp t where diatipo = 0";
            //var query = base.TraerTodos().Where(x => x.DIATIPO == 0).ToList();
            //var q = query.SingleOrDefault(x => x.DIAFECHA.Value.ToShortDateString() == fecha.ToShortDateString());
            //return (q == null);
            var lstFeriados = this.List();
            if (EsFeriado(fecha, ref lstFeriados))
            {
                return false;
            }
            return true;


            //return false;
        }

        public DateTime ObtenerFechaDiasHabiles(DateTime fInicio, int Dias)
        {
            //NOTA : el conteo comienza a parte de la siguiente fecha de inicio 

            DateTime fCompare = fInicio.AddDays(1);
            bool bDiasH = false;
            int Conteo = 1;


            while (Conteo <= Dias)
            {
                //fCompare = fInicio.AddDays(cDias);

                bDiasH = this.VerificarDias(fCompare);
                if (bDiasH)
                    Conteo++;
                fCompare = fCompare.AddDays(1);
            }

            return fCompare.AddDays(-1);
        }
    }
}
