﻿namespace AdminDepartamentos.API.Core;

public class ResponseAPI<T>
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public T? Data { get; set; }
}