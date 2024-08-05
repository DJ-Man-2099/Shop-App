using System;

namespace Shop.Models;

public class OpResult<T>
{
	public bool Succeeded { get; set; } = true;
	public T? Value { get; set; }
	public Dictionary<string, string>? Errors { get; set; }
}
