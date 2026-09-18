using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// ValueSequence entity for generating sequential numbers (client numbers, plot numbers, etc.).
/// Maps to T_ValueSequence table in the legacy database.
/// </summary>
public class ValueSequence
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("TableName")]
    [StringLength(100)]
    public string? TableName { get; set; }

    [Column("FieldName")]
    [StringLength(100)]
    public string? FieldName { get; set; }

    [Column("CurrentValue")]
    public int CurrentValue { get; set; }

    [Column("Prefix")]
    [StringLength(20)]
    public string? Prefix { get; set; }

    [Column("Suffix")]
    [StringLength(20)]
    public string? Suffix { get; set; }

    [Column("Padding")]
    public int? Padding { get; set; }

    [Column("Description")]
    [StringLength(500)]
    public string? Description { get; set; }
}
