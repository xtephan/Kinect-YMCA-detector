using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


//Kinect libraries
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;

namespace YMCA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Runtime nui = new Runtime();

        int[] complete = {0,0,0,0,0}; //complete progress bar :-)
        
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            /*
            //init kinect runtime for skeletal tracking
            nui.Initialize(RuntimeOptions.UseSkeletalTracking);
            //create an event
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            */

            // new version with camera
            nui.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);

            //video
            nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);

            //create an event
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);

            //create video event
            nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_ColorFrameReady);
        }


        //plays YMCA :-)
        void PlayMusic()
        {
            System.Diagnostics.Process.Start(@"C:\Users\juji\Documents\Visual Studio 2010\Projects\YMCA\YMCA\ymca-novegin.mp3");
        }


        //handles debug messages
        void addDebug(string msg) {
            debugg.Text += "\n" + msg;
        }


        //event Handler for video
        void nui_ColorFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            // 32-bit per pixel, RGBA image
            PlanarImage Image = e.ImageFrame.Image;
            video.Source = BitmapSource.Create(
                Image.Width, Image.Height, 96, 96, PixelFormats.Bgr32, null, Image.Bits, Image.Width * Image.BytesPerPixel);
        }



        //handle the event
        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //event fires when there is a lock on a skeleton

            //select first skeleton tracked or default
            SkeletonFrame allSkeletons = e.SkeletonFrame;

            SkeletonData skeleton = (from s in allSkeletons.Skeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();

            
            if (skeleton != null)
            {

                //addDebug("Tracking Skeleton...");

                //Canvas.
                SetEllipsePosition(headEllipse, skeleton.Joints[JointID.Head]);

                //left side
                SetEllipsePosition(leftEllipse, skeleton.Joints[JointID.HandLeft]);
                SetEllipsePosition(elbowleftEllipse, skeleton.Joints[JointID.ElbowLeft]);
                SetEllipsePosition(sholderleftEllipse, skeleton.Joints[JointID.ShoulderLeft]);
                
                //right side
                SetEllipsePosition(rightEllipse, skeleton.Joints[JointID.HandRight]);
                SetEllipsePosition(elbowrightEllipse, skeleton.Joints[JointID.ElbowRight]);
                SetEllipsePosition(sholderrightEllipse, skeleton.Joints[JointID.ShoulderRight]);

                //detect Y
                if ( complete[0]==0 && detectedY(skeleton) )
                {
                    addDebug("Detected Y!");
                    complete[0] = 1;
                }


                //detect M
                if (complete[1] == 0 && detectedM(skeleton))
                {
                    addDebug("Detected M!");
                    complete[1] = 1;
                }


                //detect C
                if (complete[2] == 0 && detectedC(skeleton))
                {
                    addDebug("Detected C!");
                    complete[2] = 1;
                }


                //detect A
                if (complete[3] == 0 && detectedA(skeleton))
                {
                    addDebug("Detected A!");
                    complete[3] = 1;
                }



                //should i play?
                if (shouldPlay())
                {
                    addDebug("Playing Music!!!!");
                    PlayMusic();
                    for (int i = 0; i <= 3; i++)
                        complete[i] = 0;
                }


            }

        }


        //are all positions detected?
        private bool shouldPlay()
        {

            int i = 0;

            for (i=0; i <= 3; i++)
                if (complete[i] == 0)
                    return false;

            return true;

        }


        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        //detect position for A
        private bool detectedA(SkeletonData skeleton)
        {


            // right hand positions
            var handRightScaled = skeleton.Joints[JointID.HandRight].ScaleTo(1000, 1000, .9f, .9f);
            float handRightScaledX = handRightScaled.Position.X;
            float handRightScaledY = 1000 - handRightScaled.Position.Y;


            // right elbow positions
            var elbowRightScaled = skeleton.Joints[JointID.ElbowRight].ScaleTo(1000, 1000, .9f, .9f);
            float elbowRightScaledX = elbowRightScaled.Position.X;
            float elbowRightScaledY = 1000 - elbowRightScaled.Position.Y;

            // right sholder positions
            var sholderRightScaled = skeleton.Joints[JointID.ShoulderRight].ScaleTo(1000, 1000, .9f, .9f);
            float sholderRightScaledX = sholderRightScaled.Position.X;
            float sholderRightScaledY = 1000 - sholderRightScaled.Position.Y;


            // left hand positions
            var handLeftScaled = skeleton.Joints[JointID.HandLeft].ScaleTo(1000, 1000, .9f, .9f);
            float handLeftScaledX = handLeftScaled.Position.X;
            float handLeftScaledY = 1000 - handLeftScaled.Position.Y;


            // left elbow positions
            var elbowLeftScaled = skeleton.Joints[JointID.ElbowLeft].ScaleTo(1000, 1000, .9f, .9f);
            float elbowLeftScaledX = elbowLeftScaled.Position.X;
            float elbowLeftScaledY = 1000 - elbowLeftScaled.Position.Y;

            // left sholder positions
            var sholderLeftScaled = skeleton.Joints[JointID.ShoulderLeft].ScaleTo(1000, 1000, .9f, .9f);
            float sholderLeftScaledX = sholderLeftScaled.Position.X;
            float sholderLeftScaledY = 1000 - sholderLeftScaled.Position.Y;


            bool rightA = (sholderRightScaledY < elbowRightScaledY) &
                          (elbowRightScaledY < handRightScaledY) &
                          (sholderRightScaledX < elbowRightScaledX ) &
                          (elbowRightScaledX >handRightScaledX  );



            bool leftA = (sholderLeftScaledY < elbowLeftScaledY) &
                         (elbowLeftScaledY < handLeftScaledY) &
                         (sholderLeftScaledX > elbowLeftScaledX) &
                         (elbowLeftScaledX < handLeftScaledX);

            bool comA = (Math.Abs(handRightScaledY - handLeftScaledY) < 25) &
                        (Math.Abs(elbowLeftScaledY - elbowRightScaledY) < 25);

            /*
            //Debug
            //dx1.Text = "left Right X: " + handLeftScaledX.ToString();
            dy1.Text = "right elbow X: " + elbowRightScaledX.ToString();


            //dx2.Text = "sholder left X:" + sholderLeftScaledX.ToString();
            dy2.Text = "right  hand x: " + handRightScaledX.ToString();
            */

            return rightA & leftA & comA;
        }



        //CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC
        //detect position for C
        private bool detectedC(SkeletonData skeleton)
        {


            // right hand positions
            var handRightScaled = skeleton.Joints[JointID.HandRight].ScaleTo(1000, 1000, .9f, .9f);
            float handRightScaledX = handRightScaled.Position.X;
            float handRightScaledY = 1000 - handRightScaled.Position.Y;


            // right elbow positions
            var elbowRightScaled = skeleton.Joints[JointID.ElbowRight].ScaleTo(1000, 1000, .9f, .9f);
            float elbowRightScaledX = elbowRightScaled.Position.X;
            float elbowRightScaledY = 1000 - elbowRightScaled.Position.Y;

            // right sholder positions
            var sholderRightScaled = skeleton.Joints[JointID.ShoulderRight].ScaleTo(1000, 1000, .9f, .9f);
            float sholderRightScaledX = sholderRightScaled.Position.X;
            float sholderRightScaledY = 1000 - sholderRightScaled.Position.Y;


            // left hand positions
            var handLeftScaled = skeleton.Joints[JointID.HandLeft].ScaleTo(1000, 1000, .9f, .9f);
            float handLeftScaledX = handLeftScaled.Position.X;
            float handLeftScaledY = 1000 - handLeftScaled.Position.Y;


            // left elbow positions
            var elbowLeftScaled = skeleton.Joints[JointID.ElbowLeft].ScaleTo(1000, 1000, .9f, .9f);
            float elbowLeftScaledX = elbowLeftScaled.Position.X;
            float elbowLeftScaledY = 1000 - elbowLeftScaled.Position.Y;

            // left sholder positions
            var sholderLeftScaled = skeleton.Joints[JointID.ShoulderLeft].ScaleTo(1000, 1000, .9f, .9f);
            float sholderLeftScaledX = sholderLeftScaled.Position.X;
            float sholderLeftScaledY = 1000 - sholderLeftScaled.Position.Y;


            bool rightC = (Math.Abs(handRightScaledY - elbowRightScaledY) < 25) &
                          ( sholderRightScaledX < handRightScaledX ) &
                          (elbowRightScaledY < sholderRightScaledY);


            
            bool leftC = (Math.Abs(handLeftScaledY - elbowLeftScaledY) < 25) &
                         (sholderLeftScaledX < elbowLeftScaledX);
            
            /*
            //Debug
            //dx1.Text = "left Right X: " + handLeftScaledX.ToString();
            dy1.Text = "right elbow X: " + elbowRightScaledX.ToString();


            //dx2.Text = "sholder left X:" + sholderLeftScaledX.ToString();
            dy2.Text = "right  hand x: " + handRightScaledX.ToString();
            */

            return rightC & leftC;
        }






        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
        //detect position for M
        private bool detectedM(SkeletonData skeleton)
        {


            // right hand positions
            var handRightScaled = skeleton.Joints[JointID.HandRight].ScaleTo(1000, 1000, .9f, .9f);
            float handRightScaledX = handRightScaled.Position.X;
            float handRightScaledY = 1000 - handRightScaled.Position.Y;


            // right elbow positions
            var elbowRightScaled = skeleton.Joints[JointID.ElbowRight].ScaleTo(1000, 1000, .9f, .9f);
            float elbowRightScaledX = elbowRightScaled.Position.X;
            float elbowRightScaledY = 1000 - elbowRightScaled.Position.Y;

            // right sholder positions
            var sholderRightScaled = skeleton.Joints[JointID.ShoulderRight].ScaleTo(1000, 1000, .9f, .9f);
            float sholderRightScaledX = sholderRightScaled.Position.X;
            float sholderRightScaledY = 1000 - sholderRightScaled.Position.Y;


            // left hand positions
            var handLeftScaled = skeleton.Joints[JointID.HandLeft].ScaleTo(1000, 1000, .9f, .9f);
            float handLeftScaledX = handLeftScaled.Position.X;
            float handLeftScaledY = 1000 - handLeftScaled.Position.Y;


            // left elbow positions
            var elbowLeftScaled = skeleton.Joints[JointID.ElbowLeft].ScaleTo(1000, 1000, .9f, .9f);
            float elbowLeftScaledX = elbowLeftScaled.Position.X;
            float elbowLeftScaledY = 1000 - elbowLeftScaled.Position.Y;

            // left sholder positions
            var sholderLeftScaled = skeleton.Joints[JointID.ShoulderLeft].ScaleTo(1000, 1000, .9f, .9f);
            float sholderLeftScaledX = sholderLeftScaled.Position.X;
            float sholderLeftScaledY = 1000 - sholderLeftScaled.Position.Y;

             
            bool rightM = (Math.Abs(sholderRightScaledY - elbowRightScaledY) < 25) &
                          (Math.Abs(handRightScaledX - elbowRightScaledX) < 25) &
                          (elbowRightScaledY > handRightScaledY);



            bool leftM = (Math.Abs(sholderLeftScaledY - elbowLeftScaledY) < 25) &
                         (Math.Abs(handLeftScaledX - elbowLeftScaledX) < 25) &
                         (elbowLeftScaledY > handLeftScaledY);

            /*
            //Debug
            //dx1.Text = "left Right X: " + handLeftScaledX.ToString();
            dy1.Text = "right elbow X: " + elbowRightScaledX.ToString();


            //dx2.Text = "sholder left X:" + sholderLeftScaledX.ToString();
            dy2.Text = "right  hand x: " + handRightScaledX.ToString();
            */
            
            return rightM & leftM;
        }




        //YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY
        //detect position for Y
        private bool detectedY(SkeletonData skeleton)
        {
            
            
            // right elbow positions
            /*
            var elbowRightScaled = skeleton.Joints[JointID.ElbowRight].ScaleTo(1000, 1000, .9f, .9f);
            float elbowRightScaledX = elbowRightScaled.Position.X;
            float elbowRightScaledY = elbowRightScaled.Position.Y;
            */

            // right hand positions
            var handRightScaled = skeleton.Joints[JointID.HandRight].ScaleTo(1000, 1000, .9f, .9f);
            float handRightScaledX = handRightScaled.Position.X;
            float handRightScaledY = 1000 - handRightScaled.Position.Y;

            // right sholder positions
            var sholderRightScaled = skeleton.Joints[JointID.ShoulderRight].ScaleTo(1000, 1000, .9f, .9f);
            float sholderRightScaledX = sholderRightScaled.Position.X;
            float sholderRightScaledY = 1000 - sholderRightScaled.Position.Y;



            // left hand positions
            var handLeftScaled = skeleton.Joints[JointID.HandLeft].ScaleTo(1000, 1000, .9f, .9f);
            float handLeftScaledX = handLeftScaled.Position.X;
            float handLeftScaledY = 1000 - handLeftScaled.Position.Y;

            // left sholder positions
            var sholderLeftScaled = skeleton.Joints[JointID.ShoulderLeft].ScaleTo(1000, 1000, .9f, .9f);
            float sholderLeftScaledX = sholderLeftScaled.Position.X;
            float sholderLeftScaledY = 1000 - sholderLeftScaled.Position.Y;

            
            bool rightHandUp = (sholderRightScaledX < handRightScaledX) & ( sholderRightScaledY < handRightScaledY );
            bool leftHandUp  = (sholderLeftScaledX > handLeftScaledX)   & (sholderLeftScaledY < handLeftScaledY);
              
            /*
            //debug messages
            dx1.Text = "left Right X: " + handLeftScaledX.ToString();
            dy1.Text = "left Right Y: " + handLeftScaledY.ToString();


            dx2.Text = "sholder left X:" + sholderLeftScaledX.ToString();
            dy2.Text = "sholder left y: " + sholderLeftScaledY.ToString();
            */

            
            return rightHandUp & leftHandUp;
        }

        //move the elipse
        private void SetEllipsePosition(FrameworkElement ellipse, Joint joint)
        {
            var scaledJoint = joint.ScaleTo(1000, 1000, .9f, .9f);

            Canvas.SetLeft(ellipse, scaledJoint.Position.X);
            Canvas.SetTop(ellipse, scaledJoint.Position.Y);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            nui.Uninitialize();
        }
    }
}
