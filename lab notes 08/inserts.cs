 class InsertPostgresBenchmark
    {
        public static void insertTest(int insertsCount) {

            String connectionString = "Server=192.168.99.100;Port=5432;Database=oz;User Id=postgres;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            connection.Open();

            for(int i = 0; i < insertsCount; i++) {

                String documentName = $"document {i}";
                String documentType = $"type {i}";
                String documentDepartment = $"department {i}";

                String sqlQuery = $"insert into documents(name, type, created_at, department, contracted_amount) values ('{documentName}','{documentType}','{DateTime.Now}','{documentDepartment}',{i})";

                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection);
                
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }