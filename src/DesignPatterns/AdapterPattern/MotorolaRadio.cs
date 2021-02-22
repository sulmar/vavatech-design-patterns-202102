using System;

namespace AdapterPattern
{
    // abstract adapter
    public interface IRadioAdapter
    {
        void Call(byte channel, string message);
    }

    // concrete adapter
    public class MotorolaRadioAdapter : IRadioAdapter
    {
        // adaptee
        private MotorolaRadio radio;

        private string pincode;

        public MotorolaRadioAdapter(string pincode)
        {
            radio = new MotorolaRadio();

            this.pincode = pincode;
        }

        public void Call(byte channel, string message)
        {
            radio.PowerOn(pincode);

            radio.SelectChannel(channel);
            radio.Send(message);
            radio.PowerOff();
        }
    }

    public class MotorolaRadio
    {
        private bool enabled;

        private byte? selectedChannel;

        public MotorolaRadio()
        {
            enabled = false;
        }

        public void PowerOn(string pincode)
        {
            if (pincode == "1234")
            {
                enabled = true;
            }
        }

        public void SelectChannel(byte channel)
        {
            this.selectedChannel = channel;
        }

        public void Send(string message)
        {
            if (enabled && selectedChannel!=null)
            {
                Console.WriteLine($"<Xml><Send Channel={selectedChannel}><Message>{message}</Message></xml>");
            }
        }

        public void PowerOff()
        {
            enabled = false;
        }
    }
}
