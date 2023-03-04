using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Gpio;

namespace Pillowsoft.GhostKeys
{
    public abstract class GpioHandler
    {
        private readonly GpioController controller;


        public GpioHandler()
        {


            //This workaround is needed on some Pis
            var assembly = typeof(GpioDriver).Assembly;
            var driverType = assembly.GetType("System.Device.Gpio.Drivers.RaspberryPi3LinuxDriver");
            var ctor = driverType.GetConstructor(new Type[] { });
            var driver = ctor.Invoke(null) as GpioDriver;

            this.controller = new GpioController(PinNumberingScheme.Board, driver);


           // this.controller = new MockGpioController();
        }


        protected void OpenOutputPin(int pin, PinValue initialValue)
        {
            if (!controller.IsPinOpen(pin))
            {
                controller.OpenPin(pin, PinMode.Output, initialValue);
            }
            else
            {
                WriteOutputPin(pin, initialValue);
            }
        }

        protected void WriteOutputPin(int pin, PinValue pinValue)
        {
            controller.Write(pin, pinValue);
        }
    }


}
