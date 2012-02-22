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
using Microsoft.Kinect;
using System.Reflection;
using System.Media;
using Coding4Fun.Kinect.Wpf;

namespace YMCA_Detector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region global variables
        private struct JPoint
        {
            public float X;
            public float Y;
        }

        //positions of the joints at a given time
        private JPoint head, sholderLeft, elbowLeft, handLeft, sholderRight, elbowRight, handRight;

        int[] complete = { 0, 0, 0, 0, 0 }; //complete progress bar :-)
        string order = "ymcar";
        int orderIndex = 0;


        Assembly assembly;
        SoundPlayer simpleSound;
        bool activeplayer = false;

        bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];

        const int CanvasWidth = 828;
        const int CanvasHeight = 509;

        const int TOLERANCE = 25;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            InitVars();
        }

        private void InitVars()
        {
            assembly = Assembly.GetExecutingAssembly();
            simpleSound = new SoundPlayer(assembly.GetManifestResourceStream("ymca2.wav"));
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


        //handles debug messages and orders
        void addDebug(string msg)
        {
            debugg.Text = msg;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
        }

        /// <summary>
        /// Choose a kinect API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor oldSensor = (KinectSensor)e.OldValue;
            StopKinect(oldSensor);

            KinectSensor newSensor = (KinectSensor)e.NewValue;

            if (newSensor == null)
                return;

            newSensor.ColorStream.Enable();
            newSensor.DepthStream.Enable();
            newSensor.SkeletonStream.Enable();

            newSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(newSensor_AllFramesReady);

            try
            {
                newSensor.Start();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }

        void newSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            
            if (closing)
                return;

            Skeleton __skeleton = GetFirstSkeleton(e);

            if (__skeleton == null)
                return;

            GetJointLocations(__skeleton);
            DrawSkeleton();

            DetectPositions();
        }


        /// <summary>
        /// Searches for Y M C and A
        /// .. and Bro Position
        /// </summary>
        private void DetectPositions()
        {
            //detect Y
            if (complete[0] == 0 && detectedY() && order[orderIndex].CompareTo('y') == 0)
            {
                addDebug("Detected Y! Give me an M!");
                complete[0] = 1;
                orderIndex = 1;
            }


            //detect M
            if (complete[1] == 0 && detectedM() && order[orderIndex].CompareTo('m') == 0)
            {
                addDebug("Detected M!  Give me an C!");
                complete[1] = 1;
                orderIndex = 2;
            }


            //detect C
            if (complete[2] == 0 && detectedC() && order[orderIndex].CompareTo('c') == 0)
            {
                addDebug("Detected C!  Give me an A!");
                complete[2] = 1;
                orderIndex = 3;
            }


            //detect A
            if (complete[3] == 0 && detectedA() && order[orderIndex].CompareTo('a') == 0)
            {
                addDebug("Detected A! Take a bro position to restart!");
                complete[3] = 1;
                orderIndex = 4;
            }


            //detect BRO
            if (detectedBRO() && order[orderIndex].CompareTo('r') == 0)
            {
                restart();
            }

            if (shouldPlay() && !activeplayer)
                PlayMusic();
                addDebug("Take a bro position in order to restart!");

        }

        /// <summary>
        /// Restarts the application
        /// </summary>
        private void restart()
        {
            for (int i = 0; i <= 3; i++)
                complete[i] = 0;
            orderIndex = 0;

            activeplayer = false;
            simpleSound.Stop();


            addDebug("Give me an Y!");
        }


        //are all positions detected?
        private bool shouldPlay()
        {
            for (int i = 0; i <= 3; i++)
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

            bool rightC = ae(handRight.Y, elbowRight.Y) &
                            (sholderRight.X < handRight.X) &
                            (elbowRight.Y < sholderRight.Y);

            bool leftC = ae(handLeft.Y, elbowLeft.Y) &
                          (sholderLeft.X < elbowLeft.X);


            return rightC & leftC;
        }

        //detect position for M
        private bool detectedM()
        {

            bool rightM = ae(sholderRight.Y, elbowRight.Y) &
                            ae(handRight.X, elbowRight.X) &
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

        //almost equal
        private bool ae(float x, float y)
        {
            return Math.Abs(x - y) < TOLERANCE;
        }
        #endregion


        /// <summary>
        /// Draws the Skeleton... or what we need from it
        /// </summary>
        private void DrawSkeleton()
        {
            SetElementPosition(smileyHead, head);
            //SetHeadPosition(head);

            //left side
            SetElementPosition(leftEllipse, handLeft);
            SetElementPosition(elbowleftEllipse, elbowLeft);
            SetElementPosition(sholderleftEllipse, sholderLeft);

            //right side
            SetElementPosition(rightEllipse, handRight);
            SetElementPosition(elbowrightEllipse, elbowRight);
            SetElementPosition(sholderrightEllipse, sholderRight);

        }

        /// <summary>
        /// Moves a framework element from canvas to a given position
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="jp"></param>
        private void SetElementPosition(FrameworkElement element, JPoint jp)
        {
            //TODO: try microsoft way with /2
            Canvas.SetLeft(element, jp.X - element.Width/2);
            Canvas.SetTop(element, CanvasWidth - jp.Y - element.Height/2);
        }


        /// <summary>
        /// Sets the global tracked vars with needed values
        /// </summary>
        private void GetJointLocations(Skeleton thisSkeleton)
        {
            head = JointLocation(thisSkeleton.Joints[JointType.Head]);

            sholderLeft = JointLocation(thisSkeleton.Joints[JointType.ShoulderLeft]);
            elbowLeft = JointLocation(thisSkeleton.Joints[JointType.ElbowLeft]);
            handLeft = JointLocation(thisSkeleton.Joints[JointType.HandLeft]);

            sholderRight = JointLocation(thisSkeleton.Joints[JointType.ShoulderRight]);
            elbowRight = JointLocation(thisSkeleton.Joints[JointType.ElbowRight]);
            handRight = JointLocation(thisSkeleton.Joints[JointType.HandRight]);
        }

        private JPoint JointLocation(Joint joint)
        {
            JPoint tmp;
            //TODO: try without .9f
            var scaledJoint = joint.ScaleTo(CanvasWidth, CanvasHeight, .9f, .9f);

            tmp.X = scaledJoint.Position.X;
            tmp.Y = CanvasWidth - scaledJoint.Position.Y;

            return tmp;
        }

        /// <summary>
        /// Return the first skeleton
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                    return null;

                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked 
                                  select s).FirstOrDefault();

                return first;
            }

        }

        /// <summary>
        /// Stops a kinect sensor. Also the Audio Source.
        /// </summary>
        /// <param name="sensor">a valid kinect sensor referance</param>
        private void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                sensor.Stop();

                if (sensor.AudioSource != null)
                    sensor.AudioSource.Stop();
            }
        }

        /// <summary>
        /// Stoping the kinect sensor & various uninitialise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(kinectSensorChooser1.Kinect);
        }

        private void kinectColorViewer1_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
