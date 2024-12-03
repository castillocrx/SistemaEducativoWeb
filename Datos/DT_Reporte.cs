using SistemaEducativoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace SistemaEducativoWeb.Datos
{
    public class DT_Reporte
    {
        public List<ReporteCursos> RetornarCursos()
        {
            List<ReporteCursos> objLista = new List<ReporteCursos>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=FRANCISCOVICTUS\\SQLEXPRESS;Initial Catalog=AltosDelAbejonalDB; Integrated Security=True"))
            {
                string query = "SP_CantidadEstudiantesCursos";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReporteCursos()
                        {
                            Curso = dr["Curso"].ToString(),
                            Estudiantes = int.Parse(dr["Estudiantes"].ToString()),
                        });
                    }
                }

                return objLista;

            }
        }


    }

}