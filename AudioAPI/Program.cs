using System;
using NAudio.CoreAudioApi;

namespace AudioControlApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Audio Control Console Application");
            Console.WriteLine("Press '+' to increase volume, '-' to decrease volume, 's' to set a specific volume, or 'q' to quit.");

            // Initialize audio device enumerator
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.OemPlus || key == ConsoleKey.Add) // '+' Key
                {
                    AdjustVolume(device, 0.05f); // Increase by 5%
                }
                else if (key == ConsoleKey.OemMinus || key == ConsoleKey.Subtract) // '-' Key
                {
                    AdjustVolume(device, -0.05f); // Decrease by 5%
                }
                else if (key == ConsoleKey.S) // 's' Key
                {
                    SetSpecificVolume(device); // Set specific volume
                }
                else if (key == ConsoleKey.Q) // 'q' Key
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }
            }
        }

        static void AdjustVolume(MMDevice device, float adjustment)
        {
            float currentVolume = device.AudioEndpointVolume.MasterVolumeLevelScalar;
            float newVolume = currentVolume + adjustment;

            // Clamp volume to valid range [0.0, 1.0]
            newVolume = Clamp(newVolume, 0.0f, 1.0f);

            // Set new volume
            device.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

            Console.WriteLine($"Volume adjusted to: {newVolume:P0}");
        }

        static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        static void SetSpecificVolume(MMDevice device)
        {
            Console.Write("Enter the desired volume percentage (0-100): ");
            string input = Console.ReadLine();

            if (float.TryParse(input, out float percentage))
            {
                // Convert percentage to scalar value between 0.0 and 1.0
                float newVolume = percentage / 100.0f;

                // Clamp volume to valid range [0.0, 1.0]
                newVolume = Clamp(newVolume, 0.0f, 1.0f);

                // Set new volume
                device.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

                Console.WriteLine($"Volume set to: {newVolume:P0}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 100.");
            }
        }
    }
}