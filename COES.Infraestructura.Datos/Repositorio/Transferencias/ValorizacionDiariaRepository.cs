using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{ 
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class ValorizacionDiariaRepository : RepositoryBase, IValorizacionDiariaRepository
    {
      
        public ValorizacionDiariaRepository(string strConn) : base(strConn)
        {
        }

        ValorizacionDiariaHelper helper = new ValorizacionDiariaHelper();

        public List<ValorizacionDiariaDTO> GetListFullByFilter(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<ValorizacionDiariaDTO> entitys = new List<ValorizacionDiariaDTO>();

            string sCommand = string.Format(helper.SqlListByFilter, emprcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorizacionDiariaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<ValorizacionDiariaDTO> GetListPageByFilter(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<ValorizacionDiariaDTO> entitys = new List<ValorizacionDiariaDTO>();

            string sCommand = string.Format(helper.SqlListPagedByFilter, emprcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha),nroPage,pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorizacionDiariaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public ValorizacionDiariaDTO GetMontoCalculadoPorMesPorParticipante(int emprcodi, DateTime fechaInicio,DateTime fechaFinal)
        {
            string sCommand = string.Format(helper.SqlListMontoCalculadoPorMes, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            ValorizacionDiariaDTO entity = new ValorizacionDiariaDTO();
            //ValorizacionDiariaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iValddemandacoincidente = dr.GetOrdinal(helper.Valddemandacoincidente);
                    if (!dr.IsDBNull(iValddemandacoincidente)) entity.Valddemandacoincidente = dr.GetDecimal(iValddemandacoincidente);

                    int iValdpfirremun = dr.GetOrdinal(helper.Valdpfirremun);
                    if (!dr.IsDBNull(iValdpfirremun)) entity.Valdpfirremun =dr.GetDecimal(iValdpfirremun);

                    int iValomr = dr.GetOrdinal(helper.Valomr);
                    if (!dr.IsDBNull(iValomr)) entity.Valomr = dr.GetDecimal(iValomr);

                    int iValopreciopotencia = dr.GetOrdinal(helper.Valopreciopotencia);
                    if (!dr.IsDBNull(iValopreciopotencia)) entity.Valopreciopotencia = dr.GetDecimal(iValopreciopotencia);

                    int iValdpeajeuni = dr.GetOrdinal(helper.Valdpeajeuni);
                    if (!dr.IsDBNull(iValdpeajeuni)) entity.Valdpeajeuni = dr.GetDecimal(iValdpeajeuni);

                    int iValofecha = dr.GetOrdinal(helper.Valofecha);
                    if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);
                }
            }

            return entity;
        }
    }
}
