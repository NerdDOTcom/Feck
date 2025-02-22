using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

class Program
{
  static void Main()
  {
    int port = 5000;

    var server = new Server(port);

    string main_path = "C:\\Users\\Maya\\Documents\\Halfburnedsmile\\WebTemplate\\website\\pages\\";

    Console.WriteLine("The server is running");
    Console.WriteLine($"Main Page: http://localhost:{port}/website/pages/index.html");

    var database = new Database();
    database.Database.EnsureDeleted();
    database.Database.EnsureCreated();

    if (!database.Cless.Any())
    {
      database.Cless.Add(new Cless("1", "1"));
      database.Cless.Add(new Cless("2", "2"));
      database.SaveChanges();
      database.Sd.Add(new Sd("1", "a", "1"));
      database.Sd.Add(new Sd("2", "aa", "1"));
      database.Sd.Add(new Sd("3", "b", "2"));
      database.SaveChanges();
    }
    while (true)
    {
      (var request, var response) = server.WaitForRequest();

      Console.WriteLine($"Recieved a request with the path: {request.Path}");
      string true_path = main_path + request.Path;
      if (File.Exists(true_path))
      {
        var file = new File(true_path);
        response.Send(file);
      }
      else if (request.ExpectsHtml())
      {
        var file = new File(main_path + "404.html");
        response.SetStatusCode(404);
        response.Send(file);
      }
      else
      {
        try
        {
          if (request.Path == "a")
          {
            var clss = request.GetBody<string>();
            var w = database.Cless.FirstOrDefault(r => r.Username == clss);
            var z = w.Id;
            var u = database.Sd.Where(r => r.ClessId == z).ToArray();
            response.Send(u);
          }
          response.SetStatusCode(405);

          //database.SaveChanges();
        }
        catch (Exception exception)
        {
          Log.WriteException(exception);
        }
      }

      response.Close();
    }
  }
}


public class Database() : DbBase("database")
{
  public DbSet<Cless> Cless { get; set; } = default!;
  public DbSet<Sd> Sd { get; set; } = default!;
}

public class Cless
{
  public Cless(string id, string username)
  {
    Id = id;
    Username = username;

  }
  [Key] public string Id { get; set; } = default!;
  public string Username { get; set; } = default!;
}

public class Sd(string id, string name, string clessId)
{
  [Key] public string Id { get; set; } = id;
  public string name { get; set; } = name;
  public string ClessId { get; set; } = clessId;
  [ForeignKey("ClessId")] public Cless Bean { get; set; } = default!;
}