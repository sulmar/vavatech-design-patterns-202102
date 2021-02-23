using System;
using System.Collections.Generic;

namespace CommandPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Command Pattern!");

            Message message = new Message("555000123", "555888000", "Hello World!");

            ICommand command1 = new PrintCommand(message.Content, 4);

            ICommand command2 = new SendCommand(message.From, message.To, message.Content);

            // ...

            Queue<ICommand> commands = new Queue<ICommand>();

            commands.Enqueue(command1);
            commands.Enqueue(command2);

           
            while(commands.Count>0)
            {
                ICommand command = commands.Dequeue();

                if (command.CanExecute())
                    command.Execute();
            }


            //if (message.CanPrint())
            //{
            //    message.Print();
            //}



        }
    }

    #region Models

    public class Message
    {
        public Message(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

     
        public void Send()
        {
            Console.WriteLine($"Send message from <{From}> to <{To}> {Content}");
        }

        public bool CanSend()
        {
            return !(string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) || string.IsNullOrEmpty(Content));
        }

        public void Print(byte copies = 1)
        {
            for (int i = 0; i < copies; i++)
            {
                Console.WriteLine($"Print message from <{From}> to <{To}> {Content}");
            }
        }

        public bool CanPrint()
        {
            return string.IsNullOrEmpty(Content);
        }



    }

    #endregion


    public interface ICommand
    {
        bool CanExecute();
        void Execute();
    }

    public class SendCommand : ICommand
    {
        public SendCommand(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

        public bool CanExecute()
        {
            return !(string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) || string.IsNullOrEmpty(Content));
        }

        public void Execute()
        {
            Console.WriteLine($"Send message from <{From}> to <{To}> {Content}");
        }
    }

    public class PrintCommand : ICommand
    {
        public PrintCommand(string content, int copies = 1)
        {
            Content = content;
            Copies = copies;
        }

        public string Content { get; set; }

        public int Copies { get; set; }

        public bool CanExecute()
        {
            return string.IsNullOrEmpty(Content);
        }

        public void Execute()
        {
            for (int i = 0; i < Copies; i++)
            {
                Console.WriteLine($"Print message {Content}");
            }
        }
    }
}
