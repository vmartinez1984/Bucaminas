using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace Media.Sound
{
    public class Player
    {
        public static void PlayMarcha()
        {
            WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();
            mediaPlayer.URL = @"C:\Users\vmartinez\Downloads\Marcha.mp3";
            mediaPlayer.controls.play();
        }
    }
}
