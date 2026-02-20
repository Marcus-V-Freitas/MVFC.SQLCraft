namespace MVFC.SQLCraft.Tests.Models;

public sealed record Person {
    public required int Id { get; init; }
    public required string Name { get; init; }
}