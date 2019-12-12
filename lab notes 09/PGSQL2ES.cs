class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri("http://192.168.99.100:9200/");

            var settings = new ConnectionSettings(uri).DefaultIndex("towns");

            ElasticClient client = new ElasticClient(settings);

            List<Town> townList = new List<Town>();

            String connectionString = "Host=192.168.99.100;Username=pdt;Password=123;Database=pdtgis;";

            String sqlQuery = @"SELECT osm_id, name, type, area 
                                        FROM planet_osm_point pol  
                                        WHERE area > 1000000 AND (type = 'town' OR type = 'city')";

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection);

            NpgsqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {                
                Town town = new Town();

                town.name = dr[1].ToString();
                town.type = dr[2].ToString();
                town.area = double.Parse(dr[3].ToString());

                townList.Add(town);
            }

            var bulkIndexResponse = client.IndexMany(townList);

            Console.WriteLine($"Inserted count: " + bulkIndexResponse.Items.Count);

            Console.ReadKey();
        }
    }