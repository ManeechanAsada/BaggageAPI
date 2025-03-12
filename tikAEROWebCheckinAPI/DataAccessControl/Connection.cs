using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataAccessControl
{
    public class Connection
    {
        private SqlDataAdapter sqlAdapter;
        private SqlConnection connection;

        public Connection(string str)
        {
           // string[] key64 = RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, RegistryExtensions.RegistryHiveType.X64).OpenSubKey("SOFTWARE").GetSubKeyNames();
           // string[] key32 = RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, RegistryExtensions.RegistryHiveType.X86).OpenSubKey("SOFTWARE").GetSubKeyNames();
           // var strConn = RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, RegistryExtensions.RegistryHiveType.X64).OpenSubKey("SOFTWARE\\tikTRAVELOFFICE\\Database\\Connections").GetValue("aero");

           // str = (string)strConn;
           // str = str.Replace("Provider=SQLOLEDB.1;", "");
            sqlAdapter = new SqlDataAdapter();
            connection = new SqlConnection(str);
        }

        private SqlConnection openConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State ==
                        ConnectionState.Broken)
            {
                connection.Open();
            }
            return connection;
        }

        public DataTable executeSelectQuery(String query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = query;
                myCommand.Parameters.AddRange(sqlParameter);
                myCommand.ExecuteNonQuery();                
                sqlAdapter.SelectCommand = myCommand;
                sqlAdapter.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (SqlException e)
            {
            }
            finally
            {

            }
            return dataTable;
        }

        public bool IsFlightFound(SqlParameter[] sqlParameter)
        {
            SqlDataReader reader = null;
            SqlCommand sqlCommand = new SqlCommand();
            const string query = @"SELECT fn.flight_id FROM dbo.flight_point fp WITH(NOLOCK) inner join flight_new fn WITH(NOLOCK) on fp.flight_id = fn.flight_id  WHERE fn.flight_id = @flight_id and origin_rcd = @origin and destination_rcd = @destination and flight_number = @flightnumber and airline_rcd = @airlinecode and fp.departure_date = @departuredate";

            bool result = false;

            try
            {
                sqlCommand.Connection = openConnection();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.CommandType = CommandType.Text;

                reader = sqlCommand.ExecuteReader();

                int counter = 0;
                while (reader.Read())
                {
                    counter++;

                }

                if (counter >= 1)
                    result = true;
                else
                    result = false;
            }
            finally
            {
                reader.Close();

                if (connection != null)
                    connection.Dispose();
            }

            return result;
        }

        public bool IsFlightFound2(SqlParameter[] sqlParameter)
        {
            
            SqlDataReader reader = null;
            SqlCommand sqlCommand = new SqlCommand();
            const string query = @"SELECT TOP 1 flight_id FROM dbo.flight_segment WITH(NOLOCK)  WHERE flight_id = @flight_id AND origin_rcd = @origin AND destination_rcd = @destination AND flight_number = @flightnumber AND fp.departure_date = @departuredate";

            bool result = false;

            try
            {

                sqlCommand.Connection = openConnection();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.CommandType = CommandType.Text;

                reader = sqlCommand.ExecuteReader();

                int counter = 0;
                while (reader.Read())
                {
                    counter++;

                }

                if (counter >= 1)
                    result = true;
                else
                    result = false;
            }
            finally
            {
                reader.Close();

                if (connection != null)
                    connection.Dispose();
            }

            return result;
        }

        public bool IsFlightFoundById(SqlParameter[] sqlParameter)
        {

            SqlDataReader reader = null;
            SqlCommand sqlCommand = new SqlCommand();
            const string query = @"SELECT flight_id FROM dbo.flight_new WITH(NOLOCK) WHERE flight_id = @flight_id";

            bool result = false;

            try
            {
                sqlCommand.Connection = openConnection();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.CommandType = CommandType.Text;

                reader = sqlCommand.ExecuteReader();

                int counter = 0;
                while (reader.Read())
                {
                    counter++;

                }

                if (counter >= 1)
                    result = true;
                else
                    result = false;
            }
            finally
            {
                reader.Close();

                if (connection != null)
                    connection.Dispose();
            }

            return result;
        }

        public DataSet GetBaggage(Guid? baggage_id)
        {

            SqlCommand sqlCommand = new SqlCommand();
            DataSet ds = new DataSet();
            string query = @"exec get_baggage_tag '" + baggage_id.ToString() + "'";


            try
            {
                sqlCommand.Connection = openConnection();
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = CommandType.Text;

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                {
                    adapter.Fill(ds);
                }
                //SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                //adapter.Fill(ds);

            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            return ds;
        }

        public void UpdateXXX(Int32 ID,string value, string connectionString)
        {

            string commandText = "UPDATE tbABC SET some_col = @value "
                + "WHERE  ID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@ID"].Value = ID;

                command.Parameters.AddWithValue("@value", value);

                try
                {
                    connection.Open();
                    Int32 rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("RowsAffected: {0}", rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public bool executeInsertQuery2(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;
                myCommand.Parameters.AddRange(sqlParameter);
                sqlAdapter.InsertCommand = myCommand;
                myCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
            }
            finally
            {
            }
            return true;
        }

        public bool executeQuery(String query,SqlParameter[] sqlParameter)
        {
            SqlCommand sqlCommand = new SqlCommand();
            int resultCount = 0;
            try
            {
                sqlCommand.Connection = openConnection();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter ResultCountParam = new SqlParameter("@ResultCount", System.Data.SqlDbType.Int);
                ResultCountParam.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(ResultCountParam);

                sqlCommand.ExecuteNonQuery();

                resultCount = System.Convert.ToInt32(ResultCountParam.Value.ToString());
            }
            catch (SqlException e)
            {
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            if (resultCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ExecuteQueryReturn(String query, SqlParameter[] sqlParameter)
        {
            SqlCommand sqlCommand = new SqlCommand();
            string result= string.Empty;

            try
            {
                sqlCommand.Connection = openConnection();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter result_code = new SqlParameter("@result_code", System.Data.SqlDbType.VarChar,5);
                result_code.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(result_code);

                SqlParameter result_message = new SqlParameter("@result_message", System.Data.SqlDbType.VarChar, 250);
                result_message.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(result_message);


                sqlCommand.ExecuteNonQuery();

                result= result_code.Value.ToString() + "," + result_message.Value.ToString();
            }
            catch (SqlException e)
            {
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            return result;
        }
                




        public bool executeUpdateQuery(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;
                myCommand.Parameters.AddRange(sqlParameter);
                sqlAdapter.UpdateCommand = myCommand;
                myCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
            }
            finally
            {
            }
            return true;
        }

    }
    
}
