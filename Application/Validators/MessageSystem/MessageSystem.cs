using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem
{
    public enum MessageType { Info,Error,Warning }
    public  class UserMessageManager
    {
        public IList<UserMessage> UserMessages { get; set; }
        public IList<UserMessage> AlertMessages { get; set; }

        public UserMessageManager()
        {
            this.UserMessages=new List<UserMessage>();
            this.AlertMessages = new List<UserMessage>();
        }

        public void AddErrorMessage(string message)
        {
            this.UserMessages.Add(new UserMessage()
            {
                MessageType = MessageType.Error,
                Message = message
            }) ;
        }

        public void AddAlertMessage(string message, string typ)
        {
            Enum.TryParse(typ, out MessageType AlertType);

            this.AlertMessages.Add(new UserMessage()
            {
                MessageType = AlertType,
                Message = message
            });
        }

        public bool IsValidForm()
        {
            return UserMessages.Where(x => x.MessageType == MessageType.Error).Count() ==0;
        }

    }


    public class UserMessage
    {
        public MessageType MessageType {  get; set; }
        public string Message {  get; set; }
    }
}
