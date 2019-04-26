using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;
using System.Threading;

namespace Framework.SikuliAutomation
{
    public class SikuliTest
    {
        APILauncher launch = new APILauncher(true);

        public void CallImage(string path)
        {
            try
            {
                Pattern Image1 = new Pattern(path);
                launch.Start();

                Screen scr = new Screen();
                scr.Click(Image1, true);
                //
            }
            catch (Exception ex)
            {
                throw;
            }            
        }

        public void CallImageForTextInput(string imagePath, string logicalFieldValue)
        {
            try
            {
                Pattern Image1 = new Pattern(imagePath);
                launch.Start();

                Screen scr = new Screen();
                scr.Type(Image1, logicalFieldValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
