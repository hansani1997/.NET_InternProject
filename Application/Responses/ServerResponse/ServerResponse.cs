using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse
{
    public enum ServerResponseType
    {
        Success = 1,
        BussinesLogicError,
        ProcessingError,
        NoPermission

    }

    public enum ServerMessageType
    {
        Error,
        Warning,
        Info
    }
    public class ServerMessage
    {
        public ServerMessageType MessageType { get; set; }

        public string Message { get; set; }
    }
    public class BaseServerResponse<T> where T : class
    {
        public ServerResponseType ResponseType { get; set; }
        public IList<ServerMessage> Messages { get; set; }
        public BaseServerResponse()
        {
            Messages = new List<ServerMessage>();
         
        }

        public void AddErrorMessage(string Message)
        {
            var message = new ServerMessage();
            message.Message = Message;
            message.MessageType = ServerMessageType.Error;
            Messages.Add(message);
        }

        public void AddWarnMessage(string Message)
        {
            var message = new ServerMessage();
            message.Message = Message;
            message.MessageType = ServerMessageType.Warning;
            Messages.Add(message);
        }

        public void AddInfoMessage(string Message)
        {
            var message = new ServerMessage();
            message.Message = Message;
            message.MessageType = ServerMessageType.Info;
            Messages.Add(message);
        }

        public T Value { get; set; }

        public bool IsValidResponse()
        {
            return Messages.Where(x => x.MessageType == ServerMessageType.Error).Count() == 0;
        }
    }


    //public class GetFromTransactionServerResponse : BaseServerResponse
    //{
    //    public IList<GetFromTransactionResponse> Transactions { get; set; }

    //    public GetFromTransactionServerResponse()
    //    {
    //        Transactions = new List<GetFromTransactionResponse>();
    //    }
    //}

    public class ApiServerResponse
    {
        public IList<ServerResponseMessae> Messages { get; set; }

        public ApiServerResponse()
        {
            Messages = new List<ServerResponseMessae>();
            ResponseId = Guid.NewGuid().ToString();
        }

        public Exception ExecutionException { get; set; }
        public DateTime ExecutionStarted { get; set; }
        public DateTime ExecutionEnded { get; set; }
        public TimeSpan GetExecutionTimeSpan()
        {
            return ExecutionEnded - ExecutionStarted;
        }

        public string ResponseId { get; set; }
    }

    public class ApiServerResponse<T> : ApiServerResponse where T : class
    {
        public T Value { get; set; }



    }

    public enum ServerResponseMessageType
    {
        ConnectionFaliure = 3,
        Exception = 2,
        Success = 1,
    }

    public class ServerResponseMessae
    {
        public ServerResponseMessageType MessageType { get; set; }
        public string Message { get; set; }
    }
}
