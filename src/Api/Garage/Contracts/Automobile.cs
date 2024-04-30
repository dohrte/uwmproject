namespace Api;

public class NewAutomobile
{
  public required string Make { get; set;}
  public required string Model { get; set;}

  public required int Year { get; set;}

  public required string Name { get; set;}

  public DateTime? LastServiceDate { get; set;}
}

public class Automobile : NewAutomobile
{
  public Guid Id { get; set; }
}