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
using System.Media;

using System.Reflection;
using System.IO;
using System.Resources;
using System.Media;
using System.Diagnostics;

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
            InitializeComponent();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            addDebug("Give me a Y!");

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
            if (!activeplayer)
            {
                simpleSound.PlayLooping();
                activeplayer = true;
            }
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


            return rightA & leftA & comA;
        }



        //detect position for C
        private bool detectedC()
        {

            bool rightC = ae(handRight.Y,elbowRight.Y) &
                            (sholderRight.X < handRight.X) &
                            ( elbowRight.Y < sholderRight.Y );

            bool leftC = ae(handLeft.Y, elbowLeft.Y) &
                          (sholderLeft.X < elbowLeft.X);


            return rightC & leftC;
        }


        //detect position for M
        private bool detectedM()
        {

            bool rightM =   ae(sholderRight.Y,elbowRight.Y) &
                            ae(handRight.X,elbowRight.X) &
                              (elbowRight.Y > handRight.Y);

            bool leftM = ae(sholderLeft.Y, elbowLeft.Y) &
                         ae(handLeft.X, elbowLeft.X) &
                           (elbowLeft.Y > handLeft.Y);
            
            return rightM & leftM;
        }


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


        private void Window_Closed(object sender, EventArgs e)
        {
            nui.Uninitialize();
        }

        private void video_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
