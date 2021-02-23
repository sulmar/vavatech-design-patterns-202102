using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VisitorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Visitor Pattern!");

            Form form = Get();

            IVisitor visitor = new HtmlVisitor();

            form.Accept(visitor);

            string html = visitor.Output;

            System.IO.File.WriteAllText("index.html", html);
        }

        public static Form Get()
        {
            Form form = new Form
            {
                Name = "/forms/customers",
                Caption = "Design Patterns",

                Body = new Collection<ControlBase>
                {
                    new LabelControl { Caption = "Person", Name = "lblName" },
                    new TextBoxControl {  Caption = "FirstName", Name = "txtFirstName", Value = "John"},
                    new CheckBoxControl { Caption = "IsAdult", Name = "chkIsAdult", Value = true },
                    new ButtonControl {  Caption = "Submit", Name = "btnSubmit", ImageSource = "save.png" },
                }

            };

            return form;
        }
    }

    #region Models

    public class Form : ControlBase
    {
        public ICollection<ControlBase> Body { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        //public string GetHtml()
        //{
        //    string html = "<html>";

        //    html += $"<title>{Title}</title>";

        //    html += "<body>";

        //    foreach (var control in Body)
        //    {
        //        switch (control.Type)
        //        {
        //            case ControlType.Label:
        //                html += $"<span>{control.Caption}</span>"; break;

        //            case ControlType.TextBox:
        //                html += $"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>"; break;

        //            case ControlType.Checkbox:
        //                html += $"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>"; break;

        //            case ControlType.Button:
        //                html += $"<button><img src='{control.ImageSource}'/>{control.Caption}</button>"; break;
        //        }

        //    }

        //    html += "</body>";
        //    html += "</html>";

        //    return html;
        //}
    }

    public class Control
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public ControlType Type { get; set; }
        public string Value { get; set; }
        public string ImageSource { get; set; }
    }

    public abstract class ControlBase
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public abstract void Accept(IVisitor visitor);
       
    }

    public class LabelControl : ControlBase
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class TextBoxControl : ControlBase
    {
        public string Value { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CheckBoxControl : ControlBase
    {
        public bool Value { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class RadioButtonControl : ControlBase
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ButtonControl : ControlBase
    {
        public string ImageSource { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public enum ControlType
    {
        Label,
        TextBox,
        Checkbox,
        Button
    }


    #endregion

    // abstract visitor
    public interface IVisitor
    {
        void Visit(Form form);
        void Visit(LabelControl control);
        void Visit(TextBoxControl control);
        void Visit(CheckBoxControl control);
        void Visit(ButtonControl control);
        void Visit(RadioButtonControl control);
        
        string Output { get; }
    }


    public class MarkdownVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Output => builder.ToString();


        public void Visit(Form form)
        {
            throw new NotImplementedException();
        }

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"**{control.Caption}**");
        }

        public void Visit(TextBoxControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(CheckBoxControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(ButtonControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(RadioButtonControl control)
        {
            throw new NotImplementedException();
        }
    }

    // concrete visitor

    public class HtmlVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Output => builder.ToString();

        public void Visit(Form form)
        {
            foreach (ControlBase control in form.Body)
            {
                control.Accept(this);
            }
        }

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span>");
        }

        public void Visit(TextBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>");
        }

        public void Visit(CheckBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>");
        }

        public void Visit(ButtonControl control)
        {
            builder.AppendLine($"<button><img src='{control.ImageSource}'/>{control.Caption}</button>");
        }

        public void Visit(RadioButtonControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='radio'></input>");
        }
    }

}
