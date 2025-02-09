using System;

namespace jellybins.Fluent.Models;

public class ErrorPageModel
{
    public ErrorPageModel(Exception e)
    {
        ShortenMessage = e.Message;
        ExceptionRawTree = e.ToString();
    }
    public string ShortenMessage { get; set; }
    public string ExceptionRawTree { get; set; }
}