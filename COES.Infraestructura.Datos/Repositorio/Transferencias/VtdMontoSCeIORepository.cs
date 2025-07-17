using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTD_valorizacio y vtd_valorizaciondetalle
    /// </summary>
    public class VtdMontoSCeIORepository: RepositoryBase, IVtdMontoSCeIORepository
    {
        public VtdMontoSCeIORepository(string strConn): base(strConn)
        {
        }

        VtdMontoSCeIOHelper helper = new VtdMontoSCeIOHelper();
        //filtro por rango de fechas -Fit
        public List<VtdMontoSCeIODTO> GetListFullByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<VtdMontoSCeIODTO> entitys = new List<VtdMontoSCeIODTO>();

            string sCommand = string.Format(helper.SqlListByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha),emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoSCeIODTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<VtdMontoSCeIODTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<VtdMontoSCeIODTO> entitys = new List<VtdMontoSCeIODTO>();

            string sCommand = string.Format(helper.SqlListPageByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize,emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoSCeIODTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
