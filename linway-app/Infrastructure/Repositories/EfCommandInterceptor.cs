using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;

public class EfCommandInterceptor : DbCommandInterceptor
{
    private static readonly AsyncLocal<Dictionary<Guid, string>> _queries = new AsyncLocal<Dictionary<Guid, string>>();
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        if (_queries.Value == null)
        {
            _queries.Value = new Dictionary<Guid, string>();
        }
        _queries.Value[eventData.CommandId] = command.CommandText + "\n" + Environment.StackTrace;
        return result;
        //Debug.WriteLine("EF QUERY:");
        //Debug.WriteLine(command.CommandText);
        //Debug.WriteLine(Environment.StackTrace);
        //return base.ReaderExecuting(command, eventData, result);
    }
    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        if (_queries?.Value != null && _queries.Value.TryGetValue(eventData.CommandId, out var info))
        {
            Debug.WriteLine("EF FAILED QUERY:");
            Debug.WriteLine(info);  // imprime información de errores de concurrencia de DB Context
        }
    }
}
