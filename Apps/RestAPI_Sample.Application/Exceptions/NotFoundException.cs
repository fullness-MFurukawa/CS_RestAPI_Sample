namespace RestAPI_Sample.Application.Exceptions;
/// <summary>
/// データが存在しないことを表す例外クラス
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) 
    : base(message){}
    public NotFoundException(string message, Exception innerException) 
    : base(message, innerException){}
}