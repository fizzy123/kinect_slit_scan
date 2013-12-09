//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ColorBasics
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Intermediate storage for the color data received from the camera
        /// </summary>
        private byte[] colorPixels;

        /// <summary>
        /// Intermediate storage for the past color data received from the camera
        /// </summary>
        private List<byte[]>[] colorPastPixels = new List<byte[]>[960];
        
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 960; i++ ){
                this.colorPastPixels[i] = new List<byte[]>();
            } 
            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the color stream to receive color frames
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Allocate space to put the pixels we'll receive
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Set the image we display to point to the bitmap where we'll put the image data
                this.Image.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.ColorFrameReady += this.SensorColorFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        int time = 0;
        /// <summary>
        /// Event handler for Kinect sensor's ColorFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    int y;
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(this.colorPixels);
                    byte[] rowPixels = new byte[1280];
                    for (int i = 0; i < 960; ++i)
                    {
                        Array.Copy(this.colorPixels, 1280 * i, rowPixels, 0,1280);
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        colorPastPixels[i].Add(rowPixels);
                        if (colorPastPixels[i].Count - 1 > i)
                        {
                            colorPastPixels[i].RemoveAt(0);
                        }
                        rowPixels = new byte[1280];
                        if (time != 0)
                        {
                            Array.Copy(colorPastPixels[i][0], 0, this.colorPixels, 1280 * i, 1280);
                        }
                    }
                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
            Console.WriteLine("{0}", time++);
        }
    }
}