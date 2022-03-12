
namespace Flight
{
    internal class Program
    {
        static void Main()
        {
            var connections = new (string, string)[] { ("ATH", "EDI"), ("ATH", "GLA"), ("ATH", "CTA"), ("BFS", "CGN"), ("BFS", "LTN"), ("BFS", "CTA"), ("BTS", "STN"), ("BTS", "BLQ"), ("CRL", "BLQ"), ("CRL", "BSL"), ("CRL", "LTN"), ("DUB", "LCA"), ("LTN", "DUB"), ("LTN", "MAD"), ("LCA", "HAM"), ("EIN", "BUD"), ("EIN", "MAD"), ("HAM", "BRS"), ("KEF", "LPL"), ("KEF", "CGN"), ("SUF", "LIS"), ("SUF", "BUD"), ("SUF", "STN"), ("STN", "EIN"), ("STN", "HAM"), ("STN", "DUB"), ("STN", "KEF") };
            var startAirport = "CGN";
            var endAirport = "ATH";

            var allConnections = GetConncetionsInBothDirections(connections.ToList());

            var initRoute = new List<string>();
            initRoute.Add(startAirport);

            var routes = new List<List<string>>();
            routes.Add(initRoute);

            while (true)
            {
                routes = GenerateNextLevelRoutes(allConnections, routes);
                if (routes.Count == 0 || routes.Any(x => x.Last() == endAirport)) break;
            }

            ShowSummary(endAirport, routes);
        }

        private static void ShowSummary(string endAirport, List<List<string>> routes)
        {
            var shortestRoutes = routes.Where(x => x.Last() == endAirport);

            if (shortestRoutes.Any())
            {
                foreach (var route in shortestRoutes)
                {
                    Console.WriteLine($"Shortest conncetion: {string.Join(" => ", route)}");
                }
            }
            else
            {
                Console.WriteLine($"Shortest conncetion is not available.");
            }

            Console.ReadKey();
        }

        private static List<List<string>> GenerateNextLevelRoutes(List<(string, string)> allConnections, List<List<string>> routes)
        {
            var nextLevelRoutes = new List<List<string>>();

            foreach (var route in routes)
            {
                var nextLevelConnections = GetDeparturesFromAirportWithoutRoutedAirports(route.Last(), allConnections, route);

                if (nextLevelConnections.Any())
                {
                    foreach (var nextConnection in nextLevelConnections)
                    {
                        var nextLevelRoute = route.Select(x => x).ToList();
                        nextLevelRoute.Add(nextConnection);
                        nextLevelRoutes.Add(nextLevelRoute);
                    }
                }
            }

            return nextLevelRoutes;
        }

        private static List<string> GetDeparturesFromAirportWithoutRoutedAirports(string airport, List<(string, string)> allConnections, List<string> currentRoute)
        {
            var connections = allConnections.Where(x => x.Item1 == airport);
            var routedFilteredConnections = connections.Where(x => currentRoute.Contains(x.Item2) == false);

            return routedFilteredConnections.Select(x => x.Item2).ToList();
        }

        private static List<(string, string)> GetConncetionsInBothDirections(List<(string, string)> connections)
        {
            var allConnections = new List<(string, string)>();
            connections.ToList().ForEach(x =>
            {
                allConnections.Add(x);
                allConnections.Add((x.Item2, x.Item1));
            });
            return allConnections;
        }
    }
}


