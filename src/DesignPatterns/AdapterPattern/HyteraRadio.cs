using System;

namespace AdapterPattern
{


    // concrete adapter
    public class HyteraRadioAdapter : IRadioAdapter
    {
        // adaptee
        private HyteraRadio radio = new HyteraRadio();

        public void Call(byte channel, string message)
        {
            radio.Init();
            radio.SendMessage(channel, message);
            radio.Release();
        }
    }

    // adapter - wariant klasowy
    public class HyteraRadioClassAdapter : MotorolaRadio, IRadioAdapter
    {
        private string pincode;

        public HyteraRadioClassAdapter(string pincode)
        {
            this.pincode = pincode;
        }

        public void Call(byte channel, string message)
        {
            base.PowerOn(pincode);
            base.SelectChannel(channel);
            base.Send(message);
            base.PowerOff();
        }
    }

    public class HyteraRadio
    {

        private RadioStatus status;

        public void Init()
        {
            status = RadioStatus.On;
        }

        public void SendMessage(byte channel, string content)
        {
            if (status == RadioStatus.On)
            {
                Console.WriteLine($"CHANNEL {channel}, MESSAGE {content}");
            }
        }

        public void Release()
        {
            status = RadioStatus.Off;
        }

        public enum RadioStatus
        {
            On,
            Off
        }

    }
}
