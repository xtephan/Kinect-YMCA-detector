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
<<<<<<< HEAD
using System.Media;

using System.Reflection;
using System.IO;
using System.Resources;
using System.Media;
using System.Diagnostics;
=======

>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c

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

<<<<<<< HEAD
        private struct JPoint
        {
            public float X;
            public float Y;
        }

        //positions of the joints at a given time
        private JPoint head, sholderLeft, elbowLeft, handLeft, sholderRight, elbowRight, handRight;
        
        Runtime nui = Runtime.Kinects[0];

        int[] complete = {0,0,0,0,0}; //complete progress bar :-)
        string order   = "ymcar";
        int orderIndex = 0;


        Assembly assembly;
        SoundPlayer simpleSound;
        bool activeplayer = false;

        public MainWindow()
        {
            assembly = Assembly.GetExecutingAssembly();
            simpleSound = new SoundPlayer(assembly.GetManifestResourceStream("YMCA.ymca2.wav"));
            //simpleSound.Play();
=======
<<<<<<< HEAD
        Runtime nui = Runtime.Kinects[0];
=======
        Runtime nui = new Runtime();
>>>>>>> 0bd3a0cfb1dce3d0dbddeb9d638735ba9d22b750

        int[] complete = {0,0,0,0,0}; //complete progress bar :-)
        
        public MainWindow()
        {
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c
            InitializeComponent();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

<<<<<<< HEAD
            addDebug("Give me a Y!");
=======
            /*
            //init kinect runtime for skeletal tracking
            nui.Initialize(RuntimeOptions.UseSkeletalTracking);
            //create an event
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            */
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c

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
<<<<<<< HEAD
            if (!activeplayer)
            {
                simpleSound.PlayLooping();
                activeplayer = true;
            }
=======
<<<<<<< HEAD
            //System.Diagnostics.Process.Start(@"C:\Users\juji\Documents\Visual Studio 2010\Projects\YMCA\YMCA\ymca-novegin.mp3");
=======
            System.Diagnostics.Process.Start(@"C:\Users\juji\Documents\Visual Studio 2010\Projects\YMCA\YMCA\ymca-novegin.mp3");
>>>>>>> 0bd3a0cfb1dce3d0dbddeb9d638735ba9d22b750
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c
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

<<<<<<< HEAD
                //head, sholderLeft, elbowLeft, handLeft, sholderRight, elbowRight, handRight

                head = JointLocation(skeleton.Joints[JointID.Head]);

                sholderLeft = JointLocation(skeleton.Joints[JointID.ShoulderLeft]);
                elbowLeft = JointLocation(skeleton.Joints[JointID.ElbowLeft]);
                handLeft = JointLocation(skeleton.Joints[JointID.HandLeft]);

                sholderRight = JointLocation(skeleton.Joints[JointID.ShoulderRight]);
                elbowRight = JointLocation(skeleton.Joints[JointID.ElbowRight]);
                handRight = JointLocation(skeleton.Joints[JointID.HandRight]);

                //head smileyHead
                //SetEllipsePosition2(headEllipse, head);

                //smileyHead
                SetEllipsePosition2(smileyHead, head);
                //SetHeadPosition(head);

                //left side
                SetEllipsePosition2(leftEllipse, handLeft);
                SetEllipsePosition2(elbowleftEllipse, elbowLeft);
                SetEllipsePosition2(sholderleftEllipse, sholderLeft);

                //right side
                SetEllipsePosition2(rightEllipse, handRight);
                SetEllipsePosition2(elbowrightEllipse, elbowRight);
                SetEllipsePosition2(sholderrightEllipse, sholderRight);


                #region detect position
                
                //detect Y
                if ( complete[0]==0 && detectedY() && order[orderIndex].CompareTo('y')==0 )
                {
                    addDebug("Detected Y!");
                    addDebug("Give me an M!");
                    complete[0] = 1;
                    orderIndex=1;
                }

                
                //detect M
                if (complete[1] == 0 && detectedM() && order[orderIndex].CompareTo('m')==0 )
                {
                    addDebug("Detected M!");
                    addDebug("Give me an C!");
                    complete[1] = 1;
                    orderIndex = 2;
                }

                
                //detect C
                if (complete[2] == 0 && detectedC() && order[orderIndex].CompareTo('c') == 0 )
                {
                    addDebug("Detected C!");
                    addDebug("Give me an A!");
                    complete[2] = 1;
                    orderIndex = 3;
                }

                
                //detect A
                if (complete[3] == 0 && detectedA() && order[orderIndex].CompareTo('a') == 0 )
                {
                    addDebug("Detected A!");
                    complete[3] = 1;
                    orderIndex = 4;
                }

                
                //detect BRO
                if (detectedBRO() && order[orderIndex].CompareTo('r') == 0)
                {
                    restart();
                }
                
                
                #endregion

                //should i play?
                if ( shouldPlay() && !activeplayer)
                {
                    
                    addDebug("Playing Music!!!!");
                    PlayMusic();
                    addDebug("Take a bro position in order to restart!");

                }
                
                

            }

        }


        private void restart()
        {
            for (int i = 0; i <= 3; i++)
                complete[i] = 0;
            orderIndex = 0;

            activeplayer = false;
            simpleSound.Stop();


            debugg.Text = "------------Log Area--------------\nGive me an Y!";
=======
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

>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c
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


<<<<<<< HEAD
        #region defined positions

        //detect position for BRO
        private bool detectedBRO()
        {
            return ae(sholderRight.Y, handRight.Y) &
                    (sholderLeft.X < handRight.X) &
                    (sholderRight.X > handRight.X);
        }

        //detect position for A
        private bool detectedA()
        {

            bool rightA = (sholderRight.Y < elbowRight.Y) &
                            (elbowRight.Y < handRight.Y) &
                            (sholderRight.X < elbowRight.X) &
                            (elbowRight.X > handRight.X);

            bool leftA = (sholderLeft.Y < elbowLeft.Y) &
                            (elbowLeft.Y < handLeft.Y) &
                            (sholderLeft.X > elbowLeft.X) &
                            (elbowLeft.X < handLeft.X);

            bool comA = ae(handRight.Y, handLeft.Y) &
                        ae(elbowLeft.Y, elbowRight.Y);


=======
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

>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c
            return rightA & leftA & comA;
        }



<<<<<<< HEAD
        //detect position for C
        private bool detectedC()
        {

            bool rightC = ae(handRight.Y,elbowRight.Y) &
                            (sholderRight.X < handRight.X) &
                            ( elbowRight.Y < sholderRight.Y );

            bool leftC = ae(handLeft.Y, elbowLeft.Y) &
                          (sholderLeft.X < elbowLeft.X);

=======
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
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c

            return rightC & leftC;
        }


<<<<<<< HEAD
        //detect position for M
        private bool detectedM()
        {

            bool rightM =   ae(sholderRight.Y,elbowRight.Y) &
                            ae(handRight.X,elbowRight.X) &
                              (elbowRight.Y > handRight.Y);

            bool leftM = ae(sholderLeft.Y, elbowLeft.Y) &
                         ae(handLeft.X, elbowLeft.X) &
                           (elbowLeft.Y > handLeft.Y);
=======




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
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c
            
            return rightM & leftM;
        }


<<<<<<< HEAD
        //detect position for Y
        private bool detectedY()
        {

            bool rightHandUp = (sholderRight.X < elbowRight.X) & (elbowRight.X < handRight.X) &
                               (sholderRight.Y < elbowRight.Y) & (elbowRight.Y < handRight.Y);

            bool leftHandUp = (sholderLeft.X > elbowLeft.X) & (elbowLeft.X > handLeft.X) &
                              (sholderLeft.Y < elbowLeft.Y) & (elbowLeft.Y < handLeft.Y);
            
            return rightHandUp & leftHandUp;
        }

        #endregion

        //almost equal
        private bool ae(float x, float y)
        {
            return Math.Abs(x - y) < 25;
        }


        //return the joint coordanates. Scaled.
        private JPoint JointLocation(Joint joint)
        {
            JPoint tmp;
            var scaledJoint = joint.ScaleTo(860, 640, .9f, .9f);

            tmp.X = scaledJoint.Position.X;
            tmp.Y = 860 - scaledJoint.Position.Y;
            
            return tmp;
        }

        //move the elipse
        private void SetEllipsePosition2(FrameworkElement ellipse, JPoint jp)
        {
            Canvas.SetLeft(ellipse, jp.X);
            Canvas.SetTop(ellipse, 860 - jp.Y);
        }

=======


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
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c

        private void Window_Closed(object sender, EventArgs e)
        {
            nui.Uninitialize();
        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c

        private void video_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
<<<<<<< HEAD
=======
=======
>>>>>>> 0bd3a0cfb1dce3d0dbddeb9d638735ba9d22b750
>>>>>>> 87b46ca045a9a292512297abf81edcc34775667c
    }
}
