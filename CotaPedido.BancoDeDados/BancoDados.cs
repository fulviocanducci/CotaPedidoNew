using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CotaPedido.BancoDeDados
{
    public class BancoDados
    {
        private string Conexao = "";//ConfigurationManager.ConnectionStrings["SqlServerConnection"].ToString();

        public int Cadastrar(string Comando, Dictionary<string, object> Parametros)
        {
            int iRetorno = -1;

            using (var Connection = new SqlConnection(Conexao))
            {
                using (var Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = Comando;

                    if (Parametros != null)
                    {
                        foreach (var parametro in Parametros)
                        {
                            Command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                        }
                    }

                    Connection.Open();
                    iRetorno = Command.ExecuteNonQuery();
                }
            }

            return iRetorno;
        }

        public int Alterar(string Comando, Dictionary<string, object> Parametros)
        {
            int iRetorno = -1;

            using (var Connection = new SqlConnection(Conexao))
            {
                using (var Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = Comando;

                    if (Parametros != null)
                    {
                        foreach (var parametro in Parametros)
                        {
                            Command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                        }
                    }

                    Connection.Open();
                    iRetorno = Command.ExecuteNonQuery();
                }
            }

            return iRetorno;
        }

        public int Excluir(string Comando, Dictionary<string, object> Parametros)
        {
            int iRetorno = -1;

            using (var Connection = new SqlConnection(Conexao))
            {
                using (var Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = Comando;

                    if (Parametros != null)
                    {
                        foreach (var parametro in Parametros)
                        {
                            Command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                        }
                    }

                    Connection.Open();
                    iRetorno = Command.ExecuteNonQuery();
                }
            }

            return iRetorno;
        }

        public DataTable Selecionar(string Comando, Dictionary<string, object> Parametros)
        {
            DataTableReader dataTableReader = null;
            DataTable dtRetorno = new DataTable();

            using (var Connection = new SqlConnection(Conexao))
            {
                using (var Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = Comando;

                    if (Parametros != null)
                    {
                        foreach (var parametro in Parametros)
                        {
                            Command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                        }
                    }

                    using (var sqlDataAdapter = new SqlDataAdapter(Command))
                    {
                        using (var dataTable = new DataTable())
                        {
                            sqlDataAdapter.Fill(dataTable);

                            dataTableReader = dataTable.CreateDataReader();
                            dtRetorno.Load(dataTableReader);
                        }
                    }
                }
            }

            return dtRetorno;
        }

        public int Escalar(string Comando, Dictionary<string, object> Parametros)
        {
            int iRetorno = -1;

            using (var Connection = new SqlConnection(Conexao))
            {
                using (var Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = Comando;

                    if (Parametros != null)
                    {
                        foreach (var parametro in Parametros)
                        {
                            Command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                        }
                    }

                    Connection.Open();
                    iRetorno = (int)Command.ExecuteScalar();
                }
            }

            return iRetorno;
        }
    }
}
