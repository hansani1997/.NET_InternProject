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

        public T DataObject { get; set; }

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
}
