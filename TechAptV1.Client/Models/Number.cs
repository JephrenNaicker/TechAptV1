// Copyright © 2025 Always Active Technologies PTY Ltd

using System.ComponentModel.DataAnnotations;

namespace TechAptV1.Client.Models;

public class Number
{
    [Key]
    public int Id { get; set; }
    public int Value { get; set; }
    public int IsPrime { get; set; } = 0;
}
