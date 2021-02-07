using System;
using System.IO;
using Vosk;

public class Test
{
   public static void Main()
   {

        Vosk.Vosk.SetLogLevel(0);
        Model model = new Model("model");
        VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);

        using(Stream source = File.OpenRead("test.wav")) {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                if (rec.AcceptWaveform(buffer, bytesRead)) {
                    Console.WriteLine(rec.Result());
                } else {
                    Console.WriteLine(rec.PartialResult());
                }
            }
        }
        Console.WriteLine(rec.FinalResult());

        rec = new VoskRecognizer(model, 16000.0f);

        using(Stream source = File.OpenRead("test.wav")) {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                float[] fbuffer = new float[bytesRead / 2];
                for (int i = 0, n = 0; i < fbuffer.Length; i++, n+=2) {
                    fbuffer[i] = (short)(buffer[n] | buffer[n+1] << 8);
                }
                if (rec.AcceptWaveform(fbuffer, fbuffer.Length)) {
                    Console.WriteLine(rec.Result());
                    GC.Collect();
                } else {
                    Console.WriteLine(rec.PartialResult());
                }
            }
        }
        Console.WriteLine(rec.FinalResult());
   }
}
